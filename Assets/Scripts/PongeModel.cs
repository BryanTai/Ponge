using UnityEngine;

//Contains all the data related to the app
public class PongeModel : PongeElement
{
    public int HalfwayYPixel;
    public GameObject BallPrefab;

    public int totalBalls;
    public int maxBalls;

    public PlayerModel player0;
    public PlayerModel player1;

    public bool bothTouched;
}