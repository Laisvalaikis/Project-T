using Assets.Scripts.Classes;
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
        if (SceneManager.GetActiveScene().name != "CharacterSelect3")
        {
            if (SceneManager.GetActiveScene().name != "CharacterSelect")
            {
                Debug.Log("Need To change function?");
                
                // UpdateCharacterBar(0);
            }
            else
            {
                UpdateCharacterButtons(0);
            }
        }
        if (SceneManager.GetActiveScene().name == "Town")
        {
            gameUi.UpdateTownCost();
            gameUi.UpdateDayNumber();
            gameUi.UpdateDifficultyButton();
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

    // public void UpdateCharacterBar(int direction = 0)//up - 1, down - -1, none - 0
    // {
    //     Transform PortraitBarButtons = GameObject.Find("CanvasCamera").transform.Find("PortraitBar").Find("CharacterButtons");
    //     // Perdaryti
    //     if (direction == 0)
    //     {
    //         if (PortraitBarButtons.GetChild(0).GetComponent<TownPortrait>().characterIndex == 6)
    //         {
    //             direction = -1;
    //         }
    //         else direction = 1;
    //     }
    //     for (int i = 0; i < 6; i++)
    //     {
    //         // townPortraits[i].gameObject.SetActive(false);
    //         // townPortraits[i].GetComponent<Image>().sprite = null;
    //         PortraitBarButtons.GetChild(i).gameObject.SetActive(false);
    //         PortraitBarButtons.GetChild(i).Find("Character").Find("Portrait").gameObject.SetActive(false);
    //         PortraitBarButtons.GetChild(i).Find("Character").Find("Portrait").GetComponent<Image>().sprite = null;
    //     }
    //     int start = 0;
    //     int finish = 6;
    //     if (direction == -1)
    //     {
    //         GameObject.Find("CanvasCamera").transform.Find("Down").GetComponent<Button>().interactable = false;
    //         GameObject.Find("CanvasCamera").transform.Find("Up").GetComponent<Button>().interactable = true;
    //         start = 6;
    //         finish = _data.Characters.Count;
    //         if (_data.Characters.Count != 6 && _data.Characters.Count != 12)
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (_data.Characters.Count % 6 - 1) * 68, 0);
    //         }
    //         else
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (6 - 1) * 68, 0);
    //         }
    //     }
    //     else if (direction == 1)
    //     {
    //         GameObject.Find("CanvasCamera").transform.Find("Down").GetComponent<Button>().interactable = _data.Characters.Count > 6;
    //         GameObject.Find("CanvasCamera").transform.Find("Up").GetComponent<Button>().interactable = false;
    //         if (_data.Characters.Count < 6)
    //         {
    //             finish = _data.Characters.Count;
    //         }
    //         else finish = 6;
    //         if (_data.Characters.Count != 6)
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (finish - 1) * 68, 0);
    //         }
    //         else
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (6 - 1) * 68, 0);
    //         }
    //     }
    //
    //     for (int i = start; i < finish; i++)
    //     {
    //         PortraitBarButtons.GetChild(i % 6).gameObject.SetActive(true);
    //         PortraitBarButtons.GetChild(i % 6).GetComponent<TownPortrait>().characterIndex = i;
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("Portrait").gameObject.SetActive(true);
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("Portrait").GetComponent<Image>().sprite =
    //             _data.Characters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("LevelText").GetComponent<Text>().text = _data.Characters[i].level.ToString();
    //         PortraitBarButtons.GetChild(i % 6).Find("AbilityPointCorner").gameObject.SetActive(_data.Characters[i].abilityPointCount > 0);
    //     }
    //     GetComponent<Town>()?.ToggleAbilityPointWarning();
    // }
    
    
    //sita vps istrint turbut reiks
    public void UpdateCharacterButtons(int direction)//up - 1, down - -1, none - 0
    {
        Transform CharacterButtons = GameObject.Find("CanvasCamera").transform.Find("CharacterButtons");
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

            //CharacterButtons.GetChild(i % 6).Find("AbilityPointCorner").gameObject.SetActive(Characters[i].abilityPointCount > 0);
            //CharacterButtons.GetChild(i % 6).Find("AbilityPointText").gameObject.SetActive(Characters[i].abilityPointCount > 0);

            CharacterButtons.GetChild(i % 6).gameObject.SetActive(true);
            if (CharacterButtons.GetChild(i % 6).GetComponent<CharacterPortrait>().characterIndex == _data.currentCharacterIndex)
            {
                CharacterButtons.GetChild(i % 6).transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
            }
        }
    }
    /*
    public void UpdateCharacterButtons()
    {
        Transform CharacterButtons = GameObject.Find("CanvasCamera").transform.Find("CharacterButtons");
        for(int i = 0; i < Characters.Count; i++)
        {
            CharacterButtons.GetChild(i).GetComponent<TownPortrait>().characterIndex = i;
            CharacterButtons.GetChild(i).Find("Character").Find("Portrait").gameObject.SetActive(true);
            CharacterButtons.GetChild(i).Find("Character").Find("Portrait").GetComponent<Image>().sprite =
                Characters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            CharacterButtons.GetChild(i).Find("Character").Find("LevelText").GetComponent<Text>().text = Characters[i].level.ToString();
            CharacterButtons.GetChild(i).gameObject.SetActive(true);
        }
    }
    */
    
    
    
    //RECRUITMENT
    public void BuyCharacter(SavedCharacter character)
    {
        if (_data.canButtonsBeClicked)
        {
            _data.Characters.Add(character);
            _data.townData.townGold -= character.cost;
            gameUi.EnableGoldChange("-" + character.cost + "g");
            // FakeUpdate();
            gameUi.UpdateTownCost();
            portraitBarControl.InsertCharacter();
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

    // //SAVESYSTEM
    // public void SaveTownData()
    // {
    //     //sita ifa visa turbut irgi istrint reiks karocia
    //     //if (SceneManager.GetActiveScene().name == "CharacterSelect")
    //     //{
    //     //    List<SavedCharacter> charactersToGoOnMission = new List<SavedCharacter>();
    //     //    charactersToGoOnMission.Clear();
    //     //    for (int i = 0; i < 3; i++)
    //     //    {
    //     //        int index = GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().PortraitButtonList[i].characterIndex;
    //     //        charactersToGoOnMission.Add(Characters[index]);
    //     //    }
    //     //    for (int i = 2; i >= 0; i--)
    //     //    {
    //     //        Characters.Remove(charactersToGoOnMission[i]);
    //     //        Characters.Insert(0, charactersToGoOnMission[i]);
    //     //    }
    //     //}
    //     selectedEnemies = new List<int>();
    //     bool allowEnemySelection = false;
    //     bool allowDuplicates = false;
    //     if (SceneManager.GetActiveScene().name == "CharacterSelect3")
    //     {
    //         GetComponent<CharacterSelect>().SaveData();
    //         allowEnemySelection = true;
    //         allowDuplicates = GetComponent<CharacterSelect>().allowDuplicates;
    //     }
    //     List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
    //     if (SceneManager.GetActiveScene().name == "Town")
    //     {
    //         RCcharacters = GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop;
    //     }
    //     else if (SaveSystem.DoesSaveFileExist() && !createNewRCcharacters)
    //     {
    //         townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
    //         //TownData loadedData = SaveSystem.LoadTownData();
    //         //for (int i = 0; i < townData.rcCharacters.Count; i++)
    //         //{
    //         //    SavedCharacter newCharacter = AllAvailableCharacters[loadedData.RCcharacters[i]];
    //         //    newCharacter.characterName = loadedData.RCcharacterNames[i];
    //         //    newCharacter.level = loadedData.RCcharacterLevels[i];
    //         //    newCharacter.XP = 0;
    //         //    newCharacter.abilityPointCount = newCharacter.level;
    //         //    newCharacter.unlockedAbilities = "0000";
    //         //    RCcharacters.Add(newCharacter);
    //         //}
    //     }
    //     if (createNewRCcharacters)
    //     {
    //         RCcharacters = null;
    //     }
    //     TownData data = new TownData(townData.difficultyLevel, townData.townGold, townData.day, Characters, CharactersOnLastMission,
    //         townData.wasLastMissionSuccessful, false, townData.singlePlayer, townData.selectedMission, townData.townHall, RCcharacters,
    //         selectedEnemies, allowEnemySelection, allowDuplicates, SaveSystem.LoadTownData().teamColor,
    //         townData.slotName, townData.selectedEncounter, townData.pastEncounters, townData.generateNewEncounters, townData.generatedEncounters, townData.gameSettings);
    //     SaveSystem.SaveTownData(data);
    //     SaveSystem.SaveStatistics(statistics);
    //     SaveSystem.SaveStatistics(globalStatistics, true);
    // }
    // public void LoadTownData()
    // {
    //     townData = SaveSystem.LoadTownData();
    //
    //     //difficultyLevel = townData.difficultyLevel;
    //     //townGold = townData.townGold;
    //     //day = townData.day;
    //     Characters.Clear();
    //     CharactersOnLastMission.Clear();
    //     //for (int i = 0; i < townData.characters.Length; i++)
    //     //{
    //     //    GameObject charPrefab = AllAvailableCharacters[townData.characters[i]].prefab;
    //     //    SavedCharacter newCharacter = new SavedCharacter(charPrefab, townData.characterLevels[i], townData.characterXP[i], townData.characterXPToGain[i],
    //     //        townData.isCharacterDead[i], townData.characterNames[i], townData.abilityPointCounts[i], townData.unlockedAbilities[i], townData.characterBlessingStrings[i]);
    //     //    Characters.Add(newCharacter);
    //     //}
    //     townData.characters.ForEach(savableCharacter => Characters.Add(new SavedCharacter(savableCharacter, AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
    //     //if (townData.wereCharactersOnAMission)
    //     //{
    //     //    for (int i = 0; i < townData.charactersOnLastMission.Length; i++)
    //     //    {
    //     //        CharactersOnLastMission.Add(townData.charactersOnLastMission[i]);
    //     //    }
    //     //    wasLastMissionSuccessful = townData.wasLastMissionSuccessful;
    //     //}
    //     CharactersOnLastMission = new List<int>(townData.charactersOnLastMission);
    //     //singlePlayer = townData.singlePlayer;
    //     //townHall = townData.townHall;
    //     //RC
    //     if (SceneManager.GetActiveScene().name == "Town" && !townData.createNewRCcharacters && !townData.newGame)
    //     {
    //         //Debug.Log("Loading RC data");
    //         List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
    //         //for (int i = 0; i < townData.RCcharacters.Length; i++)
    //         //{
    //         //    SavedCharacter newCharacter = AllAvailableCharacters[townData.RCcharacters[i]];
    //         //    newCharacter.characterName = townData.RCcharacterNames[i];
    //         //    newCharacter.level = townData.RCcharacterLevels[i];
    //         //    newCharacter.XP = 0;
    //         //    newCharacter.abilityPointCount = newCharacter.level;
    //         //    newCharacter.unlockedAbilities = "0000";
    //         //    RCcharacters.Add(newCharacter);
    //         //}
    //         townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
    //         GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = RCcharacters;
    //     }
    //     else if (SceneManager.GetActiveScene().name == "Town" && (townData.createNewRCcharacters || townData.newGame))
    //     {
    //         GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = null;
    //     }
    //     globalStatistics = SaveSystem.LoadStatistics(true);
    //     statistics = SaveSystem.LoadStatistics();
    //     //selectedMission = townData.selectedMission;
    // }
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
    
    // public void DisableHelpTables()
    // {
    //     for (int i = 0; i < GameObject.Find("Canvas").transform.Find("HelpTables").transform.childCount; i++)
    //     {
    //         Destroy(GameObject.Find("Canvas").transform.Find("HelpTables").transform.GetChild(i).gameObject);
    //     }
    //     for (int i = 0; i < GameObject.Find("Canvas").transform.Find("CharacterTable").Find("Abilities").transform.childCount; i++)
    //     {
    //         if (GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).gameObject.activeSelf
    //             && GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).transform.Find("ActionButtonFrame").GetComponent<Animator>().isActiveAndEnabled)
    //         {
    //             GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", false);
    //         }
    //     }
    // }
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
    //public void AddCharacter()
    //{
    //    GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().AddCharacterInSP(Characters[currentCharacterIndex].prefab, currentCharacterIndex);
    //}
    //public void AutoFill()//sita irgi vps bus galima istrint jei nenaudosim to seno character select
    //{
    //    GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().ClearPortraits();
    //    if (SceneManager.GetActiveScene().name == "CharacterSelect3")
    //    {
    //        /*
    //        for (int i = 0; i < 3; i++)
    //        {
    //            GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().AddCharacterInCS3(Characters[i].prefab, i);
    //            GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(i).transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
    //        }
    //        */
    //    }
    //    else
    //    {
    //        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().AddCharacterInSP(Characters[0].prefab, 0);
    //        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().AddCharacterInSP(Characters[1].prefab, 1);
    //        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().AddCharacterInSP(Characters[2].prefab, 2);
    //    }
    //}
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
                    characterInfo.CornerUIManager.GetComponent<ButtonManager>().GenerateAbilities();//SavedCharacter implementation
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
                //Blessings
                foreach (var blessing in character.GetComponent<AIBehaviour>().specialBlessings)
                {
                    if (blessing.difficultyLevel <= _data.townData.selectedEncounter.encounterLevel)
                    {
                        character.GetComponent<PlayerInformation>().BlessingsAndCurses.Add(new Blessing(blessing.blessingName, 0, "", "", "", ""));
                    }
                }
                character.GetComponent<PlayerInformation>().CornerUIManager.GetComponent<ButtonManager>().GenerateAbilitiesForEnemy(abilitiesToEnable);
            }
        }
        print("saved characters set");
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
