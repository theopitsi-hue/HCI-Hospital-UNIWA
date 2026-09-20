using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;

    [Header("Footstep Settings")]
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.4f;

    private float stepTimer;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = new Vector2(horizontal, vertical).magnitude > 0.1f;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepSounds.Length == 0)
            return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}