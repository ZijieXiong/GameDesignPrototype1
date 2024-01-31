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
    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Loading,
        Failing,
        EnemyDead
    }

    // Prefabs for object instantiation.
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject heartPrefab;

    // Reference to the player object in the game.
    // GameObject to hold all signal objects.
    public GameObject signals;

    public GameObject frame;
    public Sprite[] frameShapes;
    //Gameobjects to hold UI elements
    public GameObject startText;
    public GameObject failText;
    public Canvas canvas;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public float acceptableDistance = 0.5f;

    // Array for storing a series of shape indices.
    private int[] serie;
    // List of game objects representing signals in the game.
    private List<GameObject> signalObjects = new List<GameObject>();

    // Variable to track the current state of the game.
    private State currentState;

    private int playerHealth = 3;
    private int enemyHealth = 1;
    // Variables for calculating and storing camera bounds.
    private Vector2 cameraTopLeft;
    private Vector2 cameraTopRight;
    private Vector2 cameraBottomLeft;
    private Vector2 cameraBottomRight;
    private float cameraHeight;
    private float cameraWidth;
    private int currentSignal = 0;
    private int currentHit = 0;
    private bool isHoldingSpace = false;

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
                    enemyAnimator.SetBool("Restart Bool", true);
                }
                break;
            case State.Blocking:
                //Check the timing of user input
                if(currentSignal < serie.Length & playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player Idle") & enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                {
                    TimingDetection();

                    // Send the current signal to the StateManager
                    //stateManager.HandleSignal(currentSignal); // Add this line
                }
                
                //Check if player loss all health
                if(playerHealth == 0){
                    ChangeState(State.Failing);
                }
                else{
                    //Check if player block all attacks
                    if(currentSignal >= serie.Length){
                        ChangeState(State.Attacking);
                        enemyHealth = 0;
                    }
                }
                break;
            case State.Attacking:
                if (enemyHealth == 0) 
                {
                    ChangeState(State.EnemyDead);
                }
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

            case State.EnemyDead:
            /*
                if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("exit")) 
                {
                    enemyAnimator.gameObject.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        ChangeState(State.Idle);
                    }
                }*/
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
        Debug.Log("change shape");
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
                Debug.Log("pattern 0");
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) < acceptableDistance)
                {
                    Debug.Log("Block Success");
                    playerAnimator.SetBool("Player Clash Single", true);
                    playerAnimator.SetBool("Player Attack", true);
                    enemyAnimator.SetBool("Single Bool", true);
                    enemyAnimator.SetBool("Hurt Bool", true);
                }
                else
                {
                    Debug.Log("Block Fail");
                    playerAnimator.SetBool("Player Clash Single", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Single Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
                    Debug.Log(signalObjects[currentSignal]);
                    playerHealth-=1;
                    UpdateHealthUI();                     
                }
                currentSignal += 1;
                if(currentSignal < serie.Length){
                    ChangeFrameShape(serie[currentSignal]);
                }
            }
            else{
                //Check if player miss one signal
                if(frame.transform.position.x - signalObjects[currentSignal].transform.position.x > acceptableDistance * 2){
                    Debug.Log("Block Fail");
                    playerAnimator.SetBool("Player Clash Single", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Single Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
        }
        else if(serie[currentSignal] == 1)
        {   
            if(Input.GetKeyDown(KeyCode.Space)){
                isHoldingSpace = true;
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) > acceptableDistance * 2 + frameShapes[1].bounds.size.x/2)
                {
                    Debug.Log("Block Fail");
                    Debug.Log("wrong timing in pressing");
                    Debug.Log(signalObjects[currentSignal]);
                    playerAnimator.SetBool("Player Hold", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Hold Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                    isHoldingSpace = false;
                }
            }
            else if(Input.GetKeyUp(KeyCode.Space) & isHoldingSpace){
                Debug.Log(Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position));
                Debug.Log(acceptableDistance * 2 + frameShapes[1].bounds.size.x/2);
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) > acceptableDistance * 2 + frameShapes[1].bounds.size.x/2)
                {
                    Debug.Log("Block Fail");
                    Debug.Log("wrong timing in releasing");
                    Debug.Log(signalObjects[currentSignal]);
                    playerAnimator.SetBool("Player Hold", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Hold Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
                    playerHealth-=1;
                    UpdateHealthUI();
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                    isHoldingSpace = false;
                }
                else
                {
                    Debug.Log("Block Success");
                    Debug.Log(signalObjects[currentSignal]);
                    playerAnimator.SetBool("Player Hold", true);
                    playerAnimator.SetBool("Player Attack", true);
                    enemyAnimator.SetBool("Hold Bool", true);
                    enemyAnimator.SetBool("Hurt Bool", true);
                    currentSignal += 1;
                    if(currentSignal < serie.Length){
                        ChangeFrameShape(serie[currentSignal]);
                    }
                }
            }
            else if(frame.transform.position.x - signalObjects[currentSignal].transform.position.x > acceptableDistance * 2 + frameShapes[1].bounds.size.x/2 & !isHoldingSpace)
            {   
                Debug.Log("Block Fail");
                Debug.Log("Miss the block");
                Debug.Log(signalObjects[currentSignal]);
                playerAnimator.SetBool("Player Hold", true);
                playerAnimator.SetBool("Player Hurt", true);
                enemyAnimator.SetBool("Hold Bool", true);
                enemyAnimator.SetBool("Attack Bool", true);
                playerHealth-=1;
                UpdateHealthUI();
                currentSignal += 1;
                if(currentSignal < serie.Length){
                    ChangeFrameShape(serie[currentSignal]);
                }
                isHoldingSpace = false;
            }
        }
        else if(serie[currentSignal] == 2)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("pattern 2");
                if (Vector3.Distance(signalObjects[currentSignal].transform.position, frame.transform.position) <= acceptableDistance * 2 + frameShapes[1].bounds.size.x/2)
                {   
                    currentHit += 1;
                    if(currentHit >= 2){
                        Debug.Log("make twice hit");
                        Debug.Log("Block Success");
                        Debug.Log(signalObjects[currentSignal]);
                        playerAnimator.SetBool("Player Double", true);
                        playerAnimator.SetBool("Player Attack", true);
                        enemyAnimator.SetBool("Double Bool", true);
                        enemyAnimator.SetBool("Hurt Bool", true);
                        currentSignal += 1;
                        if(currentSignal < serie.Length){
                            ChangeFrameShape(serie[currentSignal]);
                        }
                    }
                }
                else
                {   
                    Debug.Log("Block Fail");
                    Debug.Log("Miss the block");
                    Debug.Log(signalObjects[currentSignal]);
                    playerAnimator.SetBool("Player Double", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Double Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
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
                if(frame.transform.position.x - signalObjects[currentSignal].transform.position.x > acceptableDistance * 2 + frameShapes[1].bounds.size.x/2){
                    Debug.Log("Block Fail");
                    Debug.Log(signalObjects[currentSignal]);
                    playerAnimator.SetBool("Player Double", true);
                    playerAnimator.SetBool("Player Hurt", true);
                    enemyAnimator.SetBool("Double Bool", true);
                    enemyAnimator.SetBool("Attack Bool", true);
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

    void OnEnable()
    {
        EnemyDeadController.onStateEnterEvent += HandleExitState;
    }

    void Disable()
    {
        EnemyDeadController.onStateEnterEvent -= HandleExitState;
    }

    private void HandleExitState()
    {
        //enemyAnimator.gameObject.SetActive(false);
        ChangeState(State.Idle);
    }


    // Changes the current state of the game.
    // Handles actions to be taken during state transitions, like generating attack series or setting up game objects.
    private void ChangeState(State newState)
    {
        currentState = newState;
        Debug.Log(currentState);
        int numOfSignal;
        switch (newState)
        {
            case State.Idle:
                startText.SetActive(true);
                break;
            case State.Blocking:
                enemyAnimator.gameObject.SetActive(true);
                enemyHealth = 1;
                serie = GenerateSerie(3);
                currentSignal = 0;
                playerHealth = 3;
                Debug.Log("Attacking Serie");
                numOfSignal = signalObjects.Count;
                for(int i = 0; i < numOfSignal; i++){
                    Debug.Log("Destroying");
                    Destroy(signalObjects[0]);
                    signalObjects.RemoveAt(0);
                }
                signalObjects = new List<GameObject>();
                //signals = new GameObject("Signals");
                CalculateCameraBounds();
                for (int i = 0; i < serie.Length; i++)
                {
                    Debug.Log(serie[i]);
                    InitSignal(serie[i], new Vector3(cameraWidth / 2 + 10 * (i) + 1,2.8f,0));
                }
                UpdateHealthUI();
                ChangeFrameShape(serie[0]);

                break;
            case State.Attacking:
                break;
            case State.Loading:
                break;
            case State.Failing:
                numOfSignal = signalObjects.Count;
                for(int i = 0; i < numOfSignal; i++){
                    Debug.Log("Destroying");
                    Destroy(signalObjects[0]);
                    signalObjects.RemoveAt(0);
                }
                failText.SetActive(true);
                break;
            
            case State.EnemyDead:
                enemyAnimator.SetBool("Dead Bool", true); 
                break;
        }
    }
}