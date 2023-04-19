using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlessingButton : PortraitButton
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPortraitClick()
    {
        // if (SceneManager.GetActiveScene().name == "CharacterSelect3")
        // {
        //     var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        //     gameProgress.GetComponent<CharacterSelect>().OnCharacterButtonClick(characterIndex);
        // }
        // else if (SceneManager.GetActiveScene().name == "PVPCharacterSelect" || SceneManager.GetActiveScene().name == "PVPMapSelect")
        // {
        //     GameObject.Find("PVPEventHandler").GetComponent<PVPEventHandler>().OnCharacterButtonClick(characterIndex);
        // }
        // else
        // {
        //     var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        //     if (_data.switchPortraits)
        //     {
        //         gameProgress.AddCharacterForSwitching(characterIndex);
        //     }
        //     else
        //     {
        //         if (GameObject.Find("Canvas").transform.Find("CharacterTable").gameObject.activeInHierarchy && GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex == characterIndex)
        //         {
        //             //uzdaryti lentele
        //             GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().ExitTable();
        //         }
        //         else
        //         {
        //             GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().DisplayCharacterTable(characterIndex);
        //             GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().UpdateTable();
        //             Debug.Log("Pakeisti sita vieta");
        //             //atidaryti lentele
        //         }
        //     }
        // }
    }
    
    public void ShowHideBlessings()
    {
        if(transform.parent.Find("BlessingsTable").gameObject.activeSelf)
        {
            Deselect();
            transform.parent.Find("BlessingsTable").gameObject.SetActive(false);
        }
        else
        {
            Select();
            transform.parent.Find("BlessingsTable").gameObject.SetActive(true);
        }
    }
}
