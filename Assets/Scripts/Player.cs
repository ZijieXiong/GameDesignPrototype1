using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing
    }

    public GameObject sword;
    private State currentState = State.Idle;

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
