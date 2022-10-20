using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private AudioClip[] musicList = null;

    [SerializeField] private bool destroyOnSceneChange = true;
    [SerializeField] private bool randomMusicOnStartup = true;

    void Start()
    {
        if(!destroyOnSceneChange)
        {
            GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("MusicPlayer");

            if (musicPlayers.Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("MusicPlayer");

            for (int i = 0; i < musicPlayers.Length; i++)
            {
                if (musicPlayers[i] != gameObject)
                {
                    Destroy(musicPlayers[i]);
                }
            }
        }
        if(randomMusicOnStartup)
        {
            source.clip = GetRandomAudioClip();
            source.Play();
        }
    }

    public AudioClip GetRandomAudioClip()
    {
        if(musicList.Length > 0)
        {
            return musicList[Random.Range(0, musicList.Length)];
        }
        return null;
    }

    public void SetAudioClip(int index)
    {
        source.clip = musicList[index];
    }

    public void PlayMusic()
    {
        source.Play();
    }

    public void PauseMusic(bool state)
    {
        switch (state)
        {
            default:
            case true:
            source.Pause();
                break;
            case false:
            source.UnPause();
                break;
        }
    }

    public void StopMusic()
    {
        source.Stop();
    }
}
