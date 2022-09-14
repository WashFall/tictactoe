using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameInfo
{
    public List<PlayerData> Players = new List<PlayerData>();
    public string GameID;
    public string GameName;
    public int GameRounds;
    public string Winner;
    public int PlayerTurn;
    public int draws;
    [FormerlySerializedAs("coordStates")] public int[] CoordStates = new int[9];
}
