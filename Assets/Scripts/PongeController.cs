
// Controls the app workflow.
using System;
using UnityEngine;
using UnityEngine.UI;

public class PongeController : PongeElement
{
    System.Random rnd;
    void Start()
    {
        //Setting constants
        app.model.bothTouched = false;
        app.model.HalfwayYPixel = Screen.height / 2;
        rnd = new System.Random();

        //Setting up Models and Views
        setUpPlayerModel(ref app.model.player0, true);
        setUpPlayerModel(ref app.model.player1, false);
        app.model.totalBalls = 1;
        app.model.maxBalls = 50; //TODO adjust this...or remove it?

        setUpScoreText(ref app.view.player0Score);
        setUpScoreText(ref app.view.player1Score);

        //Assign Player Models to Views
        app.view.player0.model = app.model.player0;
        app.view.player1.model = app.model.player1;

        app.view.firstBall.model = createBallModelFromBallType(BallType.Regular);

        //TODO JUST FOR DESKTOP TESTING
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Running on Editor!");
            startTheGame();
        }
    }

    private void setUpScoreText(ref Text scoreText)
    {
        scoreText.text = "0";
        RectTransform textRect = scoreText.GetComponent<RectTransform>();
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.5f);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.5f);
    }

    private void setUpPlayerModel(ref PlayerModel playerModel, bool isPlayer0)
    {
        playerModel = new PlayerModel();
        playerModel.isBottomPlayer0 = isPlayer0;
        playerModel.touchId = -1;
        playerModel.score = 0;
    }

    void Update()
    {
        handleTouches();

        if (!app.model.bothTouched)
        {
            if(app.model.player0.touchId != -1 && app.model.player1.touchId != -1)
            {
                startTheGame();
            }
        }
    }

    private void startTheGame()
    {
        app.view.firstBall.GetComponent<Rigidbody2D>().velocity = Vector2.down * BallModel.defaultSpeed;
        app.model.bothTouched = true;
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
        float playerYPixels = app.view.mainCamera.WorldToScreenPoint(playerToMove.transform.position).y;
        Vector3 newPlayerPixelVector = new Vector3(xPixel, playerYPixels);
        Vector3 newPlayerWorldVector = app.view.mainCamera.ScreenToWorldPoint(newPlayerPixelVector);
        newPlayerWorldVector.z = 0; //So the player doesn't appear BEHIND the camera...
        playerToMove.transform.position = newPlayerWorldVector;
    }

    public void OnPlayerBallHit(GameObject ball, bool lastHitPlayer0)
    {
        //Create a new ball here
        if (app.model.totalBalls < app.model.maxBalls)
        {
            //TODO delay the cloning to prevent more than one from being created on a single collision
            app.model.totalBalls++;
            SpawnNewBall(ball, lastHitPlayer0);
            //TODO create an explosion of balls on a special hit
        }
    }

    //Clone the collided ball moving at a slightly different angle
    private void SpawnNewBall(GameObject originalBall, bool lastHitPlayer0)
    {
        Vector3 newPosition = originalBall.transform.position;
        GameObject newBall = Instantiate(app.model.BallPrefab, newPosition, Quaternion.identity);
        BallModel newModel = createBallModelFromBallType(BallType.Regular); //TODO pick a random one
        newModel.lastHitPlayer0 = lastHitPlayer0;
        newBall.GetComponent<BallView>().model = newModel;

        Vector2 originalVelocity = originalBall.GetComponent<Rigidbody2D>().velocity;

        float xShift = 0.01f; //TODO adjust this
        Vector2 newDir = new Vector2(originalVelocity.x + xShift, originalVelocity.y).normalized;
        newBall.GetComponent<Rigidbody2D>().velocity = newDir * newModel.speed;
        newBall.GetComponent<SpriteRenderer>().color = newModel.color;
        Debug.Log("NEW BALL!");
        //Debug.Log("New Ball Direction: " + newDir);
        //Debug.Log("New Ball Velocity: " + newBall.GetComponent<Rigidbody2D>().velocity.ToString());
        
    }

    public void OnBallScored(GameObject ball, bool scoredOnPlayer0)
    {
        if (scoredOnPlayer0)
        {
            app.model.player0.score++;
            app.view.player0Score.text = app.model.player0.score.ToString();
            app.view.player0Score.color = ball.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            app.model.player1.score++;
            app.view.player1Score.text = app.model.player1.score.ToString();
            app.view.player1Score.color = ball.GetComponent<SpriteRenderer>().color;
        }
        app.model.totalBalls--;
        Destroy(ball);
    }

    private BallModel createBallModelFromBallType(BallType ballType)
    {
        switch (ballType)
        {
            case BallType.Regular:
                return new BallModel(BallModel.defaultSpeed, Color.white, BallModel.defaultSize, ballType);
            case BallType.Fast:
                return new BallModel(BallModel.defaultSpeed * 2, Color.red, BallModel.defaultSize, ballType);
            case BallType.Large:
                return new BallModel(BallModel.defaultSpeed * 0.8f, Color.blue, BallModel.defaultSize * 5, ballType);
            case BallType.Spread:
                return new BallModel(BallModel.defaultSpeed, Color.green, BallModel.defaultSize, ballType);
            default:
                Debug.Log("Invalid BallType, defaulting to regular");
                return new BallModel(BallModel.defaultSpeed, Color.white, BallModel.defaultSize, ballType);
        }
    }
}