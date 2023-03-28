using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Classes;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EncounterController : MonoBehaviour
{
    public List<string> encounterCategories;

    public List<Encounter> Setup(List<Encounter> pastEncounters, ref bool generateNewEncounters, List<Encounter> alreadyGeneratedEncounters)
    {
        List<Encounter> generatedEncounters;
        if (generateNewEncounters)
        {
            GenerateEncounters(out generatedEncounters, pastEncounters);
            generateNewEncounters = false;
        }
        else generatedEncounters = alreadyGeneratedEncounters;
        ToggleEncounterButtons(generatedEncounters);
        return generatedEncounters;
    }

    private void GenerateEncounters(out List<Encounter> encounterListToPopulate, List<Encounter> pastEncounters)
    {
        encounterListToPopulate = new List<Encounter>();
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        for (int i = 1; i <= 5; i++)
        {
            foreach (string category in encounterCategories)
            {
                if(i == 1 || pastEncounters.Find(x => x.missionCategory == category && x.encounterLevel == i - 1) != null)
                {
                    Encounter newEncounter = new Encounter();
                    newEncounter.missionCategory = category;
                    newEncounter.encounterLevel = i;
                    List<GameObject> suitableMaps = gameProgress.GetComponent<MapSetup>().MapPrefabs.FindAll(x => x.GetComponent<Map>().mapCategory == category && x.GetComponent<Map>().suitableLevels.Contains(i));
                    GameObject suitableMap = suitableMaps[Random.Range(0, suitableMaps.Count)];
                    newEncounter.mapName = suitableMap.name;
                    newEncounter.enemyPool = suitableMap.GetComponent<Map>().suitableEnemies;
                    newEncounter.allowDuplicates = suitableMap.GetComponent<Map>().allowDuplicates;
                    newEncounter.numOfEnemies = suitableMap.GetComponent<Map>().numberOfEnemies;
                    encounterListToPopulate.Add(newEncounter);
                }
            }
        }
    }

    private void ToggleEncounterButtons(List<Encounter> generatedEncounters)
    {
        foreach(Encounter encounter in generatedEncounters)
        {
            var button = transform.Find(encounter.missionCategory + "EncounterButtons").transform.Find("Level" + encounter.encounterLevel + "Encounter").gameObject;
            button.SetActive(true);
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject.Find("GameProgress").GetComponent<GameProgress>().ChangeSelectedEncounter(encounter);
            });
        }
    }
}
