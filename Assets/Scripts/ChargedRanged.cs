using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedRanged : Weapon
{
    [SerializeField] private float recoilFactor;
    [SerializeField] private float kickFactor;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float chargeForce;
    [SerializeField] private float chargeBullets;
    [SerializeField] private float chargeRecoil;
    [SerializeField] private float shots;
    [SerializeField] private float spread;
    public override IEnumerator UseWeapon()
    {
        float timer = 0;
        while(Input.GetMouseButton(0)){
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for(int i = 0; i < shots + Mathf.RoundToInt(chargeBullets * timer); i++){
            weaponRB.AddForce(transform.right.normalized * (-weaponForce * recoilFactor + chargeRecoil * timer));
            weaponRB.AddTorque((-weaponForce * recoilFactor + chargeRecoil * timer) * Random.Range(-kickFactor,kickFactor) * timer);
            GameObject bullet = Instantiate(projectilePrefab,transform.position,transform.rotation);
            bullet.transform.Rotate(new Vector3(0,0,Random.Range(-spread, spread)));
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right.normalized * (weaponForce + chargeForce * timer) * Random.Range(1 - (spread / 90), 1 + (spread / 90)));
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
