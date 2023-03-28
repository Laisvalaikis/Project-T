using System.Collections;
using System.Collections.Generic;
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
    void Awake()
    {
        GameInformation = GameObject.Find("GameInformation").gameObject;
        movementText = transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject;
        healthText = transform.GetChild(0).Find("HealthBar").Find("HealthText").gameObject;
    }

    /*void Update()
    {
        ChangesInCornerUI();
    }*/

    public void ChangesInCornerUI()
    {
        if (GameInformation.GetComponent<GameInformation>().SelectedCharacter != null)
        {
            currentCharacter = GameInformation.GetComponent<GameInformation>().SelectedCharacter;
        }
        else if (GameInformation.GetComponent<GameInformation>().InspectedCharacter != null)
        {
            currentCharacter = GameInformation.GetComponent<GameInformation>().InspectedCharacter;
        }
        else
        {
            currentCharacter = null;
        }
        if (currentCharacter != null &&
            currentCharacter.GetComponent<PlayerInformation>().CornerUIManager == transform.gameObject)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            /*characterPortrait.GetComponent<Image>().sprite = GameInformation.GetComponent<GameInformation>()
                .SelectedCharacter.GetComponent<PlayerInformation>().CharacterPortraitSprite;*/
            movementText.gameObject.GetComponent<Text>().text =
                currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text
            healthText.gameObject.GetComponent<Text>().text =
                currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
            transform.GetChild(0).Find("HealthBar").Find("Health").GetComponent<Animator>().SetFloat("healthPercent",
                currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage()); //Sets float of animator
            //galima tiesiog zinot sito UI owneri (characteri)
            if (ActivateOnce || selectedCharacter != currentCharacter)
            {
                this.GetComponent<ButtonManager>().DisableSelection(this.GetComponent<ButtonManager>().ButtonFrameList[0]);
                ActivateOnce = false;
                selectedCharacter = currentCharacter;
            }
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            ActivateOnce = true;
        }
    }
}
