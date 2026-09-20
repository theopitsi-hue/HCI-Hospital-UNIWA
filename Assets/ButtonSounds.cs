using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource audioSource => GameManager.Instance.globalSoundSource;
    public AudioClip clickSound;

    [Header("Pitch Settings")]
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (audioSource == null || clickSound == null)
            return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clickSound);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(PlayClickSound);
    }
}