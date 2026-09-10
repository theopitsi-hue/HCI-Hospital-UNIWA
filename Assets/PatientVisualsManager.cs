using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class PatientVisualsManager : MonoBehaviour
{
    [SerializeField] private Renderer modelRenderer;
    [SerializeField] private int[] materialSlots;

    [SerializeField] private Color hypoxiaColor = Color.blue;
    [SerializeField] private float transitionDuration = 3f;

    private Coroutine tintCoroutine;

    private Color selectedColor;
    private bool hasInitialized = false;

    public BlackboardKey skinState;

    private void Update()
    {
        if (!skinState.TryGetValue(out var v))
            return;

        float val = (float)v.GetValue();

        Color newColor;

        if (val == 0)
        {
            newColor = Color.white;
        }
        else if (val == 1)
        {
            newColor = hypoxiaColor;
        }
        else
        {
            return;
        }

        // First initialization
        if (!hasInitialized)
        {
            selectedColor = newColor;
            hasInitialized = true;

            SetMaterialColor(selectedColor);
            return;
        }

        // Only start transition if the color changed
        if (newColor != selectedColor)
        {
            selectedColor = newColor;
            Tint();
        }
    }

    public void Tint()
    {
        if (tintCoroutine != null)
            StopCoroutine(tintCoroutine);

        tintCoroutine = StartCoroutine(TintMaterials());
    }

    private IEnumerator TintMaterials()
    {
        if (modelRenderer == null)
            yield break;

        Material[] materials = modelRenderer.materials;

        // Store the starting colors only for the selected material slots
        Color[] startColors = new Color[materialSlots.Length];

        for (int i = 0; i < materialSlots.Length; i++)
        {
            int slot = materialSlots[i];

            if (slot >= 0 &&
                slot < materials.Length &&
                materials[slot].HasProperty("_Color"))
            {
                startColors[i] = materials[slot].color;
            }
            else
            {
                startColors[i] = Color.white;
            }
        }

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / transitionDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            // Only affect the selected renderer and selected material slots
            for (int i = 0; i < materialSlots.Length; i++)
            {
                int slot = materialSlots[i];

                if (slot >= 0 &&
                    slot < materials.Length &&
                    materials[slot].HasProperty("_Color"))
                {
                    materials[slot].color = Color.Lerp(
                        startColors[i],
                        selectedColor,
                        t
                    );
                }
            }

            yield return null;
        }

        // Ensure the final color is exact
        SetMaterialColor(selectedColor);

        tintCoroutine = null;
    }

    private void SetMaterialColor(Color color)
    {
        if (modelRenderer == null)
            return;

        Material[] materials = modelRenderer.materials;

        foreach (int slot in materialSlots)
        {
            if (slot >= 0 &&
                slot < materials.Length &&
                materials[slot].HasProperty("_Color"))
            {
                materials[slot].color = color;
            }
        }
    }
}