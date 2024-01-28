using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing
    }
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject trianglePrefab;

    public Player player;

    private int[] serie;

    private State currentState;

    // Start is called before the first frame update
    void Start()
    {   
        ChangeState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {   
        switch (currentState)
        {
            case State.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeState(State.Blocking);
                    player.ChangeState(Player.State.Blocking);
                }
                break;
            case State.Blocking:
                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                break;
        }
    }

    private int[] GenerateSerie(int length)
    {
        int[] res = new int[length];
        for (int i = 0; i < length; i++){
            res[i] = SpawnRandomShape();
        }

        return res;
    }

    private int SpawnRandomShape()
    {
        int shapeIndex = Random.Range(0, 3);
        return shapeIndex;
    }

    private void InitSignal(int shapeIndex)
    {
        switch (shapeIndex)
        {
            case 0:
                Instantiate(circlePrefab, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(squarePrefab, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(trianglePrefab, transform.position, Quaternion.identity);
                break;
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                break;
            case State.Blocking:
                serie = GenerateSerie(3);
                Debug.Log("Attacking Serie");
                for (int i = 0; i < serie.Length; i++)
                {
                    Debug.Log(serie[i]);
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
}
