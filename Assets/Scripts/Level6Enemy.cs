public class Level6Enemy : Level1Enemy
{
    private void Start()
    {
        this.HitPoints = 555550;
        this.speed = 7;
        this.worth = 55555;
        this.color = 6;
        this.damage = 30;
        this.attackSpeed = 2;
        agent.speed = this.speed;
        SetColor();
    }
}