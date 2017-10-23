using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollParent : MonoBehaviour {
    List<Rigidbody> ragdolls = new List<Rigidbody>();

    Animator anim;

    [SerializeField] bool freezed = true;

    public bool Freezed { get { return freezed; } set { if (freezed != value) { freezed = value;  ResetKinematic(); Freezed = value; } } }


    private void OnValidate() {
        ResetKinematic();
    }

    private void Awake() {
        anim = GetComponent<Animator>();
        ResetKinematic();
    }

    void ResetKinematic() {
        foreach(Rigidbody rb in ragdolls) {
            rb.isKinematic = Freezed;
        }
        if (Application.isPlaying && anim != null) {
            anim.enabled = freezed;
        }
    }

    void Release() {
        foreach (Rigidbody rb in ragdolls) {
            rb.isKinematic = false;
        }
    }

    void Freeze() {
        foreach (Rigidbody rb in ragdolls) {
            rb.isKinematic = true;
        }
    }

    public void Add(RagdollManager rm) {
        ragdolls.Add(rm.GetComponent<Rigidbody>());
    }

    public void Remove(RagdollManager rm) {
        ragdolls.Remove(rm.GetComponent<Rigidbody>());
    }
}
