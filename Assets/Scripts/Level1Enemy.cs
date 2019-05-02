using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Level1Enemy : MonoBehaviourPun , IPunObservable, IPunInstantiateMagicCallback
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform Destination;

    public Transform appearance;

    private Transform target;

    public float HitPoints;

    public float speed;

    public float worth;

    public float damage;

    public float attackSpeed;
    public float attack;

    Vector3 lastSyncedPos;

    public Color[] enemyColors;
    public Color currentColor { get; set; }
    public int color = 0;
    public Renderer rend;

    public Level1Enemy()
    {
        this.HitPoints = 50;
        this.speed = 2;
        this.worth = 5;
        this.damage = 5;
        this.attackSpeed = 2;
    }

    private void Start()
    {
        this.attack = this.attackSpeed;
        agent.speed = this.speed;
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        target = transform;
        agent.destination = Destination.position;
        appearance.position = Vector3.Lerp(appearance.position, target.position, speed * Time.deltaTime);
        if (attack >= 0) attack -= Time.deltaTime;
        if (Vector3.Distance(transform.position, Destination.position) < 2 && attack <= 0)
        {
            attack = this.attackSpeed;
            Destination.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
        }
    }

    [PunRPC]
    public void SetDestination(int destination)
    {
        Destination = NetworkedObjectsH.find.players[destination].transform;
        UnitSpawner.find.aliveCreeps[destination].Add(this.photonView);
        
    }

    void OnTriggerEnter(Collider col)
    {
        Bullet bullet = col.transform.GetComponent<Bullet>();
        if( bullet ) { HitPoints -= bullet.bulletDamage; }
        if (HitPoints <= 0)
        {
            Destination.GetComponent<PlayerProperties>().KillEnemy(10);
            photonView.RPC("DestroyMe", RpcTarget.MasterClient);
        }
    }

    public void SetColor()
    {
        currentColor = enemyColors[color];
        rend.material.color = currentColor;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            // don't send redundant data, like an unchanged position, over the network
            if (lastSyncedPos != transform.position)
            {
                lastSyncedPos = transform.position;

                // since there is new position data, serialize it to the data stream
                stream.SendNext(transform.position);
            }
        }
        else
        {
            // receive data from the stream in *the same order* in which it was originally serialized
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    public PhotonView getThisPhotonView() { return this.photonView; }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        agent.Warp(transform.position);
    }

    [PunRPC]
    public void DestroyMe()
    {
        UnitSpawner.find.aliveCreeps[Destination.GetComponent<PlayerController>().playerNumber - 1].Remove(this.photonView);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
