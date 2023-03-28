using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Classes;

public class HelpTableController : MonoBehaviour
{
   // public Sprite attackIcon;
    public List<GameObject> tablesToInstantiate;
    [HideInInspector] public bool hasActionButtonBeenEntered = false;

    public void EnableTableForTown(int abilityIndex)
    {
        var ability = GameObject.Find("GameProgress").GetComponent<GameProgress>().
            Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex].prefab.
            GetComponent<ActionManager>().FindActionByIndex(abilityIndex).action.GetBuffedAbility(
            GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex].blessings);
        var tableToInstantiate = tablesToInstantiate.Find(x => x.name == ability.actionStateName);
        if (tableToInstantiate != null)
        {
            if (GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(abilityIndex).transform.Find("ActionButtonFrame").GetComponent<Animator>().GetBool("select"))
            {
                GameObject.Find("GameProgress").GetComponent<GameProgress>().DisableHelpTables();
            }
            else
            {
                GameObject.Find("GameProgress").GetComponent<GameProgress>().DisableHelpTables();
                var table = Instantiate(tableToInstantiate, GameObject.Find("Canvas").transform.Find("CharacterTable").transform.position, Quaternion.identity) as GameObject;
                table.transform.SetParent(GameObject.Find("Canvas").transform.Find("HelpTables").transform);
                var character = GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex];
                //
                FillTableWithInfo(ability, table, character, character.prefab.GetComponent<ActionManager>());
                //table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
                //if (ability.maxAttackDamage == 0)
                //{
                //    table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position + new Vector3(0f, 80f);
                //}
                //else
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
                //var blessingsList = GameObject.Find("GameProgress").GetComponent<GameProgress>().Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex].blessings.FindAll(x => x.spellName == ability.actionStateName);
                //string blessingsText = "";
                //foreach (var blessing in blessingsList)
                //{
                //    blessingsText += $"{blessing.blessingName}\n";
                //}
                //table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
                //
                GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(abilityIndex).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", true);
            }
        }
    }

    private void FillTableWithInfo(BaseAction ability, GameObject table, SavedCharacter character, ActionManager actionManager)
    {

        table.transform.Find("Button").Find("TableContents").Find("SpellIconFrame").Find("Icon").GetComponent<Image>().sprite = actionManager.FindActionListByName(ability.actionStateName).AbilityIcon;

        table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
        if (ability.maxAttackDamage == 0)
        {
            table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
            table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
            table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").localPosition = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").localPosition + new Vector3(0f, 80f);
        }
        else
            table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
        table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
        table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
        table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
        var blessingsList = character.blessings.FindAll(x => x.spellName == ability.actionStateName);
        string blessingsText = "";
        foreach (var blessing in blessingsList)
        {
            blessingsText += $"{blessing.blessingName}\n";
        }
        table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
    }

    public void EnableTableForInGameButton()
    {
        var gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
        if (gameInformation.SelectedCharacter != null || gameInformation.InspectedCharacter != null)
        {
            var character = gameInformation.SelectedCharacter == null ? gameInformation.InspectedCharacter : gameInformation.SelectedCharacter;
            var ability = character.GetComponent<ActionManager>().FindActionByName(character.GetComponent<PlayerInformation>().currentState).GetBuffedAbility(character.GetComponent<PlayerInformation>().savedCharacter.blessings);
            var tableToInstantiate = tablesToInstantiate.Find(x => x.name == ability.actionStateName);
            if (tableToInstantiate != null)
            {
                var table = Instantiate(tableToInstantiate, GameObject.Find("Canvas").transform.position, Quaternion.identity);
                table.transform.SetParent(GameObject.Find("Canvas").transform.Find("HelpTables").transform);
                table.transform.SetSiblingIndex(4);
                gameInformation.helpTableOpen = true;
                gameInformation.isBoardDisabled = true;
                if (GameObject.Find("Canvas").transform.Find("HelpScreen") != null)
                {
                    GameObject.Find("Canvas").transform.Find("HelpScreen").gameObject.SetActive(true);
                }
                //
                FillTableWithInfo(ability, table, character.GetComponent<PlayerInformation>().savedCharacter, character.GetComponent<ActionManager>());
                //table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
                //if (ability.maxAttackDamage == 0)
                //{
                //    table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position + new Vector3(0f, 80f);
                //}
                //else
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
                //var blessingsList = character.GetComponent<PlayerInformation>().savedCharacter.blessings.FindAll(x => x.spellName == ability.actionStateName);
                //string blessingsText = "";
                //foreach (var blessing in blessingsList)
                //{
                //    blessingsText += $"{blessing.blessingName}\n";
                //}
                //table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
                //
            }
        }
    }

    public void EnableTableForInGameRightClick(string abilityName)
    {
        var gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
        if (gameInformation.SelectedCharacter != null || gameInformation.InspectedCharacter != null)
        {
            var character = gameInformation.SelectedCharacter == null ? gameInformation.InspectedCharacter : gameInformation.SelectedCharacter;
            var ability = character.GetComponent<ActionManager>().FindActionByName(abilityName).GetBuffedAbility(character.GetComponent<PlayerInformation>().savedCharacter.blessings);
            var tableToInstantiate = tablesToInstantiate.Find(x => x.name == abilityName);
            if (tableToInstantiate != null)
            {
                var table = Instantiate(tableToInstantiate, GameObject.Find("Canvas").transform.position, Quaternion.identity);
                table.transform.SetParent(GameObject.Find("Canvas").transform.Find("HelpTables").transform);
                table.transform.SetSiblingIndex(4);
                gameInformation.helpTableOpen = true;
                gameInformation.isBoardDisabled = true;
                if (GameObject.Find("Canvas").transform.Find("HelpScreen") != null)
                {
                    GameObject.Find("Canvas").transform.Find("HelpScreen").gameObject.SetActive(true);
                }
                //
                FillTableWithInfo(ability, table, character.GetComponent<PlayerInformation>().savedCharacter, character.GetComponent<ActionManager>());
                //table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
                //if (ability.maxAttackDamage == 0)
                //{
                //    table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
                //    table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position + new Vector3(0f, 80f);
                //}
                //else
                //    table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
                //table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
                //var blessingsList = character.GetComponent<PlayerInformation>().savedCharacter.blessings.FindAll(x => x.spellName == ability.actionStateName);
                //string blessingsText = "";
                //foreach (var blessing in blessingsList)
                //{
                //    blessingsText += $"{blessing.blessingName}\n";
                //}
                //table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
                //
            }
        }
    }

    public void EnableTableForPVP(string abilityName, SavedCharacter character)
    {
        CloseAllHelpTables();
        var tableToInstantiate = tablesToInstantiate.Find(x => x.name == abilityName);
        var ability = character.prefab.GetComponent<ActionManager>().FindActionByName(abilityName).GetBuffedAbility(character.blessings);
        if (tableToInstantiate == null)
        {
            print("Ner tokios help table lol");
        }
        else
        {
            var table = Instantiate(tableToInstantiate, GameObject.Find("CanvasCamera").transform.Find("HelpTables"));
            //
            FillTableWithInfo(ability, table, character, character.prefab.GetComponent<ActionManager>());
            //table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
            //if (ability.maxAttackDamage == 0)
            //{
            //    table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
            //    table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
            //    table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").localPosition = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").localPosition +  new Vector3(0f, 80f);
            //}
            //else
            //    table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
            //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
            //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
            //table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
            //var blessingsList = character.blessings.FindAll(x => x.spellName == ability.actionStateName);
            //string blessingsText = "";
            //foreach (var blessing in blessingsList)
            //{
            //    blessingsText += $"{blessing.blessingName}\n";
            //}
            //table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
            //
        }
    }

    public void EnableTableByName(string abilityName, SavedCharacter character)
    {
        CloseAllHelpTables();
        var ability = character.prefab.GetComponent<ActionManager>().FindActionByName(abilityName).GetBuffedAbility(character.blessings);
        if (tablesToInstantiate.Find(x => x.name == abilityName) == null)
        {
            print("Ner tokios help table lol");
        }
        else
        {
            var table = Instantiate(tablesToInstantiate.Find(x => x.name == abilityName), GameObject.Find("Canvas").transform.Find("HelpTables"));
            //
            FillTableWithInfo(ability, table, character, character.prefab.GetComponent<ActionManager>());
            //table.transform.Find("Button").Find("TableContents").Find("CooldownText").GetComponent<Text>().text = ability.AbilityCooldown.ToString();
            //if (ability.maxAttackDamage == 0)
            //{
            //    table.transform.Find("Button").Find("TableContents").Find("DamageIcon").gameObject.SetActive(false);
            //    table.transform.Find("Button").Find("TableContents").Find("DamageText").gameObject.SetActive(false);
            //    table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position = table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").position + new Vector3(0f, 80f);
            //}
            //else
            //    table.transform.Find("Button").Find("TableContents").Find("DamageText").GetComponent<Text>().text = ability.GetDamageString();
            //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Slow").gameObject.SetActive(ability.isAbilitySlow);
            //table.transform.Find("Button").Find("TableContents").Find("IsAbilitySlow").Find("Fast").gameObject.SetActive(!ability.isAbilitySlow);
            //table.transform.Find("Button").Find("TableContents").Find("RangeText").GetComponent<Text>().text = ability.AttackRange.ToString();
            //var blessingsList = character.blessings.FindAll(x => x.spellName == ability.actionStateName);
            //string blessingsText = "";
            //foreach (var blessing in blessingsList)
            //{
            //    blessingsText += $"{blessing.blessingName}\n";
            //}
            //table.transform.Find("Button").Find("TableContents").Find("BlessingsTable").Find("BlessingsList").GetComponent<Text>().text = blessingsText;
            //
        }
    }

    public void CloseAllHelpTables()
    {
        if (GameObject.Find("Canvas") != null && GameObject.Find("Canvas").transform.Find("HelpTables") != null)
        {
            foreach (Transform x in GameObject.Find("Canvas").transform.Find("HelpTables"))
            {
                Destroy(x.gameObject);
            }
        }
        if (GameObject.Find("CanvasCamera") != null && GameObject.Find("CanvasCamera").transform.Find("HelpTables") != null)
        {
            foreach (Transform x in GameObject.Find("CanvasCamera").transform.Find("HelpTables"))
            {
                Destroy(x.gameObject);
            }
        }
    }
}
