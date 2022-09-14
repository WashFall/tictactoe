using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioService
{
    public void BuildAudio();
    public void DestroyAudio();

    public void PlayOnce(AudioClip audio, float volume = 1);
    public void PlayOnce(string audio, float volume = 1);
    public void StartLoop(AudioClip audio, float volume = 1);
    public void StartLoop(string audio, float volume = 1);
    public void StopLoop(AudioClip audio);
    public void StopLoop(string audio);

    public void ToggleSound();
}
