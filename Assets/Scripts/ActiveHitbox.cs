using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActiveHitbox : MonoBehaviour
{
    [SerializeField] private int affiliation;
    public int Affiliation{get{return affiliation;}}
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    [SerializeField] private Rigidbody2D[] ragdollParts;
    [SerializeField] private float ragdollThreshold;
    [SerializeField] private float[] attachedPartResistances;
    [SerializeField] private ActiveHitbox[] attachedHitpointPools;
    [SerializeField] private float bloodAmount;
    [SerializeField] private Image healthbar;
    [SerializeField] private GameObject damageIndicatorPrefab;
    
    [SerializeField] private GameObject bloodPrefab;
    void OnCollisionEnter2D(Collision2D other){
        if(other.transform.GetComponent<AttackHitbox>() && other.transform.GetComponent<AttackHitbox>().Affiliation != affiliation){
            float baseDamage = other.transform.GetComponent<AttackHitbox>().BaseDamage;
            float force = other.transform.GetComponent<Rigidbody2D>().mass * other.relativeVelocity.magnitude;
            //Debug.Log(force + " force");
            StartCoroutine(takeDamage(baseDamage * force,other));
            for(int i = 0; i < attachedHitpointPools.Length; i++){
                StartCoroutine(attachedHitpointPools[i].takeDamage(baseDamage * force * attachedPartResistances[i],other));
            }
            if(force > ragdollThreshold && baseDamage > 0){
                StartCoroutine(ragdollify(force));
            }
        }
    }
    public IEnumerator takeDamage(float amount, Collision2D other){
        HP -= amount;
        if(amount > 0.5f && damageIndicatorPrefab != null){
            int damageNumber = Mathf.RoundToInt(amount);
            GameObject dIndicator = Instantiate(damageIndicatorPrefab,transform.position,transform.rotation);
            dIndicator.GetComponent<TextMeshPro>().SetText(damageNumber.ToString());
            dIndicator.transform.localScale *= Mathf.Max(1,Mathf.Sqrt(amount / 10));
            dIndicator.GetComponent<BoxCollider2D>().size *= new Vector2(dIndicator.GetComponent<TextMeshPro>().text.Length,1);
            dIndicator.GetComponent<Rigidbody2D>().AddForce(transform.up * 100 * Mathf.Max(1,Mathf.Sqrt(amount)));
        }
        if(HP > maxHP){HP = maxHP;}
        if(amount > 0 && bloodAmount > 0){
            int percent = Mathf.CeilToInt((amount / maxHP) * bloodAmount);
            for(int i = 0; i < percent; i++){
                if(healthbar != null){healthbar.fillAmount -= ((amount / maxHP) / (float)percent);};
                GameObject blood = Instantiate(bloodPrefab, transform.position, transform.rotation);
                float bloodSize = Mathf.Sqrt(Random.Range(1f,5f));
                blood.GetComponent<Rigidbody2D>().mass *= bloodSize;
                blood.GetComponent<Rigidbody2D>().AddForce(other.relativeVelocity * bloodSize * Mathf.Max(3, 5 / other.relativeVelocity.magnitude));
                blood.transform.localScale = new Vector3(bloodSize * 0.05f,bloodSize * 0.05f,bloodSize * 0.05f);
                bloodAmount--;
                if(bloodAmount <= 0 && HP < 0){
                    die();
                }
                yield return new WaitForEndOfFrame();
            }
        }
        else if (healthbar != null){
            healthbar.fillAmount = HP / maxHP;
        }
        if(HP < 0){
            die();
        }
    }
    void die(){
        if(gameObject.GetComponent<PlayerController>()){
            //lmao
            Application.Quit();
        }
        Destroy(gameObject);
    }
    public IEnumerator ragdollify(float force){
        float realRagdollHours = force - (ragdollThreshold * gameObject.GetComponent<Rigidbody2D>().mass);
        Debug.Log("ragdolled");
        foreach(Rigidbody2D rb in ragdollParts){
            rb.gravityScale *= -1;
            rb.angularDrag /= Mathf.Max(realRagdollHours / rb.mass, 1);
        }
        yield return new WaitForSeconds(Mathf.Min(Mathf.Sqrt(realRagdollHours),15));
        foreach(Rigidbody2D rb in ragdollParts){
            rb.gravityScale /= -1;
            rb.angularDrag *= Mathf.Max(realRagdollHours / rb.mass, 1);
        }
        Debug.Log("no longer ragdolled");
    }
}
