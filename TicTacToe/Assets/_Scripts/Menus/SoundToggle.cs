using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    private Toggle checkBox;

    void Start()
    {
        checkBox = GetComponent<Toggle>();

        if (PlayerPrefs.HasKey("sound"))
        {
            if (PlayerPrefs.GetInt("sound") == 0)
            {
                checkBox.isOn = false;
            }
            else if (PlayerPrefs.GetInt("sound") == 1)
            {
                checkBox.isOn = true;
            }
        }
        else
            checkBox.isOn = true;
    }

    public void ToggleSound()
    {
        if (checkBox.isOn)
            PlayerPrefs.SetInt("sound", 1);
        else if(!checkBox.isOn)
            PlayerPrefs.SetInt("sound", 0);

        PlayerPrefs.Save();

        ServiceLocator.GetAudio().ToggleSound();
    }
}
