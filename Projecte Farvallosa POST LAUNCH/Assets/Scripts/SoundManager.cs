using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public static SoundManager instance;
    public AudioSource musicSource;
    public AudioSource soundSource;

    public float initialVolume;
    public float fadeVolumeTime = 1.0f;

    /*private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }*/

    public void Update()
    {
        
    }

    public void SoftStopMusic(AudioClip newMusic)
    {
        initialVolume = musicSource.volume;
        StartCoroutine(MusicFadeOut(newMusic));
    }

    IEnumerator MusicFadeOut(AudioClip newMusic)
    {
        Debug.Log("SoundManager MusicFadeOut()");
        while (musicSource.volume > 0f)
        {
            musicSource.volume = Mathf.Lerp(initialVolume, 0f, fadeVolumeTime);
            yield return null;
        }

        if(newMusic != null)
        {
            Debug.Log("SoundManager MusicFadeOut() canviar musica");
            musicSource.clip = newMusic;
            musicSource.volume = initialVolume;
            musicSource.Play();
        }
        
    }

    public void StartMusic(AudioClip song)
    {
        Debug.Log("SoundManager StartMusic()");
        musicSource.clip = song;
        musicSource.Play();
    }

    public void StartMusic()
    {
        Debug.Log("SoundManager StartMusic()");
        if(musicSource.clip!= null)
            musicSource.Play();
        else
            Debug.Log("SoundManager StartMusic() Clip == null!!!");
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySound(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }

    //L'unic so del soundSource es el dels botons
    public void PlayButtonSound()
    {
        soundSource.Play();
    }
}
