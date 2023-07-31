using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float baseDamage{get;}
    public int affiliation{get;}
    [SerializeField] private float impactTimer;
    void OnCollisionEnter2D(Collision2D other){
        if(other.transform.GetComponent<ActiveHitbox>() && other.transform.GetComponent<ActiveHitbox>().affiliation != affiliation){
            StartCoroutine(startImpactTimer());
        }
    }
    IEnumerator startImpactTimer(){
        yield return new WaitForSeconds(impactTimer);
        Destroy(gameObject);
    }
}
