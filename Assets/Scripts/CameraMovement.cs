using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;

    void Update()
    {
        // Get the input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal"); // A and D
        float moveVertical = Input.GetAxis("Vertical"); // W and S

        // Calculate the movement direction
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the camera
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}