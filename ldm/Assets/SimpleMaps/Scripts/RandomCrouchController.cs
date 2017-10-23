using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomCrouchController : MonoBehaviour {
    [SerializeField] float minTime, maxTime;
    [SerializeField] string triggerName = "changePosition";
    Animator animator;


    private void OnEnable() {
        animator = GetComponent<Animator>();
        StartCoroutine(Step());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator Step() {
        while (enabled && animator.enabled) {
            yield return new WaitForSeconds(Mathf.Lerp(minTime, maxTime, Random.value));
            animator.SetTrigger(triggerName);
        }

    }
    
}
