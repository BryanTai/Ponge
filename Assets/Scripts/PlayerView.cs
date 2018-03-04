
using UnityEngine;

public class PlayerView : PongeElement
{
    public PlayerModel model;

    void OnMouseDown()
    {
        app.controller.OnPlayerTouch(model.isBottomPlayer0);
    }

    void OnMouseDrag()
    {
        app.controller.OnPlayerDrag(model.isBottomPlayer0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        app.controller.OnPlayerBallHit(collision.gameObject);
    }
}