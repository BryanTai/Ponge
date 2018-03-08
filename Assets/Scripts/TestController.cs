using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public int Player0TouchId;
    public int Player1TouchId;

    public TestView Player0View;
    public TestView Player1View;

    public Camera mainCamera;

    public int HalfwayYPixel;

    // Use this for initialization
    void Start () {
        mainCamera = GetComponent<Camera>();
        Player0TouchId = -1;
        Player1TouchId = -1;
        HalfwayYPixel = Screen.height / 2;
        Debug.Log("HalfwayYPixel: " + HalfwayYPixel);
    }
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch newTouch = Input.GetTouch(i);
            int newId = newTouch.fingerId;

            if (newId == Player0TouchId)
            {
                if(newTouch.phase == TouchPhase.Ended)
                {
                    Player0TouchId = -1;
                }
                else
                {
                    MovePlayerToXPixel(Player0View, newTouch.position.x);
                } 
            }
            else if (newId == Player1TouchId)
            {
                if (newTouch.phase == TouchPhase.Ended)
                {
                    Player1TouchId = -1;
                }else
                {
                    MovePlayerToXPixel(Player1View, newTouch.position.x);
                }
            }
            else //Touch matches neither saved paddle IDs, check if it's a new one
            {
                if (newTouch.phase == TouchPhase.Began)
                {
                    Debug.Log("NEW TOUCH DETECTED : TouchPosition " + newTouch.position.y);

                    if(newTouch.position.y < HalfwayYPixel)
                    {
                        Player0TouchId = newTouch.fingerId;
                        MovePlayerToXPixel(Player0View, newTouch.position.x);
                        Debug.Log("Touched Player 0");
                    }
                    else
                    {
                        Player1TouchId = newTouch.fingerId;
                        MovePlayerToXPixel(Player1View, newTouch.position.x);
                        Debug.Log("Touched Player 1");
                    }


                }
                //Else just ignore the touch
            }
        }
    }

    //TODO WorldToScreenPoint is pretty resource intensive (maybe)

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
