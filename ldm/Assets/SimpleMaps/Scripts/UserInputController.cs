using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour {
    SimpleCharacterController characterController;
    [SerializeField] float minAimDistance = 3f;


    [SerializeField] KeyCode keyCook = KeyCode.Mouse1;
    [SerializeField] KeyCode keyFireWeapon = KeyCode.Mouse0;
    [SerializeField] KeyCode keyReload = KeyCode.R;

    Ray ray = new Ray();
    RaycastHit hit;

    [SerializeField] LayerMask hitMask;
    [SerializeField] float maxDistance = 100f;

    private void Awake() {
        characterController = GetComponent<SimpleCharacterController>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 point;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
        if (Physics.Raycast(ray, out hit, maxDistance, hitMask.value)) {
            point = hit.point;
            Debug.DrawLine(ray.origin, point, Color.red);
            Debug.DrawLine(point, ray.GetPoint(maxDistance), new Color(1, 1, 1, .3f));
        } else {
            point = ray.GetPoint(maxDistance);
            Debug.DrawLine(ray.origin, point, Color.white);
        }
        
        if (Vector3.Dot(characterController.transform.forward, point - characterController.transform.position) > minAimDistance) {
            characterController.StandUp();
            characterController.AimWeapon(point);
        } else {
            characterController.Cover();
        }
        

        if (Input.GetKeyDown(keyCook)) {
            characterController.ToggleCook();
        } else if (Input.GetKeyDown(keyFireWeapon)) {
            characterController.FireWeapon();
        }

        if (Input.GetKeyDown(keyReload)) {
            characterController.ReloadBullet();
        }
    }
}
