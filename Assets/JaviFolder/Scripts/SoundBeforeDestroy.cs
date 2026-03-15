using UnityEngine;
using UnityEngine.VFX;

public class SoundBeforeDestroy : MonoBehaviour
{
    public AudioClip destroySound;
    public float volume = 1.0f;

    public ParticleSystem vfx;

    public void PlaySound()
    {
        vfx.Play();
        AudioSource.PlayClipAtPoint(destroySound, transform.position, volume);
    }
}
