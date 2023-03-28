using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CSManager : MonoBehaviour
{
    public GameObject TeamPortraitGameObject;
    public GameObject TeamPortraitManager;
    public TeamsList allCharacterList;
    private int i = 0;

    void Awake()
    {
            DontDestroyOnLoad(transform.gameObject);
    }
    /*void OnLevelWasLoaded()
    {
        if (GameObject.Find("GameInformation") != null)
        {
            for (int j = 0; j < allCharacterList.teams.Count; j++)
            {
                for (int k = 0; k < allCharacterList.teams[j].characters.Count; k++)
                {
                    GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[j].characters[k] = allCharacterList.teams[j].characters[k];
                }
            }
        }
    }*/ //apacioj tipo naujesnis budas
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            Destroy(gameObject);
        }
        if (GameObject.Find("GameInformation") != null)
        {
            for (int j = 0; j < allCharacterList.teams.Count; j++)
            {
                for (int k = 0; k < allCharacterList.teams[j].characters.Count; k++)
                {
                    GameObject.Find("GameInformation").GetComponent<PlayerTeams>().allCharacterList.teams[j].characters[k] = allCharacterList.teams[j].characters[k];
                }
            }
        }
    }

    public void AddTeamToList()
    {
        if (isWholeTeamChosen() && allCharacterList.teams.Count>i)
        {
            allCharacterList.teams[i].characters = new List<GameObject>();
            for(int j=0;j< TeamPortraitManager.GetComponent<CSTeamPortraitManager>().PortraitButtonList.Count; j++)
            {
                allCharacterList.teams[i].characters.Add(TeamPortraitManager.GetComponent<CSTeamPortraitManager>().PortraitButtonList[j].CharacterPrefab);
            }
            i++;
            if (i != allCharacterList.teams.Count)
            {
                GameObject.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>().ClearPortraits();
            }
        }
        if (i == allCharacterList.teams.Count)
        {
            SceneManager.LoadScene("2PlayerBattleGround", LoadSceneMode.Single);
        }
    }

    private bool isWholeTeamChosen()
    {
        if(TeamPortraitManager.GetComponent<CSTeamPortraitManager>().FindFirstUnoccupied() == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Update()
    {
        if(i<allCharacterList.teams.Count && TeamPortraitGameObject!=null)
        {
            //TeamPortraitGameObject.GetComponent<Image>().sprite = allCharacterList.teams[i].teamPortraitBoxSprite;
        }
    }
}
