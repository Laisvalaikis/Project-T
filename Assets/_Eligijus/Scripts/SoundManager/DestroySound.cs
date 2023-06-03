using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestroySound : MonoBehaviour
{
	AudioSource source;
	public TextMeshPro textField;
	public SText[] Naratortext;

	public int effectIndex;

	public int songIndex;
	public bool resetBackgroundSound = false;
    // Start is called before the first frame update
    void Start()
    {
		source = gameObject.GetComponent<AudioSource>();
		StartCoroutine(Playsubtitle(0, Naratortext.Length));
    }

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
		{
			SoundManager.instance.StopPlaying(effectIndex, songIndex);
            //Debug.Log("Destroying object");
            //textField.text = "LMAOOOYEEEETFUCKINGKILLMYSELFIHATEMYLIFEUWUBIGTITSGOTHGF";
            if (resetBackgroundSound)
            {
				MusicManager.instance.ResetVolume();
            }
			Destroy(gameObject);
		}
    }

	IEnumerator Playsubtitle(int i,int length)
	{
		if (i < length)
		{
			textField.text = Naratortext[i].Text;
			yield return new WaitForSeconds(Naratortext[i].Time);
			StartCoroutine(Playsubtitle(i + 1, length));
		}
		else
		{
			yield return null;
		}
	}
}
