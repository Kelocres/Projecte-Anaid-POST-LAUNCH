using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//REFERENCIA : https://learn.unity.com/tutorial/starting-timeline-through-a-c-script-2019-3#5ff8d183edbc2a0020996601
public class ProvaCinemachine1 : MonoBehaviour
{
    private PlayableDirector director;
    //public GameObject controlPanel;
    // Start is called before the first frame update
    public BoxCollider collider;
    
    void Awake()
    {
      //Inside Awake, we'll set our Playable Director reference and register the
      //callback functions, to be invoked when the Director is started and stopped:
      //director = GetComponent<PlayableDirector>();    
      director = FindObjectOfType<PlayableDirector>();
      director.played += Director_Played;
      director.stopped += Director_Stoped;

      //collider.GetComponent<BoxCollider>(); //NO FUNCIONA!!! ES TE QUE FER EN L'EDITOR!!!
    }

    void Start()
    {
      //collider.GetComponent<BoxCollider>();
    }

    //Director_Played will hide the UI panel used to start the Timeline once the Timeline has started playing
    private void Director_Played(PlayableDirector obj)
    {
      //controlPanel.SetActive(false);
      //collider.enabled = false;
    }

    //Director_Stoped will show the UI panel after the Timeline has stopped
    private void Director_Stoped(PlayableDirector obj)
    {
      //controlPanel.SetActive(true);
      collider.enabled = true;
    }

    public void StartTimeline()
    {
      director.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.tag=="Player")
        StartTimeline();
    }
}
