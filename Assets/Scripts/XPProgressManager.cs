using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class XPProgressManager : MonoBehaviour
{
    public int GoldToAdd = 1400;
    private bool XPGrow = true;
    private int counter = 0;
    private bool hasXPGrowthEnded = false;
    public List<Blessing> BlessingList = new List<Blessing>();
    public List<GeneratedBlessing> GeneratedBlessingList = new List<GeneratedBlessing>();
    public TextAsset blessingsFile;


    //BLESSINGS:
    [System.Serializable]
    public class GeneratedBlessing
    {
        public Blessing blessing;
        public SavedCharacter character;

        public GeneratedBlessing(Blessing blessing, SavedCharacter character)
        {
            this.blessing = blessing;
            this.character = character;
        }
    }
    public void RerollBlessings()
    {
        MakeBlessingList(BlessingList);
        GenerateBlessings(0);
        SetupBlessingCards();
    }
    private void MakeBlessingList(List<Blessing> blessingList)
    {
        blessingList.Clear();
        using (var file = new StringReader(blessingsFile.text))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(';');
                string blessingName = parts[0];
                int rarity = int.Parse(parts[1]);
                string className = parts[2];
                string spellName = parts[3];
                string condition = parts[4];
                string description = parts[5];
                Blessing blessingToAdd = new Blessing(blessingName, rarity, className, spellName, condition, description);
                blessingList.Add(blessingToAdd);
                //Debug.Log(blessingName);
            }
        }
    }
    private void GenerateBlessings(int rarityLevel)
    {
        GeneratedBlessingList.Clear();
        //su while;
        for (int i = 0; i < 3; i++)
        {
            bool wasTargetFound = false;
            while (!wasTargetFound)
            {
                if (BlessingList.Count < 1)
                {
                    wasTargetFound = true;
                    break;
                }
                //check rarity
                int randomBlessingIndex = Random.Range(0, BlessingList.Count);
                Blessing randomBlessing = BlessingList[randomBlessingIndex];
                List<SavedCharacter> BlessingTargets = PossibleBlessingTargets(randomBlessing);
                //jeigu ne tuscias
                if (BlessingTargets.Count > 0)
                {
                    wasTargetFound = true;
                    int randomTargetIndex = Random.Range(0, BlessingTargets.Count);
                    SavedCharacter randomBlessingTarget = BlessingTargets[randomTargetIndex];
                    GeneratedBlessing blessingToAdd = new GeneratedBlessing(randomBlessing, randomBlessingTarget);
                    GeneratedBlessingList.Add(blessingToAdd);
                }
                else
                {
                    //Debug.Log(randomBlessing.blessingName);
                    BlessingList.Remove(randomBlessing);
                }
                
            }
        }

    }
    private void SetupBlessingCards()
    {
        var blessingCards = GameObject.Find("CanvasCamera").transform.Find("Blessings");
        for(int i = 0; i< blessingCards.childCount; i++)
        {
            if(i < GeneratedBlessingList.Count)
            {
                blessingCards.GetChild(i).gameObject.SetActive(true);
                var card = blessingCards.GetChild(i);
                card.Find("Highlights").gameObject.GetComponent<Image>().color = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformation>().ClassColor2;
                card.Find("Portrait").gameObject.GetComponent<Image>().sprite = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
                //icon
                card.Find("BlessingIcon").gameObject.GetComponent<Image>().color = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformation>().ClassColor;
                card.Find("CharacterName").gameObject.GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].character.characterName;
                card.Find("BlessingName").gameObject.GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].blessing.blessingName;
                card.Find("BlessingDescription").gameObject.GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].blessing.description;
            }
            //Out of bound of generated blessings
            else
            {
                blessingCards.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private List<SavedCharacter> PossibleBlessingTargets(Blessing blessing)
    {
        List<SavedCharacter> targets = new List<SavedCharacter>();
        //assign characters who were on mission to list  
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        var gameProgressCharacters = gameProgress.Characters;
        var onLastMissionCharacterIndexes = gameProgress.CharactersOnLastMission;
        for(int i = 0; i < onLastMissionCharacterIndexes.Count; i++)
        {
            targets.Add(gameProgressCharacters[onLastMissionCharacterIndexes[i]]);
        }
        //ar nera tarp jau sugeneruotu, kad nerodytu dvieju tu paciu blessing+veikejas
        foreach(GeneratedBlessing x in GeneratedBlessingList)
        {
            //jeigu blessing yra to pacio tipo kaip ir jau sugeneruota, tai negeneruos tu paciu kaip ir ta sugeneruota
            if (x.blessing == blessing && targets.Contains(x.character))
            {
                targets.Remove(x.character);
            }
        }
        //ar neturi tie like veikejai sito blessingo
        List<SavedCharacter> targetsCopy = new List<SavedCharacter>(targets);
        foreach (SavedCharacter x in targetsCopy)
        {
            if (x.blessings.Find(x => x.blessingName == blessing.blessingName) != null || (x.dead && blessing.condition!="Dead"))
            {
                targets.Remove(x);
            }         
        }
        //ar atitinka conditionus
        targetsCopy = new List<SavedCharacter>(targets);
        if (blessing.className != "-")
        {
            foreach(SavedCharacter x in targetsCopy)
            {
                //if meets class requirement
                if ((x.prefab.GetComponent<PlayerInformation>().ClassName).ToUpper() != (blessing.className).ToUpper())
                {
                    targets.Remove(x);
                }
                //else check if has that spell
                else if (x.prefab.GetComponent<ActionManager>().ActionScripts.Find(y => y.actionName == blessing.spellName) != null)
                {
                    int spellIndex = x.prefab.GetComponent<ActionManager>().ActionScripts.Find(y => y.actionName == blessing.spellName).AbilityIndex;
                    if(x.unlockedAbilities[spellIndex] == '0')
                    {
                        targets.Remove(x);
                    }
                }
                
            }
        }
        if (blessing.condition == "Dead")
        {
            foreach (SavedCharacter x in targetsCopy)
            {
                if (!x.dead)
                {
                    targets.Remove(x);
                }
            }
        }
        return targets;
    }
    public void ClaimBlessing(int blessingIndex)
    {
        if(GeneratedBlessingList[blessingIndex].blessing.blessingName == "Revive")
        {
            GeneratedBlessingList[blessingIndex].character.dead = false;
        }
        else
        {
            GeneratedBlessingList[blessingIndex].character.blessings.Add(GeneratedBlessingList[blessingIndex].blessing);
        }
        ChangeScene("Town");
    }

    //XP:
    void Start()
    {
        MakeBlessingList(BlessingList);
        //transform.Find("ContinueButton").transform.Find("Text").GetComponent<Text>().text = "SKIP";
        //transform.Find("ContinueButton").GetComponent<Button>().interactable = false;
        GenerateBlessings(0);
        SetupBlessingCards();
    }

    void FixedUpdate()
    {
        if (XPGrow)
        {
            counter++;
            Transform characters = transform.Find("Characters").transform;
            for (int i = 0; i < characters.childCount; i++)
            {
                characters.GetChild(i).gameObject.GetComponent<RecruitButton>().GrowXP((counter / 100) * 5 + 1);
            }
            // ziuri kada ijungt mygtuka
            int k = 0;
            var onLastMissionCharacterIndexes = GameObject.Find("GameProgress").GetComponent<GameProgress>().CharactersOnLastMission;
            for (int i = 0; i < onLastMissionCharacterIndexes.Count; i++)
            {
                if (characters.GetChild(i).gameObject.GetComponent<RecruitButton>().character.xPToGain == 0)
                {
                    k++;
                }
            }
            if(k == onLastMissionCharacterIndexes.Count)
            {
                transform.Find("ContinueButton").transform.Find("Text").GetComponent<Text>().text = "CONTINUE";
                //transform.Find("ContinueButton").GetComponent<Button>().interactable = true;
                hasXPGrowthEnded = true;
            }
        }
    }

    public void UpdateButtons()//reikes pakeist kad rodytu tuos kurie buvo misijoj
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        Transform characters = transform.Find("Characters").transform;
        var gameProgressCharacters = gameProgress.Characters;
        var onLastMissionCharacterIndexes = gameProgress.CharactersOnLastMission;
        for (int i = 0; i < characters.childCount; i++)
        {
            if (i < onLastMissionCharacterIndexes.Count)
            {
                characters.GetChild(i).gameObject.GetComponent<RecruitButton>().character = gameProgressCharacters[onLastMissionCharacterIndexes[i]];
            }
            else
            {
                characters.GetChild(i).gameObject.GetComponent<RecruitButton>().character = null;
            }
            characters.GetChild(i).gameObject.GetComponent<RecruitButton>().UpdateXPButton();
        }
        if (!gameProgress.townData.wasLastMissionSuccessful)
        {
            transform.Find("TitleText").GetComponent<Text>().text = "MISSION FAILED";
            GoldToAdd = 0;
        }
        else
        {
            transform.Find("TitleText").GetComponent<Text>().text = "MISSION SUCCESSFUL";

        }
        transform.Find("GoldWon").GetComponent<Text>().text = "+ " + GoldToAdd + "g";
        gameProgress.townData.townGold += GoldToAdd;
        gameProgress.townData.day++; //Adds day count
    }

    public void ContinueButton()
    {
        if(hasXPGrowthEnded)
        {
            //blessingChance based on encounter
            int randomBlessingNumber = Random.Range(0, 100);
            if (randomBlessingNumber >= 0 && GeneratedBlessingList.Count > 0)//!!!!
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                GameObject.Find("CanvasCamera").transform.Find("Blessings").gameObject.SetActive(true);
                GameObject.Find("CanvasCamera").transform.Find("OtherBlessingObjects").gameObject.SetActive(true);
            }
            else
            {
                ChangeScene("Town");
            }
        }
        else
        {
            SkipXPGrowth();
        }
    }

    private void SkipXPGrowth()
    {
        Transform characters = transform.Find("Characters").transform;
        for (int i = 0; i < characters.childCount; i++)
        {
            var recruitButton = characters.GetChild(i).gameObject.GetComponent<RecruitButton>();
            if(recruitButton.character != null)
                recruitButton.GrowXP(recruitButton.character.xPToGain);
        }
        hasXPGrowthEnded = true;
    }

    public void ChangeScene(string sceneName)
    {
        GameObject.Find("GameProgress").GetComponent<GameProgress>().createNewRCcharacters = true;
        GameObject.Find("GameProgress").GetComponent<GameProgress>().SaveTownData();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public static int currentMaxLevel()
    {
        int MaxLevel = 2;
        char townHallChar = GameObject.Find("GameProgress").GetComponent<GameProgress>().townData.townHall[2];
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
