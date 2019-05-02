public class Level3Enemy : Level1Enemy
{
    private void Start()
    {
        this.HitPoints = 550;
        this.speed = 4;
        this.worth = 55;
        this.color = 2;
        this.damage = 15;
        this.attackSpeed = 2;
        agent.speed = this.speed;
        SetColor();
    }
}