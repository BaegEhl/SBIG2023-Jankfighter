using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedRanged : Weapon
{
    [SerializeField] private float recoilFactor;
    [SerializeField] private GameObject projectilePrefab;
    public override IEnumerator UseWeapon()
    {
        float timer = 0;
        while(Input.GetMouseButton(0)){
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        weaponRB.AddForce(transform.right.normalized * -weaponForce * recoilFactor * timer);
        weaponRB.AddTorque(weaponForce * recoilFactor * Random.Range(-0.25f,0.25f) * timer);
        GameObject bullet = Instantiate(projectilePrefab,transform.position,transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right.normalized * weaponForce * timer);
    }
    public override IEnumerator UseWeaponAlt()
    {
        while(Input.GetMouseButton(1)){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.fixedDeltaTime * weaponForce * altfireMultiplier);
            yield return new WaitForFixedUpdate();
        }
    }
}
