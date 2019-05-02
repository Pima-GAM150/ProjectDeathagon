public class Level2Enemy : Level1Enemy
{
    private void Start()
    {
        this.HitPoints = 550;
        this.speed = 3;
        this.worth = 55;
        this.color = 1;
        agent.speed = this.speed;
        SetColor();
    }
}
