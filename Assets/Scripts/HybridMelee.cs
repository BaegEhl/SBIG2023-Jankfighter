using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridMelee : Weapon
{
    public override IEnumerator UseWeapon()
    {
        while(Input.GetMouseButton(0)){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * Time.fixedDeltaTime * weaponForce);
            yield return new WaitForFixedUpdate();
        }
    }
    public override IEnumerator UseWeaponAlt()
    {
        while(Input.GetMouseButton(1)){
            if(transform.position.x > PlayerController.instance.transform.position.x){weaponRB.AddTorque(-weaponForce * Time.fixedDeltaTime);}
            else{weaponRB.AddTorque(weaponForce * Time.fixedDeltaTime);}
            yield return new WaitForFixedUpdate();
        }
    }
}
