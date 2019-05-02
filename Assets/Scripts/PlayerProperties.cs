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
    public int playerAmmoCapacity = 6;

    public float playerHealth = 100;
    public float playerArmor = 0;

    public int bdL = 0;
    public int frL = 0;
    public int rsL = 0;
    public int acL = 0;

    private void Start()
    {
        if (photonView.IsMine)
        {
            UnitSpawner.find.player = transform;
            UpdateFireRate();
            UpdateAmmoCapacity();
            UpdateBulletDamage();
            UpdateReloadSpeed();
        }
    }

    public void KillEnemy(int worth)
    {
        currentWallet += worth;
    }

    public void UpdatePlayerHealth() { GetComponent<PlayerController>().playerHealth = this.playerHealth; }

    public void UpdatePlayerArmor() { GetComponent<PlayerController>().playerArmor = this.playerArmor; }

    [PunRPC]
    public void TakeDamage(float dam) { this.playerHealth -= dam; }

    public void BuyBulletDamageUpgrade()
    {
        switch (bdL)
        {
            case 0: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; this.playerBulletDamage = 75; UpdateBulletDamage(); break;
            case 1: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; this.playerBulletDamage = 250; UpdateBulletDamage(); break;
            case 2: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; this.playerBulletDamage = 500; UpdateBulletDamage(); break;
            case 3: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; this.playerBulletDamage = 5000; UpdateBulletDamage(); break;
            case 4: currentWallet -= int.Parse(GetBulletDamageUpgradeCost()); bdL++; this.playerBulletDamage = 50000; UpdateBulletDamage(); break;
        }
    }

    public void UpdateBulletDamage() { GetComponent<PlayerController>().bulletDamage = this.playerBulletDamage; }

    public string GetBulletDamageUpgradeCost()
    {
        switch (bdL)
        {
            case 0: return 500.ToString();
            case 1: return 5000.ToString();
            case 2: return 50000.ToString();
            case 3: return 500000.ToString();
            case 4: return 5000000.ToString();
            case 5: return "Max";
        }
        return "what";
    }

    public void BuyFireRateUpgrade()
    {
        switch (frL)
        {
            case 0: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; this.playerFireRate = .8f; UpdateFireRate(); break;
            case 1: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; this.playerFireRate = .6f; UpdateFireRate(); break;
            case 2: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; this.playerFireRate = .4f; UpdateFireRate(); break;
            case 3: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; this.playerFireRate = .2f; UpdateFireRate(); break;
            case 4: currentWallet -= int.Parse(GetFireRateUpgradeCost()); frL++; this.playerFireRate = .1f; UpdateFireRate(); break;
        }
    }

    public void UpdateFireRate() { GetComponent<PlayerController>().fireRate = this.playerFireRate; }

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
            case 0: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; this.playerReloadSpeed = 4; UpdateReloadSpeed(); break;
            case 1: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; this.playerReloadSpeed = 3; UpdateReloadSpeed(); break;
            case 2: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; this.playerReloadSpeed = 2; UpdateReloadSpeed(); break;
            case 3: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; this.playerReloadSpeed = 1; UpdateReloadSpeed(); break;
            case 4: currentWallet -= int.Parse(GetReloadSpeedUpgradeCost()); rsL++; this.playerReloadSpeed = 0; UpdateReloadSpeed(); break;
        }
    }

    public void UpdateReloadSpeed() { GetComponent<PlayerController>().reloadSpeed = this.playerReloadSpeed; }

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
            case 0: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; this.playerAmmoCapacity = 8; UpdateAmmoCapacity(); break;
            case 1: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; this.playerAmmoCapacity = 10; UpdateAmmoCapacity(); break;
            case 2: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; this.playerAmmoCapacity = 12; UpdateAmmoCapacity(); break;
            case 3: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; this.playerAmmoCapacity = 14; UpdateAmmoCapacity(); break;
            case 4: currentWallet -= int.Parse(GetAmmoCapacityUpgradeCost()); acL++; this.playerAmmoCapacity = 16; UpdateAmmoCapacity(); break;
        }
    }

    public void UpdateAmmoCapacity() { GetComponent<PlayerController>().ammoCapacity = this.playerAmmoCapacity; }

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
