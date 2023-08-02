using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRSLaserGunshipAI : EnemyAI
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float[] attackSpeed;
    [SerializeField] private float[] attackTimer;
    [SerializeField] private float thrustForce;
    [SerializeField] private float thrustTorque;
    [SerializeField] private float xBounds;
    [SerializeField] private float yBounds;
    [SerializeField] private float angleBounds;
    [SerializeField] private float glareForce;
    [SerializeField] private GameObject spawnPrefab;
    bool tiltFwd = false;
    bool tiltBkwd = false;
    void Update(){
        for(int i = 0; i < attackTimer.Length; i++){
            attackTimer[i] += Time.deltaTime;
            if(attackTimer[i] > attackSpeed[i]){
                attackTimer[i] = 0;
                if(i < attackTimer.Length - 1){
                    StartCoroutine(weapons[i].UseWeapon());
                }
                else{
                    Instantiate(spawnPrefab, limbs[3].transform.position, limbs[3].transform.rotation);
                }
            }
        }
    }
    //this is almost as awful as the game itself
    void FixedUpdate(){
        foreach(Weapon weapon in weapons){
            StartCoroutine(weapon.UseWeaponAlt());
        }
        if(limbRBs[0] != null){
            limbRBs[0].AddForce((PlayerController.instance.transform.position - limbs[0].transform.position).normalized * Time.fixedDeltaTime * glareForce);
        }
        if(transform.position.y < yBounds){thrusterGoBrr(1,2);thrusterGoBrr(2,2);}
        if(transform.position.x < -xBounds){limbRBs[1].AddTorque(thrustTorque * Time.fixedDeltaTime);limbRBs[2].AddTorque(thrustTorque * Time.fixedDeltaTime);thrusterGoBrr(1);thrusterGoBrr(2);tiltFwd = true;}
        else if(transform.position.x > xBounds){limbRBs[1].AddTorque(-thrustTorque * Time.fixedDeltaTime);limbRBs[2].AddTorque(-thrustTorque * Time.fixedDeltaTime);thrusterGoBrr(1);thrusterGoBrr(2);tiltBkwd = true;}
        else{resetThruster(1); resetThruster(2);}
        if(transform.up.x < -angleBounds || tiltFwd){thrusterGoBrr(1,2);thrusterGoBrr(2,-2);tiltFwd = (transform.up.x < -angleBounds / 2);}
        if(transform.up.x > angleBounds || tiltBkwd){thrusterGoBrr(2,1.5f);thrusterGoBrr(1,-1.5f);tiltBkwd = (transform.up.x > angleBounds / 2);}
    }
    void thrusterGoBrr(int whichOne, float forceMultiplier = 1){
        if(limbRBs[whichOne] != null){
            limbRBs[whichOne].AddForce(limbs[whichOne].transform.up * thrustForce * forceMultiplier * Time.fixedDeltaTime * Mathf.Sqrt(whichOne));
        }
    }
    void resetThruster(int whichOne){
        if(limbs[whichOne] != null){
            if(limbs[whichOne].transform.up.x < -0.05f){
                limbRBs[whichOne].AddTorque(thrustTorque * Time.fixedDeltaTime);
            }
            if(limbs[whichOne].transform.up.x > 0.05f){
                limbRBs[whichOne].AddTorque(-thrustTorque * Time.fixedDeltaTime);
            }
        }
    }
}
