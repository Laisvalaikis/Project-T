using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Town : MonoBehaviour
{
    [SerializeField] private GameProgress _gameProgress;
    [SerializeField] private Recruitment _recruitment;
    [SerializeField] private SaveData _saveData;
    public Data _data;
    void OnEnable()
    {
        _saveData.LoadTownData();
        
    }

    private void Start()
    {
        _recruitment.RecruitmentStart();
        _gameProgress.PrepareNewTownDay();
        ToggleAbilityPointWarning();
    }

    public void ToggleAbilityPointWarning()
    {
        GameObject.Find("CanvasCamera").transform.Find("AbilityPointWarning").gameObject
            .SetActive(_data.Characters.Find(x => x.abilityPointCount > 0) != null);
        GameObject.Find("CanvasCamera").transform.Find("RecruitmentWarning").gameObject
           .SetActive(_data.Characters.Count < 3);
    }

}
