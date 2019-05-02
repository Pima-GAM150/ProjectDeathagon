public class Level5Enemy : Level1Enemy
{
    private void Start()
    {
        this.HitPoints = 55550;
        this.speed = 6;
        this.worth = 5555;
        this.color = 4;
        agent.speed = this.speed;
        SetColor();
    }
}