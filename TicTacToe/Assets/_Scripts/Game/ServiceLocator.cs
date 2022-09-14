using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static IAudioService sound;

    public static IAudioService GetAudio()
    {
        return sound;
    }

    public static void SetAudio(IAudioService service)
    {
        if (sound != null)
            sound.DestroyAudio();

        sound = service;
        service.BuildAudio();
    }
}
