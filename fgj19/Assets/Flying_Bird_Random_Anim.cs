using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Bird_Random_Anim : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine("GetRandom");
    }

    public IEnumerator GetRandom()
    {
        float randomDelay = Random.Range(5f, 20f);

        yield return new WaitForSeconds(randomDelay);

        animator.SetTrigger("Trigger");
    }
}
