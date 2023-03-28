using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public float fadeDuration = 2f;
    private float timer;

    // Start is called before the first frame update
    void OnEnable()
    {
        timer = fadeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (gameObject.name == "YourTurn")
        {
            Color color;
            for(int i = 1; i < transform.childCount; i++)
            {
                color = transform.GetChild(i).GetComponent<Image>().color;
                transform.GetChild(i).GetComponent<Image>().color = new Color(color.r, color.g, color.b, timer / fadeDuration);
            }
            color = transform.GetChild(0).GetComponent<Text>().color;
            transform.GetChild(0).GetComponent<Text>().color = new Color(color.r, color.g, color.b, timer / fadeDuration);
        }
        else
        {
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, timer / fadeDuration);
        }
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
