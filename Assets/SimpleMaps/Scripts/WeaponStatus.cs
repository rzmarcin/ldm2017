using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponStatus : MonoBehaviour {
    public Weapon weapon;

    Animator anim;
    
    [SerializeField] string turnTrigger = "roll";
    [SerializeField] SpriteRenderer drum;
    [SerializeField] SpriteRenderer[] slots;
    [SerializeField] Color noneColor = Color.clear, shellColor = Color.yellow, loadedColor = Color.white;

    [SerializeField] ParticleSystem shellParticle, smokeParticle;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Start() {
        weapon.OnNextSlot += TurnChamber;
        weapon.OnInsertOne += InsertBullet;
        weapon.OnBulletFired += BulletFired;
    }

    void InsertBullet() {
        shellParticle.Emit(1);
    }

    void BulletFired() {
        smokeParticle.Emit(30);
    }

    void TurnChamber(bool fired, bool loaded) {
        anim.SetTrigger(turnTrigger);
        drum.transform.localEulerAngles = drum.transform.localEulerAngles - Vector3.forward * 60f;
    }
    private void LateUpdate() {
        for (int i = 0; i < weapon.Slots.Length; i++) {
            //for (int i = weapon.Slots.Length - 1; i >= 0; i--) {
            //int viewIndex = (i - turnCount + weapon.Slots.Length) % weapon.Slots.Length;
            switch (weapon.Slots[i]) {
                case Weapon.ChamberStatus.Loaded:
                    slots[i].color = loadedColor;
                    break;
                case Weapon.ChamberStatus.EmptyShell:
                    slots[i].color = shellColor;
                    break;
                default:
                    slots[i].color = noneColor;
                    break;
            }
        }
    }
}
