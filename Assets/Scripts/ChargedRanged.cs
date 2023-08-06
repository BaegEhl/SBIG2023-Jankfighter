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
    [SerializeField] private AudioClip shootNoise;
    [SerializeField] private AudioSource source;
    void Awake(){source.Pause();}
    public override IEnumerator UseWeapon()
    {
        source.UnPause();
        //source.mute = false;
        float timer = 0;
        while(Input.GetMouseButton(0)){
            timer += Time.deltaTime * PlayerController.instance.StatModifiers[6];
            yield return new WaitForEndOfFrame();
        }
        source.Pause();
        PlayerController.source.PlayOneShot(shootNoise, Mathf.Max(timer / (timer + 10), 0.05f));
        for(int i = 0; i < shots + Mathf.RoundToInt(chargeBullets * timer); i++){
            weaponRB.AddForce(transform.right.normalized * (-weaponForce * recoilFactor + chargeRecoil * timer) * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[2] * PlayerController.instance.StatModifiers[1]);
            weaponRB.AddTorque((-weaponForce * recoilFactor + chargeRecoil * timer) * Random.Range(-kickFactor,kickFactor) * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[2] * PlayerController.instance.StatModifiers[1]);
            GameObject bullet = Instantiate(projectilePrefab,transform.position,transform.rotation);
            bullet.GetComponent<Rigidbody2D>().mass *= PlayerController.instance.StatModifiers[4];
            bullet.transform.Rotate(new Vector3(0,0,Random.Range(-spread * PlayerController.instance.StatModifiers[5], spread * PlayerController.instance.StatModifiers[5])));
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right.normalized * (weaponForce + chargeForce * timer) * Random.Range(1 - ((spread * PlayerController.instance.StatModifiers[5]) / 90), 1 + ((spread * PlayerController.instance.StatModifiers[5]) / 90)) * PlayerController.instance.StatModifiers[3] * PlayerController.instance.StatModifiers[1]);
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
