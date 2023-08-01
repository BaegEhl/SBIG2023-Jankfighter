using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustMelee : Weapon
{
    [SerializeField] private float chargeDuration;
    [SerializeField] private bool playerWeapon;
    public override IEnumerator UseWeapon()
    {
        if(playerWeapon){
            while(Input.GetMouseButton(0)){
                weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.fixedDeltaTime * weaponForce);
                yield return new WaitForFixedUpdate();
            }
        }
        else{
            weaponRB.AddForce(((PlayerController.instance.transform.position) - transform.position).normalized * Time.fixedDeltaTime * weaponForce);
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
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * charge * weaponForce * altfireMultiplier * Time.fixedDeltaTime / chargeDuration);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
