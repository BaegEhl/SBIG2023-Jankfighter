using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float legSpeed;
    [SerializeField] private float armSpeed;
    [SerializeField] private int playerAttackState;
    public static PlayerController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSwap();
        PlayerAttack();
    }
    void FixedUpdate(){
        MovePlayer();
        PlayerArms();
    }
    //"movement" is actually just spinning a bunch of colliders around until something sufficiently funny happens
    void MovePlayer(){
        if(Input.GetKey(KeyCode.D)){limbRBs[0].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.E)){limbRBs[1].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.A)){limbRBs[0].AddTorque(-Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.Q)){limbRBs[1].AddTorque(-Time.fixedDeltaTime * legSpeed);}
    }
    //swing the player's arms rapidly with right and left click
    void PlayerArms(){
        switch(playerAttackState){
            case 1:
            if(Input.GetMouseButton(0)){limbRBs[2].AddTorque(Time.fixedDeltaTime * armSpeed);}
            if(Input.GetMouseButton(1)){limbRBs[2].AddTorque(-Time.fixedDeltaTime * armSpeed);}
            break;
            case 2:
            if(Input.GetMouseButton(0)){limbRBs[3].AddTorque(Time.fixedDeltaTime * armSpeed);}
            if(Input.GetMouseButton(1)){limbRBs[3].AddTorque(-Time.fixedDeltaTime * armSpeed);}
            break;
        }
    }
    //weapon states used to determine what mouse action to take (i.e. which arm to swing or weapon to use)
    void WeaponSwap(){
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){playerAttackState = 1;}
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){playerAttackState = 2;}
        if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){playerAttackState = 3;}
        if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){playerAttackState = 4;}
    }
    //use the player's weapons
    void PlayerAttack(){
        switch(playerAttackState){
            case 3:
            if(Input.GetMouseButtonDown(0)){StartCoroutine(weapons[0].UseWeapon());}
            if(Input.GetMouseButtonDown(1)){StartCoroutine(weapons[0].UseWeaponAlt());}
            break;
            case 4:
            if(Input.GetMouseButtonDown(0)){StartCoroutine(weapons[1].UseWeapon());}
            if(Input.GetMouseButtonDown(1)){StartCoroutine(weapons[1].UseWeaponAlt());}
            break;
        }
    }
}
