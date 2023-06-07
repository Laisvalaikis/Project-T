using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignSound : MonoBehaviour
{
    #region Inspector

    [HideInInspector] public int effectSelection;
    [HideInInspector] public int songSelection;
    [HideInInspector] public Sound sounds;
    [HideInInspector] public string[] soundNames;
    #endregion

    
    [HideInInspector] public int selectedEffectIndex;
    [HideInInspector] public int selectedSongIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedSong(int effectIndex, int soundIndex)
    {
        selectedEffectIndex = effectIndex;
        selectedSongIndex = soundIndex;
    }

    public bool IsSoundPlaying()
    {
        return SoundManager.instance.IsPlaying(selectedEffectIndex, selectedSongIndex);
    }

    public void PlaySound()
    {
        SoundManager.instance.CreateSound(selectedEffectIndex, selectedSongIndex, this.transform);
    }
    
    public void PlaySound(int selection)
    {
        if (soundNames.Length > selection && selection >= 0)
        {
            try
            {
                Debug.Log(SoundsData.Instance.name);
            }
            catch
            {
                Debug.Log("SoundsData.Instance null ref");
            }
            SoundManager.instance.CreateSound(selectedEffectIndex, selection, this.transform);
        }
    }

    public void PlaySound(int effectIndex, int soundIndex)
    {
        if (soundNames.Length > soundIndex && soundIndex >= 0)
        {
            try
            {
                Debug.Log(SoundsData.Instance.name);
            }
            catch
            {
                Debug.Log("SoundsData.Instance null ref");
            }
            SoundManager.instance.CreateSound(effectIndex, soundIndex, this.transform);
        }
    }

    public void PlaySound(bool isActive)
    {
        if(isActive)
            SoundManager.instance.CreateSound(selectedEffectIndex, selectedSongIndex, this.transform);
    }
}
