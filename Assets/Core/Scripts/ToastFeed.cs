using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ToastFeed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform toastContainer;
    [SerializeField] private GameObject toastPrefab;

    [Header("Settings")]
    [SerializeField] private float toastLifetime = 3f;

    public void Start()
    {

    }

    /// <summary>
    /// Spawns a new toast notification.
    /// Example: feed.SpawnToast("My cool text", Color.red);
    /// </summary>
    public void SpawnToast(string message, Color color)
    {
        GameObject toastObject = Instantiate(toastPrefab, toastContainer);
        Toast toast = toastObject.GetComponent<Toast>();

        if (toast == null)
        {
            Debug.LogError("Toast prefab is missing the Toast component.", toastObject);
            Destroy(toastObject);
            return;
        }
        toast.Setup(message, color);
    }
}