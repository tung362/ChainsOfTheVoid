using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class BaseResource : MonoBehaviour {
    Collision collisionHandler;



    //what type of resource is this going to be?
    public enum ResourceType {DAMAGE, HEALTH, SHIELD};
    //states wether or not the resource is building, paused, stoped or not doing anything
    public enum BuildState   {BUILD_RES, PAUSE, STOP, NONE};
    //will set what state the AI is currently in for this resource
    public enum AIState {ATTACK, DO_NOTHING };

    //Visualization of the cooldown timer
    public Slider CoolDownSlider;
    
    
    public GameObject DamageComponentPart, HealthComponentPart, ShieldComponentPart;
    public float NumberOfWaitingResources = 3;
    
    //Get player team name
    //Cooldown timer
    [Tooltip("Cooldown timer for this resource to spawn")]
    public float CoolDownTimer; //the cd rate that we will reset to 
    [Tooltip("This timer determines how long it will take to start building another resource")]
    public float ResetTimer;

    protected float Timer; // timer value that will change 
    protected float restartTimer;

    [Tooltip("Area of effect for RNG spawn")]
    [Range(0,10)]
    public float Radius;

    [Range(0,10)]
    public float DefenseRadius;
    //What Team is this resource "working" for and fighting against?
    public enum Team{TEAM1, TEAM2, NEUTRAL };
    public Team myTeam = Team.NEUTRAL;

    [Tooltip("Time required not to be damaged before starting repair")]
    public    float repairNoDamageTimer = 5.0f;
    protected float       noDamageTimer = 5.0f;
    protected bool           isTakingDamage;
    public float           healthRegenSpeed; // need to finish
    protected enum RepairState  {NEED_REPAIR, IN_COMBAT, FULL_HEALTH };
                   RepairState myRepairState = RepairState.NEED_REPAIR;

    
    protected int        Health,           Shield,           Armor;
    public int initHealth = 100, initShield = 100, initArmor = 100;
    public int RepairAmt = 10;

    
    //the current health, damage, and shield rating for this resource
    protected float[] resourceStats = {0.0f, 0.0f, 0.0f };

    public ResourceType typeInstance       = new ResourceType();
    [Tooltip("BUILD_RES will build / finish building the current part, PAUSE will pause the current part being built, Stop will throw out the current part it is working on, NONE makes it neutral / produce nothing")]
    public BuildState   buildStateInstance = new BuildState  ();
    Rect rect = new Rect(100,100,200,200);

    //the gui for this resource object
    GUI mygui;

    public List<GameObject>         PlayerList;
    public List<GameObject> ResourcesWaitingForPickUp;

    private Vector3 myVelocity;


    void Start()
    {
        rect.position.Set(this.transform.position.x, this.transform.position.y);

        typeInstance            =  ResourceType.DAMAGE;
        buildStateInstance      = BuildState.BUILD_RES;
        CoolDownSlider.minValue =          0.0f;
        CoolDownSlider.maxValue = CoolDownTimer;
        CoolDownSlider.value    =         Timer;
        Timer                   = CoolDownTimer;
        restartTimer            =    ResetTimer;
        noDamageTimer = repairNoDamageTimer;
        isTakingDamage = false;

        
     
    }

    void Update()
    {
        rect.position = this.transform.position;
        //currently under testing for resource control


        //contains all of the debug calls for this class
        debugUpdate();

        resourceFacilitySystems();
    }

    void debugUpdate()
    {
        //Debug.Log("Timer: " + Timer);
        //Debug.Log("Slider: " + CoolDownSlider.value);
        //Debug.Log(myTeam);
    }

    void OnGUI()
    {
        GUI.HorizontalSlider(rect, Radius, 0, 10.0f);

    }

    // gizmos == debugging tools
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, Radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, DefenseRadius);

        Gizmos.color = Color.green;
        foreach (GameObject res in ResourcesWaitingForPickUp)
        {
            
            Gizmos.DrawLine(this.transform.position, res.transform.position);
        }
    }


   

    
    // Building ship components / parts
    void Manufacturing(){
        CoolDownSlider.value = Timer;
        //reseting timers when necesarry
        if (ResourcesWaitingForPickUp.Count >= NumberOfWaitingResources) { buildStateInstance = BuildState.NONE; return; }
        if (Timer        <= 0.0f) {Timer = CoolDownTimer; buildStateInstance = BuildState.STOP;      SpawnResource(typeInstance);  }
        if (restartTimer <= 0.0f) {restartTimer = 2.0f  ; buildStateInstance = BuildState.BUILD_RES; }




        switch (buildStateInstance)
        {
            case BuildState.BUILD_RES:
                Timer -= Time.deltaTime;
                break;
            case BuildState.PAUSE:
                break;
            case BuildState.STOP:
                restartTimer -= Time.deltaTime;
                break;
            case BuildState.NONE:
                break;
            default:
                break;
        }


    }

    //when the part is finished "building" it will spawn in
    void SpawnResource(ResourceType resource) {
        switch (resource)
        {
            case ResourceType.DAMAGE:
                ResourcesWaitingForPickUp.Add( Instantiate(DamageComponentPart, new Vector3(Random.insideUnitCircle.x * Radius + this.transform.position.x,0, Random.insideUnitCircle.y * Radius + this.transform.position.z),Quaternion.identity));

                
                break;
            case ResourceType.HEALTH:
                ResourcesWaitingForPickUp.Add( Instantiate(HealthComponentPart, new Vector3(Random.insideUnitCircle.x * Radius + this.transform.position.x, 0, Random.insideUnitCircle.y * Radius + this.transform.position.z), Quaternion.identity));


                break;
            case ResourceType.SHIELD:
                ResourcesWaitingForPickUp.Add( Instantiate(ShieldComponentPart, new Vector3(Random.insideUnitCircle.x * Radius + this.transform.position.x, 0, Random.insideUnitCircle.y * Radius + this.transform.position.z), Quaternion.identity));


                break;
            default:
                break;
        }
    }

    //stuff for what happens after the parts are created
    void WaitingForPickUp()
    {
        if (ResourcesWaitingForPickUp.Count > 1) {
            //ResourcesWaitingForPickUp
            foreach (GameObject thisResource in ResourcesWaitingForPickUp)
            {                
                Vector3 avoidanceDirection = Vector3.zero;
                foreach (GameObject otherResource in ResourcesWaitingForPickUp)
                    if(otherResource != thisResource)
                {

                    //Debug.Log("This Resource = " + thisResource.GetInstanceID() + " Other Resource = " + otherResource.GetInstanceID());
                    

                    if(thisResource.GetComponent<Rigidbody>())
                    myVelocity = thisResource.GetComponent<Rigidbody>().velocity;
                   // Debug.DrawLine(thisResource.transform.position, otherResource.transform.position);
                    

                    if (TooClose(thisResource, otherResource))
                    {
                        //Debug.Log("Should be lerping");

                            avoidanceDirection += Vector3.Normalize(thisResource.transform.position - otherResource.transform.position);

                       //thisResource.transform.position =  Vector3.MoveTowards(otherResources.transform.position, new Vector3(, 0, Random.Range(0, 5)),2);
                    }
                }
                if(avoidanceDirection != Vector3.zero)
                {                
                    thisResource.transform.position = Vector3.MoveTowards(thisResource.transform.position, thisResource.transform.position + avoidanceDirection.normalized, 4 * Time.deltaTime);
                }
            }
        }

    }

    //for checking if two complete parts are too close together
    bool TooClose(GameObject lhs_res, GameObject rhs_res)
    {

       float dist = Vector3.Distance(lhs_res.transform.position, rhs_res.transform.position);

        if (lhs_res.GetComponent<SphereCollider>() != null && rhs_res.GetComponent<SphereCollider>() != null)
        {
            if (dist < lhs_res.GetComponent<SphereCollider>().radius*2)
            {
                //Debug.Log("Distance" + dist);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.Log("Neither parts have a sphere collider");
            return false;
        }

        
    }


    //Contains all of the main systems of the resource builder
    void resourceFacilitySystems()
    {
        Manufacturing();
         DamageReport();
        DefenseSystem();
        WaitingForPickUp();
    }


    void DefenseSystem()
    {
        //Identify Ships in range

        switch (myTeam)
        {
            case Team.TEAM1:
                AttackTeam2();
                break;
            case Team.TEAM2:
                AttackTeam1();
                break;
            default:
            case Team.NEUTRAL:
                break;

        }
    }
    //Handles taking Damage and Repairing updates
    void DamageReport()
    {
        if (noDamageTimer <= 0.0f) isTakingDamage = false;

        if (isTakingDamage) { noDamageTimer -= Time.deltaTime; myRepairState = RepairState.IN_COMBAT; }


        if (Health < initHealth && isTakingDamage == false) { Repair(RepairAmt); }
    }

    //implement AI Code for both Attack functions
    void AttackTeam1() { }
    void AttackTeam2() { }
    void TakeDamage(int amount) { Health -= amount; isTakingDamage = true;  noDamageTimer = repairNoDamageTimer; }
    //checks to see if repairs are needed, if so it will conduct them
    void Repair(int amount) {

        switch (myRepairState)
        {
            case RepairState.FULL_HEALTH:

                break;
            case RepairState.NEED_REPAIR:
                if (Health + amount > initHealth)
                {
                    Health += initHealth - Health;
                    myRepairState = RepairState.FULL_HEALTH;
                }
                else if (Health < initHealth) { Health += amount; } else myRepairState = RepairState.FULL_HEALTH;
                 


                

                break;
            case RepairState.IN_COMBAT:
                break;
            default:
                break;
        }

        
            
                
           
    
    }


                                                                                            /*GET FUNCTIONS*/
    int GetHealth() { return Health; }
    int GetArmor()  { return  Armor; }
    int GetShield() { return Shield; }
    



}
