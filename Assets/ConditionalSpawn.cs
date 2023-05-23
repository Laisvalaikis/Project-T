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
    public GameObject buttonObject; // Assign your button GameObject to this in the inspector.
    public TownHall townHallScript;

    void Start()
    {
            if (_data.townData.townHall[5] == '0')
            {
                buttonObject.SetActive(false);
            }
            else if (_data.townData.townHall[5] == '4')
            {
                buttonObject.SetActive(true);
            }
            else
            {
                string townHall = _data.townData.townHall;
                townHallScript.SetupMerchantSprite();
                StartCoroutine(PlayFlashingAnimation());
                buttonObject.SetActive(true);
                char[] townHallChars = townHall.ToCharArray();
                townHallChars[5] = '4';
                townHall = new string(townHallChars);
                _data.townData.townHall = townHall;
            }
    }


    private System.Collections.IEnumerator PlayFlashingAnimation()
    {
        // Flash the button for the specified duration
        Debug.Log("Flashing");
        buttonObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        buttonObject.SetActive(true);
    }
}