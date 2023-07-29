using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustMelee : Weapon
{
    [SerializeField] private float chargeDuration;
    public override IEnumerator UseWeapon()
    {
        while(Input.GetMouseButton(0)){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * Time.fixedDeltaTime * weaponForce);
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
        float timer = chargeDuration;
        while(timer > 0){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * charge * weaponForce * altfireMultiplier * Time.fixedDeltaTime / chargeDuration);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
