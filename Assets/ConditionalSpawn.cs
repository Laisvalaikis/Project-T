using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public Data _data;
    public float flashDuration = 1f;
    public float flashInterval = 0.2f;
    private Button button;
    Image image;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        Debug.Log(_data.townData.townHall);
        if (_data.townData.townHall[5] == '0')
        {
            gameObject.SetActive(false);
        }
        else if (_data.townData.townHall[5] == '4')
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayFlashingAnimation());
            string townHall = _data.townData.townHall;
            char[] townHallChars = townHall.ToCharArray();
            townHallChars[5] = '4';
            townHall = new string(townHallChars);
            _data.townData.townHall = townHall;
        }
    }

    private void OnEnable()
    {
        if (_data.townData.townHall[5] == '0')
        {
            gameObject.SetActive(false);
        }
        else if (_data.townData.townHall[5] == '5')
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayFlashingAnimation());
            string townHall = _data.townData.townHall;
            char[] townHallChars = townHall.ToCharArray();
            townHallChars[5] = '4';
            townHall = new string(townHallChars);
            _data.townData.townHall = townHall;
        }
    }

    private System.Collections.IEnumerator PlayFlashingAnimation()
    {
        // Flash the button for the specified duration
        float timer = 0f;
        while (timer < flashDuration)
        {
            // Toggle the visibility of the gameObject
            gameObject.SetActive(!gameObject.activeSelf);

            // Wait for the specified interval
            yield return new WaitForSeconds(flashInterval);

            timer += flashInterval;
        }

        // Ensure the gameObject is active after the flashing animation ends
        gameObject.SetActive(true);
    }
}
