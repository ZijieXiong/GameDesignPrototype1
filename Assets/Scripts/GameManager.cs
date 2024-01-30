using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    // Enumeration defining possible game states.
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing
    }
    // Prefabs for object instantiation.
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject heartPrefab;

    // Reference to the player object in the game.
    public Player player;
    // GameObject to hold all signal objects.
    public GameObject signals;

    public GameObject frame;
    public Sprite[] frameShapes;
    //Gameobjects to hold UI elements
    public GameObject startText;
    public GameObject failText;

    public Canvas canvas;

    public float acceptableDistance = 0.5f;

    // Array for storing a series of shape indices.
    private int[] serie;
    // List of game objects representing signals in the game.
    private List<GameObject> signalObjects = new List<GameObject>();

    // Variable to track the current state of the game.
    private State currentState;

    private int playerHealth = 3;

    // Variables for calculating and storing camera bounds.
    private Vector2 cameraTopLeft;
    private Vector2 cameraTopRight;
    private Vector2 cameraBottomLeft;
    private Vector2 cameraBottomRight;
    private float cameraHeight;
    private float cameraWidth;
    private int currentSignal = 0;
    private int currentHit = 0;
    // Start is called before the first frame update
    void Start()
    {   
        startText.SetActive(true);
        ChangeState(State.Idle);
        failText.SetActive(false);
        UpdateHealthUI();
        frame.SetActive(false);
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
                    startText.SetActive(false);
                    frame.SetActive(true);
                }
                break;
            case State.Blocking:
                for(int i = 0; i < signalObjects.Count; i++){
                    if(signalObjects[i].transform.position.x < -cameraWidth - 2){
                        Destroy(signalObjects[i]);
                        signalObjects.RemoveAt(i);
                    }
                }

                //Check the timing of user input
                TimingDetection();
                
                //Check if player loss all health
                if(playerHealth == 0){
                    ChangeState(State.Failing);
                }
                else{
                    //Check if player block all attacks
                    if(currentSignal >= serie.Length){
                        ChangeState(State.Attacking);
                    }
                }
                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    failText.SetActive(false);
                    ChangeState(State.Blocking);
                }
                break;
        }
    }

    // Generates an array of integers representing a random series of shapes.
    // The length of the series is specified by the 'length' parameter.
    private int[] GenerateSerie(int length)
    {
        int[] res = new int[length];
        for (int i = 0; i < length; i++){
            res[i] = SpawnRandomShape();
            Debug.Log(res[i]);
        }

        return res;
    }

    // Selects a random shape by generating a random index.
    // Returns the index of the shape (0 for circle, 1 for square, 2 for triangle).
    private int SpawnRandomShape()
    {
        int shapeIndex = UnityEngine.Random.Range(0, 3);
        return shapeIndex;
    }

    // Initializes a signal game object based on the shape index and position.
    // The shape index determines which prefab to instantiate (circle, square, triangle).
    private void InitSignal(int shapeIndex, Vector3 position)
    {
        GameObject signal = null;
        switch (shapeIndex)
        {
            case 0:
                signal = Instantiate(circlePrefab, position, Quaternion.identity, signals.transform);
                break;
            case 1:
                signal = Instantiate(squarePrefab, position, Quaternion.identity, signals.transform);
                break;
            case 2:
                signal = Instantiate(trianglePrefab, position, Quaternion.identity, signals.transform);
                break;
        }
        if(signal){
            signalObjects.Add(signal);
        }
    }

    //Update number of heart in UI based on player's health
    public void UpdateHealthUI()
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.CompareTag("HeartUI"))
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < playerHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, canvas.transform);
            heart.tag = "HeartUI";

            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-330 + (rect.sizeDelta.x /2 * i), 200);
        }
    }


    // Calculates the bounds of the camera view in world space.
    // Useful for positioning objects within the camera's view.
     void CalculateCameraBounds()
    {
        Camera cam = Camera.main;
        if (cam == null) 
        {
            Debug.LogError("Main Camera is not assigned.");
            return;
        }
        
        if (cam.orthographic)
        {
            cameraHeight = cam.orthographicSize * 2;
            cameraWidth = cameraHeight * cam.aspect; 

            cameraBottomLeft = new Vector2(cam.transform.position.x - cameraWidth / 2, cam.transform.position.y - cam.orthographicSize);
            cameraTopRight = new Vector2(cam.transform.position.x + cameraWidth / 2, cam.transform.position.y + cam.orthographicSize);

            cameraTopLeft = new Vector2(cameraBottomLeft.x, cameraTopRight.y);
            cameraBottomRight = new Vector2(cameraTopRight.x, cameraBottomLeft.y);
            
            Debug.Log($"Camera Bounds:\nTop Left: {cameraTopLeft}\nTop Right: {cameraTopRight}\nBottom Left: {cameraBottomLeft}\nBottom Right: {cameraBottomRight}");
        }
        else
        {
            Debug.LogError("Camera is not orthographic.");
        }
    }

    public void ChangeFrameShape(int shapeIndex)
    {
        if (shapeIndex >= 0 && shapeIndex < frameShapes.Length)
        {
            frame.GetComponent<SpriteRenderer>().sprite = frameShapes[shapeIndex];
        }
    }

    //Player input timing detection
    private void TimingDetection()
    {
        if(serie[currentSignal] == 0){
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) < acceptableDistance)
                {
                    Debug.Log("Block Success");
                    player.ChangeState(Player.State.BlockSuccess);
                }
                else
                {
                    Debug.Log(signalObjects[currentSignal]);
                    playerHealth-=1;
                    UpdateHealthUI();
                    player.ChangeState(Player.State.BlockFail);                        
                }
                currentSignal += 1;
                if(currentSignal < serie.Length){
                    ChangeFrameShape(serie[currentSignal]);
                }
            }
            else{
                //Check if player miss one signal
                if(frame.transform.position.x - signalObjects[currentSignal].transform.position.x > acceptableDistance * 2){
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                    player.ChangeState(Player.State.BlockFail);
                }
            }
        }
        else if(serie[currentSignal] == 1)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) > acceptableDistance + frame.GetComponent<SpriteRenderer>().sprite.rect.width/2)
                {
                    Debug.Log("Block Fail");
                    Debug.Log(signalObjects[currentSignal]);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) <=frame.GetComponent<SpriteRenderer>().sprite.rect.width/2 - acceptableDistance)
                {
                    Debug.Log("Block Fail");
                    Debug.Log(signalObjects[currentSignal]);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
        }
        else if(serie[currentSignal] == 2)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) > acceptableDistance + frame.GetComponent<SpriteRenderer>().sprite.rect.width/2)
                {   
                    if(currentHit < 2){
                        currentHit += 1;
                    }
                    else if(currentHit >= 2){
                        Debug.Log("Block Success");
                        Debug.Log(signalObjects[currentSignal]);
                        currentSignal += 1;
                        if(currentSignal < serie.Length){
                            ChangeFrameShape(serie[currentSignal]);
                        }
                    }
                }
                else
                {
                    Debug.Log("Block Fail");
                    Debug.Log(signalObjects[currentSignal]);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentHit = 0;
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
            else
            {   
                if(frame.transform.position.x - signalObjects[currentSignal].transform.position.x > acceptableDistance + frame.GetComponent<SpriteRenderer>().sprite.rect.width/2){
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentHit = 0;
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
        }


    }

    public int GetCurrentSerie()
    {
        return serie[currentSignal];
    }

    // Changes the current state of the game.
    // Handles actions to be taken during state transitions, like generating attack series or setting up game objects.
    private void ChangeState(State newState)
    {
        currentState = newState;
        Debug.Log(currentState);

        switch (newState)
        {
            case State.Idle:
                break;
            case State.Blocking:
                player.ChangeState(Player.State.Blocking);
                serie = GenerateSerie(3);
                currentSignal = 0;
                playerHealth = 3;
                Debug.Log("Attacking Serie");
                signalObjects = new List<GameObject>();
                //signals = new GameObject("Signals");
                CalculateCameraBounds();
                for (int i = 0; i < serie.Length; i++)
                {
                    Debug.Log(serie[i]);
                    InitSignal(serie[i], new Vector3(cameraWidth / 2 + 4 * (i) + 1,2.8f,0));
                }
                UpdateHealthUI();
                ChangeFrameShape(serie[0]);

                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                int numOfSignal = signalObjects.Count;
                for(int i = 0; i < numOfSignal; i++){
                    Debug.Log("Destroying");
                    Destroy(signalObjects[0]);
                    signalObjects.RemoveAt(0);
                }
                failText.SetActive(true);
                break;
        }
    }
}
