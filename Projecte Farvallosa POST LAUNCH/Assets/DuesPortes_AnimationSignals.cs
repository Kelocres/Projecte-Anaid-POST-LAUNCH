using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuesPortes_AnimationSignals : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /*public void SetIsObert_False()
    {
        anim
    }

    public void SetIsObert_True()
    {

    }*/

    public void SetIsObert(bool intro)
    {
        animator.SetBool("isObert", intro);
    }

}
