using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes;
using UnityEngine.UI;

public class CooldownText : MonoBehaviour
{
    private string ActionName;
    [HideInInspector]public BaseAction action;

    void Start()
    {
        ActionName = transform.parent.GetComponent<ActionButton>().buttonState;
        action = transform.parent.parent.parent.GetComponent<ButtonManager>().CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ActionName);
    }

    private void OnEnable()
    {
        if (action != null)
        {
            if (action.AbilityCooldown - action.AbilityPoints > 0)
            {
                gameObject.GetComponent<Text>().text = (action.AbilityCooldown - action.AbilityPoints).ToString();
            }
            else gameObject.GetComponent<Text>().text = null;
        }
        else gameObject.GetComponent<Text>().text = null;
    }
    private void Update()
    {
        if (action.AbilityCooldown - action.AbilityPoints > 0)
        {
            gameObject.GetComponent<Text>().text = (action.AbilityCooldown - action.AbilityPoints).ToString();
        }
        else gameObject.GetComponent<Text>().text = "";
    }
}
