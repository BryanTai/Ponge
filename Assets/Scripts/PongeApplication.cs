using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Applying the Application-Model-View-Controller-Component pattern specified here
 * https://www.toptal.com/unity-unity3d/unity-with-mvc-how-to-level-up-your-game-development
*/

// Application entry point
public class PongeApplication : MonoBehaviour {

    public PongeModel model;
    public PongeView view;
    public PongeController controller;

	// Use this for initialization
	void Start () {
		
	}
}
