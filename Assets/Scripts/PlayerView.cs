
using UnityEngine;

public class PlayerView : PongeElement
{
    public PlayerModel model;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Should only ever collide with Ball
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play();   
    }
}