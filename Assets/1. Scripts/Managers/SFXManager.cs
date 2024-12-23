using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFX
{
    public string sfxName;
    public AudioClip sfxClip;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private List<SFX> sfxList;
    [SerializeField] private List<AudioSource> sfxSources;

    private void Start()
    {
        instance = this;
    }

    public void PlaySFX(string sfxName)
    {
        for (int i = 0; i < sfxList.Count; i++)
        {
            if(sfxList[i].sfxName == sfxName)
            {
                for(int j = 0; j < sfxSources.Count; j++)
                {
                    if (!sfxSources[j].isPlaying)
                    {
                        sfxSources[j].clip = sfxList[i].sfxClip;
                        sfxSources[j].Play();

                        return;
                    }
                }
            }
        }
    }
}
