using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRanged : Weapon
{
    [SerializeField] private float recoilFactor;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject projectilePrefab;
    private int ammo = 0;
    public override IEnumerator UseWeapon()
    {
        float timer = 0;
        while(Input.GetMouseButton(0)){
            if(ammo > 0){
                weaponRB.AddForce(transform.right.normalized * -weaponForce * recoilFactor);
                weaponRB.AddTorque(weaponForce * recoilFactor * Random.Range(-0.25f,0.25f));
                ammo--;
                timer = 0;
                yield return new WaitForSeconds(fireRate);
            }
            else{
                //exploitable but im pretending its intended because it would be funnier
                timer += Time.deltaTime;
                if(timer > reloadTime){ammo = maxAmmo;break;}
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public override IEnumerator UseWeaponAlt()
    {
        while(Input.GetMouseButton(1)){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.fixedDeltaTime * weaponForce * altfireMultiplier);
            yield return new WaitForFixedUpdate();
        }
    }
}
