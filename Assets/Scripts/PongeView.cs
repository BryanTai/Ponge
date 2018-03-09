using UnityEngine;
using UnityEngine.UI;
//Contains all views

public class PongeView : PongeElement
{
    public BallView ball; //TODO going to need a collection of these
    public PlayerView player0;
    public PlayerView player1;
    public Camera mainCamera;

    public Text player0Score;
    public Text player1Score;
}