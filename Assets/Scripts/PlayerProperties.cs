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

    public int bdL = 0;
    public int frL = 0;
    public int rsL = 0;
    public int acL = 0;

    private void Start()
    {
        if (photonView.IsMine) UnitSpawner.find.player = transform;
    }

    public void KillEnemy(int worth)
    {
        currentWallet += worth;
    }

    public void BuyBulletDamageUpgrade()
    {
        switch (bdL)
        {
            case 0: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; playerBulletDamage = 75; break;
            case 1: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; playerBulletDamage = 225; break;
            case 2: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; playerBulletDamage = 675; break;
            case 3: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; playerBulletDamage = 2025; break;
            case 4: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; playerBulletDamage = 6075; break;
        }
    }

    public string GetBulletDamageUpgradeCost()
    {
        switch (bdL)
        {
            case 0: return 500.ToString();
            case 1: return 1500.ToString();
            case 2: return 4500.ToString();
            case 3: return 13500.ToString();
            case 4: return 40500.ToString();
            case 5: return "Max";
        }
        return "what";
    }

    public void BuyFireRateUpgrade()
    {
        switch (frL)
        {
            case 0: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; playerFireRate = .8f; break;
            case 1: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; playerFireRate = .6f; break;
            case 2: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; playerFireRate = .4f; break;
            case 3: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; playerFireRate = .2f; break;
            case 4: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; playerFireRate = .1f; break;
        }
    }

    public string GetFireRateUpgradeCost()
    {
        switch (frL)
        {
            case 0: return 500.ToString();
            case 1: return 1500.ToString();
            case 2: return 4500.ToString();
            case 3: return 13500.ToString();
            case 4: return 40500.ToString();
            case 5: return "Max";
        }
        return "what";
    }

    public void BuyReloadSpeedUpgrade()
    {
        switch (rsL)
        {
            case 0: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; playerReloadSpeed = 4; break;
            case 1: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; playerReloadSpeed = 3; break;
            case 2: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; playerReloadSpeed = 2; break;
            case 3: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; playerReloadSpeed = 1; break;
            case 4: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; playerReloadSpeed = 0; break;
        }
    }

    public string GetReloadSpeedUpgradeCost()
    {
        switch (rsL)
        {
            case 0: return 500.ToString();
            case 1: return 1500.ToString();
            case 2: return 4500.ToString();
            case 3: return 13500.ToString();
            case 4: return 40500.ToString();
            case 5: return "Max";
        }
        return "what";
    }

    public void BuyAmmoCapacityUpgrade()
    {
        switch (acL)
        {
            case 0: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; playerAmmoCapacity = 8; break;
            case 1: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; playerAmmoCapacity = 10; break;
            case 2: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; playerAmmoCapacity = 12; break;
            case 3: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; playerAmmoCapacity = 14; break;
            case 4: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; playerAmmoCapacity = 16; break;
        }
    }

    public string GetAmmoCapacityUpgradeCost()
    {
        switch (acL)
        {
            case 0: return 500.ToString();
            case 1: return 1500.ToString();
            case 2: return 4500.ToString();
            case 3: return 13500.ToString();
            case 4: return 40500.ToString();
            case 5: return "Max";
        }
        return "what";
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
