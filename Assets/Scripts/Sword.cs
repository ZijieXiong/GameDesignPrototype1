using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{   
    // Public variable to hold a reference to the owner of the sword (likely the player)
    public GameObject owner;
    // Private variable to keep track of the rotation axis object.
    private GameObject rotationAxis;

    // Start is called before the first frame update
    void Start()
    {
        rotationAxis = new GameObject("RotationAxis");
        // Set the position of the rotation axis to the sword's position.
        rotationAxis.transform.position = transform.position;
        // Calculate the height of the sword using its sprite renderer bounds.
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        // Adjust the rotation axis position to be at the base of the sword.
        rotationAxis.transform.position -= new Vector3(0, height / 2, 0);
        // Set the rotation axis as a child of the owner (player) object.
        rotationAxis.transform.SetParent(owner.transform);

        if (rotationAxis.transform.parent == owner.transform)
        {
            Debug.Log("RotationAxis is successfully set as a child of player.");
        }
        else
        {
            Debug.LogError("RotationAxis is not a child of player.");
        }
        // Set the sword as a child of the rotation axis and adjust its local position.
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
