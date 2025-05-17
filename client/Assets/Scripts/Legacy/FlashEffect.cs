using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour
{
    public Image flashImage; // Assign the UI Image in the Inspector
    public float flashDuration = 0.5f;

    void Start()
    {
        flashImage.color = new Color(1, 1, 1, 0); // Start transparent
    }

    public void TriggerFlash(float time)
    {
        StartCoroutine(FlashRoutine(time));
    }

    private IEnumerator FlashRoutine(float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / time);
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        flashImage.color = new Color(1, 1, 1, 0); // Ensure it's fully transparent
    }
}