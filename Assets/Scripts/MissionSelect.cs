using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{
    public EncounterController encounterController;
    public SaveData _saveData;
    public Data _data;
    void OnEnable()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        _saveData.LoadTownData();
        var townData = _data.townData;
        townData.generatedEncounters = encounterController.Setup(townData.pastEncounters, ref townData.generateNewEncounters, townData.generatedEncounters);
        gameProgress.PrepareNewTownDay();
        _data.townData.selectedEncounter = null; // Reset Encounter
    }

}
