using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.SetAudio(new NormalAudioService());
    }
}
