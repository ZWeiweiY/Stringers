using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : Singleton<SoundManager>
{
    //public LevelManager levelManager;

    [Header("SoundEffects")]
    public AudioClip countDownSE;
    public AudioClip deathSE;
    public AudioClip rebornSE;
    public AudioClip levelFinishSE;
    public AudioClip eatPlanetSE;
    public AudioClip firingSE;
    public AudioClip dashSE;
    public AudioClip explosionSE;
    public AudioClip shrinkSE;
    public AudioClip expandSE;


    //private STGShipController shipController;
    //private RBMoveController playerController;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        SetTheGameBGM();
        //shipController = gameObject.GetComponent<STGShipController>();
        //playerController = gameObject.GetComponent<RBMoveController>();
    }
    private void Start()
    {

    }
    private void OnEnable()
    {
        //shipController.FireActionTriggered += PlayFiringSound;
        //shipController.DashActionTriggered += PlayDashSound;
    }
    private void OnDisable()
    {
        //shipController.FireActionTriggered -= PlayFiringSound;
        //shipController.DashActionTriggered -= PlayDashSound;
    }
    public void SetTheGameBGM() 
    {

    }

    public void PlayCountdownSound()
    {
        if (countDownSE != null)
        {
            audioSource.PlayOneShot(countDownSE);
        }
    }
    public void PlayDeathSound()
    {
        if (deathSE != null)
        {
            audioSource.PlayOneShot(deathSE);
        }
    }

    public void PlayRebornSound()
    {
        if (rebornSE != null)
        {
            audioSource.PlayOneShot(rebornSE);
        }
    }

    public void PlayLevelFinishSound()
    {
        if (levelFinishSE != null)
        {
            audioSource.PlayOneShot(levelFinishSE);
        }
    }

    public void PlayEatPlanetSound()
    {
        if (eatPlanetSE != null)
        {
            audioSource.PlayOneShot(eatPlanetSE);
        }
    }

    public void PlayFiringSound() 
    {
        if (firingSE != null) 
        {
            audioSource.PlayOneShot(firingSE);
        }
    }
    public void PlayDashSound()
    {
        if (dashSE != null)
        {
            audioSource.PlayOneShot(dashSE);
        }
    }
    public void PlayExplosionSound()
    {
        if (explosionSE != null)
        {
            audioSource.PlayOneShot(explosionSE);
        }
    }

    public void PlayShrinkSound()
    {
        if (shrinkSE != null)
        {
            audioSource.PlayOneShot(shrinkSE);
        }
    }

    public void PlayExpandSound()
    {
        if (expandSE != null)
        {
            audioSource.PlayOneShot(expandSE);
        }
    }
}
