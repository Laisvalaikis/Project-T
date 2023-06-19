using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseEffect : MonoBehaviour
{
    public float scaleFactor = 0.7f;
    public float speed = 1f;
    public Color color1 = Color.red;
    public Color color2 = Color.yellow;
    public float speedColor = 1.0f;
    public float minScale = 1f;
    public float maxScale = 1.5f;

    private TextMeshProUGUI textMeshPro;
    private Vector3 initialScale;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        initialScale = textMeshPro.transform.localScale;
        StartCoroutine(BlinkColor());
        StartCoroutine(Pulse());

    }

    void Update()
    {
        //float scale = (Mathf.Sin(Time.time * speed) / 2.0f) + 0.5f;
        //textMeshPro.transform.localScale = initialScale + new Vector3(scale, scale, scale) * scaleFactor;
    }


    IEnumerator Pulse()
    {
        while (true)
        {
            // animate from minScale to maxScale
            for (float t = 0; t <= 1; t += Time.deltaTime / speed)
            {
                float scale = Mathf.Lerp(minScale, maxScale, t);
                textMeshPro.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }

            // animate from maxScale to minScale
            for (float t = 0; t <= 1; t += Time.deltaTime / speed)
            {
                float scale = Mathf.Lerp(maxScale, minScale, t);
                textMeshPro.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
    }

    IEnumerator BlinkColor()
    {
        while (true)
        {
            // animate color1 to color2
            for (float t = 0; t <= 1; t += Time.deltaTime / speedColor)
            {
                textMeshPro.color = Color.Lerp(color1, color2, t);
                yield return null;
            }

            // animate color2 to color1
            for (float t = 0; t <= 1; t += Time.deltaTime / speedColor)
            {
                textMeshPro.color = Color.Lerp(color2, color1, t);
                yield return null;
            }
        }
    }
}
