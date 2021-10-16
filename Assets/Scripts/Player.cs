using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player's rigidbody (default variable has been depreciated).
    public Rigidbody rigidbody = null;

    [Header("Movement")]
    // movement speed.
    private Vector3 moveSpeed = new Vector3(2500.0F, 000.0F, 2500.0F);

    // rotation speed 
    private float rotSpeed = 90.0F;


    [Header("Hovering")]
    // if hovering, turn off gravity.
    public bool hovering = true;

    // used to keep the player above the ground
    // TODO: maybe make array
    private Ray hoverRay;

    // hovering distance above the ground.
    private float hoverDist = 1.0F;

    // TRACKER BEAM
    [Header("Tractor Beam")]

    // the object used for the tracker beam
    public TractorBeam trackerBeam = null;

    // the beam
    public bool trackerBeamActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // grab rigid body attached to player.
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        // find tracker beam if not set.
        if (trackerBeam == null)
            trackerBeam = FindObjectOfType<TractorBeam>();

        // if the tracker beam should not be active.
        if (trackerBeam != null)
            trackerBeam.gameObject.SetActive(trackerBeamActive);

        // ray for keeping fixed distance above the ground (directly down)
        hoverRay = new Ray(transform.position, -transform.up);
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
            newVel *= slowDownRate * Time.deltaTime; // slow down rate TODO: make rate variable


            // checks if item should stop. TODO: make 
            float zeroStop = GameplaySingleton.GetInstance().ZeroedVelocity;

            if (Mathf.Abs(newVel.x) <= zeroStop) // x
                newVel.x = 0.0F;

            if (Mathf.Abs(newVel.y) <= zeroStop) // y
                newVel.y = 0.0F;

            if (Mathf.Abs(newVel.z) <= zeroStop) // z
                newVel.z = 0.0F;

            // sets hte velocity
            rigidbody.velocity = newVel;
        }


        // HOVER //
        // hover distance above ground.
        // if(hovering)
        // {
        // 
        // }
        // 
        // // if hovering, don't use gravity.
        // rigidbody.useGravity = !hovering;

        // TODO: include death


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
    }
}
