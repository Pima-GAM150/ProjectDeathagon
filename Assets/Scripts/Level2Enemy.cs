using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Enemy : Level1Enemy
{

    private void Start()
    {
        this.HitPoints = 80;
        this.speed = 3;
        this.worth = 20;
    }
}
