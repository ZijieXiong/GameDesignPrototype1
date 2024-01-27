using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject trianglePrefab;

    private float timer = 10f;

    // Start is called before the first frame update
    void Start()
    {   
        for(int i = 0; i < 3; i++){
            SpawnRandomShape();
        }


    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnRandomShape();
            timer = 10f;
        }
    }

    int SpawnRandomShape()
    {
        int shapeIndex = Random.Range(0, 3);

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
        return shapeIndex;
    }
}
