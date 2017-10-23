using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour {
    [SerializeField] Sprite emptyHead;
    [SerializeField] SpriteRenderer headRenderer, hatRenderer;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Hit(Vector3 position, Vector3 force) {
        headRenderer.sprite = emptyHead;
        transform.SetParent(null);
        hatRenderer.enabled = true;
        rb.isKinematic = false;
        rb.AddForceAtPosition(force, position);
        Invoke("Destroy", 2f + Random.value);
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
