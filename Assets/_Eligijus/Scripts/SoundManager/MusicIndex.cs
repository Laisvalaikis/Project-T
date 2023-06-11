using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIndex : MonoBehaviour
{

    [SerializeField] private int musicIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        MusicManager.instance.ChangeLevelMusic(musicIndex);
    }
}
