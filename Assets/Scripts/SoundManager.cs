using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    public AudioClip[] musicTracks;

    public AudioSource musicPlayer;
    public AudioSource sfxCorrect;
    public AudioSource sfxIncorrect;
    public AudioSource sfxButtonClick;

    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else if (INSTANCE != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        PlayRandomMusic();
    }

    private void PlayRandomMusic()
    {
        var randomIndex = Random.Range(0, musicTracks.Length);
        musicPlayer.clip = musicTracks[randomIndex];
        musicPlayer.Play();
    }

    public void PlayButtonClick()
    {
        sfxButtonClick.Play();
    }
}
