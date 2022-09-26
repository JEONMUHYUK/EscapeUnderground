using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneAudioManager : MonoBehaviour
{
    [Header("[ AudioSources ]")]
    [SerializeField] AudioSource oneShotAudioSource = null;

    [Header("[ AudioClips ]")]
    [SerializeField] AudioClip clickSound = null;

    public AudioClip ClickSound { get { return clickSound; } }

    // Ŭ���� ��ü�ؼ� �÷����ϴ� �Լ�.
    public void SoundPlay(AudioClip clip)
    { 
        oneShotAudioSource.Stop();
        oneShotAudioSource.PlayOneShot(clip);
    }
}
