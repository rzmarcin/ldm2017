using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollManager : MonoBehaviour {
    RagdollParent parent;
    Rigidbody rb;

    private void Awake() {
        parent = GetComponentInParent<RagdollParent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        parent.Add(this);

    }

    void ReceiveHit(params object[] args) {
        Vector3 position = (Vector3)args[0];
        Vector3 force = (Vector3)args[1];
        ReceiveHit(position, force);
    }

    public void ReceiveHit(Vector3 position, Vector3 force) {

        parent.Freezed = false;

        rb.AddForceAtPosition(force, position);

    }
}
