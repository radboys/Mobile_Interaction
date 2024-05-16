using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource BGMSource = null;
    private float BGMVolume = 1;

    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float SoundVolume = 1;

    public void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// play BGM
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGM(string name, bool isLoop)
    {
        if (BGMSource == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM";
            BGMSource = obj.AddComponent<AudioSource>();
        }

        ResourcesLoader.Instance.LoadAsync<AudioClip>("Music/BGM/" + name, (clip) =>
        {
            BGMSource.clip = clip;
            BGMSource.loop = isLoop;
            BGMSource.volume = BGMVolume;
            BGMSource.Play();
        });
    }
    /// <summary>
    /// stop BGM
    /// </summary>
    public void StopBGM()
    {
        BGMSource?.Stop();
    }
    /// <summary>
    /// pause BGM
    /// </summary>
    public void PauseBGM()
    {
        BGMSource?.Pause();
    }
    public void ChangeBgmVolume(float volume)
    {
        BGMVolume = volume;
        if (BGMSource != null)
        {
            BGMSource.volume = BGMVolume;
        }        
    }
    /// <summary>
    /// play effect sound
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }


        ResourcesLoader.Instance.LoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            soundList.Add(source);
            source.clip = clip;
            source.loop = isLoop;
            source.volume = SoundVolume;
            source.Play();
            if (callback != null)
            {
                callback(source);
            }
        });
    }

    /// <summary>
    /// mute effect sound
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
    public void ChangeSoundVolume(float volume)
    {
        SoundVolume = volume;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = SoundVolume;
        }
    }
}
