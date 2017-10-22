using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMaps {
    public class PositionController : MonoBehaviour {
        [SerializeField] Channel walkingChannel = Channel.A;

        [SerializeField] float baseWalkingSpeed = 1f;

        MapData data;

        public Vector4 currentData;
        public Vector4 directionData;
        Vector3 currentDirection;
        float currentSpeed;

        private void Start() {
            data = FindObjectOfType<MapData>();
        }

        private void Update() {
            currentData = data[transform.position];
            currentDirection = Vector2.ClampMagnitude(Vector2.right * Input.GetAxis("Horizontal") 
                + Vector2.up * Input.GetAxis("Vertical"), 1);
            if (directionData.sqrMagnitude > 0) {
                Vector3 ds = currentDirection * Time.fixedDeltaTime * baseWalkingSpeed;
                directionData = data[transform.position + ds];
                transform.position = transform.position + ds * directionData.GetValue(walkingChannel);
            } else {
                directionData = currentData;
            }

        }

        private void FixedUpdate() {
        }
    }
}