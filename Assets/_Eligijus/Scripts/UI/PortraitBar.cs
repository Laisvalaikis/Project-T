using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PortraitBar : MonoBehaviour
{
    public List<CharacterPortrait> townPortraits;
    
    public List<Animator> buttonOnHover;
    
    public Button up;

    public Button down;

    public Data _data;

    private int _lastElement = -1; // array starts from zero 

    private List<SavedCharacter> _currentCharacters;

    private int _scrollCharacterSelectIndex;

    // Start is called before the first frame update
    void Start()
    {
        _currentCharacters = _data.Characters;
        _scrollCharacterSelectIndex = 0;
        SetupCharacters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCharacters()
    {
        if (true) // up
        {
            
        }
        else if (false) // down
        {
            
        }
    }

    public void SetupCharacters()
    {
        if (_scrollCharacterSelectIndex + townPortraits.Count < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
        for (int i = 0; i < _currentCharacters.Count; i++)
        {
            if (i < townPortraits.Count)
            {
                townPortraits[i].gameObject.SetActive(true);
                townPortraits[i].characterIndex = i;
                townPortraits[i].characterImage.sprite = _currentCharacters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
                townPortraits[i].levelText.text = _currentCharacters[i].level.ToString();
                if (_currentCharacters[i].abilityPointCount > 0)
                {
                    townPortraits[i].abilityPointCorner.SetActive(true);
                }
                else
                {
                    townPortraits[i].abilityPointCorner.SetActive(false); 
                }

                _lastElement = i;
                // townPortraits[i] = _currentCharacters[i].
            }
            else
            {
                break;
            }
        }
    }

    public void InsertCharacter()
    {
        int index = _lastElement + 1;
        if (index < _currentCharacters.Count && index < townPortraits.Count)
        {
            townPortraits[index].gameObject.SetActive(true);
            townPortraits[index].characterIndex = index;
            townPortraits[index].characterImage.sprite = _currentCharacters[index].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            townPortraits[index].levelText.text = _currentCharacters[index].level.ToString();
            if (_currentCharacters[index].abilityPointCount > 0)
            {
                townPortraits[index].abilityPointCorner.SetActive(true);
            }
            else
            {
                townPortraits[index].abilityPointCorner.SetActive(false); 
            }

            _lastElement = index;
            
        }
        else if (_scrollCharacterSelectIndex + townPortraits.Count < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
    }

    public void RemoveCharacter(int characterIndex)
    {
        int index = characterIndex-_scrollCharacterSelectIndex;
        Debug.Log(characterIndex);
        Debug.Log(index);
        CharacterPortrait characterPortrait = townPortraits[index];
        Animator animator = buttonOnHover[index];
        townPortraits.RemoveAt(index);
        buttonOnHover.RemoveAt(index);
        for (int i = index, count = characterIndex; i < townPortraits.Count; i++, count++)
        {
            townPortraits[i].characterIndex = count;
        }
        int siblingIndex = townPortraits[townPortraits.Count - 1].transform.GetSiblingIndex();
        characterPortrait.transform.SetSiblingIndex(siblingIndex);
        characterPortrait.gameObject.SetActive(false);
        townPortraits.Add(characterPortrait);
        buttonOnHover.Add(animator);
        if ( townPortraits.Count >= _currentCharacters.Count)
        {
            Scroll(-1);
        }

        _lastElement--;
        
    }

    public void Scroll(int direction) // up - (-1), down - (1)
    {
        int scrollCalculation = _scrollCharacterSelectIndex + townPortraits.Count * direction;
        if (townPortraits.Count <= _currentCharacters.Count && scrollCalculation >= 0 && scrollCalculation < _currentCharacters.Count)
        {
            _scrollCharacterSelectIndex += townPortraits.Count * direction;
            int count = _currentCharacters.Count - _scrollCharacterSelectIndex;
            for (int i = 0; i < townPortraits.Count; i++)
            {
                if (i < count)
                {
                    int index = i + _scrollCharacterSelectIndex;
                    townPortraits[i].gameObject.SetActive(true);
                    townPortraits[i].characterIndex = index;
                    townPortraits[i].characterImage.sprite = _currentCharacters[index].prefab
                        .GetComponent<PlayerInformation>().CharacterPortraitSprite;
                    townPortraits[i].levelText.text = _currentCharacters[index].level.ToString();
                    if (_currentCharacters[index].abilityPointCount > 0)
                    {
                        townPortraits[i].abilityPointCorner.SetActive(true);
                    }
                    else
                    {
                        townPortraits[i].abilityPointCorner.SetActive(false);
                    }
                }
                else
                {
                    townPortraits[i].gameObject.SetActive(false);
                }
            }

            _lastElement = _scrollCharacterSelectIndex + count;
        }

        
        
        if(scrollCalculation <= 0)
        {
            up.gameObject.SetActive(false);
            
        }
        else if (_currentCharacters.Count > townPortraits.Count)
        {
            up.gameObject.SetActive(true);
        }

        if (_currentCharacters.Count > townPortraits.Count && scrollCalculation + townPortraits.Count * direction < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
        else if (scrollCalculation <= _currentCharacters.Count)
        {
            down.gameObject.SetActive(false); 
        }
        else if (scrollCalculation >= _currentCharacters.Count)
        {
            down.gameObject.SetActive(false); 
        }

    }

    public void OpenCharacterCharacterTable()
    {
        
    }

    // public void UpdateCharacterBar(int direction = 0)//up - 1, down - -1, none - 0
    // {
    //     Transform PortraitBarButtons = GameObject.Find("CanvasCamera").transform.Find("PortraitBar").Find("CharacterButtons");
    //     // Perdaryti
    //     // if (direction == 0)
    //     // {
    //     //     if (PortraitBarButtons.GetChild(0).GetComponent<TownPortrait>().characterIndex == 6)
    //     //     {
    //     //         direction = -1;
    //     //     }
    //     //     else direction = 1;
    //     // }
    //     for (int i = 0; i < townPortraits.Count; i++)
    //     {
    //         townPortraits[i].gameObject.SetActive(false);
    //         townPortraits[i].GetComponent<Image>().sprite = null;
    //         // PortraitBarButtons.GetChild(i).gameObject.SetActive(false);
    //         // PortraitBarButtons.GetChild(i).Find("Character").Find("Portrait").gameObject.SetActive(false);
    //         // PortraitBarButtons.GetChild(i).Find("Character").Find("Portrait").GetComponent<Image>().sprite = null;
    //     }
    //     int start = 0;
    //     int finish = 6;
    //     if (direction == -1)
    //     {
    //         GameObject.Find("CanvasCamera").transform.Find("Down").GetComponent<Button>().interactable = false;
    //         GameObject.Find("CanvasCamera").transform.Find("Up").GetComponent<Button>().interactable = true;
    //         start = 6;
    //         finish = Characters.Count;
    //         if (Characters.Count != 6 && Characters.Count != 12)
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (Characters.Count % 6 - 1) * 68, 0);
    //         }
    //         else
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (6 - 1) * 68, 0);
    //         }
    //     }
    //     else if (direction == 1)
    //     {
    //         GameObject.Find("CanvasCamera").transform.Find("Down").GetComponent<Button>().interactable = Characters.Count > 6;
    //         GameObject.Find("CanvasCamera").transform.Find("Up").GetComponent<Button>().interactable = false;
    //         if (Characters.Count < 6)
    //         {
    //             finish = Characters.Count;
    //         }
    //         else finish = 6;
    //         if (Characters.Count != 6)
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (finish - 1) * 68, 0);
    //         }
    //         else
    //         {
    //             PortraitBarButtons.localPosition = new Vector3(-125, -340 + (6 - 1) * 68, 0);
    //         }
    //     }
    //     THISSS UPDATES SOMETHING
    //     for (int i = start; i < finish; i++)
    //     {
    //         PortraitBarButtons.GetChild(i % 6).gameObject.SetActive(true);
    //         PortraitBarButtons.GetChild(i % 6).GetComponent<TownPortrait>().characterIndex = i;
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("Portrait").gameObject.SetActive(true);
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("Portrait").GetComponent<Image>().sprite =
    //             Characters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
    //         PortraitBarButtons.GetChild(i % 6).Find("Character").Find("LevelText").GetComponent<Text>().text = Characters[i].level.ToString();
    //         PortraitBarButtons.GetChild(i % 6).Find("AbilityPointCorner").gameObject.SetActive(Characters[i].abilityPointCount > 0);
    //     }
    //     GetComponent<Town>()?.ToggleAbilityPointWarning();
    // }
}
