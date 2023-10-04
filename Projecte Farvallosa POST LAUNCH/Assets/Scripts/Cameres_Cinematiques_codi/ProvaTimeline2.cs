using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ProvaTimeline2 : MonoBehaviour
{
    //Referencia per a crear NavMeshSurface en Unity 2019:
    //https://docs.unity3d.com/es/2019.4/Manual/nav-BuildingNavMesh.html
    
    public PlayableDirector director;
    //public GameObject controlPanel;
    // Start is called before the first frame update
    public BoxCollider collider;


    public CineTriggerScript [] cineTriggers;
    
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
      //collider.enabled = false;   //EL COLLIDER HA DE ESTAR SEMPRE ACTIVAT PER A COL·LISIONAR DESPRÉS AMB EL CINETRIGGER
    }

    //Director_Stoped will show the UI panel after the Timeline has stopped
    private void Director_Stoped(PlayableDirector obj)
    {
      //controlPanel.SetActive(true);
      //collider.enabled = true;
    }

    public void StartTimeline()
    {
      //Activar tots els CineTriggers asociats al timeline
      foreach(CineTriggerScript ct in cineTriggers)
        if(ct!=null)
          ct.activat = true;
          
      director.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.tag=="Player")
        StartTimeline();
    }
}
