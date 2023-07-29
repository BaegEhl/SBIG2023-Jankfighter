using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private float legSpeed;
    [SerializeField] private float armSpeed;
    [SerializeField] private int playerAttackState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        MovePlayer();
        PlayerArms();
    }
    void MovePlayer(){
        if(Input.GetKey(KeyCode.D)){limbRBs[0].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.E)){limbRBs[1].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.A)){limbRBs[0].AddTorque(-Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.Q)){limbRBs[1].AddTorque(-Time.fixedDeltaTime * legSpeed);}
    }
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
}
