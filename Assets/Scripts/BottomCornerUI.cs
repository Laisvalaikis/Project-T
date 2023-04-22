using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomCornerUI : MonoBehaviour
{
    public Color ButtonIconColor;
    //public GameObject characterPortrait;
    private GameObject GameInformation;
    private bool ActivateOnce = true;
    private GameObject selectedCharacter;
    private GameObject currentCharacter;
    private GameObject movementText;
    private GameObject healthText;
    public GameObject buttonPrefab;
    public List<TextMeshProUGUI> buttonText;
    public List<ActionButton> buttonAction;
    public List<Image> buttonImages;
    public List<Image> buttonBackgroundImages;
    public List<Image> backgroundImages;
    public Image staminaBackground;
    public Image portrait;
    public TextMeshProUGUI staminaPoints;
    public TextMeshProUGUI healthPoints;
    public Animator healthAnimator;
    public CharacterUiData characterUiData;
    public GameObject cornerUi;
    private ButtonManager _buttonManager;
    void Awake()
    {
        // GameInformation = GameObject.Find("GameInformation").gameObject;
        // movementText = transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject;
        // healthText = transform.GetChild(0).Find("HealthBar").Find("HealthText").gameObject;
        _buttonManager = GetComponent<ButtonManager>();
    }

    /*void Update()
    {
        ChangesInCornerUI();
    }*/

    // public void ChangesInCornerUI()
    // {
    //     if (GameInformation.GetComponent<GameInformation>().SelectedCharacter != null)
    //     {
    //         currentCharacter = GameInformation.GetComponent<GameInformation>().SelectedCharacter;
    //     }
    //     else if (GameInformation.GetComponent<GameInformation>().InspectedCharacter != null)
    //     {
    //         currentCharacter = GameInformation.GetComponent<GameInformation>().InspectedCharacter;
    //     }
    //     else
    //     {
    //         currentCharacter = null;
    //     }
    //     if (currentCharacter != null &&
    //         currentCharacter.GetComponent<PlayerInformation>().characterUiData == characterUiData)
    //     {
    //         this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    //         /*characterPortrait.GetComponent<Image>().sprite = GameInformation.GetComponent<GameInformation>()
    //             .SelectedCharacter.GetComponent<PlayerInformation>().CharacterPortraitSprite;*/
    //         movementText.gameObject.GetComponent<Text>().text =
    //             currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text
    //         healthText.gameObject.GetComponent<Text>().text =
    //             currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
    //         transform.GetChild(0).Find("HealthBar").Find("Health").GetComponent<Animator>().SetFloat("healthPercent",
    //             currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage()); //Sets float of animator
    //         //galima tiesiog zinot sito UI owneri (characteri)
    //         if (ActivateOnce || selectedCharacter != currentCharacter)
    //         {
    //             this.GetComponent<ButtonManager>().DisableSelection(this.GetComponent<ButtonManager>().ButtonFrameList[0]);
    //             ActivateOnce = false;
    //             selectedCharacter = currentCharacter;
    //         }
    //     }
    //     else
    //     {
    //         this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    //         ActivateOnce = true;
    //     }
    // }

    public void UpdateData()
    {
        if (_buttonManager.CharacterOnBoard != null)
        {
            cornerUi.SetActive(true);
        }
        else
        {
            cornerUi.SetActive(false);
        }

        portrait.sprite = characterUiData.characterSprite;
        Color half = characterUiData.backgroundColor;
        half.a = 0.5f;
        staminaBackground.color = half;
        for (int i = 0; i < characterUiData.abilities.Count; i++)
        {
            buttonText[i].color = characterUiData.textColor;
            buttonImages[i].sprite = characterUiData.abilities[i].sprite;
            buttonAction[i].buttonState = characterUiData.abilities[i].abilityAction.ToString();
            buttonBackgroundImages[i].color = characterUiData.backgroundColor;
            if (i < backgroundImages.Count)
            {
                backgroundImages[i].color = characterUiData.backgroundColor;
            }
        }
        
    }
    
    public void UpdateData(int buttonIndex, int index)
    {
        if (_buttonManager.CharacterOnBoard != null)
        {
            cornerUi.SetActive(true);
        }
        else
        {
            cornerUi.SetActive(false);
        }

        portrait.sprite = characterUiData.characterSprite;
        Color half = characterUiData.backgroundColor;
        half.a = 0.5f;
        staminaBackground.color = half;
        buttonText[buttonIndex].color = characterUiData.textColor;
        buttonImages[buttonIndex].sprite = characterUiData.abilities[index].sprite;
        buttonAction[buttonIndex].buttonState = characterUiData.abilities[index].abilityAction.ToString();
        buttonBackgroundImages[buttonIndex].color = characterUiData.backgroundColor;
        for (int i = 0; i < backgroundImages.Count; i++)
        {
            backgroundImages[i].color = characterUiData.backgroundColor;
        }
        
        currentCharacter = _buttonManager.CharacterOnBoard;
        staminaPoints.text = currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text
        healthPoints.text = currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
        healthAnimator.SetFloat("healthPercent", currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage());
    }

    public void EnableAbilities(SavedCharacter savedCharacter)
    {
        int currentButtonIndex = 2;
        int buttonIndex = 0;
        if (savedCharacter != null)
        {
            for (int i = 0; i < savedCharacter.unlockedAbilities.Length; i++)
            {
                if (_buttonManager.ButtonList.Count > currentButtonIndex)
                {
                    if (savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null && savedCharacter.unlockedAbilities[i] == '1')
                    {
                        _buttonManager.ButtonList[currentButtonIndex].transform.parent.gameObject.SetActive(true);
                        UpdateData(buttonIndex, i);
                        // ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
                        // ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i).actionName;
                        currentButtonIndex++;
                        buttonIndex++;
                    }
                }
            }
            for (int i = currentButtonIndex; i < _buttonManager.ButtonList.Count; i++)
            {
                _buttonManager.ButtonList[i].transform.parent.gameObject.SetActive(false);
                // string extensionName = "Extension" + (i + 1).ToString();
                // transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
        }
    }
}
