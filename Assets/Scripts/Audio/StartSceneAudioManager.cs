using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneAudioManager : MonoBehaviour
{
    //public static StartSceneAudioManager instance;
    [Header("[ AudioSources ]")]
    [SerializeField] AudioSource oneShotAudioSource = null;

    [Header("[ AudioClips ]")]
    [SerializeField] AudioClip clickSound = null;

    public AudioClip ClickSound { get { return clickSound; } }

    private void Awake()
    {
        StartSceneAudioManager[] obj = FindObjectsOfType<StartSceneAudioManager>();
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }

    // Ŭ���� ��ü�ؼ� �÷����ϴ� �Լ�.
    public void SoundPlay(AudioClip clip)
    { 
        oneShotAudioSource.Stop();
        oneShotAudioSource.PlayOneShot(clip);
    }
}
