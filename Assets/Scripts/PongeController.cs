
// Controls the app workflow.
using System;
using UnityEngine;

public class PongeController : PongeElement
{

    void Start()
    {
        //Setting constants
        app.model.bothTouched = false;
        app.model.HalfwayYPixel = Screen.height / 2;

        //Setting up Models and Views
        setUpModel(ref app.model.player0, true);
        setUpModel(ref app.model.player1, false);
        app.model.ballSpeed = 5;

        //Assign Player Models to Views
        app.view.player0.model = app.model.player0;
        app.view.player1.model = app.model.player1;

        
    }

    private void setUpModel(ref PlayerModel playerModel, bool isPlayer0)
    {
        playerModel = new PlayerModel();
        playerModel.isBottomPlayer0 = isPlayer0;
        playerModel.touchId = -1;
    }

    void Update()
    {
        handleTouches();

        if (!app.model.bothTouched)
        {
            if(app.model.player0.touchId != -1 && app.model.player1.touchId != -1)
            {
                app.view.ball.GetComponent<Rigidbody2D>().velocity = Vector2.down * app.model.ballSpeed;
                app.model.bothTouched = true;
            }
        }
    }

    private void handleTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch newTouch = Input.GetTouch(i);
            int newId = newTouch.fingerId;

            if (newId == app.model.player0.touchId)
            {
                if (newTouch.phase == TouchPhase.Ended)
                {
                    app.model.player0.touchId = -1;
                }
                else
                {
                    movePlayerToXPixel(app.view.player0, newTouch.position.x);
                }
            }
            else if (newId == app.model.player1.touchId)
            {
                if (newTouch.phase == TouchPhase.Ended)
                {
                    app.model.player1.touchId = -1;
                }
                else
                {
                    movePlayerToXPixel(app.view.player1, newTouch.position.x);
                }
            }
            else //Touch matches neither saved paddle IDs, check if it's a new one
            {
                if (newTouch.phase == TouchPhase.Began)
                {
                    Debug.Log("NEW TOUCH DETECTED : TouchPosition " + newTouch.position.y);

                    if (newTouch.position.y < app.model.HalfwayYPixel)
                    {
                        app.model.player0.touchId = newTouch.fingerId;
                        movePlayerToXPixel(app.view.player0, newTouch.position.x);
                        Debug.Log("Touched Player 0");
                    }
                    else
                    {
                        app.model.player1.touchId = newTouch.fingerId;
                        movePlayerToXPixel(app.view.player1, newTouch.position.x);
                        Debug.Log("Touched Player 1");
                    }


                }
                //Else just ignore the touch
            }
        }
    }

    //TODO WorldToScreenPoint might be resource intensive, especially in Update

    private void movePlayerToXPixel(PlayerView playerToMove, float xPixel)
    {
        //Debug.Log("Time to MOVE!");
        float playerYPixels = app.view.mainCamera.WorldToScreenPoint(playerToMove.transform.position).y;
        Vector3 newPlayerPixelVector = new Vector3(xPixel, playerYPixels);
        //Debug.Log("Created new Pixel Vector " + newPlayerPixelVector.ToString());
        Vector3 newPlayerWorldVector = app.view.mainCamera.ScreenToWorldPoint(newPlayerPixelVector);
        newPlayerWorldVector.z = 0; //So the player doesn't appear BEHIND the camera...
        //Debug.Log("moving Paddle to WorldSpace Vector " + newPlayerWorldVector.ToString());
        playerToMove.transform.position = newPlayerWorldVector;
        //Debug.Log("OnPlayerTouch COMPLETE!");
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