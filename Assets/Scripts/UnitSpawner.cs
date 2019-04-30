﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;
using Photon.Realtime;

public class UnitSpawner : MonoBehaviour
{
    public Transform player;

    public List<GameObject> creepPrefabList;
    public List<BoxCollider> unitSpawnsOne;
    public List<BoxCollider> unitSpawnsTwo;
    public List<BoxCollider> unitSpawnsThree;
    public List<BoxCollider> unitSpawnsFour;
    public List<BoxCollider> unitSpawnsFive;
    public List<BoxCollider> unitSpawnsSix;
    public List<BoxCollider> unitSpawnsSeven;
    public List<BoxCollider> unitSpawnsEight;
    public List<List<BoxCollider>> arenaSpawns = new List<List<BoxCollider>>();

    public static UnitSpawner find;
    // singleton assignment
    void Awake()
    {
        find = this;
    }

    public Vector3 GetCreepSpawn(Bounds spawnBounds)
    {
        return new Vector3(UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x), spawnBounds.center.y, UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z));
    }

    [PunRPC]
    public void SpawnCreeps(List<int> creeplist,int playerNumber)
    {
        int counter = 0;
        //if (NetworkedObjectsH.find.players[playerNumber].GetComponent<PlayerProperties>().playerNumber - 1 == playerNumber && creeplist.Count > 0)
        {
            for (int i = 0; i < creeplist.Count; i++)
            {
                GameObject creepToSpawn = Instantiate(creepPrefabList[creeplist[i]]);
                creepToSpawn.transform.position = GetCreepSpawn(arenaSpawns[playerNumber][counter].bounds);
                creepToSpawn.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(creepToSpawn.transform.position);
                creepToSpawn.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = creepToSpawn.GetComponent<Level1Enemy>().speed;
                creepToSpawn.GetComponent<Level1Enemy>().SetDestination(NetworkedObjectsH.find.players[playerNumber].transform);
                
                if (counter == 4) counter = 0;
                else counter++;
            }
        }
    }
}
