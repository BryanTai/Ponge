using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public int Player0TouchId = -1;
    public int Player1TouchId = -1;

    public TestView Player0;
    public TestView Player1;

    public Camera mainCamera;

    public int HalfwayYPixel;

    // Use this for initialization
    void Start () {
        mainCamera = GetComponent<Camera>();
        HalfwayYPixel = Screen.height / 2;
    }
	
	// Update is called once per frame
	void Update () {
        Touch player0Touch;
        Touch player1Touch;

        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch newTouch = Input.GetTouch(i);
            int newId = newTouch.fingerId;

            if (newId == Player0TouchId || newId == Player1TouchId)
            {
                TestView playerToMove;
                if (newId == Player0TouchId)
                {
                    playerToMove = Player0;
                }
                else
                {
                    playerToMove = Player1;
                }
                MovePlayerToXPixel(playerToMove, newTouch.position.x);
            }
            else //Touch matches neither saved paddle IDs, check if it's a new one or a random touch
            {
                if (newTouch.phase == TouchPhase.Began)
                {
                    if(newTouch.position.y < HalfwayYPixel)
                    {
                        Player0TouchId = newTouch.fingerId;
                        //Move paddle to finger
                        MovePlayerToXPixel(Player0, newTouch.position.x);
                    }
                    else
                    {
                        Player1TouchId = newTouch.fingerId;
                        MovePlayerToXPixel(Player1, newTouch.position.x);
                    }


                }
                //Else just ignore the touch
            }
        }
    }

    private void MovePlayerToXPixel(TestView playerToMove, float touchXPixels)
    {
        //Debug.Log("Time to MOVE!");
        float playerYPixels = mainCamera.WorldToScreenPoint(playerToMove.transform.position).y;
        Vector3 newPlayerPixelVector = new Vector3(touchXPixels, playerYPixels);
        //Debug.Log("Created new Pixel Vector " + newPlayerPixelVector.ToString());
        Vector3 newPlayerWorldVector = mainCamera.ScreenToWorldPoint(newPlayerPixelVector);
        newPlayerWorldVector.z = 0; //So it doesn't appear BEHIND the camera...
                                    //Debug.Log("moving Paddle to WorldSpace Vector " + newPlayerWorldVector.ToString());
        playerToMove.transform.position = newPlayerWorldVector;
        //Debug.Log("OnPlayerTouch COMPLETE!");
    }
}
