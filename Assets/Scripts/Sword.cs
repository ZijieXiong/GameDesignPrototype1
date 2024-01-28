using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{   
    public GameObject owner;

    private GameObject rotationAxis;

    // Start is called before the first frame update
    void Start()
    {
        rotationAxis = new GameObject("RotationAxis");

        rotationAxis.transform.position = transform.position;

        float height = GetComponent<SpriteRenderer>().bounds.size.y;

        rotationAxis.transform.position -= new Vector3(0, height / 2, 0);
        rotationAxis.transform.SetParent(owner.transform);
        if (rotationAxis.transform.parent == owner.transform)
        {
            Debug.Log("RotationAxis is successfully set as a child of player.");
        }
        else
        {
            Debug.LogError("RotationAxis is not a child of player.");
        }

        transform.SetParent(rotationAxis.transform);

        transform.localPosition = new Vector3(0, height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(Vector3 angle)
    {
        rotationAxis.transform.Rotate(angle);
    }
}
