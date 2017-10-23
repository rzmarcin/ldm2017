using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    const int maxAmmo = 6;
    [SerializeField] Transform muzzle;
    [Space(10)]
    [SerializeField] LayerMask hitMask;
    [SerializeField] float hitForce = 1000f;
    [SerializeField] float range = 1000f;
    [Space(10)]
    [SerializeField] LineRenderer shotLine;
    [SerializeField] float lineTime = .1f;
    [Space(10)]
    [SerializeField] bool easyReload = true;
    [Header("Debug data"), SerializeField]
    ChamberStatus[] slots = new ChamberStatus[maxAmmo];

    ChamberStatus currentSlot { get { return slots[drumPosition]; } set { slots[drumPosition] = value; } }

    int drumPosition;
    public bool Cooked { get; private set; }
    public bool Opened { get; private set; }

    bool closed { get { return !Opened; } set { Opened = !value; } }

    public ChamberStatus[] Slots { get { return slots; } }

    public delegate void NextSlotAction(bool fired, bool loaded);
    public event NextSlotAction OnNextSlot;

    public delegate void InsertOneAction();
    public event InsertOneAction OnInsertOne;

    public delegate void BulletFiredAction();
    public event BulletFiredAction OnBulletFired;

    public void PullTrigger() {
        closed |= easyReload;
        if (CanFire) {
            Cooked = false;
            if (ChamberStatus.Loaded.Equals(currentSlot)) {
                FireBullet();
                if (OnBulletFired != null) {
                    OnBulletFired();
                }
                currentSlot = ChamberStatus.EmptyShell;
            }
            NextSlot();
        }
    }

    bool CanFire { get { return Cooked && closed; } }

    void FireBullet() {
        Ray ray = new Ray(muzzle.position, muzzle.transform.forward);
        RaycastHit hit;
        Vector3 point;
        if (Physics.Raycast(ray, out hit, range, hitMask.value)) {
            point = hit.point;

            if (hit.rigidbody != null) {
                if (hit.rigidbody.GetComponent<RagdollManager>() != null) {
                    hit.rigidbody.GetComponent<RagdollManager>().ReceiveHit(hit.point, ray.direction * hitForce);
                } else if (hit.rigidbody.GetComponent<HatController>() != null) {
                    hit.rigidbody.GetComponent<HatController>().Hit(hit.point, ray.direction * hitForce);
                    } else {
                    hit.rigidbody.AddForceAtPosition(ray.direction * hitForce, hit.point);
                }
            }
        } else {
            point = ray.GetPoint(range);
        }
        StartCoroutine(ShotFrame(point));
    }

    IEnumerator ShotFrame(Vector3 hitPoint) {
        shotLine.positionCount = 2;
        shotLine.SetPositions(new Vector3[] { muzzle.transform.position, hitPoint });
        yield return new WaitForSeconds(lineTime);
        shotLine.positionCount = 0;
    }

    #region ammo control
    public void InsertOne() {
        if (easyReload) {
            //reload first empty chamber
            for (int i = 0; i < slots.Length; i++) {
                int checkPosition = (drumPosition + i) % slots.Length;
                if (!ChamberStatus.Loaded.Equals(slots[checkPosition])) {
                    if (OnInsertOne != null) {
                        OnInsertOne();
                    }
                    slots[checkPosition] = ChamberStatus.Loaded;
                    break;
                }
            }
        } else if (Opened && ChamberStatus.None.Equals(currentSlot)) {
            currentSlot = ChamberStatus.Loaded;
            if (OnInsertOne != null) {
                OnInsertOne();
            }
            NextSlot();
        }
    }

    public void DropAmmo() {
        if (Opened) {
            for (int i = 0; i < slots.Length; i++) {
                slots[i] = ChamberStatus.None;
            }
        }
    }

    public void Cook() {
        if (closed) {
            Cooked = true;
        }
    }
    
    public void Uncook() {
        Cooked = false;
    }

    public void OpenDrum() {
        Uncook();
        Opened = true;
    }

    public void CloseDrum() {
        Uncook();
        closed = true;
    }

    void NextSlot() {
        ChamberStatus status = currentSlot;
        drumPosition = (drumPosition + 1) % slots.Length;

        if (OnNextSlot != null) {
            OnNextSlot(ChamberStatus.EmptyShell.Equals(status), ChamberStatus.Loaded.Equals(status));
        }
    }

    public void ReloadAll() {
        Uncook();
        if (easyReload) {
            for (int i = 0; i < slots.Length; i++) {
                slots[i] = ChamberStatus.Loaded;
            }
        } else {
            OpenDrum();
        }
    }

    public enum ChamberStatus {
        None,
        EmptyShell,
        Loaded,
    }
    #endregion
}
