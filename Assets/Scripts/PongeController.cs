
// Controls the app workflow.
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PongeController : PongeElement
{
    System.Random rnd;
    void Start()
    {
        //Setting constants and global variables
        app.model.bothTouched = false;
        app.model.HalfwayYPixel = Screen.height / 2;
        rnd = new System.Random();
        app.model.isGameOver = false;
        app.model.maxScore = 100; //TODO Adjust this

        //Setting up Models and Views
        setUpPlayerModel(ref app.model.player0, true);
        setUpPlayerModel(ref app.model.player1, false);
        app.model.totalBalls = 1;
        app.model.maxBalls = 50; //TODO adjust this...or remove it?

        setUpScoreText(ref app.view.player0Score);
        setUpScoreText(ref app.view.player1Score);

        //Assign Models to Views
        app.view.player0.model = app.model.player0;
        app.view.player1.model = app.model.player1;

        app.view.firstBall.model = createBallModelFromBallType(BallType.Regular);
        app.view.firstBall.model.lastHitPlayer0 = false;

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
        if (!app.model.isGameOver)
        {
            handleTouches();

            if (!app.model.bothTouched)
            {
                if (app.model.player0.touchId != -1 && app.model.player1.touchId != -1)
                {
                    startTheGame();
                }
            }

            if (app.model.totalBalls <= 0)
            {
                //TODO Spawn new ball
                Debug.Log("Out of balls!");
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

    public void OnPlayerBallHit(GameObject originalBall)
    {
        //Create a new ball here
        if (app.model.totalBalls < app.model.maxBalls)
        {
            app.model.totalBalls++;
            SpawnNewBall(originalBall);
            //TODO create an explosion of balls on a special hit
        }
    }

    //Clone the collided ball moving at a slightly different angle
    private void SpawnNewBall(GameObject originalBall)
    {
        //Create a new BallModel
        Vector3 newPosition = originalBall.transform.position;
        GameObject newBall = Instantiate(app.model.BallPrefab, newPosition, Quaternion.identity);
        BallType nextBallType = getRandomBallType();
        BallModel newModel = createBallModelFromBallType(nextBallType);
        BallModel originalModel = originalBall.GetComponent<BallView>().model;
        newModel.lastHitPlayer0 = originalModel.lastHitPlayer0;
        newBall.GetComponent<BallView>().model = newModel;

        //Adjust the new BallView
        Vector2 originalVelocity = originalBall.GetComponent<Rigidbody2D>().velocity;
        float xShift = 1f; //TODO adjust this
        Vector2 newDir = new Vector2(originalVelocity.x + xShift, originalVelocity.y).normalized;
        newBall.GetComponent<Rigidbody2D>().velocity = newDir * newModel.speed;
        newBall.GetComponent<SpriteRenderer>().color = newModel.color;
        if(nextBallType == BallType.Large)
        {
            newBall.transform.localScale += new Vector3(0.3f, 0.3f, 0);
        }
        
        //Debug.Log("NEW BALL!");
        //Debug.Log("New Ball Direction: " + newDir);
        //Debug.Log("New Ball Velocity: " + newBall.GetComponent<Rigidbody2D>().velocity.ToString());
        
    }

    public void OnBallScored(GameObject ball, bool scoredOnPlayer0)
    {
        if (app.model.isGameOver)
        {
            return;
        }

        if (scoredOnPlayer0)
        {
            app.model.player0.score++;
            app.view.player0Score.text = app.model.player0.score.ToString();
            //app.view.player0Score.color = ball.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            app.model.player1.score++;
            app.view.player1Score.text = app.model.player1.score.ToString();
            //app.view.player1Score.color = ball.GetComponent<SpriteRenderer>().color;
        }
        app.model.totalBalls--;
        Destroy(ball);

        if(app.model.player0.score >= app.model.maxScore 
        || app.model.player1.score >= app.model.maxScore)
        {
            handleGameOver(app.model.player0.score >= app.model.player1.score);
        }
    }

    private void handleGameOver(bool player0won)
    {
        Debug.Log("GAME OVER!");
        app.model.isGameOver = true;
        app.view.GameOverTextPlayer0.enabled = player0won;
        app.view.GameOverTextPlayer1.enabled = !player0won;
        Text textToFadeIn = player0won ? app.view.GameOverTextPlayer0 : app.view.GameOverTextPlayer1;

        StartCoroutine(FadeInPanel(0.5f, 3));
        StartCoroutine(FadeInText(textToFadeIn, 1, 3));
    }

    IEnumerator FadeInPanel(float aValue, float aTime)
    {
        Image panelImage = app.view.GameOverPanel.GetComponent<Image>();
        float alpha = panelImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            panelImage.color = newColor;
            yield return null;
        }
    }
    IEnumerator FadeInText(Text text, float aValue, float aTime)
    {
        float alpha = text.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            text.color = newColor;
            yield return null;
        }
    }

    //TODO move hard coded values to BallModel
    private BallModel createBallModelFromBallType(BallType ballType)
    {
        switch (ballType)
        {
            case BallType.Regular:
                return new BallModel(BallModel.defaultSpeed, Color.white, BallModel.defaultSize, ballType);
            case BallType.Fast:
                return new BallModel(BallModel.defaultSpeed * 1.5f, Color.green, BallModel.defaultSize * 0.8f, ballType);
            case BallType.Large:
                return new BallModel(BallModel.defaultSpeed * 0.8f, Color.red, BallModel.defaultSize * 1.2f, ballType);
            case BallType.Spread:
                return new BallModel(BallModel.defaultSpeed, Color.yellow, BallModel.defaultSize, ballType);
            default:
                Debug.Log("Invalid BallType, defaulting to regular");
                return new BallModel(BallModel.defaultSpeed, Color.white, BallModel.defaultSize, ballType);
        }
    }

    private BallType getRandomBallType()
    {
        Array values = Enum.GetValues(typeof(BallType));
        return (BallType)values.GetValue(rnd.Next(values.Length));
    }
}