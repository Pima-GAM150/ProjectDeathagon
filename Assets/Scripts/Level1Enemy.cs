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

    Vector3 lastSyncedPos;

    public Level1Enemy()
    {
        this.HitPoints = 50;
        this.speed = 2;
        this.worth = 5;
    }

    // Update is called once per frame
    void Update()
    {
        target = transform;
        agent.destination = Destination.position;
        appearance.position = Vector3.Lerp(appearance.position, target.position, speed * Time.deltaTime);
        agent.speed = this.speed;
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
        PhotonNetwork.Destroy(this.gameObject);
    }
}
