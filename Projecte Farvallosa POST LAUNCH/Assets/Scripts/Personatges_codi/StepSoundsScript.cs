using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundsScript : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StepSound()
    {
        if(audioSource == null || clips == null || clips.Length == 0)
        {
            Debug.Log("StepSoundsScript StepSound() audioSource == null || clips == null || clips.Length == 0");
            return;
        }

        int random = Random.Range(0, clips.Length);
        if(clips[random]!=null)
        {
            audioSource.clip = clips[random];
            audioSource.Play();
        }
        else
        {
            Debug.Log("StepSoundsScript StepSound() clips[random]==null");
        }
    }
}
