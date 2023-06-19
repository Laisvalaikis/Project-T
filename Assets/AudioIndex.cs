using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIndex : MonoBehaviour
{
    [SerializeField] private int musicIndex = 0;
    [SerializeField] private bool changeOnAwake = false;
    // Start is called before the first frame update
    void Start()
    {
        if (changeOnAwake)
        {
            MusicManager.instance.ChangeLevelMusic(musicIndex);
        }
    }

    public void ChangeLevelMusic()
    {
        MusicManager.instance.ChangeLevelMusic(musicIndex);
    }
}
