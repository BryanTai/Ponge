public class PlayerView : PongeElement
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        app.controller.OnBallPlayerHit(collision.gameObject);
    }
}