using UnityEngine;

public class SoundBeforeDestroy : MonoBehaviour
{
    public AudioClip destroySound;
    public float volume = 1.0f;

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(destroySound, transform.position, volume);
    }
}
