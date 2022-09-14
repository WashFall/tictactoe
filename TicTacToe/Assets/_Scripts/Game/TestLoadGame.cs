using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        GameKey.Key = "-N9WrImNROFcROzUeAKb";
        SceneManager.LoadScene("GameView");
    }
}
