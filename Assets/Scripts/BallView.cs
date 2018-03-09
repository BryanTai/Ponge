using UnityEngine;

//Borrowed from https://noobtuts.com/unity/2d-pong-game
public class BallView : PongeElement
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player 0")
        {
            handleCollision(col, 1);
        }
        else if (col.gameObject.name == "Player 1")
        {
            handleCollision(col, -1);
        }

    }

    void handleCollision(Collision2D col, int yVector)
    {
        float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);

        Vector2 dir = new Vector2(x, yVector).normalized;
        Debug.Log("Old Velocity: " + GetComponent<Rigidbody2D>().velocity.ToString());
        GetComponent<Rigidbody2D>().velocity = dir * app.model.ballSpeed;

        //At this point, the ball should be moving in the other direction already

        app.controller.OnPlayerBallHit(this.gameObject);
        //TODO what info should be sent to Controller?
        Debug.Log("New Velocity: " + GetComponent<Rigidbody2D>().velocity.ToString());
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.x - racketPos.x) / racketHeight;
    }
}