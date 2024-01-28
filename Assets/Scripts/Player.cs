using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    // Define the possible states of the player.
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing
    }

    // Public variable for the sword game object.
    public GameObject sword;
    // Private variable to keep track of the player's current state.
    private State currentState = State.Idle;
    // Private reference to the Sword script component.
    private Sword swordScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(sword == null)
        {
            Debug.LogError("Sword reference lost at runtime");
        }
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Blocking:
                // When in the Blocking state, rotate the sword if the space key is held down.
                if (Input.GetKey(KeyCode.Space))
                {
                    sword.GetComponent<Sword>().Rotate(new Vector3(0,0,-30)* Time.deltaTime);
                }
                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                break;
        }
    }
    // Change the current state of the player and perform necessary setup for each state.
    public void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                break;
            case State.Blocking:
                Debug.Log("Player blocking");
                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                break;
        }
    }
}
