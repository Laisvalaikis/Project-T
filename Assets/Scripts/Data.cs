using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;

public class Data : MonoBehaviour
{
    
    public List<SavedCharacter> Characters;
    public List<SavedCharacter> AllAvailableCharacters;
    public List<GameObject> AllEnemyCharacterPrefabs;
    public List<SavedCharacter> AllEnemySavedCharacters;
    [HideInInspector] public List<int> CharactersOnLastMission;
    //[HideInInspector] public bool wasLastMissionSuccessful;
    public TownData newGameData;
    [HideInInspector] public bool canButtonsBeClicked = true;
    public bool canButtonsBeClickedState = true;
    public List<int> XPToLevelUp;
    public bool isCurrentScenePlayableMap = false;
    [HideInInspector] public bool switchPortraits;
    [HideInInspector] public List<int> SwitchedCharacters;
    [HideInInspector] public int currentCharacterIndex = -1;
    public int maxCharacterCount;
    [HideInInspector] public bool createNewRCcharacters = false;
    public List<int> selectedEnemies;
    [HideInInspector] public Statistics statistics;
    [HideInInspector] public Statistics globalStatistics;
    /*[HideInInspector]*/
    public TownData townData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
