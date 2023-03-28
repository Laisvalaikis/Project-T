using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldChange : MonoBehaviour
{
    [HideInInspector] public float time;
    public float disappearSpeed = 1f;
    public float moveSpeed = 0.005f;
    private Vector3 originalPosition;
    private Color originalColor;
    private Color color;

    // Start is called before the first frame update
    void OnEnable()
    {
        time = 1f;
        originalPosition = transform.localPosition;
        originalColor = GetComponent<Text>().color;
        color = originalColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time -= Time.deltaTime;
        if (!GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
        {
            transform.position += new Vector3(0f, moveSpeed);
            color.a -= (disappearSpeed * Time.fixedDeltaTime);
        }
        GetComponent<Text>().color = color;
        if (time <= 0)
        {
            gameObject.SetActive(false);
            transform.localPosition = originalPosition;
            GetComponent<Text>().color = originalColor;
        }
    }
}
