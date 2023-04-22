using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes;
using TMPro;
using UnityEngine.UI;

public class CooldownText : MonoBehaviour
{
    public TextMeshProUGUI coolDown;
    public ActionButton _actionButton;
    [HideInInspector]public BaseAction action;
    private ButtonManager _buttonManager;
    private string actionName;
    private ActionManager _actionManager;
    

    void Start()
    {
        actionName = _actionButton.buttonState;
        _buttonManager = _actionButton.buttonManager;
        if (_buttonManager.CharacterOnBoard != null)
        {
            _actionManager = _buttonManager.CharacterOnBoard.GetComponent<ActionManager>();
            action = _actionManager.FindActionByName(actionName);
        }
        
    }

    private void OnEnable()
    {
        if (action != null)
        {
            if (action.AbilityCooldown - action.AbilityPoints > 0)
            {
                coolDown.text = (action.AbilityCooldown - action.AbilityPoints).ToString();
            }
            else coolDown.text = null;
        }
        else coolDown.text = null;
    }
    private void Update()
    {
        actionName = _actionButton.buttonState;
        _actionManager = _buttonManager.CharacterOnBoard.GetComponent<ActionManager>();
        action = _actionManager.FindActionByName(actionName);
        if (action != null && action.AbilityCooldown - action.AbilityPoints > 0)
        {
            coolDown.text = (action.AbilityCooldown - action.AbilityPoints).ToString();
        }
        else coolDown.text = "";
    }
}
