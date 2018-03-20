using UnityEngine;
using UnityEngine.UI;
//Contains all views

public class PongeView : PongeElement
{
    public BallView firstBall;
    public PlayerView player0;
    public PlayerView player1;
    public Camera mainCamera;

    //Canvas elements
    public Text player0Score;
    public Text player1Score;
    public GameObject GameOverPanel;
    public Text GameOverTextPlayer0;
    public Text GameOverTextPlayer1;
}