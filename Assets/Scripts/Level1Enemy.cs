using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform Destination;

    public float HitPoints;

    public float speed;

    public float worth;

    public Level1Enemy()
    {
        this.HitPoints = 50;
        this.speed = 2;
        this.worth = 10;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Destination.position;
    }

    public void SetDestination(Transform destination)
    {
        Destination = destination;
    }

    void OnTriggerEnter(Collider col)
    {
        Bullet bullet = col.transform.GetComponent<Bullet>();
        if( bullet ) { HitPoints -= bullet.bulletDamage; }
        if (HitPoints <= 0)
        {
            Destination.GetComponent<PlayerProperties>().KillEnemy(10);
            Destroy(this.gameObject);
        }
    }
}
