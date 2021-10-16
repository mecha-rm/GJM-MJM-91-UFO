using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player's rigidbody (default variable has been depreciated).
    public Rigidbody rigidbody = null;

    // the game manager
    public GameplayManager manager;

    [Header("Movement")]
    // movement speed.
    private Vector3 moveSpeed = new Vector3(2500.0F, 2500.0F, 2500.0F);

    // use movement caps for each axis (y is true so objects pulled up don't cause the ship to move upwards infinitely).
    private bool useSDX = true, useSDY = true, useSDZ = true;

    // rotation speed 
    private float rotSpeed = 90.0F;


    [Header("Hovering")]
    // if hovering, turn off gravity.
    public bool hovering = true;

    // used to keep the player above the ground
    // TODO: maybe make array
    // private Ray hoverRay;

    // distance the ray is casted.
    private float rayDist = 500.0F;

    // if 'true', the starting y-value is considered the ray distance.
    private bool startYAsHoverDist = true;

    // hovering distance above the ground.
    private float hoverDist = 3.0F;

    // TRACKER BEAM
    [Header("Tractor Beam")]

    // the object used for the tracker beam
    public TractorBeam trackerBeam = null;

    // the beam
    public bool trackerBeamActive = false;

    // score //
    [Header("Health")]
    // health
    public float health = 100.0F;

    // max health
    public float maxHealth = 100.0F;

    // amount of damage that gets taken.
    public float damageAmnt = 1.0F;

    [Header("Attack Power")]
    // attack power
    public float attackPower = 100.0F;

    // maximum attack power
    public float maxAttackPower = 100.0F;

    // different scores
    public int genericPickup = 0;


    // Start is called before the first frame update
    void Start()
    {
        // grab rigid body attached to player.
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        // find gameplay manager.
        if (manager == null)
            manager = FindObjectOfType<GameplayManager>();


        // if the starting y-value should be used.
        if (startYAsHoverDist)
            hoverDist = Mathf.Abs(transform.position.y);

        // find tracker beam if not set.
        if (trackerBeam == null)
            trackerBeam = FindObjectOfType<TractorBeam>();

        // if the tracker beam should not be active.
        if (trackerBeam != null)
            trackerBeam.gameObject.SetActive(trackerBeamActive);

        // ray for keeping fixed distance above the ground (directly down)
        // hoverRay = new Ray(transform.position, -transform.up);
    }

    // current health
    public float Health
    {
        get
        {
            return health;
        }
    }

    // maximum health
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    // current attack
    public float AttackPower
    {
        get
        {
            return attackPower;
        }
    }

    // maximum attack
    public float MaxAttackPower
    {
        get
        {
            return maxAttackPower;
        }
    }

    // on enter for the collision.
    public void OnCollisionEnter(Collision collision)
    {
        Tractable tbl = collision.gameObject.GetComponent<Tractable>(); // gets the tractable component.

        // this is a tractable object.
        if(tbl != null)
        {
            genericPickup += 1; // TODO: change which thing gets added to.
            Destroy(tbl.gameObject); // destroy the object.
        }
    }

    // called when the player takes damage.
    public void Damage(float amount)
    {
        health -= amount;
    }

    // updates the player's hover above the ground.
    private void UpdateHovering()
    {
        // if you're not hovering, don't do anything.
        if (!hovering)
            return;

        // TODO: cast multiple rays
        // checsk to infinity
        Ray hoverRay = new Ray(transform.position, -transform.up);

        // grabs all the hits
        RaycastHit[] hits = Physics.RaycastAll(hoverRay, Mathf.Max(rayDist, hoverDist));

        // the max y-distance
        bool foundHoverPoint = false;
        Vector3 closestOnY = Vector3.zero;
        float closestDistY = hoverDist;

        // goes through each hit.
        foreach(RaycastHit hit in hits)
        {
            // if the ray hit a stage object.
            if(hit.collider.gameObject.tag == "Stage")
            {
                // gets height above the other object.
                float height = Mathf.Abs(transform.position.y - hit.transform.position.y);

                // if this is the closest ground object to the player.
                if (height <= closestDistY)
                {
                    foundHoverPoint = true; // found a point to hover over.
                    closestDistY = height;
                    closestOnY = hit.transform.position;
                }     
            }
        }

        // point to hover over.
        if(foundHoverPoint == true)
        {
            rigidbody.useGravity = false;

            // y-value
            float y = closestOnY.y + hoverDist;

            // new position
            transform.position = new Vector3(
                transform.position.x, 
                y, 
                transform.position.z);

        }
        else
        {
            rigidbody.useGravity = true;
        }

    }


    // Update is called once per frame
    void Update()
    {
        // gets movement (adjust for turning around)
        // player cannot move upwards
        // if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        // {
        //     // the amount of force being applied.
        //     Vector3 force = new Vector3(
        //         Input.GetAxis("Horizontal") * moveSpeed.x * Time.deltaTime,
        //         moveSpeed.y,
        //         Input.GetAxis("Vertical") * moveSpeed.y * Time.deltaTime
        //         );
        // 
        //     // transform.rotation = Quaternion.identity;
        //     // float theta = Vector3.Angle(transform.forward, direc);
        //     // transform.Rotate(Vector3.up, theta);
        // 
        //     // add force to the rigid body.
        //     rigidbody.AddForce(force, ForceMode.Acceleration);
        //     // rigidbody.AddForce(Vector3.Scale(transform.forward, moveSpeed), ForceMode.Acceleration);
        //     
        //     // model set.
        //     // if(model != null)
        //     // {
        //     //     // the direction for the model to face.
        //     //     Vector3 direc = force;
        //     //     direc.y = model.position.y;
        //     // 
        //     //     // direction
        //     //     model.rotation = modelDefRot; // reset to default
        //     //     float theta = Vector3.Angle(model.forward, direc.normalized); // get angle
        //     //     model.Rotate(0.0F, theta, 0.0F); // apply angle
        //     // }
        // }


        // moving forward
        if (Input.GetAxis("Vertical") != 0)
        {
            // the amount of force being applied.
            Vector3 force = transform.forward;
            force.Scale(moveSpeed);
            force *= Input.GetAxis("Vertical") * Time.deltaTime;

            // adds force to rigid body.
            rigidbody.AddForce(force, ForceMode.Acceleration);
            // rigidbody.AddForce(force, ForceMode.Force);
        }
        
        // rotation
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.Rotate(0.0F, rotSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0.0F);
        }

        // slow down
        if(Input.GetAxis("Vertical") == 0)
        {
            Vector3 newVel = rigidbody.velocity; // new velocity
            float slowDownRate = GameplaySingleton.GetInstance().AirDrag; // TODO: maybe ease-in/ease out?
            
            // applies slow down rate.
            if(useSDX) // X
                newVel.x *= slowDownRate * Time.deltaTime;
            
            if (useSDY) // Y
                newVel.y *= slowDownRate * Time.deltaTime;
            
            if (useSDZ) // Z
                newVel.z *= slowDownRate * Time.deltaTime;


            // checks if item should stop. TODO: make 
            float zeroStop = GameplaySingleton.GetInstance().ZeroedVelocity;

            if (useSDX && Mathf.Abs(newVel.x) <= zeroStop) // x
                newVel.x = 0.0F;

            if (useSDY && Mathf.Abs(newVel.y) <= zeroStop) // y
                newVel.y = 0.0F;

            if (useSDZ && Mathf.Abs(newVel.z) <= zeroStop) // z
                newVel.z = 0.0F;

            // sets the velocity
            rigidbody.velocity = newVel;
        }


        // TRACKER BEAM //
        if(Input.GetKeyDown(KeyCode.Space)) // space bar
        {
            trackerBeamActive = !trackerBeamActive; // toggle
            // trackerBeamActive = true; // while held
            trackerBeam.gameObject.SetActive(trackerBeamActive);
        }
        // else if(Input.GetKeyUp(KeyCode.Space)) // let go
        // {
        //     trackerBeamActive = false;
        //     trackerBeam.gameObject.SetActive(trackerBeamActive);
        // }

        // update hovering
        UpdateHovering();

        // the player is dead.
        if (GameplayManager.InDeathPlane(transform.position))
            manager.GameOver();
    }
}
