using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float weaponForce;
    [SerializeField] protected float altfireMultiplier;
    [SerializeField] protected Rigidbody2D weaponRB;
    public abstract IEnumerator UseWeapon();
    public abstract IEnumerator UseWeaponAlt();
}
