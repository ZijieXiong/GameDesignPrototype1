using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    private Vector2 cameraTopLeft;
    private Vector2 cameraTopRight;
    private Vector2 cameraBottomLeft;
    private Vector2 cameraBottomRight;
    private float cameraHeight;
    private float cameraWidth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-3,0,0) * Time.deltaTime);
    }
}
