
using UnityEngine;

public class PlayerView : PongeElement
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        app.controller.OnPlayerBallHit(collision.gameObject);
    }
}