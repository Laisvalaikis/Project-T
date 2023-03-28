using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string buttonState;
    private GameInformation gameInformation;

    void Start()
    {
        gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
    }

    public void ChangePlayersState()
    {
        var buttonManager = transform.parent.parent.GetComponent<ButtonManager>();
        transform.GetChild(0).GetComponent<Animator>().SetBool("select", true);

        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        character.GetComponent<PlayerInformation>().currentState = buttonState;
        if (buttonState == "Movement")
        {
            gameInformation.DisableGrids();;
            character.GetComponent<GridMovement>().EnableGrid();
        }
        else //galima prideti else if jei kazkokie jau special abilities.
        {
            gameInformation.DisableGrids();
            if (character.GetComponent<ActionManager>().FindActionByName(buttonState) != null)
            {
                character.GetComponent<ActionManager>().FindActionByName(buttonState).EnableGrid();
            }
        }
        buttonManager.DisableSelection(transform.Find("ActionButtonFrame").gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            gameInformation.GetComponent<HelpTableController>().EnableTableForInGameRightClick(buttonState);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameInformation.GetComponent<HelpTableController>().hasActionButtonBeenEntered = true;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", true);
        gameInformation.isBoardDisabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameInformation.GetComponent<HelpTableController>().hasActionButtonBeenEntered = false;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", false);
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = gameInformation.helpTableOpen;
    }
}
