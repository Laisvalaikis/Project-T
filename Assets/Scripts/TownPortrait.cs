using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Classes;

public class TownPortrait : MonoBehaviour
{
    public int characterIndex = 0;
    [HideInInspector]public bool available = true;

    public void OnPortraitClick()
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelect3")
        {
            var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
            gameProgress.GetComponent<CharacterSelect>().OnCharacterButtonClick(characterIndex);
        }
        else if (SceneManager.GetActiveScene().name == "PVPCharacterSelect" || SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            GameObject.Find("PVPEventHandler").GetComponent<PVPEventHandler>().OnCharacterButtonClick(characterIndex);
        }
        else
        {
            var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
            if (gameProgress.switchPortraits)
            {
                gameProgress.AddCharacterForSwitching(characterIndex);
            }
            else
            {
                if (GameObject.Find("Canvas").transform.Find("CharacterTable").gameObject.activeInHierarchy && GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex == characterIndex)
                {
                    //uzdaryti lentele
                    GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().ExitTable();
                }
                else
                {
                    gameProgress.DisplayCharacterTable(characterIndex);
                    //atidaryti lentele
                }
            }
        }
    }
    public void OnHover()
    {
        if(available)
        {
            transform.Find("Hover").GetComponent<Animator>().SetBool("hover", true);
        }
    }
    public void OffHover()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("hover", false);
    }

    public void Select()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
    }

    public void Deselect()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("select", false);
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

    public void DisplayCharacterInfo()
    {
        GameObject.Find("GameProgress").GetComponent<GameProgress>().DisplayCharacterTable(characterIndex);
    }
}
