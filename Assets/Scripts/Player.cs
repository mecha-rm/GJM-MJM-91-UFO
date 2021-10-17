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

    // the maximum y-value the player can reached.
    private float maxPosY = 20.0F;

    [Header("Hovering")]
    // if hovering, turn off gravity.
    public bool hovering = true;

    // used to keep the player above the ground
    // TODO: maybe make array
    // private Ray hoverRay;

    // distance the ray is casted.
    private float rayDist = 800.0F;

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

    // TODO: setup attack bar
    [Header("Attack Power")]
    // attack power
    public float attackPower = 100.0F;

    // maximum attack power
    public float maxAttackPower = 100.0F;

    // if 'true', the player can attack infinitely.
    public bool infiniteAttackPower = false;

    // decreasing rate.
    public float attackDecRate = 20.0F;

    // replenish rate.
    public float attackRepRate = 8.5F;

    // amount of time until an attack can go off
    public float attackDelay = 0.0F;

    // max amount of delay time.
    public float attackDelayMax = 0.5F;

    // the offset for the attack's position.
    public float attackPosOffset = 5.0F;

    // pool of projectiles fired by the player. 
    public ProjectilePool projPool;

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

        // projectile pool
        if (projPool == null)
            projPool = GetComponent<ProjectilePool>();
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
        // if hit an enemy.
        if (collision.gameObject.tag == "Enemy")
        {
            // hit by enemy
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            
            // damage the player.
            if(e != null)
            {
                DamagePlayer(e.contactAttackPower);

                // pushback
                rigidbody.AddForce((transform.position - collision.transform.position).normalized * 10.0F, ForceMode.Acceleration);
            }    
        }

        Tractable tbl = collision.gameObject.GetComponent<Tractable>(); // gets the tractable component.

        // this is a tractable object.
        if(tbl != null)
        {
            genericPickup += 1; // TODO: change which thing gets added to.
            Destroy(tbl.gameObject); // destroy the object.
        }
    }

    // called when the player takes damage.
    public void DamagePlayer(float amount)
    {
        health -= amount;
        trackerBeamActive = false; // turn off tracker beam
    }

    // updates the player's hover above the ground.
    private void UpdateHovering()
    {
        // if you're not hovering, don't do anything.
        if (!hovering)
            return;

        // casts out rays in 5 directions (n/s/e/w and directly down)
        Ray[] hoverRays = new Ray[5];

        // offsets the ray origin in the direction it's pointing.
        float rayOriginOffset = 2.50F;

        {
            // angles of the rays
            float xAngle = 60.0F;
            // float yAngle = 0.0F;
            // float yInc = 360.0F / (hoverRays.Length - 1); // makes rays around the object.

            // centre/directly down
            hoverRays[0] = new Ray(transform.position, -transform.up);

            // top (rotate on x - changed y and z)
            // hoverRays[1] = new Ray(transform.position + transform.forward * rayOriginOffset, GameplayPhysics.RotateEulerX(hoverRays[0].direction, xAngle, true));
            hoverRays[1] = new Ray(transform.position, GameplayPhysics.RotateEulerX(hoverRays[0].direction, xAngle, true));

            // rotate values along y-axis
            // left
            // yAngle += yInc;
            // hoverRays[2] = new Ray(transform.position + -transform.right * rayOriginOffset, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 90.0F, true));
            hoverRays[2] = new Ray(transform.position, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 90.0F, true));

            // bottom
            // hoverRays[3] = new Ray(transform.position + -transform.forward * rayOriginOffset, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 180.0F, true));
            hoverRays[3] = new Ray(transform.position, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 180.0F, true));

            // right
            // hoverRays[4] = new Ray(transform.position + transform.right * rayOriginOffset, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 270.0F, true));
            hoverRays[4] = new Ray(transform.position, GameplayPhysics.RotateEulerY(hoverRays[1].direction, 270.0F, true));
        }

        // checking collisions.
        bool foundHoverPoint = false; // found a point to hover over.
        Vector3 closestOnY = Vector3.zero;

        // goes through each hover ray.
        foreach (Ray hoverRay in hoverRays)
        {
            // grabs all the hits
            RaycastHit[] hits = Physics.RaycastAll(hoverRay, Mathf.Max(rayDist, hoverDist));

            // the max y-distance
            float closestDistY = hoverDist;

            // goes through each hit on a regular collider.
            foreach (RaycastHit hit in hits)
            {
                // if the ray hit a stage object.
                if (hit.collider.gameObject.tag == "Stage")
                {

                    // gets height above the other object.
                    float height = Mathf.Abs(transform.position.y - hit.point.y);

                    // if this is the closest ground object to the player.
                    if (height <= closestDistY)
                    {
                        foundHoverPoint = true; // found a point to hover over.
                        closestDistY = height;
                        closestOnY = hit.point;
                    }
                }
            }
        }

        // point to hover over.
        if (foundHoverPoint == true)
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

    // used to make the player shoot a projectile.
    public void Attack()
    {
        // the projectile pool
        if(projPool == null)
        {
            Debug.LogError("No projectile pool available.");
            return;
        }

        // if the attack power is not infinite
        if(!infiniteAttackPower)
        {
            // if there isn't enough attack power, don't let them fire.
            if (attackPower - attackDecRate < 0.0F)
                return;
        }

        // Debug.Log("Mouse View: " + Camera.main.ScreenToViewportPoint(Input.mousePosition).ToString());

        // TODO: fix projectile aiming, and check if in view.
        // player wants to fire
        if (Input.GetAxisRaw("Fire1") != 0 && Input.anyKeyDown && attackDelay <= 0.0F)
        {
            // // // go relative to the player.
            // // // the screen projection in fornt of the player.
            // // 
            // // // for smaller the FOV, the farther it is, and the less you can see adjacent to you.
            // // float targetPlaneDist = Camera.main.fieldOfView; // this will be the distance from the camera to go forward
            // // 
            // // // essentially it projects a screen in front of the player that they will aim for.
            // // 
            // // // player position in screen view
            // // Vector3 plyrScreenPos = Camera.main.WorldToScreenPoint(transform.position); // convert player to screen pos
            // // 
            // // // Debug.Log("Mouse Pos Screen: " + Input.mousePosition.ToString());
            // // // Debug.Log("Mouse Pos World: " + Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString());
            // // 
            // // // difference between screen pos of player and mouse click
            // // Vector3 diffScreen = Input.mousePosition - plyrScreenPos; // direction towards mouse in screen space.
            // // 
            // // // projects the screen to be in front of the player by 'targetPlaneDist'.
            // // Vector3 target = transform.position + transform.forward * targetPlaneDist;
            // // target += diffScreen; // offset target by screen position
            // // 
            // // 
            // // Vector3 diffWorld = Camera.main.ScreenToWorldPoint(diffScreen); // convert to world point.
            // // diffWorld.z += transform.forward.z * 5.0F; // offsets to be in front of the player.
            // // 
            // // // gets the direction the shot should go in.
            // // Vector3 direc = diffWorld - transform.position;
            // // direc.z *= -1;
            // // direc.Normalize(); // normalize direction
            // // 
            // // direc = target - transform.position;
            // // direc.Normalize();
            // 
            // // for smaller the FOV, the farther it is, and the less you can see adjacent to you.
            // // float targetPlaneDist = Camera.main.fieldOfView; // this will be the distance from the camera to go forward
            // 
            // // Camera pCam = Instantiate(Camera.main);
            // // pCam.transform.position = transform.position; // move camera to player position
            // 
            // // converts mouse position to its place in the viewport.
            // // this also doubles as the direction from the camera view.
            // Vector3 mouseViewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            // 
            // // gets mouse view pos angle relative to a vector with an angle of 0.
            // // SOH - CAH - TOA
            // 
            // // angle between current x and desired x
            // // z is 1, so x = 0 for adjacent. For hypotenuse, z is 
            // Vector3 temp1 = new Vector3();
            // Vector3 temp2 = new Vector3();
            // 
            // // only the x-value is different
            // temp1 = Vector3.forward;
            // temp2.x = mouseViewPos.x - Camera.main.rect.center.x;
            // temp2.y = temp1.y;
            // temp2.z = temp1.z;
            // 
            // // angle to rotate along the y-axis (horizontal change).
            // // float yAngle = Vector3.Angle(temp1, temp2);
            // float yAngle = Mathf.Acos(temp1.magnitude / temp2.magnitude); // cah (cos a = adj/hyp)
            // 
            // // angle between current y and desired y.
            // // only the y-value is different
            // temp1 = Vector3.forward;
            // temp2.x = temp1.x;
            // temp2.y = mouseViewPos.y - Camera.main.rect.center.y;
            // temp2.z = temp1.z;
            // 
            // // angle to rotate along the x-axis (vertical change).
            // // float xAngle = Vector3.Angle(temp1, temp2);
            // float xAngle = Mathf.Acos(temp1.magnitude / temp2.magnitude); // cah (cos a = adj/hyp)
            // 
            // // gets target direction
            // // rotate on the y
            // Vector3 direc;
            // direc = GameplayPhysics.RotateEulerY(Camera.main.transform.forward, yAngle, false);
            // 
            // // rotate on the x
            // direc = GameplayPhysics.RotateEulerX(direc, xAngle, false);
            // 
            // // rotate on the y
            // // yAngle += Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.y;
            // // direc = GameplayPhysics.RotateEulerY(Vector3.forward, yAngle, false);
            // // 
            // // // rotate on the x
            // // xAngle += Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.x;
            // // direc = GameplayPhysics.RotateEulerX(direc, xAngle, false);
            // 
            // direc.Normalize();

            // TODO: figure out how to offset it from the player

            // setting up auto fire
            // find all enemies.
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            Vector3 target = transform.position + transform.forward * 10.0F;
            float range = 200.0F;
            float closest = range;

            // checks each enemy
            foreach(Enemy e in enemies)
            {
                float eDist = Vector3.Distance(transform.position, e.transform.position);

                // if the enemy is within range, and it is the closest enemy.
                if(eDist < closest)
                {
                    closest = eDist;
                    target = e.transform.position;
                }
            }

            // direction
            Vector3 direc = target - transform.position;
            direc.Normalize();

            // getting the projectile.
            Projectile proj = projPool.GetProjectile();

            // set projectile in direction.
            if (proj != null)
            {
                proj.owner = gameObject;
                proj.transform.position = transform.position + direc * attackPosOffset;
                proj.direcNormal = direc;

                // attack delay
                attackDelay = attackDelayMax;// attackDelayMax;
                attackPower -= attackDecRate;
            }


        }
        else if (attackDelay > 0.0F) // shot delay in place.
        {
            // attack delay countdown

            attackDelay -= Time.deltaTime;

            if (attackDelay < 0.0F)
                attackDelay = 0.0F;
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
            float slowDownRate = GameplayPhysics.GetInstance().AirDrag; // TODO: maybe ease-in/ease out?
            
            // applies slow down rate.
            if(useSDX) // X
                newVel.x *= slowDownRate * Time.deltaTime;
            
            if (useSDY) // Y
                newVel.y *= slowDownRate * Time.deltaTime;
            
            if (useSDZ) // Z
                newVel.z *= slowDownRate * Time.deltaTime;


            // checks if item should stop. TODO: make 
            float zeroStop = GameplayPhysics.GetInstance().ZeroedVelocity;

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
            // TOOD: move to function.
            trackerBeamActive = !trackerBeamActive; // toggle
            // trackerBeamActive = true; // while held
        }

        // active beam
        trackerBeam.gameObject.SetActive(trackerBeamActive);

        // else if(Input.GetKeyUp(KeyCode.Space)) // let go
        // {
        //     trackerBeamActive = false;
        //     trackerBeam.gameObject.SetActive(trackerBeamActive);
        // }

        // update hovering
        UpdateHovering();

        // restores attack power
        attackPower += attackRepRate * Time.deltaTime;
        attackPower = Mathf.Clamp(attackPower, 0, maxAttackPower);

        // update shooting.
        Attack();

        // the player is dead.
        if (GameplayManager.InDeathPlane(transform.position))
            manager.GameOver();

        // TODO: this does not work.
        // if the player has reached their maximum height above the ground, adjust their position.
        // if(transform.position.y >= maxPosY)
        // {
        //     transform.position.Set(transform.position.x, maxPosY, transform.position.z);
        // }
    }
}
