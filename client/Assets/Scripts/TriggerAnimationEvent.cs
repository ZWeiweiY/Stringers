using UnityEngine;

public class TriggerAnimationEvent : MonoBehaviour
{
    public GameObject vfx;
    public bool isplaying;
    public void TriggerVFX()
    {
        vfx.SetActive(true);
        
    }

    public void DisableVFX()
    {
        vfx.SetActive(false);
        
    }

    public void StartPlaying()
    {
        isplaying = true;
    }

    public void StopPlaying()
    {
        isplaying = false;
    }
}
