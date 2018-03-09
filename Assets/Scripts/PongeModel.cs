using System.Collections.Generic;
using UnityEngine;

//Contains all the data related to the app
public class PongeModel : PongeElement
{
    public int HalfwayYPixel;
    public GameObject BallPrefab;

    public int totalBalls;
    public int maxBalls;
    public List<GameObject> balls;
    public float ballSpeed;
    public Color[] ballColors;
    public int maxColors;

    public PlayerModel player0;
    public PlayerModel player1;

    public bool bothTouched;
}