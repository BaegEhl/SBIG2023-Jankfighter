using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private float baseDamage;
    public float BaseDamage{get{return baseDamage;}}
    [SerializeField] private int affiliation;
    public int Affiliation{get{return affiliation;}}
    [SerializeField] private float impactTimer;
    void OnCollisionEnter2D(Collision2D other){
        if(impactTimer >= 0 && (!other.transform.GetComponent<ActiveHitbox>() || other.transform.GetComponent<ActiveHitbox>().Affiliation != affiliation)){
            StartCoroutine(startImpactTimer());
        }
    }
    IEnumerator startImpactTimer(){
        yield return new WaitForSeconds(impactTimer);
        Destroy(gameObject);
    }
}
