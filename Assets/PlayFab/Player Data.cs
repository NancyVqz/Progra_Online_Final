using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    [SerializeField] private Data[] data;
    public Dictionary<string, string> playerData = new Dictionary<string, string>();

    public override async void Spawned() 
    {
        if (Object.HasInputAuthority)
        {
            var result = await PlayFabManager._PlayfabManager.GetUserData();
            int wins = result.Data.ContainsKey("Wins") ? int.Parse(result.Data["Wins"].Value) : 0;
            int losses = result.Data.ContainsKey("Losses") ? int.Parse(result.Data["Losses"].Value) : 0;

            SaveUsername(); //guardar el username como un player data

            Rpc_SetUsername(Runner.LocalPlayer.PlayerId, PlayFabManager._PlayfabManager.username);
            Rpc_SetStats(Runner.LocalPlayer.PlayerId, wins, losses);
        }
    }

    public void SaveUsername()
    {
        string username = PlayFabManager._PlayfabManager.username;
        UpdateClassData("Username", username);
        Debug.Log("Se guardo: " + username);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void Rpc_SetUsername(int playerId, string username)
    {
        if (playerId == 1)
        {
            ScoreManager.instance.player1Username = username;
        }
        else if (playerId == 2) 
        { 
            ScoreManager.instance.player2Username = username; 
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void Rpc_SetStats(int playerId, int wins, int losses)
    {
        if (playerId == 1)
        {
            ScoreManager.instance.player1Victories = wins;
            ScoreManager.instance.player1Losses = losses;
        }
        else if (playerId == 2)
        {
            ScoreManager.instance.player2Victories = wins;
            ScoreManager.instance.player2Losses = losses;
        }
    }

    //Metodo para cambiar un valor de la classe en codigo
    public void UpdateClassData(string statName, string newValue)
    {
        Data name = Array.Find(data, stat => stat.name == statName);

        if (name != null)
        {
            name.value = newValue;
            SavePlayerData();
        }
    }

    [ContextMenu("Update Stats")]
    public void SavePlayerData()
    {
        foreach (Data stat in data)
        {
            if (playerData.ContainsKey(stat.name))
            {
                playerData[stat.name] = stat.value;
            }
            else
            {
                playerData.Add(stat.name, stat.value);
            }
        }
        PlayFabManager._PlayfabManager.UpdateData(playerData);
    }

    [Serializable]
    public class Data
    {
        public string name; //Wins  Losses  Username
        public string value;
    }
}
