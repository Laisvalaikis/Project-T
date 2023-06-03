using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        SetupDontDestroyManager(this);
    }

    private static void SetupDontDestroyManager(DontDestroyManager dontDestroyManager)
    {
        if (instance == null)
        {
            instance = dontDestroyManager;
            DontDestroyOnLoad(dontDestroyManager.gameObject);
        }
        else
        {
            Destroy(dontDestroyManager.gameObject);
        }
    }

}
