using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] protected float baseDamage;
    public float BaseDamage{get{return baseDamage;}}
    [SerializeField] protected float bleedFactor = 1;
    public float BleedFactor{get{return bleedFactor;}}
    [SerializeField] protected int affiliation;
    public int Affiliation{get{return affiliation;}}
    [SerializeField] protected float xp;
    public float XP{get{return xp;}}
    [SerializeField] protected float impactTimer;
    public static int deleted;
    void OnCollisionEnter2D(Collision2D other){
        if(impactTimer >= 0 /*&& (!other.transform.GetComponent<ActiveHitbox>() || other.transform.GetComponent<ActiveHitbox>().Affiliation != affiliation)*/){
            StartCoroutine(startImpactTimer());
        }
    }
    IEnumerator startImpactTimer(){
        if(Time.unscaledDeltaTime < 1f / PlayerController.instance.LagTolerance){
            yield return new WaitForSeconds(impactTimer);
        }
        else{
            Debug.Log("deleting for performance " + deleted);
            deleted++;
            yield return null;
        }
        Destroy(gameObject);
    }
}
