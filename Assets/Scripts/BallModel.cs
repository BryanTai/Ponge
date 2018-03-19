using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Regular,
    Fast,
    Large,
    Spread
}

public class BallModel {
    public const float defaultSpeed = 5;
    public const float defaultSize = 0.1f;

    public float speed;
    public Color color; //TODO might not need to store all this data :I
    public float size;
    public BallType ballType;
    public bool lastHitPlayer0; //

    public BallModel(float speed, Color color, float size, BallType ballType, bool lastHitPlayer0 = true)
    {
        this.speed = speed;
        this.color = color;
        this.size = size;
        this.ballType = ballType;
        this.lastHitPlayer0 = lastHitPlayer0;
    }
}
