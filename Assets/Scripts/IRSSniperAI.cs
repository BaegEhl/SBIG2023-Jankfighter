using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRSSniperAI : MonoBehaviour
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float[] attackSpeed;
    bool isStabilizing = false;
    [SerializeField] private float[] attackTimer;
    [SerializeField] private float legPower;
    [SerializeField] private float reverseForce;
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
                /*limbRBs[0].mass *= 100;
                limbRBs[1].mass *= 100;
                limbRBs[2].mass *= 100;
                limbRBs[3].mass *= 100;*/
                //limbRBs[4].gravityScale /= 2;
                //limbRBs[5].gravityScale /= 2;
            }
        }
        else{
            if(Mathf.Abs(Vector2.Dot(limbs[0].transform.up.normalized,Vector2.up.normalized)) < 0.5f || Mathf.Abs(Vector2.Dot(limbs[1].transform.up.normalized,Vector2.up.normalized)) < 0.5f){
                isStabilizing = true;
                /*limbRBs[0].mass /= 100;
                limbRBs[1].mass /= 100;
                limbRBs[2].mass /= 100;
                limbRBs[3].mass /= 100;*/
                //limbRBs[4].gravityScale *= 2;
                //limbRBs[5].gravityScale *= 2;
            }
        }
    }
    void FixedUpdate(){
        foreach(Weapon weapon in weapons){
            StartCoroutine(weapon.UseWeaponAlt());
        }
        GetComponent<Rigidbody2D>().AddForce((transform.position - PlayerController.instance.transform.position).normalized * Time.fixedDeltaTime * reverseForce);
        if(isStabilizing){
            limbRBs[0].AddTorque(legPower * Time.fixedDeltaTime);
            limbRBs[1].AddTorque(-legPower * Time.fixedDeltaTime);
        }
    }
}
