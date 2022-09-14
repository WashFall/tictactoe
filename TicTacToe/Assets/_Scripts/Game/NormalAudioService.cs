using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NormalAudioService : IAudioService
{
    int sourceCount = 10;
    List<AudioSource> sources = new List<AudioSource>();
    List<AudioClip> clips = new List<AudioClip>();
    Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
    string clipLocation = "SFX/";
    bool toggleSound = true;

    public void BuildAudio()
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            if(PlayerPrefs.GetInt("sound") == 0)
            {
                toggleSound = false;
            }
            else if(PlayerPrefs.GetInt("sound") == 1)
            {
                toggleSound = true;
            }
        }

        for(int i = 0; i < sourceCount; i++)
        {
            AudioSource source  = new GameObject().AddComponent<AudioSource>();
            MonoBehaviour.DontDestroyOnLoad(source);
            sources.Add(source);
        }

        clips.AddRange(Resources.LoadAll<AudioClip>(clipLocation));

        foreach(AudioClip clip in clips)
        {
            clipDic.Add(clip.name, clip);
        }
    }

    public void DestroyAudio()
    {
        foreach(AudioSource source in sources)
        {
            MonoBehaviour.Destroy(source);
        }
    }

    public void PlayOnce(AudioClip audio, float volume = 1)
    {
        AudioSource source = GetFreeSource();
        if (toggleSound)
        {
            source.volume = volume;
        }
        else source.volume = 0;
        source.PlayOneShot(audio);
    }

    public void PlayOnce(string audio, float volume = 1)
    {
        AudioSource source = GetFreeSource();
        if (toggleSound)
        {
            source.volume = volume;
        }
        else source.volume = 0;
        source.PlayOneShot(clipDic[audio]);
    }

    public void StartLoop(AudioClip audio, float volume = 1)
    {
        AudioSource source = GetFreeSource();
        if (toggleSound)
        {
            source.volume = volume;
        }
        else source.volume = 0;
        source.clip = audio;
        source.loop = true;
        source.Play();
    }

    public void StartLoop(string audio, float volume = 1)
    {
        AudioSource source = GetFreeSource();
        if (toggleSound)
        {
            source.volume = volume;
        }
        else source.volume = 0;
        source.clip = clipDic[audio];
        source.loop = true;
        source.Play();
    }

    public void StopLoop(AudioClip audio)
    {
        foreach(AudioSource source in sources)
        {
            if(source.clip == audio)
            {
                source.Stop();
                source.clip = null;
                source.loop = false;
                break;
            }
        }
    }

    public void StopLoop(string audio)
    {
        foreach (AudioSource source in sources)
        {
            if (source.clip == clipDic[audio])
            {
                source.Stop();
                source.clip = null;
                source.loop = false;
                break;
            }
        }
    }

    public AudioSource GetFreeSource()
    {
        foreach(var source in sources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return sources.Where(x => !x.loop).ToList()[0];
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            toggleSound = false;
        }
        else if (PlayerPrefs.GetInt("sound") == 1)
        {
            toggleSound = true;
        }
    }
}
