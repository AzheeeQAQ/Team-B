using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(Text))]
public class PlayerNameDisplay : MonoBehaviourPun {

    private TextMeshProUGUI playerNameText;

    void Awake() {
        playerNameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start() {
        if(photonView.IsMine) {
            playerNameText.enabled = false;
        }
    }

    // public void SetPlayerName(string name) {
    //     if (playerNameText != null) {
    //         playerNameText.text = name;
    //     } else {
    //         Debug.LogError("Text component not found on the GameObject.");
    //     }
    // }

    [PunRPC]
    public void SetPlayerNameRPC(string name) {
        if (playerNameText != null) {
            playerNameText.text = name;
        } else {
            Debug.LogError("Text component not found on the GameObject.");
        }
    }
    
    void Update() {
        if(Camera.main != null) {
            var cameraPosition = Camera.main.transform.position;
            // 使UI元素面向摄像机，需要反转面向方向
            transform.LookAt(transform.position * 2 - cameraPosition);
        }
    }

}