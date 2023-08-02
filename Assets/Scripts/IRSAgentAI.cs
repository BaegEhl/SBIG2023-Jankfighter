using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRSAgentAI : EnemyAI
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float[] attackSpeed;
    bool isStabilizing = false;
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
        if(isStabilizing){
            if(Mathf.Abs(Vector2.Dot(limbs[0].transform.up.normalized,Vector2.up.normalized)) > 0.75f && Mathf.Abs(Vector2.Dot(limbs[1].transform.up.normalized,Vector2.up.normalized)) > 0.75f){
                isStabilizing = false;
            }
        }
        else{
            if(Mathf.Abs(Vector2.Dot(limbs[0].transform.up.normalized,Vector2.up.normalized)) < 0.5f || Mathf.Abs(Vector2.Dot(limbs[1].transform.up.normalized,Vector2.up.normalized)) < 0.5f){
                isStabilizing = true;
            }
        }
    }
    void FixedUpdate(){
        foreach(Weapon weapon in weapons){
            StartCoroutine(weapon.UseWeaponAlt());
        }
        if(isStabilizing){
            limbRBs[0].AddTorque(legPower * Time.fixedDeltaTime);
            limbRBs[1].AddTorque(-legPower * Time.fixedDeltaTime);
        }
    }
}
