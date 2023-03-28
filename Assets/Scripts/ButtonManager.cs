using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public List<GameObject> ButtonList;
    [HideInInspector] public List<GameObject> ButtonFrameList;
    private List<GameObject> ButtonIconList;
    [HideInInspector] public GameObject CharacterOnBoard;
    private KeyCode[] AbilityChangingButtonSequence = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };

    void Awake()
    {
        ButtonFrameList = new List<GameObject>();
        ButtonIconList = new List<GameObject>();
        for (int i = 0; i < ButtonList.Count; i++)
        {
            ButtonFrameList.Add(ButtonList[i].transform.Find("ActionButtonFrame").gameObject);
            ButtonIconList.Add(ButtonList[i].transform.Find("ActionButtonImage").gameObject);
            ButtonIconList[i].GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
            var CantAttackIcon = ButtonList[i].transform.Find("CantAttackImage");
            if (CantAttackIcon != null)
            {
                CantAttackIcon.gameObject.GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
            }
        }
        transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject.GetComponent<Text>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
    }
    void Start()
    {
        if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        {
            transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
        }
    }
    void Update()
    {
        if(transform.GetChild(0).gameObject.activeSelf)
        {
            for(int i = 0; i < ButtonList.Count; i++)
            {
                if (Input.GetKeyDown(AbilityChangingButtonSequence[i]))
                {
                    ButtonList[i].GetComponent<ActionButton>().ChangePlayersState();
                }
            }
        }
    }
    public void DisableSelection(GameObject selected)
    {
        for (int i = 0; i < ButtonFrameList.Count; i++)
        {
            if (ButtonFrameList[i] != selected)
            {
                ButtonFrameList[i].GetComponent<Animator>().SetBool("select", false);
            }
        }
        selected.GetComponent<Animator>().SetBool("select", true);
    }

    /*void Update()
    {
        //if (GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == CharacterOnBoard)
    }*/

    public void ChangeCooldownVisuals()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
                if (ButtonList[i].GetComponent<ActionButton>().buttonState != "Movement" &&
                CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState) != null)
                {
                    var buttonActionScript = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
                    if (buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown || buttonActionScript.AvailableAttacks == 0)
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.gray;
                        for(int j = 0; j < ButtonList[i].transform.childCount; j++)
                        {
                            if(ButtonList[i].transform.GetChild(j).name == "CoolDownText" && buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown)
                            {
                                var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.1f);
                            }
                        }
                        /*if (ButtonList[i].transform.Find("CoolDownText") != null
                            && (ButtonList[i].transform.Find("CoolDownText").GetComponent<CooldownText>().action.AbilityCooldown - ButtonList[i].transform.Find("CoolDownText").GetComponent<CooldownText>().action.AbilityPoints) > 0)
                        {
                            var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.1f);
                        }*/
                    }
                    else
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.white;
                        if (ButtonList[i].transform.Find("CoolDownText") != null)
                        {
                            var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        }
                    }
                }

                else if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
                {
                    var characterMovementScript = CharacterOnBoard.GetComponent<GridMovement>();
                    if (characterMovementScript.AvailableMovementPoints == 0)
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.gray;
                        if (ButtonList[i].transform.Find("CoolDownText") != null
                            && (ButtonList[i].transform.Find("CoolDownText").GetComponent<CooldownText>().action.AbilityCooldown - ButtonList[i].transform.Find("CoolDownText").GetComponent<CooldownText>().action.AbilityPoints) > 0)
                        {
                            var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.1f);
                        }
                    }
                    else
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.white;
                        if (ButtonList[i].transform.Find("CoolDownText") != null)
                        {
                            var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        }
                    }
                }
            }
        }
    }
    public void ChangesInCornerUIButtons()// This changes debuff icons and cantattack icons
    {
        if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        {
            transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
            transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().UpdateDebuffs();
        }

        //
        for (int i = 0; i < ButtonList.Count; i++)
        {
            /*var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            if (buttonAction != null && buttonAction.AttackAbility)
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition);
                var actionButtonImage = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                var actionButtonImageBackground = ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>();
                var buttonActionScript = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
                if (CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition)
                {
                    actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 0.1f);
                    actionButtonImageBackground.color = Color.gray;
                }
                else if (!(buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown || buttonActionScript.AvailableAttacks == 0))
                {
                    actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 1f);
                    actionButtonImageBackground.color = Color.white;
                }
                //ButtonList[i].transform.Find("CantAttackImage").GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
            }*/
            ChangeAbilityDisabledConditions();
            //
            var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            var actionButtonImage = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
            var actionButtonImageBackground = ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>();
            if ((buttonAction != null && buttonAction.isDisabled)
                || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().isDisabled))
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(true);
                //
                actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 0.1f);
                actionButtonImageBackground.color = Color.gray;
            }
            else if ((buttonAction != null && buttonAction.canGridBeEnabled())
                || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().canGridBeEnabled()))
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(false);
                actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 1f);
                actionButtonImageBackground.color = Color.white;
            }
            else
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(false);
            }
        }
    }
    public void ChangeAbilityDisabledConditions()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            // veiksmai, ne movement
            if (buttonAction != null && 
                (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                || (buttonAction.AttackAbility && CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition)
                || (CharacterOnBoard.GetComponent<PlayerInformation>().Silenced && !(buttonAction is PlayerAttack))
                || (CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)))
            {

                buttonAction.isDisabled = true;
            }
            else if (buttonAction != null)
            {
                buttonAction.isDisabled = false;

            }
            // movement
            if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement"
                && (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                || CharacterOnBoard.GetComponent<PlayerInformation>().CantMove)
                || CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)
            {
                CharacterOnBoard.GetComponent<GridMovement>().isDisabled = true;
            }
            else if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
            {
                CharacterOnBoard.GetComponent<GridMovement>().isDisabled = false;
            }
        }
    }
    public void GenerateAbilities()
    {
        int currentButtonIndex = 2;
        SavedCharacter character = CharacterOnBoard.GetComponent<PlayerInformation>().savedCharacter;
        if (character != null)
        {
            for (int i = 0; i < character.unlockedAbilities.Length; i++)
            {
                if (ButtonList.Count > currentButtonIndex)
                {
                    if (character.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null && character.unlockedAbilities[i] == '1')
                    {
                        ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
                        ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).actionName;
                        currentButtonIndex++;
                    }
                }
            }
            for (int i = currentButtonIndex; i < ButtonList.Count; i++)
            {
                    ButtonList[i].gameObject.SetActive(false);
                    string extensionName = "Extension" + (i + 1).ToString();
                    transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
        }
    }

    public void GenerateAbilitiesForEnemy(List<string> abilitiesToEnable)
    {
        int currentButtonIndex = 2;
        GameObject character = CharacterOnBoard;
        if (character != null)
        {
            foreach(string ability in abilitiesToEnable)
            {
                if (ButtonList.Count > currentButtonIndex && character.GetComponent<ActionManager>().FindActionListByName(ability) != null)
                {
                    ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().FindActionListByName(ability).AbilityIcon;
                    ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = character.GetComponent<ActionManager>().FindActionListByName(ability).actionName;
                    currentButtonIndex++;
                }
            }
            for (int i = currentButtonIndex; i < ButtonList.Count; i++)
            {
                ButtonList[i].gameObject.SetActive(false);
                string extensionName = "Extension" + (i + 1).ToString();
                transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
        }
    }
}
