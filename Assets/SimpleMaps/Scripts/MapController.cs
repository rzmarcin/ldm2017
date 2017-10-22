using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMaps {
    public class MapController : MonoBehaviour {
        [SerializeField] Transform target;

        MapData data;
        

        private void Awake() {

        }
        


        Vector2 WorldToMap(Vector2 world) {
            return default(Vector2);
        }
    }
}