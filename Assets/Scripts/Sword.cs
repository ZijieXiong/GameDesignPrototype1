using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject rotationAxis = new GameObject("RotationAxis");

        rotationAxis.transform.position = transform.position;

        float height = GetComponent<SpriteRenderer>().bounds.size.y;

        rotationAxis.transform.position -= new Vector3(0, height / 2, 0);

        transform.SetParent(rotationAxis.transform);

        transform.localPosition = new Vector3(0, height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(Vector3 angle)
    {
        transform.Rotate(angle);
    }
}
