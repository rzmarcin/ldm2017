using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EditorTools {
    [MenuItem("Component/Attach ragdoll manager")]
    public static void AttachRagdollScripts() {
        int parents = 0, rigidbodies = 0;
        foreach (GameObject go in Selection.gameObjects) {
            foreach (Component cp in go.GetComponentsInChildren(typeof(RagdollManager))
                .Union(go.GetComponentsInChildren<RagdollParent>())) {
                GameObject.DestroyImmediate(cp);
            }
            go.AddComponent<RagdollParent>();
            
            parents++;
            foreach (CharacterJoint rb in go.GetComponentsInChildren<CharacterJoint>()) {
                    rb.gameObject.AddComponent<RagdollManager>();
                    rigidbodies++;
            }
        }
        Debug.LogFormat("Ragdolls: {0}, joints: {1}", parents, rigidbodies);
    }
}
