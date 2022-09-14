using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateGame : MonoBehaviour
{

    public void SaveGameSettings()
    {
        GameInfo newGame = new GameInfo();
        newGame.GameName = SaveUserData.data.Name + "'s Game";
        newGame.GameRounds = 0;
        newGame.PlayerTurn = 1;

        string key = SaveManager.Instance.GetKey("games/");
        newGame.GameID = key;

        PlayerData thisPlayer = new PlayerData();
        thisPlayer.User = SaveUserData.data;
        thisPlayer.Name = SaveUserData.data.Name;
        thisPlayer.PLayerNumber = 1;
        newGame.Players.Add(thisPlayer);
        
        SaveUserData.data.ActiveGames.Add(key);

        string gamePath = "games/" + newGame.GameID;

        var data = JsonUtility.ToJson(newGame);

        SaveManager.Instance.SaveData(gamePath, data);
        SaveManager.Instance.SaveData(SaveUserData.userPath, JsonUtility.ToJson(SaveUserData.data));
        
        GameKey.Key = key;
        SceneManager.LoadScene("GameView");
    }
}
