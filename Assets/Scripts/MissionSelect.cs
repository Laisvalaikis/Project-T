using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{
    void Start()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        gameProgress.LoadTownData();
        var townData = gameProgress.townData;
        townData.generatedEncounters = GameObject.Find("CanvasCamera").transform.Find("EncounterButtons").GetComponent<EncounterController>().Setup(townData.pastEncounters, ref townData.generateNewEncounters, townData.generatedEncounters);
        gameProgress.PrepareNewTownDay();
    }

}
