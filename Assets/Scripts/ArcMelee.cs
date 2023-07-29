using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMelee : Weapon
{
    [SerializeField] private float chargeSmash;
    public override IEnumerator UseWeapon()
    {
        while(Input.GetMouseButton(0)){
            if(transform.position.x > PlayerController.instance.transform.position.x){weaponRB.AddTorque(-weaponForce * Time.fixedDeltaTime);}
            else{weaponRB.AddTorque(weaponForce * Time.fixedDeltaTime);}
            yield return new WaitForFixedUpdate();
        }
    }
    public override IEnumerator UseWeaponAlt()
    {
        float charge = 0;
        while(Input.GetMouseButton(1)){
            charge += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(transform.position.x > PlayerController.instance.transform.position.x){weaponRB.AddTorque(-weaponForce * charge * altfireMultiplier);}
        else{weaponRB.AddTorque(weaponForce * charge * altfireMultiplier);}
        weaponRB.mass *= chargeSmash;
        yield return new WaitForSeconds(1);
        weaponRB.mass /= chargeSmash;
    }
}
