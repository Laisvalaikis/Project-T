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
    [SerializeField] private Button recruitmentCenterButton;
    [SerializeField] private int _charactersCount = 3;
    public Data _data;
    [SerializeField] private GameObject _abilityPointWarning;
    [SerializeField] private GameObject _recruitmentWarning;
    [SerializeField] private Vector3 pulseScale = new Vector3(1.1f, 1.1f, 1.1f); // The scale of the button pulse
    [SerializeField] private float pulseDuration = 1.3f; // How long the button takes to pulse
    private Image buttonImage;
    private float initialAlpha;
    [SerializeField] private float pulseWaitTime = 2.0f; // Time to wait between each pair of pulses

    private void Awake()
    {
        buttonImage = _embark.GetComponent<Image>();
        if (buttonImage != null)
        {
            initialAlpha = buttonImage.color.a;
        }
        else
        {
            Debug.LogError("No Image component found on the button object.");
        }
    }
    void OnEnable()
    {
        _saveData.LoadTownData();
        /* Because of time constraints and the fact that we're gonna re-do
        everything, the button blinks whenever it is interactable 
        and the player has not completed both missions. Basically, the
        checks are fucky and not great*/
        StartCoroutine(PulseLoopEmbark(5.0f, _embark));
        StartCoroutine(PulseLoopRC(5.0f, recruitmentCenterButton));
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

    private IEnumerator PulseLoopRC(float delay, Button button)
    {
        yield return new WaitForSeconds(delay);

        while (_data.Characters.Count==0)
        {
            if (_data.Characters.Count == 0)
            {
                yield return StartCoroutine(PulseButton(button));
                yield return StartCoroutine(PulseButton(button));
                
                yield return new WaitForSeconds(pulseWaitTime);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private IEnumerator PulseLoopEmbark(float delay, Button button)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            //if interactable AND merchant mission hasn't been played
            if (_embark.interactable && _data.townData.townHall[5] == '0')
            {
                yield return StartCoroutine(PulseButton(button));
                yield return StartCoroutine(PulseButton(button));

                yield return new WaitForSeconds(pulseWaitTime);
            }
            else
                yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator PulseButton(Button button)
    {
        Vector3 initialScale = button.transform.localScale;
        float time = 0;

        while (time < pulseDuration)
        {
            float pulseTime = time / pulseDuration;

            if (pulseTime < 0.5f)
            {
                // First half of the pulse - button gets larger and fades in
                button.transform.localScale = Vector3.Lerp(initialScale, pulseScale, pulseTime / 0.5f);
                buttonImage.CrossFadeAlpha(Mathf.Lerp(initialAlpha, 0.5f, pulseTime / 0.5f), 0, false);
            }
            else
            {
                // Second half of the pulse - button gets smaller and fades out
                button.transform.localScale = Vector3.Lerp(pulseScale, initialScale, (pulseTime - 0.5f) / 0.5f);
                buttonImage.CrossFadeAlpha(Mathf.Lerp(0.5f, initialAlpha, (pulseTime - 0.5f) / 0.5f), 0, false);
            }

            time += Time.deltaTime;

            yield return null;
        }

        button.transform.localScale = initialScale; // Ensure scale is reset to initial after each pulse
        buttonImage.CrossFadeAlpha(initialAlpha, 0, false); // Ensure alpha is reset to initial after each pulse
    }

}
