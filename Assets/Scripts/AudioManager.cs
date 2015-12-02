using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioClipsType
{
    MusicNormal,MusicScape
}


public class AudioManager : Singleton<AudioManager>
{




    AudioSource mainAudioSource;
    [System.Serializable]
    public class AudioType
    {
        public AudioClipsType type;
        public AudioClip clip;
    }

    [SerializeField]
    List<AudioClip> backgroundSongs;

    public List<AudioType> audioList = new List<AudioType>();
    Dictionary<AudioClipsType, AudioClip> audioMap = new Dictionary<AudioClipsType, AudioClip>();

    public static void PlayClip(AudioClipsType type)
    {
        Instance.mainAudioSource.PlayOneShot(Instance.audioMap[type]);

    }

    public static void PlayBgSong(int i)
    {

        //Instance.mainAudioSource.PlayOneShot(Instance.backgroundSongs[i]);
        Instance.mainAudioSource.clip = Instance.backgroundSongs[i];
        Instance.mainAudioSource.loop = true;
        Instance.mainAudioSource.Play();

    }

    public static void AudioShutdown()
    {
        StopAll();
        PlayBgSong(1);
    }

    public static void StopAll()
    {
        Instance.mainAudioSource.Stop();
    }

    // Use this for initialization
    void Start()
    {
        mainAudioSource = transform.GetComponent<AudioSource>();
        foreach (AudioType audio in audioList)
        {
            audioMap.Add(audio.type, audio.clip);
        }


    }




}