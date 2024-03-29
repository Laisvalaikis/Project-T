﻿using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using System.IO;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;
    public PortraitBar portraitBarControl;
    public GameUi gameUi;
    public SaveData _saveData;
    public ButtonManager cornerButtonManager;
    public Data _data;
    void Awake()
    {
        instance = this;
        List<int> enemies = SaveSystem.LoadTownData().enemies;
        bool allowEnemySelection = SaveSystem.LoadTownData().allowEnemySelection;
        bool allowDuplicates = SaveSystem.LoadTownData().allowDuplicates;
        if (_data.isCurrentScenePlayableMap && SaveSystem.DoesSaveFileExist())
        {
            _saveData.LoadTownData();
            GetComponent<MapSetup>().MapName = SaveSystem.LoadTownData().selectedMission;
            if (GetComponent<MapSetup>().GetSelectedMap() != null)
            {
                GameObject map = Instantiate(GetComponent<MapSetup>().GetSelectedMap(), GameObject.Find("Map").transform) as GameObject;
                GameObject.Find("CM vcam1").GetComponent<CameraController>().DefaultToFollow = map.transform.Find("ToFollow").gameObject;
                GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitX.x = map.transform.Find("LowerLimit").position.x;
                GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitX.y = map.transform.Find("UpperLimit").position.x;
                GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitY.x = map.transform.Find("LowerLimit").position.y;
                GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitY.y = map.transform.Find("UpperLimit").position.y;
                GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = map.transform.Find("ToFollow");
            }
            if (SceneManager.GetActiveScene().name == "Campfire" && _data.townData.difficultyLevel == 0)
            {
                GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[1].characters.RemoveRange(3, 2);
            }
            if (SceneManager.GetActiveScene().name == "Campfire" && _data.townData.difficultyLevel == 1)
            {
                GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[1].characters.RemoveRange(4, 1);
            }
            if (GameObject.Find("GameInformation") != null && _data.townData.singlePlayer)
            {
                if (_data.Characters.Count > 0)
                {
                    var gameInformation = GameObject.Find("GameInformation").GetComponent<PlayerTeams>();
                    gameInformation.allCharacterList.teams[0].teamName = SaveSystem.LoadTownData().teamColor;
                    _data.CharactersOnLastMission.Clear();
                    gameInformation.allCharacterList.teams[0].characters.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        _data.CharactersOnLastMission.Add(i);
                        gameInformation.allCharacterList.teams[0].characters.Add(_data.Characters[i].prefab);
                        _data.statistics.charactersSelectedCountByClass[Statistics.getClassIndex(_data.Characters[i].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                        _data.globalStatistics.charactersSelectedCountByClass[Statistics.getClassIndex(_data.Characters[i].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                    }
                }
            }
            _saveData.SaveTownData();
        }
        if (_data.isCurrentScenePlayableMap && _data.townData.singlePlayer)
        {
            var playerTeams = GameObject.Find("GameInformation").GetComponent<PlayerTeams>();
            playerTeams.allCharacterList.teams[1].characters.Clear();
            playerTeams.allCharacterList.teams[1].isTeamAI = true;
            List<GameObject> possibleEnemyPrefabs = _data.AllEnemyCharacterPrefabs;
            if (allowEnemySelection)
            {
                possibleEnemyPrefabs.Clear();
                foreach (int enemyIndex in enemies)
                {
                    possibleEnemyPrefabs.Add(_data.AllEnemySavedCharacters[enemyIndex].prefab);
                }
            }
            List<int> UsedIndexes = new List<int>();
            int enemyCount = 3;
            if (GetComponent<MapSetup>().GetSelectedMap() != null)
            {
                enemyCount = GetComponent<MapSetup>().GetSelectedMap().GetComponent<Map>().numberOfEnemies;
            }
            for (int i = 0; i < enemyCount; i++)
            {
                int index = UnityEngine.Random.Range(0, possibleEnemyPrefabs.Count);
                if (UsedIndexes.Contains(index) && !allowDuplicates)
                {
                    i--;
                }
                else
                {
                    playerTeams.allCharacterList.teams[1].characters.Add(possibleEnemyPrefabs[index]);
                    UsedIndexes.Add(index);
                }
            }
        }
    }
    
    void Start()
    {
        for (int i = 0; i < _data.AllAvailableCharacters.Count; i++)
        {
            _data.AllAvailableCharacters[i].prefabIndex = i;
        }
        //if (SceneManager.GetActiveScene().name == "Town" || SceneManager.GetActiveScene().name == "MissionSelect" || SceneManager.GetActiveScene().name == "CharacterSelect")
        //{
        //    LoadTownData();
        //}
        //if (SceneManager.GetActiveScene().name == "Town")
        //{
        //    GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().RecruitmentStart();
        //}
        //if (SceneManager.GetActiveScene().name == "MissionSelect")
        //{
        //    townData.generatedEncounters = GameObject.Find("CanvasCamera").transform.Find("EncounterButtons").GetComponent<EncounterController>().Setup(townData.pastEncounters, ref townData.generateNewEncounters, townData.generatedEncounters);
        //}
        //if (SceneManager.GetActiveScene().name == "MissionEnd")
        //{
        //    LoadTownData();
        //    GameObject.Find("CanvasCamera").transform.Find("XPProgressTable").GetComponent<XPProgressManager>().UpdateButtons();
        //    //canButtonsBeClicked = true;
        //    //canButtonsBeClickedState = true;
        //}
        //if (SceneManager.GetActiveScene().name == "Town" || SceneManager.GetActiveScene().name == "MissionSelect" || SceneManager.GetActiveScene().name == "CharacterSelect")
        //{
        //    PrepareNewTownDay();
        //}
        //if (SceneManager.GetActiveScene().name == "CharacterSelect")
        //{
        //    GameObject.Find("CanvasCamera").transform.Find("AutoFill").GetComponent<Button>().interactable = Characters.Count >= 3;
        //}
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            // LoadTownData();
            PrepareNewTownDay();
            GameObject.Find("CanvasCamera").transform.Find("AutoFill").GetComponent<Button>().interactable = _data.Characters.Count >= 3;
        }
    }

    public void PrepareNewTownDay()
    {
        RemoveDeadCharacters();
        FakeUpdate();
        _data.canButtonsBeClicked = true;
        _data.canButtonsBeClickedState = true;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Town" || SceneManager.GetActiveScene().name == "MissionSelect" || SceneManager.GetActiveScene().name == "CharacterSelect" || SceneManager.GetActiveScene().name == "CharacterSelect3")
        {
            if (Input.GetKeyDown("escape"))
            {
                if (GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
                    UnpauseGame();
                else PauseGame();

                FakeUpdate();
            }
            if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        /*
        if (Input.GetKeyDown("u"))
        {
            SaveTownData();
        }
        if (Input.GetKeyDown("i"))
        {
            LoadTownData();
            FakeUpdate();
        }
        if (Input.GetKeyDown("o"))
        {
            StartNewGame();
            LoadTownData();
            FakeUpdate();
        }*/
        _data.statistics.playTime += Time.deltaTime;
        _data.globalStatistics.playTime += Time.deltaTime;
        if (_data.isCurrentScenePlayableMap)
        {
            _data.statistics.battleTime += Time.deltaTime;
            _data.globalStatistics.battleTime += Time.deltaTime;
        }
    }
    //UPDATES
    public void FakeUpdate()
    {
        UpdateMaxCharacterCount();

        if (SceneManager.GetActiveScene().name == "Town")
        {
            gameUi.UpdateTownCost();
            gameUi.UpdateDayNumber();
            gameUi.UpdateDifficultyButton();
            gameUi.UpdateUnspentPointWarnings();
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().UpdateButtons();
            GameObject.Find("CanvasCamera").transform.Find("TownHallTable").GetComponent<TownHall>().UpdateButtons();
        }
        // GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().UpdateTable();
    }

    private void UpdateMaxCharacterCount()
    {
        if (_data.townData.townHall[0] == '1')
        {
            _data.maxCharacterCount = 12;
        }
        else
        {
            _data.maxCharacterCount = 6;
        }
    }
    
    //sita vps istrint turbut reiks
    public void UpdateCharacterButtons(int direction)//up - 1, down - -1, none - 0
    {
        Transform CharacterButtons = GameObject.Find("CanvasCamera").transform.Find("CharacterButtons");
        gameUi.UpdateUnspentPointWarnings();
        for (int i = 0; i < 6; i++)
        {
            CharacterButtons.GetChild(i).gameObject.SetActive(false);
        }
        int start = 0;
        int finish = 6;
        if (direction == 0)
        {
            start = 0;
            finish = _data.Characters.Count;
            if (finish > 6)
            {
                finish = 6;
                CharacterButtons.Find("Down").GetComponent<Button>().interactable = true;
            }
        }
        else if (direction == -1)
        {
            CharacterButtons.Find("Down").GetComponent<Button>().interactable = false;
            CharacterButtons.Find("Up").GetComponent<Button>().interactable = true;
            start = 6;
            finish = _data.Characters.Count;
        }
        else if (direction == 1)
        {
            CharacterButtons.Find("Down").GetComponent<Button>().interactable = true;
            CharacterButtons.Find("Up").GetComponent<Button>().interactable = false;
        }
        for (int i = start; i < finish; i++)
        {
            if (i != _data.Characters.Count - 1 || i % 2 == 1)
            {
                CharacterButtons.GetChild(i % 6).localPosition = new Vector3(-17 + (i % 2) * 34, CharacterButtons.GetChild(i % 6).localPosition.y, 0);

            }
            else
            {
                CharacterButtons.GetChild(i % 6).localPosition = new Vector3(0, CharacterButtons.GetChild(i % 6).localPosition.y, 0);
            }
            CharacterButtons.GetChild(i % 6).GetComponent<CharacterPortrait>().characterIndex = i;
            CharacterButtons.GetChild(i % 6).Find("Character").Find("Portrait").gameObject.SetActive(true);
            CharacterButtons.GetChild(i % 6).Find("Character").Find("Portrait").GetComponent<Image>().sprite =
                _data.Characters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            CharacterButtons.GetChild(i % 6).Find("Character").Find("LevelText").GetComponent<Text>().text = _data.Characters[i].level.ToString();
            /////////// CIA GAL?
            //CharacterButtons.GetChild(i % 6).Find("AbilityPointCorner").gameObject.SetActive(_data.AllAvailableCharacters[i].abilityPointCount > 0);
            //CharacterButtons.GetChild(i % 6).Find("AbilityPointText").gameObject.SetActive(_data.AllAvailableCharacters[i].abilityPointCount > 0);

            CharacterButtons.GetChild(i % 6).gameObject.SetActive(true);
            if (CharacterButtons.GetChild(i % 6).GetComponent<CharacterPortrait>().characterIndex == _data.currentCharacterIndex)
            {
                CharacterButtons.GetChild(i % 6).transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
            }
        }
    }
    
    //RECRUITMENT
    public void BuyCharacter(SavedCharacter character)
    {
        if (_data.canButtonsBeClicked)
        {
            _data.InsertCharacter(character);
            _data.townData.townGold -= character.cost;
            gameUi.EnableGoldChange("-" + character.cost + "g");
            // FakeUpdate();
            gameUi.UpdateTownCost();
            portraitBarControl.InsertCharacter();
            gameUi.UpdateUnspentPointWarnings();
            gameUi.UpdateBuyRecruitsWarning();
            _data.statistics.charactersBoughtCountByClass[Statistics.getClassIndex(character.prefab.GetComponent<PlayerInformation>().ClassName)]++;
            _data.globalStatistics.charactersBoughtCountByClass[Statistics.getClassIndex(character.prefab.GetComponent<PlayerInformation>().ClassName)]++;
            
        }
    }
    public void SpendGold(int cost)
    {
        _data.townData.townGold -= cost;
        gameUi.EnableGoldChange("-" + cost + "g");
        FakeUpdate();
    }
    public void RemoveCharacter(int index)
    {
        _data.Characters.RemoveAt(index);
        GameObject.Find("Canvas").transform.Find("CharacterTable").gameObject.SetActive(false);
        // FakeUpdate();
        gameUi.UpdateTownCost();
        portraitBarControl.RemoveCharacter(index);
        if (_data.Characters.Count == 6)
        {
            Debug.Log("Need To change");
            // UpdateCharacterBar(1);
        }
    }
    public void RemoveDeadCharacters()
    {
        List<SavedCharacter> CharactersCopy = new List<SavedCharacter>(_data.Characters);
        for (int i = 0; i < CharactersCopy.Count; i++)
        {
            if (CharactersCopy[i].dead)
            {
                portraitBarControl.RemoveCharacter(i);
                _data.Characters.Remove(CharactersCopy[i]);
            }
        }
        FakeUpdate();
    }
    //SWITCH PORTRAITS
    public void TurnOnSwitching()
    {
        _data.switchPortraits = true;
        _data.SwitchedCharacters.Clear();
    }
    public void AddCharacterForSwitching(int index)
    {
        _data.SwitchedCharacters.Add(index);
        if (_data.SwitchedCharacters.Count == 2)//switch
        {
            _data.switchPortraits = false;
            SavedCharacter temp = _data.Characters[_data.SwitchedCharacters[0]];
            _data.Characters[_data.SwitchedCharacters[0]] = _data.Characters[_data.SwitchedCharacters[1]];
            _data.Characters[_data.SwitchedCharacters[1]] = temp;
            FakeUpdate();
        }
    }
    
    public void StartNewGame()
    {
        SaveSystem.SaveTownData(_data.newGameData);
    }
    public void PauseGame()
    {
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = false;
        if (SceneManager.GetActiveScene().name == "Town")
        {
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterButton").GetComponent<Button>().interactable = false;
            GameObject.Find("CanvasCamera").transform.Find("TownHallButton").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<Button>().interactable = false;
        GameObject.Find("Canvas").transform.Find("PauseMenu").transform.Find("PauseMenu").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("PauseMenu").transform.Find("ConfirmMenu").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        StartCoroutine(ExecuteAfterTime(0.1f, () =>
        {
            if (!GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
                _data.canButtonsBeClicked = _data.canButtonsBeClickedState;
        }));
        if (SceneManager.GetActiveScene().name == "Town")
        {
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterButton").GetComponent<Button>().interactable = true;
            GameObject.Find("CanvasCamera").transform.Find("TownHallButton").GetComponent<Button>().interactable = true;
        }
        if (SceneManager.GetActiveScene().name == "Town" || _data.townData.selectedMission != "")
        {
            GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<Button>().interactable = true;
        }
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void DisplayCharacterInfo(int index)
    {
        _data.currentCharacterIndex = index;
        Transform CharInfo = GameObject.Find("CanvasCamera").transform.Find("CharacterInformationFrame");
        SavedCharacter Char = _data.Characters[index];
        CharInfo.Find("Name").GetComponent<Text>().color = Char.prefab.GetComponent<PlayerInformation>().ClassColor;
        CharInfo.Find("Name").GetComponent<Text>().text = Char.characterName;
        CharInfo.Find("Class").GetComponent<Text>().color = Char.prefab.GetComponent<PlayerInformation>().ClassColor;
        CharInfo.Find("Class").GetComponent<Text>().text = Char.prefab.GetComponent<PlayerInformation>().ClassName;
        CharInfo.Find("Role").GetComponent<Text>().color = Char.prefab.GetComponent<PlayerInformation>().ClassColor;
        CharInfo.Find("Role").GetComponent<Text>().text = Char.prefab.GetComponent<PlayerInformation>().role;

        for (int i = 0; i < 5; i++)
        {
            CharInfo.Find("Abilities").GetChild(i).gameObject.SetActive(true);
            CharInfo.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().sprite = Char.prefab.GetComponent<ActionManager>().AbilityBackground;
            CharInfo.Find("Abilities").GetChild(i).Find("AbilityIcon").GetComponent<Image>().color = Char.prefab.GetComponent<PlayerInformation>().ClassColor;
        }
        int currentIndex = 2;
        for (int j = 0; j < Char.unlockedAbilities.Length; j++)
        {
            if (Char.unlockedAbilities[j] == '1' && currentIndex < CharInfo.Find("Abilities").childCount)
            {
                CharInfo.Find("Abilities").GetChild(currentIndex).Find("AbilityIcon").GetComponent<Image>().sprite
                = Char.prefab.GetComponent<ActionManager>().FindActionByIndex(j).AbilityIcon;
                currentIndex++;
            }
        }
        if (currentIndex == 3)
        {
            CharInfo.Find("Abilities").GetChild(2).localPosition = new Vector3(0, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
        }
        if (currentIndex == 4)
        {
            CharInfo.Find("Abilities").GetChild(2).localPosition = new Vector3(-11, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
            CharInfo.Find("Abilities").GetChild(3).localPosition = new Vector3(11, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
        }
        if (currentIndex == 5)
        {
            CharInfo.Find("Abilities").GetChild(2).localPosition = new Vector3(-22, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
            CharInfo.Find("Abilities").GetChild(3).localPosition = new Vector3(0, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
            CharInfo.Find("Abilities").GetChild(4).localPosition = new Vector3(22, CharInfo.Find("Abilities").GetChild(2).localPosition.y, CharInfo.Find("Abilities").GetChild(2).localPosition.z);
        }
        for (int i = currentIndex; i < 5; i++)
        {
            CharInfo.Find("Abilities").GetChild(i).gameObject.SetActive(false);
        }
        //pratesti...
    }
    public void UpdateCharacterInfo()
    {
        DisplayCharacterInfo(_data.currentCharacterIndex);
    }
   
    public void ToggleRemoveButton(bool enable)
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            GameObject.Find("CanvasCamera").transform.Find("Remove").GetComponent<Button>().interactable = enable;
        }
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
    public void ChangeSingePlayerState(bool singlePlayer)
    {
        _data.townData.singlePlayer = singlePlayer;
    }
    
    // Cia button Problema

    public void ChangeDifficulty()
    {
        if (_data.townData.difficultyLevel == 0)
        {
            _data.townData.difficultyLevel = 1;
        }
        else
        {
            _data.townData.difficultyLevel = 0;
        }
        FakeUpdate();
    }
    public void SetSceneToLoad()
    {
        string sceneToLoad = SaveSystem.LoadTownData().selectedMission;
        GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<SceneChangingButton>().SceneToLoad =
            (GetComponent<MapSetup>().MapPrefabs.Find(x => x.name == sceneToLoad) != null) ? "RegularEncounter" : sceneToLoad;
    }

    public void SetSavedCharactersOnPrefabs()
    {
        if (GameObject.Find("GameInformation") != null && _data.townData.singlePlayer)
        {
            if (_data.CharactersOnLastMission.Count > 0)
            {
                for (int i = 0; i < _data.CharactersOnLastMission.Count; i++)
                {
                    var characterInfo = GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>();
                    characterInfo.savedCharacter = _data.Characters[i];
                    cornerButtonManager.GenerateAbilities();//SavedCharacter implementation
                }
            }
            //Enemies
            foreach (var character in GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[1].characters)
            {
                List<string> abilitiesToEnable = new List<string>();
                //Abilities
                foreach (var ability in character.GetComponent<AIBehaviour>().specialAbilities)
                {
                    if (ability.difficultyLevel <= _data.townData.selectedEncounter.encounterLevel)
                    {
                        abilitiesToEnable.Add(ability.abilityName);
                    }
                }
                character.GetComponent<PlayerInformation>().enabledAbilitiesEnemy = abilitiesToEnable;
                //Blessings
                foreach (var blessing in character.GetComponent<AIBehaviour>().specialBlessings)
                {
                    if (blessing.difficultyLevel <= _data.townData.selectedEncounter.encounterLevel)
                    {
                        character.GetComponent<PlayerInformation>().BlessingsAndCurses.Add(new Blessing(blessing.blessingName, 0, "", "", "", ""));
                    }
                }
                // cornerButtonManager.GenerateAbilitiesForEnemy(abilitiesToEnable);
                //Debug.LogError("Disabled Enemies Ability Generation");
                //Debug.Log("Cia Sukuria Enemies ir pateikia ju abilities");
            }
        }
        //print("saved characters set");
    }
    
    public static int currentMaxLevel()
    {
        int MaxLevel = 2;
        char townHallChar = instance._data.townData.townHall[2];
        if (townHallChar == '0')
        {
            MaxLevel = 2;
        }
        if (townHallChar == '1')
        {
            MaxLevel = 3;
        }
        if (townHallChar == '2')
        {
            MaxLevel = 4;
        }
        /* if (townHallChar == '3')
         {
         }*/
        return MaxLevel;
    }
}
