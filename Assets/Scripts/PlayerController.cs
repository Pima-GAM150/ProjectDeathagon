﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public Text playerNumberText;
    public Text incomeText;
    public Text walletText;
    public Text waveTimer;

    public RectTransform panelSendCreeps;

    public RectTransform panelUpgrades;

    public Text textHealth;
    public Text textArmor;
    public Text textAmmo;
    public Text textReloadSpeed;
    public Text textFireRate;
    public Text textBulletDamage;
    public Text textAmmoCapacity;
    public Text buttonTextReloadSpeed;
    public Text buttonTextFireRate;
    public Text buttonTextBulletDamage;
    public Text buttonTextAmmoCapacity;
    public Text textWin;

    public float fireRate;
    public float bulletDamage;
    public float reloadSpeed;
    public int ammoCapacity;

    public float playerHealth;
    public float playerArmor;

    public UnityEngine.AI.NavMeshAgent agent;

    public Camera playerCamera;

    public Transform target;

    public Transform appearance;

    public Transform NavMeshTarget;

    public float speed = 10;

    Vector3 lastSyncedPos;

    public int playerNumber;

    public GameObject Bullet;
    private GameObject bullet;

    public Canvas gameUI;
    public Canvas spectatorUI;

    public bool testing;

    public bool shoot = true;
    public bool reloadShoot = true;
    

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    // Start is called before the first frame update
    void Start()
    {
        testing = false;
        if (!testing) Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine)
        {
            transform.GetComponent<PlayerProperties>().currentIncome = 200;
            transform.GetComponent<PlayerProperties>().currentWallet = 200;
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsOne);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsTwo);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsThree);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsFour);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsFive);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsSix);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsSeven);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsEight);
            panelSendCreeps.gameObject.SetActive(false);
            panelUpgrades.gameObject.SetActive(false);
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerNumberText.text = "Player: " + playerNumber;
        if (photonView.IsMine)
        {
            if (NetworkedObjectsH.find.waveNumber > 2 && NetworkedObjectsH.find.players.Count == 1) textWin.enabled = true;
            if (reloadSpeed >= 0) reloadSpeed -= Time.deltaTime;
            if (reloadSpeed < 0) reloadShoot = true;
            if (fireRate >= 0) fireRate -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Q) && !panelUpgrades.gameObject.activeInHierarchy)
            {
                if (!panelSendCreeps.gameObject.activeInHierarchy)
                {
                    panelSendCreeps.gameObject.SetActive(true);
                    shoot = false;
                    if (!testing) Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    panelSendCreeps.gameObject.SetActive(false);
                    shoot = true;
                    if (!testing) Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !panelSendCreeps.gameObject.activeInHierarchy)
            {
                if (!panelUpgrades.gameObject.activeInHierarchy)
                {
                    panelUpgrades.gameObject.SetActive(true);
                    shoot = false;
                    if (!testing) Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    panelUpgrades.gameObject.SetActive(false);
                    shoot = true;
                    if (!testing) Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (testing)
            {
                if (Input.GetMouseButton(1))
                {
                    var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                    var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                    playerCamera.transform.eulerAngles -= new Vector3(mouseMovement.y * mouseSensitivityFactor, 0f, 0f);
                    transform.eulerAngles += new Vector3(0f, mouseMovement.x * mouseSensitivityFactor, 0f);
                }
            }
            else
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                playerCamera.transform.eulerAngles -= new Vector3(mouseMovement.y * mouseSensitivityFactor, 0f, 0f);
                transform.eulerAngles += new Vector3(0f, mouseMovement.x * mouseSensitivityFactor, 0f);
            }

            NavMeshTarget.position = target.position;
            RotateDestination(GetDestination());
            agent.destination = NavMeshTarget.position;

            if (Input.GetKeyDown(KeyCode.R)) ReloadGun();

            if (Input.GetMouseButtonDown(0) && shoot && reloadShoot && fireRate <= 0 && ammoCapacity >= 1)
            {
                bullet = Instantiate(Bullet);
                bullet.transform.GetComponent<Bullet>().bulletDamage = bulletDamage;
                bullet.transform.position = transform.position + Vector3.up;
                bullet.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                GetComponent<PlayerProperties>().UpdateFireRate();
                ammoCapacity--;
            }
            else if (ammoCapacity == 0) ReloadGun();

            SetUIText();
            if (playerHealth <= 0)
            {
                gameUI.gameObject.SetActive(false);
                spectatorUI.gameObject.SetActive(true);
                agent.enabled = false;
                GetComponent<Spectator>().enabled = true;
                GetComponent<Spectator>().Warp();
                Cursor.lockState = CursorLockMode.None;
                this.photonView.RPC("DestroyMe", RpcTarget.All);
            }
        }
        else
        {
            // interpolate from the renderer's current position to its ideal position
            appearance.position = Vector3.Lerp(appearance.position, target.position, speed * Time.deltaTime);
        } 
    }

    public void ReloadGun()
    {
        reloadShoot = false;
        GetComponent<PlayerProperties>().UpdateReloadSpeed();
        GetComponent<PlayerProperties>().UpdateAmmoCapacity();
    }

    private void SetUIText()
    {
        incomeText.text = "Income: " + transform.GetComponent<PlayerProperties>().currentIncome;
        walletText.text = "Wallet: " + transform.GetComponent<PlayerProperties>().currentWallet;
        textReloadSpeed.text = "Current Reload Speed: " + GetComponent<PlayerProperties>().playerReloadSpeed + " Seconds";
        textAmmoCapacity.text = "Current Ammo Capacity: " + GetComponent<PlayerProperties>().playerAmmoCapacity;
        textBulletDamage.text = "Current Bullet Damage: " + GetComponent<PlayerProperties>().playerBulletDamage;
        textFireRate.text = "Current Fire Rate: " + (1 / GetComponent<PlayerProperties>().playerFireRate) + "/bps";
        buttonTextReloadSpeed.text = "ReloadSpeed: $" + GetComponent<PlayerProperties>().GetReloadSpeedUpgradeCost();
        buttonTextAmmoCapacity.text = "Ammo Capacity: $" + GetComponent<PlayerProperties>().GetAmmoCapacityUpgradeCost();
        buttonTextBulletDamage.text = "Bullet Damage: $" + GetComponent<PlayerProperties>().GetBulletDamageUpgradeCost();
        buttonTextFireRate.text = "Fire Rate: $" + GetComponent<PlayerProperties>().GetFireRateUpgradeCost();
        waveTimer.text = System.Math.Round(NetworkedObjectsH.find.waveTimer, 2).ToString();
        if (ammoCapacity == 0) textAmmo.text = "Ammo: Press R to reload";
        else textAmmo.text = "Ammo: " + ammoCapacity;
        GetComponent<PlayerProperties>().UpdatePlayerHealth();
        GetComponent<PlayerProperties>().UpdatePlayerArmor();
        textHealth.text = "Current Health: " + playerHealth;
        textArmor.text = "Current Armor: " + playerArmor;
    }

    public Vector3 GetDestination()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal") * 10,0f, Input.GetAxis("Vertical") * 10);
        if (Input.GetKey(KeyCode.LeftShift)) agent.speed = 7f;
        else agent.speed = 3.5f;
        return direction * Time.deltaTime * 8;
    }

    public void RotateDestination(Vector3 direction)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * direction;

        NavMeshTarget.transform.position += rotatedTranslation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            // don't send redundant data, like an unchanged position, over the network
            if (lastSyncedPos != target.position)
            {
                lastSyncedPos = target.position;

                // since there is new position data, serialize it to the data stream
                stream.SendNext(target.position);
            }
        }
        else
        {
            // receive data from the stream in *the same order* in which it was originally serialized
            target.position = (Vector3)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void DestroyMe()
    {   
        NetworkedObjectsH.find.photonView.RPC("DestroyMePlayer", RpcTarget.MasterClient, playerNumber - 1);
        GetComponent<PlayerController>().enabled = false;
    }

    [PunRPC]
    public void PhotonDestroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        agent.Warp(position);
    }

    [PunRPC]
    public void SyncWaveTimer(int timer)
    {
        NetworkedObjectsH.find.SyncWaveTimer(timer);
    }
}
