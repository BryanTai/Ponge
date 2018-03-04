
// Controls the app workflow.
using System;
using UnityEngine;

public class PongeController : PongeElement
{
    

    void Start()
    {
        app.model.player0 = new PlayerModel();
        app.model.player1 = new PlayerModel();
        app.model.player0.isBottomPlayer0 = true;
        app.model.player1.isBottomPlayer0 = false;

        app.model.HalfwayYPixel = Screen.height / 2;
        
    }

    //Player is touching the paddle
    internal void OnPlayerTouch(bool isPlayer0)
    {
        Touch touch = findPlayerTouch(isPlayer0);

        float touchXPixels = touch.position.x;
        PlayerView playerToMove;
        if (isPlayer0)
        {
            playerToMove = app.view.player0;
        }else
        {
            playerToMove = app.view.player1;
        }

        float playerYPixels = app.view.mainCamera.WorldToScreenPoint(playerToMove.transform.position).y;
        Vector3 newPlayerWorldVector = app.view.mainCamera.ScreenToWorldPoint(new Vector3(touchXPixels, playerYPixels));

        playerToMove.transform.position = newPlayerWorldVector;
    }

    //Guaranteed to find a Touch
    private Touch findPlayerTouch(bool isPlayer0)
    {
        if (isPlayer0)
        {
            foreach (Touch touch in Input.touches)
            {
                if(touch.position.y < app.model.HalfwayYPixel)
                {
                    return touch;
                }
            }
        } else
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.y > app.model.HalfwayYPixel)
                {
                    return touch;
                }
            }
        }
        //This should never happen
        return Input.GetTouch(0);
    }

    //Move the paddle view 
    internal void OnPlayerDrag()
    {
        throw new NotImplementedException();
    }

    public void OnPlayerBallHit(GameObject collider)
    {
        //Create a new ball here
        if (app.model.totalBalls < app.model.maxBalls)
        {
            app.model.totalBalls++;
            CloneBallWithAngle();
            //TODO might need to delay the cloning to prevent more than one from being created on a single collision
        }
        throw new NotImplementedException();
    }

    //Clone the collided ball moving at a slightly different angle
    private void CloneBallWithAngle()
    {
        throw new NotImplementedException();
    }
}