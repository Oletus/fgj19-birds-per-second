using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Bird_Random_Anim : MonoBehaviour
{
    int randomNum;
    float newRandomNum;
    Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine("GetRandom");
    }

    public IEnumerator GetRandom()
    {
        randomNum = Random.Range(1, 10);
        newRandomNum = randomNum * 1.0f;
        //Debug.Log("waiting for... " + newRandomNum);

        yield return new WaitForSeconds(newRandomNum);

        // Debug.Log("should have called... " + newRandomNum);
        animator.SetTrigger("Trigger");
    }
}
