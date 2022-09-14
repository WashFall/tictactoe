using System;
using System.Collections;
using System.Collections.Generic;
using Google.MiniJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager INSTANCE { get { return _instance; } }

    public int pos;
    public int player;
    public Coordinate[] coordinates = new Coordinate[9];
    public int winnerNum;
    public bool gameOver;
    public Coordinate clickedCoord;
    public List<int> drawCheck = new List<int>();
    public bool draw;
    public GameInfo activeGame;
    public PlayerData leader;
    public GameObject roundButton;
    public TMP_Text playerText, opponentText;
    private PlayerData thisPlayer, opponent;
    private string symbol, opSymbol;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        SaveManager.Instance.gameManager = this;
        gameOver = false;
        winnerNum = 0;

        SaveManager.Instance.LoadData("/games/" + GameKey.Key, StartUp);
    }

    void StartUp(string json)
    {
        activeGame = JsonUtility.FromJson<GameInfo>(json);
        GameSetup();
        SaveManager.Instance.Subscribe("/games/" + GameKey.Key);
    }

    void GameSetup()
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            if (coordinates[i].value != activeGame.CoordStates[i])
                ServiceLocator.GetAudio().PlayOnce("click3", 0.3f);
            coordinates[i].value = activeGame.CoordStates[i];
            coordinates[i].SpriteCheck();
        }

        for (int i = 0; i < activeGame.Players.Count; i++)
        {
            activeGame.Players[i].PLayerNumber = i + 1;
            if (activeGame.Players[i].Name == SaveUserData.data.Name)
            {
                thisPlayer = activeGame.Players[i];
            }
            else
            {
                opponent = activeGame.Players[i];
            }
        }

        if (player == 0)
            player = 1;

        if (thisPlayer.PLayerNumber == 1)
        {
            symbol = "X";
            opSymbol = "O";
        }
        else if (thisPlayer.PLayerNumber == 2)
        {
            symbol = "O";
            opSymbol = "X";
        }
        playerText.text = String.Format("{0} - {1} - {2}", symbol, thisPlayer.Name, thisPlayer.Points);
        if(opponent != null)
            opponentText.text = String.Format("{0} - {1} - {2}", opSymbol, opponent.Name, opponent.Points);
    }

    public void GameUpdate(GameInfo gameUpdate)
    {
        if (gameUpdate.GameRounds > activeGame.GameRounds)
        {
            NextRound();
        }
        activeGame = gameUpdate;
        GameSetup();
        player = activeGame.PlayerTurn;
        StateCheck();

    }

    public void CheckAndSaveGame()
    {
        StateCheck();
        string data = JsonUtility.ToJson(activeGame);
        SaveManager.Instance.SaveData("/games/" + GameKey.Key, data);
    }

    public void StateCheck()
    {
        if (player != 0)
        {
            foreach (var coord in activeGame.CoordStates)
            {
                if (coord == 0)
                {
                    draw = false;
                    break;
                }
                else
                {
                    draw = true;
                }
            }
            
            if (coordinates[0].value == 1 && coordinates[1].value == 1 && coordinates[2].value == 1
                || coordinates[0].value == 1 && coordinates[4].value == 1 && coordinates[8].value == 1
                || coordinates[0].value == 1 && coordinates[3].value == 1 && coordinates[6].value == 1
                || coordinates[1].value == 1 && coordinates[4].value == 1 && coordinates[7].value == 1
                || coordinates[2].value == 1 && coordinates[5].value == 1 && coordinates[8].value == 1
                || coordinates[2].value == 1 && coordinates[4].value == 1 && coordinates[6].value == 1
                || coordinates[3].value == 1 && coordinates[4].value == 1 && coordinates[5].value == 1
                || coordinates[6].value == 1 && coordinates[7].value == 1 && coordinates[8].value == 1)
            {
                gameOver = true;
                winnerNum = 1;
                foreach (var playr in activeGame.Players)
                {
                    if (playr.PLayerNumber == winnerNum)
                    {
                        leader = playr;
                        int bothPoints = activeGame.Players[0].Points + activeGame.Players[1].Points;
                        if (activeGame.draws + bothPoints == activeGame.GameRounds)
                        {
                            playr.Points++;
                        }

                        if(activeGame.GameRounds == 0)
                        {
                            playr.Points = 1;
                        }
                    }
                }

                if (thisPlayer.PLayerNumber == 1)
                    ServiceLocator.GetAudio().PlayOnce("winSound", 0.3f);
                else
                    ServiceLocator.GetAudio().PlayOnce("loseSound", 0.3f);
    
                activeGame.Winner = leader.Name;
                roundButton.SetActive(true);

            }
            else if (coordinates[0].value == 2 && coordinates[1].value == 2 && coordinates[2].value == 2
                || coordinates[0].value == 2 && coordinates[4].value == 2 && coordinates[8].value == 2
                || coordinates[0].value == 2 && coordinates[3].value == 2 && coordinates[6].value == 2
                || coordinates[1].value == 2 && coordinates[4].value == 2 && coordinates[7].value == 2
                || coordinates[2].value == 2 && coordinates[5].value == 2 && coordinates[8].value == 2
                || coordinates[2].value == 2 && coordinates[4].value == 2 && coordinates[6].value == 2
                || coordinates[3].value == 2 && coordinates[4].value == 2 && coordinates[5].value == 2
                || coordinates[6].value == 2 && coordinates[7].value == 2 && coordinates[8].value == 2)
            {
                gameOver = true;
                winnerNum = 2;
                foreach (var playr in activeGame.Players)
                {
                    if (playr.PLayerNumber == winnerNum)
                    {
                        leader = playr;
                        int bothPoints = activeGame.Players[0].Points + activeGame.Players[1].Points;
                        if (activeGame.draws + bothPoints == activeGame.GameRounds)
                        {
                            playr.Points++;
                        }

                        if (activeGame.GameRounds == 0)
                        {
                            playr.Points = 1;
                        }
                    }
                }

                if (thisPlayer.PLayerNumber == 2)
                    ServiceLocator.GetAudio().PlayOnce("winSound", 0.3f);
                else
                    ServiceLocator.GetAudio().PlayOnce("loseSound", 0.3f);

                activeGame.Winner = leader.Name;
                roundButton.SetActive(true);

            }
            else if (draw && winnerNum == 0)
            {
                gameOver = true;
                roundButton.SetActive(true);
                ServiceLocator.GetAudio().PlayOnce("drawSound", 0.3f);
            }
    
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i] == clickedCoord)
                {
                    pos = i;
                }
            }
    
            if(clickedCoord != null)
                activeGame.CoordStates[pos] = clickedCoord.value;
            
            activeGame.PlayerTurn = player;
        }
    }

    public void NextRound()
    {
        roundButton.SetActive(false);
        foreach (Coordinate coord in coordinates)
        {
            coord.value = 0;
            coord.clickCount = 0;
            coord.rend.sprite = null;
        }
        drawCheck.Clear();
        for (int i = 0; i < activeGame.CoordStates.Length; i++)
        {
            activeGame.CoordStates[i] = 0;
        }
        activeGame.GameRounds++;
        if (draw)
            activeGame.draws++;
        string data = JsonUtility.ToJson(activeGame);
        SaveManager.Instance.SaveData("/games/" + GameKey.Key, data);
        gameOver = false;
        draw = false;
        winnerNum = 0;
    }
    
    public void MainMenu()
    {
        SaveManager.Instance.gameManager = null;
        SceneManager.LoadScene("MainMenu");
        _instance = null;
        Destroy(this);
    }
}
