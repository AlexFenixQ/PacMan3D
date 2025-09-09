using UnityEngine;

public class PlayerViewModel
{
    float speed;
    IPlayer player;

    public PlayerViewModel(IPlayer player, float speed = 4f)
    {
        this.player = player;
        this.speed = speed;
    }

    public void UpdateMovement(Vector2 input)
    {
        Vector3 dir = new Vector3(input.x, 0, input.y);
        if (dir.sqrMagnitude > 0.01f) player.Move(dir.normalized * speed * Time.deltaTime);
    }
}