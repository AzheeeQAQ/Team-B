using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RespawnUI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RespawnCountdown(5));
        Cursor.visible = false;
        GameObject fightManagerObject = GameObject.Find("fight");

        if (fightManagerObject == null)
        {
            Debug.Log("fightManagerObject is null");
        }
        if (fightManagerObject != null)
        {
            FightManager fightManager = fightManagerObject.GetComponent<FightManager>();
            fightManager.RespawnCheese();
        }
    }

    IEnumerator RespawnCountdown(int seconds)
    {
        while (seconds > 0)
        {
            Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMask("You will be respawn in " + seconds + " sec...");
            yield return new WaitForSeconds(1);
            seconds--;
        }

        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMask("You will be respawn in " + seconds + " sec...");        
    }

}
