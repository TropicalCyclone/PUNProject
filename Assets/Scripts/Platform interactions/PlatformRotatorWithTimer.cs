using UnityEngine;

public class PlatformRotatorWithTimer : MonoBehaviour
{
    public float rotationDuration = 5f; // Total rotation duration in seconds
    public Vector3 rotationAxis = Vector3.up; // Rotate around Y-axis by default
    public float rotationAngle = 360f; // Rotation angle in degrees

    private float currentRotationTime = 0f;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Store the starting rotation
        startRotation = transform.rotation;

        // Calculate the target rotation based on the rotation axis, angle, and duration
        targetRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis) * startRotation;
    }

    void Update()
    {
        if (currentRotationTime < rotationDuration)
        {
            // Calculate the interpolation factor based on the elapsed time and duration
            float t = currentRotationTime / rotationDuration;

            // Smoothly rotate the platform using Slerp (Spherical Linear Interpolation)
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Increment the current rotation time
            currentRotationTime += Time.deltaTime;
        }
    }
}