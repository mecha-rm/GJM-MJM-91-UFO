using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player's rigidbody (default variable has been depreciated).
    public Rigidbody rigidbody = null;

    // movement speed.
    private Vector3 moveSpeed = new Vector3(2500.0F, 000.0F, 2500.0F);

    // rotation speed 
    private float rotSpeed = 90.0F;

    // Start is called before the first frame update
    void Start()
    {
        // grab rigid body attached to player.
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
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
        }
        
        // rotation
        if(Input.GetAxis("Horizontal") != 0)
        {
            transform.Rotate(0.0F, rotSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0.0F);
        }
    }
}
