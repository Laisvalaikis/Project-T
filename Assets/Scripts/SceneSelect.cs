using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class SceneSelect : MonoBehaviour
{
    public InputField slotNameInput;
    private int difficulty;
    private string color;
    [SerializeField] private Button buttonForCreation;

    private MusicIndex _musicIndex;
    //public TownData newGameData;

    void Start()
    {
        _musicIndex = GetComponent<MusicIndex>();
        for(int i = 0; i < 3; i++)
        {
            Transform slotCard = GameObject.Find("CanvasCamera").transform.Find("NewGameContinue").Find("SlotCards").GetChild(i);
            
        }
        slotNameInput.onValidateInput += delegate (string input, int charIndex, char addedChar) { return CharacterTable.MyValidate(addedChar); };
        difficulty = -1;
        color = "";
        //if (!SaveSystem.DoesSaveFileExist() || SaveSystem.LoadTownData().newGame)
        //{
        //    Vector3 startPosition = GameObject.Find("CanvasCamera").transform.Find("NewGameContinue").transform.position;
        //    GameObject.Find("CanvasCamera").transform.Find("NewGameContinue").Find("Continue").gameObject.SetActive(false);
        //    GameObject.Find("CanvasCamera").transform.Find("NewGameContinue").transform.localPosition = startPosition + new Vector3(-2f, 73f, -91f);
        //}

        PVPManager.instance = null;
        if (GameObject.Find("PVPManager") != null)
        {
            Destroy(GameObject.Find("PVPManager"));
        }
    }
    public void OnInputValueChanged()
    {
        if(slotNameInput.text == " ")
        {
            slotNameInput.text = "";
        }
        slotNameInput.text = slotNameInput.text.ToUpper();
        if (slotNameInput.text != "" && difficulty != -1 && color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }

    public void StartNewGame()
    {
        string slotName = (slotNameInput.text == "") ? "" : slotNameInput.text;
        SaveSystem.SaveTownData(TownData.NewGameData(color, difficulty, slotName));
        //SaveSystem.SaveTownData(TownData.newGameData);
    }

    public void ChangeScene(string SceneToLoad)
    {
        Time.timeScale = 1;
    }

    public void SceneTransition(string sceneName)
    {
        _musicIndex.ChangeLevelMusic();
        GameObject.Find("LoadingScreenCanvas").GetComponent<LoadingScreenController>().LoadScene(sceneName);
    }

    public void ChangeSinglePlayerState(bool state)
    {
        if(SaveSystem.DoesSaveFileExist())
        {
            TownData data = SaveSystem.LoadTownData();
            data.singlePlayer = state;
            SaveSystem.SaveTownData(data);
        }
    }

    public void SaveData(int slotIndex)
    {
        SaveSystem.SaveCurrentSlot(slotIndex);
        //cia po sito turbut vps viska galima istrint
        //if (SaveSystem.LoadTownData() == null)
        //{
        //    //Debug.Log("Saving new town data");
        //    //SaveSystem.SaveTownData(TownData.newGameData);
        //    throw new System.Exception("Save file not found");
        //}
        if (SaveSystem.LoadStatistics(true) == null)
        {
            //Debug.Log("Saving new global statistics");
            SaveSystem.SaveStatistics(new Statistics(), true);
        }
        if (SaveSystem.LoadStatistics() == null)
        {
            //Debug.Log("Saving new statistics");
            SaveSystem.SaveStatistics(new Statistics());
        }
    }

    public void DeleteSlot(int slotIndex)
    {
        SaveSystem.DeleteSlot(slotIndex);
    }

    public void ClearGameData()
    {
        SaveSystem.ClearGameData();
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        if (slotNameInput.text != "" && difficulty != -1 && color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }

    public void SetColor(string color)
    {
        this.color = color;
        if (slotNameInput.text != "" && difficulty != -1 && color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }
}
