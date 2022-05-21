using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Index : MonoBehaviour
{
     int PlayerCount;

    public enum GameMode
    {
        PvP,
        VsNpc,
    }
    public GameMode Gamemode;
    void Start()
    {
        Gamemode = GameMode.VsNpc;
    }

    public void SetPlayerCount(int count)
    {
        PlayerCount = count;
    }
    public int GetPlayerCount()
    {
       return PlayerCount;
    }

    public void SetGameMode(GameMode gamemode)
    {
        Gamemode = gamemode;
    }
}
