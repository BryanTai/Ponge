using UnityEngine;

//Borrowed from https://noobtuts.com/unity/2d-pong-game
public class BallView : PongeElement
{
    public BallModel model;

    void OnCollisionEnter2D(Collision2D col)
    {
        //TODO store name values in Model
        if (col.gameObject.name == "Player 0")
        {
            handlePlayerCollision(col, true);
        }
        else if (col.gameObject.name == "Player 1")
        {
            handlePlayerCollision(col, false);
        }
        else if (col.gameObject.name == "Goal 0")
        {
            handleGoalCollision(true);
        }
        else if (col.gameObject.name == "Goal 1")
        {
            handleGoalCollision(false);
        }

    }

    //TODO maybe combine with the function in PongeController?
    void handlePlayerCollision(Collision2D col, bool collidedWithPlayer0)
    {
        if (model.lastHitPlayer0 == collidedWithPlayer0)
        {
            return;
        }

        float x = hitFactor(transform.position, col.transform.position,
            col.collider.bounds.size.x);
        float yVector = collidedWithPlayer0 ? 1 : -1;

        Vector2 dir = new Vector2(x, yVector).normalized;
        //Debug.Log("Old Velocity: " + GetComponent<Rigidbody2D>().velocity.ToString());
        GetComponent<Rigidbody2D>().velocity = dir * model.speed;

        //At this point, the ball should be moving in the other direction already
        model.lastHitPlayer0 = collidedWithPlayer0;
        app.controller.OnPlayerBallHit(this.gameObject);
        //Debug.Log("New Velocity: " + GetComponent<Rigidbody2D>().velocity.ToString());
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.x - racketPos.x) / racketHeight;
    }

    void handleGoalCollision(bool scoredOnPlayer0)
    {
        app.controller.OnBallScored(this.gameObject, scoredOnPlayer0);
    }
}