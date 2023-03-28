using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToTeamButton : MonoBehaviour
{
    public void PressButton()
    {
        GameObject character = GameObject.Find("GameInformation").GetComponent<GameInformation>().InspectedCharacter;
        if (character != null && character.GetComponent<PlayerInformation>().CharactersTeam == "Default")
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().InspectACharacter(character);
            GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().AddCharacterToCurrentTeam(character);
            OffHover();
        }
    }
    /*void Update()
    {
        GameObject character = GameObject.Find("GameInformation").GetComponent<GameInformation>().InspectedCharacter;
        if (character == null)
        {
            gameObject.SetActive(false);
        }
    }*/
    public void OnHover()
    {
        GameObject.Find("GameInformation").GetComponent<GameInformation>().isBoardDisabled = true;
    }
    public void OffHover()
    {
        GameObject.Find("GameInformation").GetComponent<GameInformation>().isBoardDisabled = false;

    }
}
