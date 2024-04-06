using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;


public class MiniMapController : MonoBehaviourPunCallbacks
{
    public RectTransform minimapRect;
    public GameObject localPlayerIconPrefab;
    public GameObject networkPlayerIconPrefab;
    private RectTransform _playerIcon;
    private Dictionary<GameObject, RectTransform> _playerIcons = new Dictionary<GameObject, RectTransform>();

    private float _mapScale;
    private string _localPlayerType;
    private Vector2 _minimapPosition;

    private void InitializeMiniMap()
    {
        float worldSize = 4000f; // TODO: change it to the real game world size
        float mapSize = minimapRect.sizeDelta.y; // The height of the minimap UI
        _mapScale = mapSize / worldSize;
    }

    IEnumerator WaitForPlayerTypeAndInitialize()
    {
        while (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerType"))
        {
            yield return null;
        }

        _localPlayerType = PhotonNetwork.LocalPlayer.CustomProperties["PlayerType"] as string;
        // Debug.Log("localPlayerType: " + _localPlayerType);
        InitializeMiniMap();
    }

    void Start()
    {
        StartCoroutine(WaitForPlayerTypeAndInitialize());
    }


    public void AddPlayerIcon(GameObject player)
    {
        string playerType = player.GetComponent<PhotonView>().Owner.CustomProperties["PlayerType"] as string;
        PhotonView playerPv = player.GetComponent<PhotonView>();
        if (playerType == _localPlayerType)
        {
            GameObject iconPrefab = playerPv.IsMine ? localPlayerIconPrefab : networkPlayerIconPrefab;
            _playerIcon = Instantiate(iconPrefab, minimapRect).GetComponent<RectTransform>();
            _playerIcons[player] = _playerIcon;
        }
    }


    [PunRPC]
    public void AddPlayerIconRPC(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameObject player = targetView.gameObject;
            AddPlayerIcon(player);
        }
        else
        {
            Debug.LogError("Unable to find player with PhotonView ID: " + viewID);
        }
    }


    public void UpdatePlayerIcon(GameObject player, Vector3 newPosition, Quaternion newRotation)
    {
        if (_playerIcons.ContainsKey(player))
        {
            RectTransform iconTransform = _playerIcons[player];
            Vector2 minimapPosition = new Vector2(newPosition.x, newPosition.z) * _mapScale;
            iconTransform.anchoredPosition = minimapPosition;
            iconTransform.localEulerAngles = new Vector3(0, 0, -newRotation.eulerAngles.y);
        }
        else
        {
            AddPlayerIcon(player);
        }
    }

    [PunRPC]
    public void HidePlayerIconRPC(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameObject player = targetView.gameObject;
            if (_playerIcons.ContainsKey(player))
            {
                Destroy(_playerIcons[player].gameObject);
                _playerIcons.Remove(player);
            }
        }
        else
        {
            Debug.LogError("Unable to find PhotonView with ID: " + viewID);
        }
    }
}