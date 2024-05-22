using UnityEngine;
using UnityEngine.UI;

public class hueChanger : MonoBehaviour
{
    public float cycleSpeed = 1f; // Speed of the hue cycle

    [SerializeField] private Image image;
    private float hueValue;

    void Start()
    {
        // Get the Image component
        image = GetComponent<Image>();

        // Check if there's an image component attached
        if (image == null)
        {
            Debug.LogError("RainbowEffect script requires an Image component attached to the GameObject.");
            enabled = false; // Disable the script if no Image component is found
            return;
        }

        // Set initial color with a random hue
        hueValue = Random.value; // Random hue value between 0 and 1
        ApplyRainbowEffect();
    }

    void Update()
    {
        // Update the hue value based on time
        hueValue += cycleSpeed * Time.deltaTime;
        if (hueValue > 1f)
            hueValue -= 1f;

        // Apply rainbow effect
        ApplyRainbowEffect();
    }

    void ApplyRainbowEffect()
    {
        // Convert HSV color to RGB color
        Color newColor = Color.HSVToRGB(hueValue, 1f, 1f);
        // Apply the new color to the image
        image.color = newColor;
    }
}
