using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHitbox : MonoBehaviour
{
    public int affiliation{get;}
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    [SerializeField] private Rigidbody2D[] ragdollParts;
    [SerializeField] private float ragdollThreshold;
    [SerializeField] private float[] attachedPartResistances;
    [SerializeField] private ActiveHitbox[] attachedHitpointPools;
    void OnCollisionEnter2D(Collision2D other){
        if(other.transform.GetComponent<AttackHitbox>() && other.transform.GetComponent<AttackHitbox>().affiliation != affiliation){
            float baseDamage = other.transform.GetComponent<AttackHitbox>().baseDamage;
            float force = other.transform.GetComponent<Rigidbody2D>().mass * other.relativeVelocity.magnitude;
            Debug.Log(force + " force");
            takeDamage(baseDamage * force);
            for(int i = 0; i < attachedHitpointPools.Length; i++){
                attachedHitpointPools[i].takeDamage(baseDamage * force * attachedPartResistances[i]);
            }
            if(force > ragdollThreshold){

            }
        }
    }
    public void takeDamage(float amount){
        HP -= amount;
        if(HP < 0){
            if(gameObject.GetComponent<PlayerController>()){
                //lmao
                Application.Quit();
            }
            Destroy(gameObject);
        }
    }
    public IEnumerator ragdollify(float force){
        float realRagdollHours = force - (ragdollThreshold * gameObject.GetComponent<Rigidbody2D>().mass);
        foreach(Rigidbody2D rb in ragdollParts){
            rb.gravityScale *= -(realRagdollHours / rb.mass);
            rb.angularDrag /= (realRagdollHours / rb.mass);
        }
        yield return new WaitForSeconds(realRagdollHours);
        foreach(Rigidbody2D rb in ragdollParts){
            rb.gravityScale /= -(realRagdollHours / rb.mass);
            rb.angularDrag *= (realRagdollHours / rb.mass);
        }
    }
}
