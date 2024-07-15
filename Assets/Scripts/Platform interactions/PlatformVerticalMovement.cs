using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVerticalMovement : MonoBehaviour
{
    [SerializeField][Range(0, 2)] float speed = 0.5f;
    [SerializeField][Range(0, 5)] float amplitude = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}