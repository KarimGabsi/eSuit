using UnityEngine;
using System.Collections;

//Reference the eSuit
using eSuitLibrary;

public class MyUnityClass : MonoBehaviour
{
    //eSuit Instatiation
    private eSuit _eSuit = new eSuit();
    public int health = 100;

    //Unity Start
    void Start()
    {
        //Start eSuit syncing
        _eSuit.Start();
    }

    //Unity Update
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    //A trigger event, or any other event, depending on the game.
    void OnTriggerEnter2D()
    {
        health--;
        _eSuit.ExecuteHit(HitPlaces.Left_Arm, 50, 400);
    }
    void Die()
    {
        Destroy(gameObject);
    }

    //IMPORTANT
    //Dispose eSuit, otherwise the backgroundthread of the syncing will remain.
    void OnDestroy()
    {
        _eSuit.Dispose();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200, 0, 350, 100), "eSuit: " + _eSuit.connected().ToString() + " on " + _eSuit.currentPort());
    }
}
