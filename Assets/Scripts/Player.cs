using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    // Enumeration for different player states.
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing
    }

    public GameObject sword; // Reference to the player's sword object.
    private State currentState = State.Idle; // Tracks the current state of the player.

    // Reference to the GameManager instance.
    public GameManager gameManager;

    // Start is called before the first frame update.
    void Start()
    {
        // Initialize player's state to match the GameManager's state.
        if (gameManager != null)
        {
            //ChangeState(gameManager.CurrentState);
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Check if the sword reference is still valid.
        if(sword == null)
        {
            Debug.LogError("Sword reference lost at runtime");
        }
    }

    // Method to change the player's state.
    public void ChangeState(State newState)
    {
        currentState = newState; // Update the current state.

        // Handle actions based on the new state.
        switch (newState)
        {
            case State.Idle:
                // Logic for when the player is in the Idle state.
                break;
            case State.Blocking:
                // Logic for when the player is in the Blocking state.
                break;
            case State.Attacking:
                // Logic for when the player is in the Attacking state.
                Attack();
                break;
            case State.Loading:
                // Logic for when the player is in the Loading state.
                break;
            case State.Failing:
                // Logic for when the player is in the Failing state.
                HandleFailingState();
                break;
        }
    }

    // Method to handle the attack action.
    private void Attack()
    {
        StartCoroutine(PerformAttack());
    }

    // Coroutine for performing the attack action.
    private IEnumerator PerformAttack()
    {
        Vector3 originalPosition = transform.position; // Store the original position.
        float moveDistance = 2f; // Set the distance to move during the attack.
        Vector3 targetPosition = originalPosition + new Vector3(-moveDistance, 0, 0); // Calculate the target position.

        // Move towards the target position.
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
            yield return null;
        }

        // Wait for a short duration.
        yield return new WaitForSeconds(0.5f);

        // Move back to the original position.
        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }

    // Method to handle the Failing state.
    private void HandleFailingState()
    {
        Debug.Log("Player has failed!"); // Log the failure message.
        GetComponent<Renderer>().material.color = Color.red; // Change the player's color to red to indicate failure.
    }
}
