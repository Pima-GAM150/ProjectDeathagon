public class Level4Enemy : Level1Enemy
{
    private void Start()
    {
        this.HitPoints = 5550;
        this.speed = 5;
        this.worth = 555;
        this.color = 3;
        this.damage = 20;
        this.attackSpeed = 2;
        agent.speed = this.speed;
        SetColor();
    }
}