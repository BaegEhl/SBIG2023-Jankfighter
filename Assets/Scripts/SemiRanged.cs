using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiRanged : Weapon
{
    [SerializeField] private float recoilFactor;
    [SerializeField] private float kickFactor;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int reloadAmount;
    [SerializeField] private int projectileCount;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool playerWeapon;
    [SerializeField] private float gravityFactor;
    [SerializeField] private float spread;
    private int ammo = 0;
    private bool isReloading = false;
    public override IEnumerator UseWeapon()
    {
        if(ammo > 0 && !isReloading){
            for(int i = 0; i < projectileCount; i++){
                weaponRB.AddForce(transform.right.normalized * -weaponForce * recoilFactor);
                weaponRB.AddTorque(weaponForce * recoilFactor * Random.Range(-kickFactor,kickFactor));
                GameObject bullet = Instantiate(projectilePrefab,transform.position,transform.rotation);
                bullet.transform.Rotate(new Vector3(0,0,Random.Range(-spread,spread)));
                bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right.normalized * weaponForce * Random.Range(1 - (spread / 90), 1 + (spread / 90)));
                ammo--;
            }
        }
        else{
            ammo += reloadAmount;
            if(ammo >= maxAmmo){ammo = maxAmmo; isReloading = false;}
            else{isReloading = true;}
        }
        yield return null;
    }
    public override IEnumerator UseWeaponAlt()
    {
        if(playerWeapon){
            while(Input.GetMouseButton(1)){
                weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.fixedDeltaTime * weaponForce * altfireMultiplier);
                yield return new WaitForFixedUpdate();
            }
        }
        else{
            weaponRB.AddForce(((PlayerController.instance.transform.position + (Vector3.up * Vector2.Distance(transform.position,PlayerController.instance.transform.position) * gravityFactor)) - transform.position).normalized * Time.fixedDeltaTime * weaponForce * altfireMultiplier);
        }
    }
}
