using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingText : MonoBehaviour
{
    [SerializeField] public TMP_Text loadingText; // Reference to the TextMeshProUGUI component where "Loading..." will be displayed
    public float typeSpeed = 0.1f; // Speed at which characters are typed out
    public float clearDelay = 1f; // Delay before clearing the text

    [SerializeField] private string originalText;

    void Start()
    {
        // Cache the original text
        originalText = loadingText.text;

        // Start the typing loop
        StartCoroutine(TypingLoop());
    }

    IEnumerator TypingLoop()
    {
        while (true)
        {
            // Type out "Loading..."
            for (int i = 0; i <= originalText.Length; i++)
            {
                loadingText.text = originalText.Substring(0, i);
                yield return new WaitForSeconds(typeSpeed);
            }

            // Wait before clearing the text
            yield return new WaitForSeconds(clearDelay);

            // Clear the text
            for (int i = originalText.Length; i >= 0; i--)
            {
                loadingText.text = originalText.Substring(0, i);
                yield return new WaitForSeconds(typeSpeed);
            }

            // Wait before starting the next loop
            yield return new WaitForSeconds(0.5f);
        }
    }
}
