
// Controls the app workflow.
using System;
using UnityEngine;

public class PongeController : PongeElement
{
    public void OnPlayerBallHit(GameObject collider)
    {
        //Create a new ball here
        if (app.model.totalBalls < app.model.maxBalls)
        {
            app.model.totalBalls++;
            CloneBallWithAngle();
            //TODO might need to delay the cloning to prevent more than one from being created on a single collision
        }
        throw new NotImplementedException();
    }

    //Clone the collided ball moving at a slightly different angle
    private void CloneBallWithAngle()
    {
        throw new NotImplementedException();
    }
}