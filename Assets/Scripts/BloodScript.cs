using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : AttackHitbox
{
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.LogError("blood spawned");
        switch(affiliation){
            case 0:
            baseDamage = PlayerController.instance.StatModifiers[11] * PlayerController.instance.StatModifiers[0];;
            break;
            case 1:
            baseDamage = PlayerController.instance.StatModifiers[12];
            break;
        }
    }
}
