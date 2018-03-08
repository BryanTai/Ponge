using UnityEngine;

//Borrowed from https://noobtuts.com/unity/2d-pong-game
public class BallView : PongeElement
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player 0")
        {
            float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);

            Vector2 dir = new Vector2(x, 1).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * app.model.ballSpeed;
        }
        else if (col.gameObject.name == "Player 1")
        {
            float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);

            Vector2 dir = new Vector2(x, -1).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * app.model.ballSpeed;
        }

    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.x - racketPos.x) / racketHeight;
    }
}