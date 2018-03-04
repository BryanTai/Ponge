
// Controls the app workflow.
using System;
using UnityEngine;

public class PongeController : PongeElement
{
    

    void Start()
    {
        //Setting up Models and Views
        app.model.player0 = new PlayerModel();
        app.model.player1 = new PlayerModel();
        app.model.player0.isBottomPlayer0 = true;
        app.model.player1.isBottomPlayer0 = false;

        //Assign Player Models to Views
        app.view.player0.model = app.model.player0;
        app.view.player1.model = app.model.player1;

        //Setting constants
        app.model.HalfwayYPixel = Screen.height / 2;
    }

    //Player first touches the paddle
    internal void OnPlayerTouch(bool isPlayer0)
    {
        //Debug.Log("OnPlayerTouch!!! isPlayer0 is " + isPlayer0);
        moveTouchedPlayerView(isPlayer0);
    }

    //Player drags finger across
    internal void OnPlayerDrag(bool isPlayer0)
    {
        //Debug.Log("DRAGGIN");
        moveTouchedPlayerView(isPlayer0);
        //throw new NotImplementedException();
    }

    private void moveTouchedPlayerView(bool isPlayer0)
    {
        Touch touch = findPlayerTouch(isPlayer0);

        float touchXPixels = touch.position.x;
        PlayerView playerToMove;
        if (isPlayer0)
        {
            playerToMove = app.view.player0;
        }
        else
        {
            playerToMove = app.view.player1;
        }

        //Debug.Log("Time to MOVE!");
        float playerYPixels = app.view.mainCamera.WorldToScreenPoint(playerToMove.transform.position).y;
        Vector3 newPlayerPixelVector = new Vector3(touchXPixels, playerYPixels);
        Debug.Log("Created new Pixel Vector " + newPlayerPixelVector.ToString());
        Vector3 newPlayerWorldVector = app.view.mainCamera.ScreenToWorldPoint(newPlayerPixelVector);
        newPlayerWorldVector.z = 0; //So it doesn't appear BEHIND the camera...
        Debug.Log("moving Paddle to WorldSpace Vector " + newPlayerWorldVector.ToString());
        playerToMove.transform.position = newPlayerWorldVector;
        //Debug.Log("OnPlayerTouch COMPLETE!");
    }

    //Guaranteed to find a Touch
    private Touch findPlayerTouch(bool isPlayer0)
    {
        Debug.Log("Input.touchCount = " + Input.touchCount);

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
        Debug.LogError("findPlayerTouch couldn't find a touch WTF");
        return new Touch(); //TODO make this a halfway point Touch vector :I
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