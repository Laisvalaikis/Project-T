using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    public CanvasGroup darkScreen;
    public GameObject loadingScreen;
    public float fadeLength;
    public float waitTime;
    private static LoadingScreenController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        StartCoroutine(SceneTransition(sceneToLoad));
    }

    IEnumerator SceneTransition(string sceneName)
    {
        darkScreen.alpha = 0;
        darkScreen.gameObject.SetActive(true);
        float timer = 0f;
        while(timer < fadeLength)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        loadingScreen.SetActive(true);
        timer = fadeLength;
        while (timer > 0)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        darkScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
        }
        darkScreen.alpha = 0;
        darkScreen.gameObject.SetActive(true);
        timer = 0f;
        while (timer < fadeLength)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        loadingScreen.SetActive(false);
        timer = fadeLength;
        while (timer > 0)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        darkScreen.gameObject.SetActive(false);
    }
}
