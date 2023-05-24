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
    [SerializeField] private Button _embark;
    [SerializeField] private int _charactersCount = 3;
    public Data _data;
    [SerializeField] private GameObject _abilityPointWarning;
    [SerializeField] private GameObject _recruitmentWarning;
    
    void OnEnable()
    {
        _saveData.LoadTownData();
        
    }

    private void Start()
    {
        _recruitment.RecruitmentStart();
        _gameProgress.PrepareNewTownDay();
        ToggleAbilityPointWarning();
        _data.characterRecruitmentEvent.AddListener(RecruitedCharacter);
        if (_data.Characters.Count >= 3)
        {
            _embark.interactable = true;
        }
    }

    public void RecruitedCharacter()
    {
        if (_data.Characters.Count >= 3)
        {
            _embark.interactable = true;
        }
        else
        {
            _embark.interactable = false; 
        }
    }

    public void ToggleAbilityPointWarning()
    {

        if (_data.Characters.Count < _charactersCount)
        {
            _recruitmentWarning.SetActive(true);
        }
        else
        {
            _recruitmentWarning.SetActive(false);
        }

        if (_data.Characters.Find(x => x.abilityPointCount > 0) != null)
        {
            _abilityPointWarning.SetActive(true);
        }
        else
        {
            _abilityPointWarning.SetActive(false);
        }
    }

}
