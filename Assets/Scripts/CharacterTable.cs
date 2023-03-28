using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Classes;

public class CharacterTable : MonoBehaviour
{
    [HideInInspector] public int characterIndex;
    [HideInInspector] public string originalName;
    public InputField nameInput;
    public static string allowedCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890 /_<>?!@#$%^&*()-;:";
    private List<int> tempUnlockedAbilities = new List<int>();

    private void Awake()
    {
        ResetTempUnlockedAbilities();
    }

    private void Start()
    {
        nameInput.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(addedChar); };
    }

    private void OnEnable()
    {
        ResetTempUnlockedAbilities();
    }

    private void OnDisable()
    {
        ResetTempUnlockedAbilities();
    }

    public void ResetTempUnlockedAbilities()
    {
        tempUnlockedAbilities = new List<int>();
    }

    public void ChangeCharacterName()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text = transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text.ToUpper();
        gameProgress.Characters[characterIndex].characterName = transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text;
    }

    public void PreventEmptyName()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        while (transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text != "" && transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text[0] == ' ')
        {
            transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text = transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text.Remove(0, 1);
        }
        if (transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text == "")
        {
            gameProgress.Characters[characterIndex].characterName = originalName;
            transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text = originalName;
        }
    }

    public static char MyValidate(char charToValidate)
    {
        if (allowedCharacters.IndexOf(charToValidate) == -1)
            charToValidate = '\0';
        return charToValidate;
    }
    public void SellCharacter()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        int cost = gameProgress.AllAvailableCharacters.Find(x => x.prefab == gameProgress.Characters[characterIndex].prefab).cost;
        gameProgress.townData.townGold += cost / 2;
        gameProgress.EnableGoldChange("+" + cost / 2 + "g");
        gameProgress.RemoveCharacter(characterIndex);
    }
    public void UpdateTable()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        if (gameProgress.Characters.Count > 0 && characterIndex < gameProgress.Characters.Count && characterIndex >= 0)
        {
            UpdateAbilities();
            UpdateTableInformation();
            UpdateConfirmationTable();
        }
        if (GameObject.Find("CanvasCamera").transform.Find("PortraitBar") != null)
        {
            if (GameObject.Find("Canvas").transform.Find("CharacterTable").gameObject.activeInHierarchy)
            {
                HighlightCurrentCharacterInPortraitBar();
            }
            else
            {
                RemoveHighlightsFromPortraitBar();
            }
        }
        transform.Find("ConfirmationTable").gameObject.SetActive(false);
        transform.Find("Undo").gameObject.SetActive(tempUnlockedAbilities.Count > 0);
        transform.Find("LeftArrow").GetComponent<Button>().interactable = characterIndex > 0;
        transform.Find("RightArrow").GetComponent<Button>().interactable = characterIndex < gameProgress.Characters.Count - 1;
    }

    public void UpdateConfirmationTable()
    {
        var character = GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[characterIndex];
        Color color = character.prefab.GetComponent<PlayerInformation>().ClassColor;
        transform.Find("ConfirmationTable").Find("Text").GetComponent<Text>().text = $"Are you sure you want to sell <color=#{ColorUtility.ToHtmlStringRGBA(color)}>{character.characterName}</color> for <color=yellow>{character.cost / 2}</color> gold?";
        transform.Find("ConfirmationTable").Find("Portrait").GetComponent<Image>().sprite = character.prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
    }

    private void RemoveHighlightsFromPortraitBar()
    {
        var characterButtons = GameObject.Find("CanvasCamera").transform.Find("PortraitBar").transform.Find("CharacterButtons");
        for (int i = 0; i < characterButtons.childCount; i++)
        {
            characterButtons.GetChild(i).Find("Hover").GetComponent<Animator>().SetBool("select", false);
        }
    }

    private void HighlightCurrentCharacterInPortraitBar()
    {
        var characterButtons = GameObject.Find("CanvasCamera").transform.Find("PortraitBar").transform.Find("CharacterButtons");
        for (int i = 0; i < characterButtons.childCount; i++)
        {
            if (characterButtons.GetChild(i).GetComponent<TownPortrait>().characterIndex == characterIndex)
            {
                characterButtons.GetChild(i).Find("Hover").GetComponent<Animator>().SetBool("select", true);
            }
            else
            {
                characterButtons.GetChild(i).Find("Hover").GetComponent<Animator>().SetBool("select", false);
            }
        }
    }

    public void UpdateAbilities()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        var character = gameProgress.Characters[characterIndex];
        for (int i = 0; i < transform.Find("Abilities").childCount; i++)
        {
            transform.Find("Abilities").GetChild(i).Find("Add").GetComponent<Button>().interactable = character.abilityPointCount > 0;
            transform.Find("Abilities").GetChild(i).Find("Add").gameObject.SetActive
            (character.unlockedAbilities[i] == '0'
                && character.abilityPointCount > 0);
            if (character.unlockedAbilities[i] == '0')
            {
                transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().color = Color.gray;
            }
            else
            {
                transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().color = Color.white;
            }
        }

    }
    public void UpdateTableInformation()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        SavedCharacter character = gameProgress.Characters[characterIndex];
        GetComponent<CharacterTable>().originalName = character.characterName;
        Color color = character.prefab.GetComponent<PlayerInformation>().ClassColor;
        transform.Find("TableBorder").GetComponent<Image>().color = character.prefab.GetComponent<PlayerInformation>().ClassColor2;
        transform.Find("Info").transform.Find("Name").GetComponent<InputField>().text = character.characterName;
        transform.Find("Info").transform.Find("ClassName").GetComponent<Text>().text = character.prefab.GetComponent<PlayerInformation>().ClassName;
        transform.Find("Info").transform.Find("ClassName").GetComponent<Text>().color = color;
        transform.Find("Info").transform.Find("Role").GetComponent<Text>().text = character.prefab.GetComponent<PlayerInformation>().role;
        transform.Find("Info").transform.Find("Role").GetComponent<Text>().color = color;
        UpdateRoleIcon(character);
        transform.Find("Info").transform.Find("Level").GetComponent<Text>().text = "LEVEL: " + character.level.ToString();
        transform.Find("Info").transform.Find("Level").GetComponent<Text>().color = color;
        transform.Find("Info").transform.Find("MaxHP").GetComponent<Text>().text = "MAX HP: " + CalculateMaxHP(character);//character.prefab.GetComponent<PlayerInformation>().MaxHealth.ToString();
        transform.Find("Info").transform.Find("MaxHP").GetComponent<Text>().color = color;
        transform.Find("Info").transform.Find("XpProgress").GetComponent<Text>().text = (character.level >= XPProgressManager.currentMaxLevel()) ? "MAX LEVEL" : character.xP + "/" + gameProgress.XPToLevelUp[character.level - 1] + " XP";
        transform.Find("Info").transform.Find("XpProgress").GetComponent<Text>().color = color;
        transform.Find("Info").transform.Find("AbilityPointCount").GetComponent<Text>().text = character.abilityPointCount.ToString();
        transform.Find("Info").transform.Find("AbilityPointCount").GetComponent<Text>().color = color;
        //transform.Find("CharacterModel").GetComponent<Image>().sprite = character.prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
        transform.Find("CharacterArt").GetComponent<Image>().sprite = character.prefab.GetComponent<PlayerInformation>().CharacterSplashArt;
        transform.Find("BlessingList").GetComponent<TextMeshProUGUI>().text = character.CharacterTableBlessingString();
        //
        for (int i = 0; i < transform.Find("Abilities").transform.childCount; i++)
        {
            transform.Find("Abilities").GetChild(i).gameObject.SetActive(true);
            transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().AbilityBackground;
            transform.Find("Abilities").GetChild(i).Find("AbilityIcon").GetComponent<Image>().color = character.prefab.GetComponent<PlayerInformation>().ClassColor;
            if (character.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null)
            {
                transform.Find("Abilities").GetChild(i).Find("AbilityIcon").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
            }
            else
            {
                transform.Find("Abilities").GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private int CalculateMaxHP(SavedCharacter character)
    {
        int maxHP = character.prefab.GetComponent<PlayerInformation>().MaxHealth;
        maxHP += (character.level - 1) * 2;
        if (character.blessings.Find(x => x.blessingName == "Healthy") != null)
        {
            maxHP += 3;
        }
        return maxHP;
    }
    private void UpdateRoleIcon(SavedCharacter character)
    {
        for (int i = 0; i < transform.Find("Info").transform.Find("Role").childCount; i++)
        {
            if (character.prefab.GetComponent<PlayerInformation>().role == transform.Find("Info").transform.Find("Role").GetChild(i).name)
            {
                transform.Find("Info").transform.Find("Role").GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Info").transform.Find("Role").GetChild(i).gameObject.SetActive(false);
            }

        }

    }
    public void UpgradeAbility(int abilityIndex)
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        var unlockedAbilities = GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[characterIndex].unlockedAbilities;
        string newUnlockedAbilities = "";
        for (int i = 0; i < unlockedAbilities.Length; i++)
        {
            if (i != abilityIndex)
            {
                newUnlockedAbilities += unlockedAbilities[i];
            }
            else
            {
                newUnlockedAbilities += 1.ToString();
            }
        }
        gameProgress.Characters[characterIndex].unlockedAbilities = newUnlockedAbilities;
        gameProgress.Characters[characterIndex].abilityPointCount--;
        tempUnlockedAbilities.Add(abilityIndex);
        gameProgress.FakeUpdate();
    }
    private void RemoveAbility(int abilityIndex)
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        var unlockedAbilities = GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[characterIndex].unlockedAbilities;
        string newUnlockedAbilities = "";
        for (int i = 0; i < unlockedAbilities.Length; i++)
        {
            if (i != abilityIndex)
            {
                newUnlockedAbilities += unlockedAbilities[i];
            }
            else
            {
                newUnlockedAbilities += 0.ToString();
            }
        }
        gameProgress.Characters[characterIndex].unlockedAbilities = newUnlockedAbilities;
        gameProgress.Characters[characterIndex].abilityPointCount++;
    }
    public void DisableHelpTables()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        gameProgress.DisableHelpTables();
    }

    public void OnLeftArrowClick()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, gameProgress.Characters.Count - 1);
        if (newCharacterIndex == 5)//newCharacterIndex / 6 < characterIndex / 6)
        {
            gameProgress.UpdateCharacterBar(1);
        }
        gameProgress.DisplayCharacterTable(newCharacterIndex);
    }

    public void OnRightArrowClick()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, gameProgress.Characters.Count - 1);
        if (newCharacterIndex == 6)//newCharacterIndex / 6 > characterIndex / 6)
        {
            gameProgress.UpdateCharacterBar(-1);
        }
        gameProgress.DisplayCharacterTable(newCharacterIndex);
    }

    public void UndoAbilitySelection()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        foreach (int abilityIndex in tempUnlockedAbilities)
        {
            RemoveAbility(abilityIndex);
        }
        tempUnlockedAbilities.Clear();
        gameProgress.FakeUpdate();
    }

    public void ExitTable()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        gameObject.SetActive(false);
        gameProgress.DisableHelpTables();
        gameProgress.FakeUpdate();
    }
}
