using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class SimpleCharacterController : MonoBehaviour {
    [Header("Weapon")]
    [SerializeField] Weapon weaponPrototype;
    [SerializeField] WeaponStatus weaponStatusPrototype;
    [SerializeField] Transform weaponHook, revolverStatusHook;
    [SerializeField] bool createView = true;

    Animator anim;
    Weapon weapon;
    WeaponStatus weaponStatus;

    [Header("Animation config")]
    [SerializeField] string m_crouching = "crouching";
    [SerializeField] string m_changePose = "change";

    bool crouching;


    private void Awake() {
        anim = GetComponent<Animator>();
        weapon = Instantiate(weaponPrototype, weaponHook);

        if (createView) {
            weaponStatus = Instantiate(weaponStatusPrototype, revolverStatusHook);
            weaponStatus.weapon = weapon;
        }
    }

    public void AimWeapon(Vector3 target) {
        if (!crouching) {
            weaponHook.LookAt(target, Vector3.up);
        } else {
            weapon.transform.localRotation = Quaternion.identity;
        }
    }

    public void FireWeapon() {
         weapon.PullTrigger();
    }

    public void ToggleCook() {
        if (weapon.Cooked) {
            weapon.Uncook();
        } else {
            weapon.Cook();
        }
    }

    public void ReloadBullet() {
        weapon.InsertOne();
    }

    public void StandUp() {
        crouching = false;
        anim.SetBool(m_crouching, false);
        anim.SetTrigger(m_changePose);
    }

    public void Cover() {
        crouching = true;
        anim.SetBool(m_crouching, true);
        anim.SetTrigger(m_changePose);
    }
	
	
}
