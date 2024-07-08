using UnityEngine;

public class CircularPlatformRotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // Degrees per second
    public Vector3 rotationAxis = Vector3.up; // Rotate around Y-axis by default

    void Update()
    {
        // Rotate the platform
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}