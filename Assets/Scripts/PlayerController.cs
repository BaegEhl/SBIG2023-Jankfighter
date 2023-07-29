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
    [SerializeField] private int activeWeaponLeft;
    [SerializeField] private int activeWeaponRight;
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
            if(Input.GetKey(KeyCode.R)){limbRBs[2].AddTorque(Time.fixedDeltaTime * armSpeed);}
            if(Input.GetKey(KeyCode.F)){limbRBs[2].AddTorque(-Time.fixedDeltaTime * armSpeed);}
            break;
            case 2:
            if(Input.GetKey(KeyCode.R)){limbRBs[3].AddTorque(Time.fixedDeltaTime * armSpeed);}
            if(Input.GetKey(KeyCode.F)){limbRBs[3].AddTorque(-Time.fixedDeltaTime * armSpeed);}
            break;
        }
    }
    //weapon states used to determine what mouse action to take (i.e. which arm to swing or weapon to use)
    void WeaponSwap(){
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){playerAttackState = 1;}
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){playerAttackState = 2;}
    }
    //use the player's weapons
    void PlayerAttack(){
        switch(playerAttackState){
            case 1:
            if(Input.GetMouseButtonDown(0)){StartCoroutine(weapons[activeWeaponRight].UseWeapon());}
            if(Input.GetMouseButtonDown(1)){StartCoroutine(weapons[activeWeaponRight].UseWeaponAlt());}
            break;
            case 2:
            if(Input.GetMouseButtonDown(0)){StartCoroutine(weapons[activeWeaponLeft].UseWeapon());}
            if(Input.GetMouseButtonDown(1)){StartCoroutine(weapons[activeWeaponLeft].UseWeaponAlt());}
            break;
        }
    }
}
