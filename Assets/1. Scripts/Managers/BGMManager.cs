using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;  

    [SerializeField] private AudioClip[] bgmClips;
    private AudioSource bgmSource;

    private void Start()
    {
        instance = this;

        bgmSource = GetComponent<AudioSource>();

        AudioClip randomClip = bgmClips[Random.Range(0, bgmClips.Length)];
        bgmSource.clip = randomClip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource?.Stop();
    }

    public void PlayBGM()
    {
        AudioClip randomClip = bgmClips[Random.Range(0, bgmClips.Length)];
        bgmSource.clip = randomClip;

        bgmSource.Play();
    }
}
