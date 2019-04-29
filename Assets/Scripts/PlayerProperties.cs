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
                NetworkedObjectsH.find.AddToMasterCreepList(playerNumber - 1, 1);
                currentWallet -= 50;
                currentIncome += 20;
            }
        } 
    }

    public void SpawnLevel2()
    {
        if (currentWallet >= 100)
        {

            if (photonView.IsMine)
            {
                NetworkedObjectsH.find.AddToMasterCreepList(playerNumber - 1, 2);
                currentWallet -= 100;
                currentIncome += 40;
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
