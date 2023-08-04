using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ActiveHitbox[] limbs;
    [SerializeField] private Rigidbody2D[] limbRBs;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float legSpeed;
    [SerializeField] private float armSpeed;
    [SerializeField] private int playerAttackState;
    [SerializeField] private int activeWeaponLeft;
    [SerializeField] private int activeWeaponRight;
    [SerializeField] private GameObject[] weaponUItexts;
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private float nextLevelXP;
    [SerializeField] private float levelScaling;
    [SerializeField] private Image XPbar;
    [SerializeField] private float bloodNerfResist;
    public static PlayerController instance;
    private float[] statUpgradeModifierThings;
    private float xp;
    private float bloodNerf;
    public float[] StatModifiers{get{return statUpgradeModifierThings;}}
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Application.logMessageReceived += HandleLog;
        statUpgradeModifierThings = new float[]{1,1,1,1,1,1,1,1,1,1,0,0,0,0};
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSwap();
        PlayerAttack();
        PlayerStance();
        bloodNerf = Mathf.Max(0,bloodNerf * (1 - 0.1f * Time.deltaTime));
    }
    void FixedUpdate(){
        MovePlayer();
        PlayerArms();
        BloodSpray();
    }
    //"movement" is actually just spinning a bunch of colliders around until something sufficiently funny happens
    void MovePlayer(){
        if(Input.GetKey(KeyCode.D)){limbRBs[0].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.E)){limbRBs[1].AddTorque(Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.A)){limbRBs[0].AddTorque(-Time.fixedDeltaTime * legSpeed);}
        if(Input.GetKey(KeyCode.Q)){limbRBs[1].AddTorque(-Time.fixedDeltaTime * legSpeed);}
    }
    //swing the player's arms (mostly useless)
    void PlayerArms(){
        //why are you looking at line 54 of playercontroller
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
    void PlayerStance(){
        if(Input.GetKeyDown(KeyCode.W)){limbRBs[4].mass *= 2; limbRBs[5].mass *= 2;}
        if(Input.GetKeyUp(KeyCode.W)){limbRBs[4].mass /= 2; limbRBs[5].mass /= 2;}
        if(Input.GetKeyDown(KeyCode.S)){limbRBs[4].gravityScale *= -2; limbRBs[5].gravityScale *= -2;}
        if(Input.GetKeyUp(KeyCode.S)){limbRBs[4].gravityScale /= -2; limbRBs[5].gravityScale /= -2;}
    }
    void BloodSpray(){
        if(Input.GetKey(KeyCode.Space)){limbs[4].BloodSpray(Mathf.RoundToInt(statUpgradeModifierThings[10]));}
    }
    public void swapSecondary(int weapon){
        for(int i = 0; i < weapons.Length; i++){
            if(i != activeWeaponRight){
                //weapons[i].GetComponent<BoxCollider2D>().enabled = false;
                weapons[i].GetComponent<SpriteRenderer>().enabled = false;
                weaponUItexts[i].SetActive(false);
            }
        }
        activeWeaponLeft = weapon;
        //weapons[weapon].GetComponent<BoxCollider2D>().enabled = true;
        weapons[weapon].GetComponent<SpriteRenderer>().enabled = true;
        weaponUItexts[weapon].SetActive(true);
    }
    public void swapPrimary(int weapon){
        for(int i = 0; i < weapons.Length; i++){
            if(i != activeWeaponLeft){
                //weapons[i].GetComponent<BoxCollider2D>().enabled = false;
                weapons[i].GetComponent<SpriteRenderer>().enabled = false;
                weaponUItexts[i].SetActive(false);
            }
        }
        activeWeaponRight = weapon;
        //weapons[weapon].GetComponent<BoxCollider2D>().enabled = true;
        weapons[weapon].GetComponent<SpriteRenderer>().enabled = true;
        weaponUItexts[weapon].SetActive(true);
    }
    public void deactivateOtherWeapons(){
        for(int i = 0; i < weapons.Length; i++){
            if(i != activeWeaponLeft && i != activeWeaponRight){
                weapons[i].gameObject.SetActive(false);
                weaponUItexts[i].SetActive(false);
            }
        }
    }
    public void initiateUpgrade(){
        bloodNerf += 20;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        Physics2D.simulationMode = SimulationMode2D.Script;
        XPbar.fillAmount = 0;
        List<GameObject> temp = new List<GameObject>();
        GameObject upgrade = upgradeButtons[Random.Range(0,upgradeButtons.Length)];
        upgrade.SetActive(true);
        upgrade.transform.localPosition = new Vector2(-500,0);
        temp.Add(upgrade);
        while(temp.Contains(upgrade)){upgrade = upgradeButtons[Random.Range(0,upgradeButtons.Length)];}
        upgrade.SetActive(true);
        upgrade.transform.localPosition = new Vector2(0,0);
        temp.Add(upgrade);
        while(temp.Contains(upgrade)){upgrade = upgradeButtons[Random.Range(0,upgradeButtons.Length)];}
        upgrade.SetActive(true);
        upgrade.transform.localPosition = new Vector2(500,0);
    }
    public void selectUpgrade(int option){
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        for(int i = 0; i < upgradeButtons.Length;i++){upgradeButtons[i].SetActive(false);}
        switch(option){
            //+10 blood regen body, +2 limbs
            case 0:
            for(int i = 0; i < limbs.Length; i++){limbs[i].modifyStats(0,2);}
            limbs[4].modifyStats(0,8);
            break;
            //+1 blood spray, +65% blood absorb efficiency
            case 1:
            statUpgradeModifierThings[10]++;
            statUpgradeModifierThings[13] += 0.65f;
            break;
            //+1 blood damage
            case 2:
            statUpgradeModifierThings[11]++;
            break;
            //+35% damage
            case 3:
            statUpgradeModifierThings[0] *= 1.35f;
            break;
            //+40% ranged force, +52% recoil, +20% spread
            case 4:
            statUpgradeModifierThings[1] *= 1.4f;
            statUpgradeModifierThings[2] *= 1.52f;
            statUpgradeModifierThings[5] *= 1.2f;
            break;
            //+20% force
            case 5:
            statUpgradeModifierThings[3] *= 1.2f;
            break;
            case 6:
            //+40% mass, +27% force
            statUpgradeModifierThings[3] *= 1.27f;
            statUpgradeModifierThings[4] *= 1.4f;
            weapons[activeWeaponLeft].GetComponent<Rigidbody2D>().mass *= 1.4f;
            weapons[activeWeaponRight].GetComponent<Rigidbody2D>().mass *= 1.4f;
            break;
            //-30% recoil, -20% spread
            case 7:
            statUpgradeModifierThings[2] *= 0.7f;
            statUpgradeModifierThings[5] *= 0.8f;
            break;
            //+25% charge rate
            case 8:
            statUpgradeModifierThings[6] *= 1.25f;
            break;
            //+1 blood heal
            case 9:
            statUpgradeModifierThings[12]--;
            break;
            //+15% limb force
            case 10:
            armSpeed *= 1.15f;
            legSpeed *= 1.15f;
            break;
            //+0.1% damage per error
            case 11:
            statUpgradeModifierThings[7] *= 1.001f;
            break;
            //+60% leg force
            case 12:
            legSpeed *= 1.6f;
            break;
            //+30% arm force
            case 13:
            armSpeed *= 1.3f;
            break;
            //+35% auto fire rate
            case 14:
            statUpgradeModifierThings[8] *= 1.35f;
            break;
            //+40% mag size
            case 15:
            statUpgradeModifierThings[9] *= 1.4f;
            break;
            case 16:
            for(int i = 0; i < limbs.Length; i++){limbs[i].modifyStats(1,1);}
            limbs[4].modifyStats(1,1);
            break;
            case 17:
            for(int i = 0; i < limbs.Length; i++){limbs[i].modifyStats(2,50);}
            limbs[4].modifyStats(2,75);
            break;
        }
    }
    public void addXP(float amount){
        xp += amount * getBloodFactor();
        if(xp > nextLevelXP){
            xp = 0;
            nextLevelXP += levelScaling;
            initiateUpgrade();
        }
        XPbar.fillAmount = xp / nextLevelXP;
    }
    public void quitTheGame(){
        Application.Quit();
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Error){statUpgradeModifierThings[0] *= statUpgradeModifierThings[7];}
    }
    public float getBloodFactor(){
        return bloodNerfResist / (bloodNerf + bloodNerfResist);
    }
}
