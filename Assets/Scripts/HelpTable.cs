using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpTable : MonoBehaviour
{
    public void closeHelpTable()
    {
        if(SceneManager.GetActiveScene().name != "PVPAbilitySelect" && SceneManager.GetActiveScene().name != "PVPCharacterSelect")
        {
            if (GameObject.Find("GameInformation") != null)
            {
                //GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().helpTableOpen = false;
                if (GameObject.Find("Canvas").transform.Find("HelpScreen") != null)
                {
                    GameObject.Find("Canvas").transform.Find("HelpScreen").gameObject.SetActive(false);
                    GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().enableBoardWithDelay(0.5f);
                }
            }
            else
            {
                for (int i = 0; i < GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.childCount; i++)
                {
                    GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", false);
                }
            }
        }
        Destroy(gameObject.transform.parent.gameObject);
    }
}
