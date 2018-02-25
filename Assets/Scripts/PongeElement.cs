using UnityEngine;

// Base class for all elements in this application.
public class PongeElement : MonoBehaviour {
    // Gives access to the application and all instances.
    public PongeApplication app { get { return FindObjectOfType<PongeApplication>(); } }
}
