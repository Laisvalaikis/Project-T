using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Town : MonoBehaviour
{
    void Start()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        gameProgress.LoadTownData();
        GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().RecruitmentStart();
        gameProgress.PrepareNewTownDay();
        ToggleAbilityPointWarning();
    }

    public void ToggleAbilityPointWarning()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        GameObject.Find("CanvasCamera").transform.Find("AbilityPointWarning").gameObject
            .SetActive(gameProgress.Characters.Find(x => x.abilityPointCount > 0) != null);
        GameObject.Find("CanvasCamera").transform.Find("RecruitmentWarning").gameObject
           .SetActive(gameProgress.Characters.Count < 3);
    }

}
