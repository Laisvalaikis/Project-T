using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangingButton : MonoBehaviour
{
    public string SceneToLoad;
    public bool TextHighlight = false;
    public bool FrameHighlight = true;
    private Color OriginalTextColor;
    // Start is called before the first frame update
    void Start()
    {
        OriginalTextColor = transform.Find("Text").GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    { 
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        Time.timeScale = 1;
    }
    public void SceneTransition()
    {
        GameObject.Find("LoadingScreenCanvas").GetComponent<LoadingScreenController>().LoadScene(SceneToLoad);
    }
    public void OnHover()
    {
        if (TextHighlight)
        {
            transform.Find("Text").GetComponent<Text>().color = Color.white;
        }
        if(FrameHighlight)
        {
            transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", true);
        }
    }
    public void OffHover()
    {
        if (TextHighlight)
        {
            transform.Find("Text").GetComponent<Text>().color = OriginalTextColor;
        }
        if(FrameHighlight)
        {
            transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", false);
        }
    }
    public void QuitGame() 
    {
        Application.Quit();
    }
}
