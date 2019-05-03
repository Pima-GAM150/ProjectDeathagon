using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;
using Photon.Realtime;


public class NetworkedObjectsH : MonoBehaviourPun
{
    public Transform[] spawnPos = new Transform[8];

    public List<PhotonView> players = new List<PhotonView>();
    public List<List<int>> creepList = new List<List<int>>();

    public int myPlayerNumber;

    public int waveNumber = 0;
    public float waveTimer = 5;

    public static NetworkedObjectsH find;

    int seed; // only matters on the master client

    // singleton assignment
    void Awake()
    {
        find = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // try to create a truly random number to use as your starting random seed
            seed = DateTime.Now.Millisecond + System.Threading.Thread.CurrentThread.GetHashCode();
        }
        PhotonNetwork.Instantiate("Player2", spawnPos[players.Count].position, Quaternion.identity, 0);
    }

    private void Update()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for(int i = 0; i < players.Count; i++)
                {
                    players[i].RPC("PayPlayers", RpcTarget.All);
                    players[i].RPC("SyncWaveTimer", RpcTarget.All,30);
                    if (i == 0) UnitSpawner.find.SpawnCreeps(creepList[players.Count - 1], i);
                    else UnitSpawner.find.SpawnCreeps(creepList[i - 1], i);
                }
                for (int i = 0; i < creepList.Count; i++)
                {
                    creepList[i].Clear();
                }
                waveNumber += 1;
            }
            
        }
    }

    

    public void AddPlayer(PhotonView player)
    {
        // add a player to the list of all tracked players
        players.Add(player);
        creepList.Add(new List<int>());
        // only the "server" has authority over which color the player should be and its seed
        
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(players.Count);
            player.RPC("SetColor", RpcTarget.AllBuffered, players.Count - 1); // buffer the color change so it applies to new arrivals in the room
            player.RPC("SetPosition", RpcTarget.AllBuffered, spawnPos[players.Count - 1].position);
            player.RPC("SetPlayerNumber", RpcTarget.AllBuffered, players.Count);
            UnitSpawner.find.aliveCreeps.Add(new List<PhotonView>());
        }
    }

    public void RemoveMe(int playerIndex)

    {
        if (PhotonNetwork.IsMasterClient) { creepList.RemoveAt(playerIndex); players.RemoveAt(playerIndex); }
        for (int i = playerIndex+1; i < players.Count; i++)
        {
            players[i].RPC("ShiftPlayerNumber", RpcTarget.All);
        }
    }

    [PunRPC]
    public void AddToMasterCreepList(int playerNumber,int creep) { creepList[playerNumber].Add(creep); }

    [PunRPC]
    public void DestroyMePlayer(int playerIndex)
    {
        for (int i = UnitSpawner.find.aliveCreeps[playerIndex].Count - 1; i >= 0; i--)
        {
            UnitSpawner.find.aliveCreeps[playerIndex][i].RPC("DestroyMe", RpcTarget.MasterClient,playerIndex);
        }
        RemoveMe(playerIndex);
    }

    public void SyncWaveTimer(int timer)
    {
        waveTimer = timer;
    }
}
