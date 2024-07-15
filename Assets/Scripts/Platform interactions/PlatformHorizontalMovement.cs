using UnityEngine;

public class PlatformHorizontalMovement : MonoBehaviour
{
    public enum HorizontalDirection
    {
        Left,
        Right
    }

    [SerializeField] private HorizontalDirection direction = HorizontalDirection.Right;
    [SerializeField][Range(0, 5)] float speed = 0.5f;
    [SerializeField][Range(0, 10)] float amplitude = 1f;

    private Vector3 startPosition;
    private float angle;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new angle based on speed and time
        angle += speed * Time.deltaTime;

        // Calculate the offset using sine wave
        float xOffset = Mathf.Sin(angle) * amplitude;

        // Determine the direction multiplier based on the selected direction
        int directionMultiplier = (direction == HorizontalDirection.Right) ? 1 : -1;

        // Update the platform's position
        transform.position = startPosition + new Vector3(xOffset * directionMultiplier, 0, 0);
    }
}