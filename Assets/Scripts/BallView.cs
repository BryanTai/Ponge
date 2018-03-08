using UnityEngine;

//Borrowed from https://noobtuts.com/unity/2d-pong-game
public class BallView : PongeElement
{
    //TODO adapt this to AMVCC ... after it works
    private float speed = 5f;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider
        if(col.gameObject.name == "Player 0")
        {
            float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);

            Vector2 dir = new Vector2(x, 1).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        else if (col.gameObject.name == "Player 1")
        {
            float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);

            Vector2 dir = new Vector2(x, -1).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.x - racketPos.x) / racketHeight;
    }
}