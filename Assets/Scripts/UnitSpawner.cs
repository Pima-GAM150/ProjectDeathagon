using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;
using Photon.Realtime;

public class UnitSpawner : MonoBehaviour
{
    public Transform player;

    public List<List<PhotonView>> aliveCreeps = new List<List<PhotonView>>();

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
        return new Vector3(UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x), spawnBounds.center.y - 3, UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z));
    }

    //[PunRPC]
    public void SpawnCreeps(List<int> creeplist,int playerNumber)
    {
        int counter = 0;
        if (creeplist.Count > 0)
        {
            for (int i = 0; i < creeplist.Count; i++)
            {
                GameObject creepToSpawn = PhotonNetwork.Instantiate("Level"+creeplist[i].ToString(), GetCreepSpawn(arenaSpawns[playerNumber][counter].bounds),Quaternion.identity,0);
                creepToSpawn.GetComponent<PhotonView>().RPC("SetDestination", RpcTarget.All, playerNumber);
                
                if (counter == 4) counter = 0;
                else counter++;
            }
        }
    }
}
