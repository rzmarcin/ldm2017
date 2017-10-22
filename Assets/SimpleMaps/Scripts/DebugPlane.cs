using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class DebugPlane : MonoBehaviour {

#if UNITY_EDITOR
    [SerializeField] Color color = Color.white;
    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position - transform.right * 1000f, transform.position + transform.right * 1000f);
        Gizmos.DrawLine(transform.position - transform.forward * 1000f, transform.position + transform.forward * 1000f);
    }
#endif
}