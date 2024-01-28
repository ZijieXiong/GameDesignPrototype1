using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    private List<GameObject> signalObjects = new List<GameObject>();
    private State currentState;
    private GameObject signals;

    private Vector2 cameraTopLeft;
    private Vector2 cameraTopRight;
    private Vector2 cameraBottomLeft;
    private Vector2 cameraBottomRight;
    private float cameraHeight;
    private float cameraWidth;

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
                for(int i = 0; i < signalObjects.Count; i++){
                    if(signalObjects[i].transform.position.x < -cameraWidth - 2){
                        Destroy(signalObjects[i]);
                        signalObjects.RemoveAt(i);
                    }
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
        signalObjects.Add(signal);
        /*
        if(signal){
            signal.transform.SetParent(signals.transform);
        }*/
    }

     void CalculateCameraBounds()
    {
        Camera cam = Camera.main; // 获取主摄像机
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
                signals = new GameObject("Signals");
                CalculateCameraBounds();
                for (int i = 0; i < serie.Length; i++)
                {
                    Debug.Log(serie[i]);
                    InitSignal(serie[i], new Vector3(cameraWidth / 2 + 2 * (i) + 1,2.8f,0));
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
