using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitButton : MonoBehaviour
{
    public SavedCharacter character;
    private int XPToLevelUp;

    public void UpdateRecruitButton()
    {
        if (character == null)
        {
            transform.Find("CharacterTable").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("CharacterTable").gameObject.SetActive(true);
            var charInformation = character.prefab.GetComponent<PlayerInformation>();
            transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().text = charInformation.ClassName;
            transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().color = charInformation.ClassColor;
            transform.Find("CharacterTable").Find("Portrait").GetComponent<Image>().sprite = charInformation.CharacterPortraitSprite;
            transform.Find("CharacterTable").Find("Cost").GetComponent<Text>().text = character.cost.ToString() + "g";
            if (GameObject.Find("GameProgress").GetComponent<GameProgress>().townData.townGold >= character.cost && GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters.Count < GameObject.Find("GameProgress").GetComponent<GameProgress>().maxCharacterCount)
            {
                transform.Find("CharacterTable").Find("BuyButton").GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.Find("CharacterTable").Find("BuyButton").GetComponent<Button>().interactable = false;
            }
        }
    }
    public void UpdateXPButton()
    {
        if (character == null)
        {
            transform.Find("CharacterTable").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("CharacterTable").gameObject.SetActive(true);
            var charInformation = character.prefab.GetComponent<PlayerInformation>();
            transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().text = character.characterName; //charInformation.ClassName;
            transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().color = charInformation.ClassColor;
            transform.Find("CharacterTable").Find("Portrait").GetComponent<Image>().sprite = charInformation.CharacterPortraitSprite;
            XPToLevelUp = GameObject.Find("GameProgress").GetComponent<GameProgress>().XPToLevelUp[character.level - 1];
            transform.Find("CharacterTable").Find("XP").GetComponent<Text>().text = character.xP + "/" + XPToLevelUp + " XP";
            if (character.level  >= XPProgressManager.currentMaxLevel())
                transform.Find("CharacterTable").Find("XP").GetComponent<Text>().text = "MAX LEVEL";
                transform.Find("CharacterTable").Find("LevelText").GetComponent<Text>().text = character.level.ToString();
                transform.Find("CharacterTable").Find("XPToGain").GetComponent<Text>().text = "+" + character.xPToGain.ToString() + " XP";

            if (character.dead)
            {
                transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().color = Color.gray;
                transform.Find("CharacterTable").Find("XPToGain").GetComponent<Text>().text = "DEAD";
                transform.Find("CharacterTable").Find("XPToGain").GetComponent<Text>().color = Color.gray;
                transform.Find("CharacterTable").Find("XP").GetComponent<Text>().color = Color.gray;
            }

        }
    }

    public void GrowXP(int XPToGrow)
    {
        if(character != null && character.xPToGain > 0)
        {
            if (character.level >= XPProgressManager.currentMaxLevel())
            {
                character.xPToGain = 0;
            }
            if (character.xPToGain < XPToGrow)
            {
                XPToGrow = character.xPToGain;
            }
            character.xP += XPToGrow;
            character.xPToGain -= XPToGrow;
            if(character.xP >= XPToLevelUp)
            {
                character.level++;
                if(character.level != 4/*XPProgressManager.currentMaxLevel()*/)
                {
                    character.abilityPointCount++;
                }
                if (character.level >= XPProgressManager.currentMaxLevel())
                {
                    character.xPToGain = 0;
                    character.xP = 0;
                }
                else
                {
                    character.xP = character.xP - XPToLevelUp;
                }
                XPToLevelUp = GameObject.Find("GameProgress").GetComponent<GameProgress>().XPToLevelUp[character.level - 1];
                //kazkokia animacija
            }
            transform.Find("CharacterTable").Find("XP").GetComponent<Text>().text = character.xP + "/" + XPToLevelUp + " XP";
            if (character.level >= XPProgressManager.currentMaxLevel())
                transform.Find("CharacterTable").Find("XP").GetComponent<Text>().text = "MAX LEVEL";
            transform.Find("CharacterTable").Find("LevelText").GetComponent<Text>().text = character.level.ToString();
        }
    }

    public void BuyCharacter()
    {
        if (GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters.Count < GameObject.Find("GameProgress").GetComponent<GameProgress>().maxCharacterCount)
        {
            GameObject.Find("GameProgress").GetComponent<GameProgress>().BuyCharacter(character);
            transform.parent.parent.gameObject.GetComponent<Recruitment>().CharactersInShop.Remove(character);
            transform.parent.parent.gameObject.GetComponent<Recruitment>().UpdateButtons();
        }
        else Debug.Log("Ziurek ka darai, kvaily!");
    }
    //private int currentMaxLevel()
    //{
    //    int MaxLevel = 2;
    //    char townHallChar = GameObject.Find("GameProgress").GetComponent<GameProgress>().townData.townHall[2];
    //    if(townHallChar == '0')
    //    {
    //        MaxLevel = 2;
    //    }
    //    if (townHallChar == '1')
    //    {
    //        MaxLevel = 3;
    //    }
    //    if (townHallChar == '2')
    //    {
    //        MaxLevel = 4;
    //    }
    //    /* if (townHallChar == '3')
    //     {
    //     }*/
    //    return MaxLevel;
    //}
}
