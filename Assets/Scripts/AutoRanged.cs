using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRanged : Weapon
{
    [SerializeField] private float recoilFactor;
    [SerializeField] private float kickFactor;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip reloadNoise;
    //[SerializeField] private AudioClip emptyNoise;
    [SerializeField] private AudioClip shootNoise;
    private int ammo = 0;
    public override IEnumerator UseWeapon()
    {
        float timer = 0;
        while(Input.GetMouseButton(0)){
            if(ammo > 0){
                PlayerController.source.PlayOneShot(shootNoise);
                weaponRB.AddForce(transform.right.normalized * -weaponForce * recoilFactor * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[2] * PlayerController.instance.StatModifiers[1]);
                weaponRB.AddTorque(weaponForce * recoilFactor * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[2] * PlayerController.instance.StatModifiers[1] * Random.Range(-kickFactor,kickFactor));
                GameObject bullet = Instantiate(projectilePrefab,transform.position,transform.rotation);
                bullet.GetComponent<Rigidbody2D>().mass *= PlayerController.instance.StatModifiers[4];
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.right.normalized * weaponForce * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[1]);
                ammo--;
                timer = 0;
                yield return new WaitForSeconds(fireRate / PlayerController.instance.StatModifiers[8]);
            }
            else{
                //exploitable but im pretending its intended because it would be funnier
                timer += Time.deltaTime;
                if(timer > reloadTime){ammo = Mathf.RoundToInt(maxAmmo * PlayerController.instance.StatModifiers[9]);PlayerController.source.PlayOneShot(reloadNoise);break;}
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public override IEnumerator UseWeaponAlt()
    {
        while(Input.GetMouseButton(1)){
            weaponRB.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.fixedDeltaTime * weaponForce * altfireMultiplier * PlayerController.instance.StatModifiers[3]);
            yield return new WaitForFixedUpdate();
        }
    }
}
