using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : PortraitButton
{
    public int characterIndex = 0;
    public CharacterSelect characterSelect;
    public Image portrait;
    public TextMeshProUGUI levelText;
    public Animator onHover;
    public Button selectionButton;
    public CharacterPortrait characterPortrait;
    public CSTeamPortraitManager manager;

    public override void OnPortraitClick()
    {
        characterSelect.OnCharacterButtonClick(characterIndex);
    }
    
    
    
}
