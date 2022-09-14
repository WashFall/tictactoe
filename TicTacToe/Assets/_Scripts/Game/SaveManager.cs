using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    public delegate void OnLoadedDelegate(string json);
    public delegate void OnSaveDelegate();

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public GameListExpand gameList;
    public GameObject buttonPrefab;

    FirebaseDatabase database;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        database = FirebaseDatabase.DefaultInstance;
    }

    public void LoadData(string path, OnLoadedDelegate onLoadedDelegate)
    {
        database.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            onLoadedDelegate(task.Result.GetRawJsonValue());
        });
    }

    public void SaveData(string path, string data, OnSaveDelegate onSaveDelegate = null)
    {
        database.RootReference.Child(path).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            onSaveDelegate?.Invoke();
        });
    }

    public string GetKey(string path)
    {
        return database.RootReference.Child(path).Push().Key;
    }
    
    public void Subscribe(string id)
    {
        FirebaseDatabase.DefaultInstance.GetReference(id).ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        GameInfo updatedGame = JsonUtility.FromJson<GameInfo>(args.Snapshot.GetRawJsonValue());

        gameManager.GameUpdate(updatedGame);
    }
    public void ListSubscribe(string id)
    {
        FirebaseDatabase.DefaultInstance.GetReference(id).ValueChanged += ListHandleValueChanged;
    }

    public void ListUnsubscribe(string id)
    {
        FirebaseDatabase.DefaultInstance.GetReference(id).ValueChanged -= ListHandleValueChanged;
    }

    void ListHandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if(gameList != null)
            ListGames();
    }
    
    public void ListGames()
    {
        foreach (Transform child in gameList.transform)
            GameObject.Destroy(child.gameObject);

        LoadDataMultiple("games/", ShowGames);
    }

    public void ShowGames(string json)
    {
        var gameInfo = JsonUtility.FromJson<GameInfo>(json);

        if (!gameList.myGames)
        {
            if (SaveUserData.data.ActiveGames.Contains(gameInfo.GameID) || gameInfo.Players.Count > 1)
            {
                return;
            }
        }
        else
        {
            if (!SaveUserData.data.ActiveGames.Contains(gameInfo.GameID))
            {
                return;
            }
        }

        GameObject newButton = Instantiate(buttonPrefab, gameList.transform);
        GameObject[] texts = new GameObject[2];
        for (int i = 0; i < newButton.transform.childCount; i++)
        {
            texts[i] = newButton.transform.GetChild(i).gameObject;
        }
        
        texts[0].GetComponent<TMP_Text>().text = gameInfo.GameName;
        texts[1].GetComponent<TMP_Text>().text = "Round: " + gameInfo.GameRounds;
        if(!gameList.myGames)
            newButton.GetComponent<Button>().onClick.AddListener(() => JoinGame(gameInfo));
        else
            newButton.GetComponent<Button>().onClick.AddListener(() => JoinMyGame(gameInfo));
    }
    
    public void LoadDataMultiple(string path, OnLoadedDelegate onLoadedDelegate)
    {
        database.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            string jsonData = task.Result.GetRawJsonValue();

            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            foreach (var item in task.Result.Children)
            {
                onLoadedDelegate(item.GetRawJsonValue());
            }
        });
    }

    public void JoinGame(GameInfo gameInfo)
    {
        PlayerData newPlayer = new PlayerData();
        newPlayer.User = SaveUserData.data;
        newPlayer.Name = SaveUserData.data.Name;
        newPlayer.PLayerNumber = 2;
        gameInfo.Players.Add(newPlayer);
        SaveUserData.data.ActiveGames.Add(gameInfo.GameID);
        SaveData("games/" + gameInfo.GameID, JsonUtility.ToJson(gameInfo));
        SaveData(SaveUserData.userPath, JsonUtility.ToJson(SaveUserData.data));
        
        
        GameKey.Key = gameInfo.GameID;
        SceneManager.LoadScene("GameView");
    }

    public void JoinMyGame(GameInfo gameInfo)
    {
        GameKey.Key = gameInfo.GameID;
        SceneManager.LoadScene("GameView");
    }
}