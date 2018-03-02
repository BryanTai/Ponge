using System.Collections.Generic;
using UnityEngine;

//Contains all the data related to the app
public class PongeModel : PongeElement
{
    public GameObject BallPrefab;
    public int totalBalls;
    public int maxBalls;
    //public List<GameObject> balls;
    public PlayerModel player0;
    public PlayerModel player1;

    public int HalfwayYPixel;
}