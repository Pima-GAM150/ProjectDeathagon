using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerProperties : MonoBehaviourPun
{
    public int playerNumber { get; set; }
    public int currentWallet;
    public float currentIncome { get; set; }
    public float playerBulletDamage = 25;
    public float playerFireRate = 1;
    public float playerReloadSpeed = 5;
    public float playerAmmoCapacity = 6;

    private void Start()
    {
        if (photonView.IsMine) UnitSpawner.find.player = transform;
    }

    public void KillEnemy(int worth)
    {
        currentWallet += worth;
    }

    public void SpawnLevel1()
    {
        if (currentWallet >= 50)
        {
            
            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC( "AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 0);
                currentWallet -= 50;
                currentIncome += 20;
            }
        } 
    }

    public void SpawnLevel2()
    {
        if (currentWallet >= 500)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC("AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 1);
                currentWallet -= 500;
                currentIncome += 210;
            }
        }
    }

    public void SpawnLevel3()
    {
        if (currentWallet >= 5000)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC("AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 2);
                currentWallet -= 5000;
                currentIncome += 2200;
            }
        }
    }

    public void SpawnLevel4()
    {
        if (currentWallet >= 50000)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC("AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 3);
                currentWallet -= 50000;
                currentIncome += 23000;
            }
        }
    }

    public void SpawnLevel5()
    {
        if (currentWallet >= 500000)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC("AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 4);
                currentWallet -= 500000;
                currentIncome += 240000;
            }
        }
    }

    public void SpawnLevel6()
    {
        if (currentWallet >= 5000000)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.photonView.RPC("AddToMasterCreepList", RpcTarget.MasterClient, playerNumber - 1, 5);
                currentWallet -= 5000000;
                currentIncome += 2500000;
            }
        }
    }

    [PunRPC]
    public void PayPlayers()
    {
        currentWallet += Mathf.RoundToInt(currentIncome);
    }

    [PunRPC]
    public void SetPlayerNumber(int number)
    {
        Debug.Log(number);
        playerNumber = number;
        GetComponent<PlayerController>().playerNumber = number;
    }
}
