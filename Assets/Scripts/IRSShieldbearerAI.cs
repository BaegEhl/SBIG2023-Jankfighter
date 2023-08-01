using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRSShieldbearerAI : MonoBehaviour
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float[] attackSpeed;
    [SerializeField] private float[] attackTimer;
    [SerializeField] private float legPower;
    void Update(){
        for(int i = 0; i < attackTimer.Length; i++){
            attackTimer[i] += Time.deltaTime;
            if(attackTimer[i] > attackSpeed[i]){
                attackTimer[i] = 0;
                StartCoroutine(weapons[i].UseWeapon());
            }
        }
    }
    void FixedUpdate(){
        for(int i = 0; i < attackTimer.Length; i++){
            StartCoroutine(weapons[i].UseWeaponAlt());
        }
        StartCoroutine(weapons[weapons.Length - 1].UseWeapon());
        if(transform.position.x > PlayerController.instance.transform.position.x){
            limbRBs[0].AddTorque(legPower * Time.fixedDeltaTime);
            limbRBs[1].AddTorque(legPower * Time.fixedDeltaTime);
        }
        else{
            limbRBs[0].AddTorque(-legPower * Time.fixedDeltaTime);
            limbRBs[1].AddTorque(-legPower * Time.fixedDeltaTime);
        }
    }
}
