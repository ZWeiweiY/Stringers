using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Header("BGM")]
    public AudioClip mainMenu;
    public AudioClip playgroundMusic;
    public AudioClip level2Music;
    public AudioClip level3Music;


    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
            return;
        }
        audioSource.loop = true;
        PlayBGM(mainMenu, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayBGM(AudioClip audioClip, float volume)
    {
        if (audioSource.isPlaying && audioSource.clip == audioClip)
        {
            return;
        }

        if (volume != audioSource.volume)
        {
            audioSource.volume = volume;
            return;
        }

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

}
