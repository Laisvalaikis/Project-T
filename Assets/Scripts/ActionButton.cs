using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string buttonState;
    private bool _isSelected = false;
    private GameInformation gameInformation;
    public HelpTableController _helpTableController;
    public GameObject actionButtonFrame;
    public ButtonManager buttonManager;
    private Button _button;
    void Start()
    {
        gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
        _button = GetComponent<Button>();
    }

    public void ChangePlayersState()
    {
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }

        bool interact = false;
        if (buttonState == "Movement")
        {
            BaseAction action = character.GetComponent<GridMovement>();
            if (!action.canGridBeEnabled())
            {
                interact = false;
            }
            else
            {
                interact = true;
                transform.GetChild(0).GetComponent<Animator>().SetBool("select", true);
            }
        }
        else
        {
            if (character.GetComponent<ActionManager>().FindActionByName(buttonState) != null)
            {
                BaseAction action = character.GetComponent<ActionManager>().FindActionByName(buttonState);
                if (!action.canGridBeEnabled())
                {
                    interact = false;
                }
                else
                {
                    interact = true;
                    transform.GetChild(0).GetComponent<Animator>().SetBool("select", true);
 
                }
            }
        }

        if (interact)
        {
            _isSelected = true;
            _helpTableController.helpTable.closeHelpTable();
            character.GetComponent<PlayerInformation>().currentState = buttonState;
            if (buttonState == "Movement")
            {
                BaseAction action = character.GetComponent<GridMovement>();
                if (!action.canGridBeEnabled())
                {
                    _button.interactable = false;
                }
                else
                {
                    _button.interactable = true;
                }

                gameInformation.DisableGrids();
                action.EnableGrid();
            }
            else //galima prideti else if jei kazkokie jau special abilities.
            {
                gameInformation.DisableGrids();
                if (character.GetComponent<ActionManager>().FindActionByName(buttonState) != null)
                {
                    BaseAction action = character.GetComponent<ActionManager>().FindActionByName(buttonState);
                    action.EnableGrid();
                    if (!action.canGridBeEnabled())
                    {
                        _button.interactable = false;
                    }
                    else
                    {
                        _button.interactable = true;
                    }
                }

            }
            buttonManager.DisableSelection(actionButtonFrame);
        }

        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            _helpTableController.EnableTableForInGameRightClick(buttonState);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        _helpTableController.hasActionButtonBeenEntered = true;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", true);
        _helpTableController.EnableTableForInGameRightClick(buttonState);
        gameInformation.isBoardDisabled = true;
        EnableGridPreview(buttonState);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        _helpTableController.hasActionButtonBeenEntered = false;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", false);
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = gameInformation.helpTableOpen;
        _helpTableController.helpTable.closeHelpTable();
        EnableGridPreview(character.GetComponent<PlayerInformation>().currentState);
        
    }

    private void EnableGridPreview(string selectedButtonState)
    {
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        if (selectedButtonState == "Movement")
        {
            gameInformation.DisableGrids();
            character.GetComponent<GridMovement>().EnableGrid();
        }
        else //galima prideti else if jei kazkokie jau special abilities.
        {
            gameInformation.DisableGrids();
            // panaudoti DisableWayTiles()
            if (character.GetComponent<ActionManager>().FindActionByName(selectedButtonState) != null)
            {
                character.GetComponent<ActionManager>().FindActionByName(selectedButtonState).EnableGrid();
            }
            
        }
    }
}
