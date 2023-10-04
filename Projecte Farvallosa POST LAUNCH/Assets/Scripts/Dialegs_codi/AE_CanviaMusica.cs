using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_CanviaMusica : ActivadorEvent
{
    // Start is called before the first frame update
    public AudioClip audioClip;
    private SoundManager soundManager;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }


    public override void Activar()
    {
        if (soundManager != null && audioClip != null)
        {

            soundManager.SoftStopMusic(audioClip);
        }
        else
            Debug.Log("AE_CanviaMusica Activar() no s'ha pogut canviar la musica");
    }
}
