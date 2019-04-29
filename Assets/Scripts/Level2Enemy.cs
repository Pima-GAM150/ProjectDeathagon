using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Enemy : Level1Enemy
{
    public Level2Enemy()
    {
        this.HitPoints = 50;
        this.speed = 2;
        this.worth = 10;
    }
}
