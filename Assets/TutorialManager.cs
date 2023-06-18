using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    public GameObject firstGameObject;
    public GameObject secondGameObject;
    public float pulseSpeed = 1f;
    public float minScale = 0.5f;
    public float maxScale = 1f;
    public float fadeDuration = 1f;

    private IEnumerator Start()
    {
        secondGameObject.SetActive(false); 
        firstGameObject.SetActive(false);
        yield return new WaitForSeconds(2);

        //Fades in text that tells player to move
        //camera and pulse it then make it disappear
        firstGameObject.SetActive(true);    
        yield return StartCoroutine(FadeInObject(firstGameObject)); 
        yield return StartCoroutine(PulseAndFadeObject(firstGameObject, 2));
        yield return new WaitForSeconds(1f);


        //Start checking if player selects character
        //(Could not add event listener through code, List remained empty no matter
        //what I added, something to look out for when re-writing maybe :( )
        StartCoroutine(CheckForCharacterSelect());
    }

    //Maybe observer pattern in future?
    private IEnumerator CheckForCharacterSelect()
    {
        GameObject selectedChar = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter;
        if(selectedChar == null)
        {
            secondGameObject.SetActive(true);
            yield return StartCoroutine(FadeInObject(secondGameObject));
        }
        while (true)
        {
            //Taip, .Find in a while loop, sorry for good code
            //Ne, bet for real, jei neiseina su singleton, tai kitaip is hierarchijos
            //nelabai pasiima. Nu, kita vertus, kai perrasinesim, kita struktura bus
            //tai gal devgammui nieko tokio o tada galesim daryt, pavyzdziui, atskiras scenas
            //kiekvienam leveliui.
            if (GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == null)
                StartCoroutine(PulseObject(secondGameObject,1));
            else
            {
                StartCoroutine(FadeOutObject(secondGameObject));
                //yield return null;
                break;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator FadeInObject(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color startColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        Color endColor = spriteRenderer.color;

        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / fadeDuration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
    }



    private IEnumerator PulseAndFadeObject(GameObject obj, int pulseCount)
    {
        yield return StartCoroutine(PulseObject(obj, pulseCount));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOutObject(obj));
    }

    private IEnumerator PulseObject(GameObject obj, int pulseCount)
    {
        float pulseDuration = 0.5f; // Duration of each pulse
        float delayDuration = 1f; // Duration of the delay between pulses

        float startScale = obj.transform.localScale.x;
        float targetScale = startScale * (1f + 0.1f);

        for (int i = 0; i < pulseCount; i++)
        {
            float startTime = Time.time;
            float endTime = startTime + pulseDuration;

            while (Time.time <= endTime)
            {
                float t = (Time.time - startTime) / pulseDuration;

                // Animate from startScale to targetScale linearly
                float scale = Mathf.Lerp(startScale, targetScale, t);
                obj.transform.localScale = new Vector3(scale, scale, scale);

                yield return null;
            }

            // Ensure the final scale is exactly targetScale
            obj.transform.localScale = new Vector3(targetScale, targetScale, targetScale);

            // Wait for the delay duration
            yield return new WaitForSeconds(delayDuration);

            // Reverse the pulse animation
            startTime = Time.time;
            endTime = startTime + pulseDuration;

            while (Time.time <= endTime)
            {
                float t = (Time.time - startTime) / pulseDuration;

                // Animate from targetScale to startScale linearly
                float scale = Mathf.Lerp(targetScale, startScale, t);
                obj.transform.localScale = new Vector3(scale, scale, scale);

                yield return null;
            }

            // Set the final scale to startScale
            obj.transform.localScale = new Vector3(startScale, startScale, startScale);

            // Wait for the delay duration
            yield return new WaitForSeconds(delayDuration);
        }
    }




    private IEnumerator FadeOutObject(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / fadeDuration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        obj.SetActive(false);
    }
}
