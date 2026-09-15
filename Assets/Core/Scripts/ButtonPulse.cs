using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float lowAlpha = 0.1f;
    [SerializeField] private float highAlpha = 0.5f;
    [SerializeField] private float pulseSpeed = 2f;

    [Header("Outline")]
    [SerializeField] private Color outlineColor = Color.black;
    [SerializeField] private Vector2 outlineDistance = new Vector2(3f, 3f);

    private Image buttonImage;
    private Outline outline;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();

        // Add an Outline component automatically if one doesn't exist
        outline = GetComponent<Outline>();

        if (outline == null)
            outline = gameObject.AddComponent<Outline>();

        outline.effectColor = outlineColor;
        outline.effectDistance = outlineDistance;
    }

    private void Update()
    {
        // Smoothly oscillate between 0 and 1
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // Convert pulse into alpha range
        float alpha = Mathf.Lerp(lowAlpha, highAlpha, pulse);

        Color color = buttonImage.color;
        color.a = alpha;
        buttonImage.color = color;
    }

}