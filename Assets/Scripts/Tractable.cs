using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put on objects that can be pulled in by the tractor beam.
public class Tractable : MonoBehaviour
{
    // the tractable object.
    // if not set, it is set to the object this component is attached to.
    public GameObject self;

    // if 'true', then the object can be pulled by the tractor beam.
    public bool isTractable = true;

    // the object's rigid body.
    public Rigidbody rigidBody;

    // the value for gravity when not in the beam.
    public bool outsideBeamGravity = true;

    // the beam the object is trapped in.
    public TractorBeam tractorBeam;


    // Start is called before the first frame update
    void Start()
    {
        // checks self variable.
        if (self == null)
        {
            // if(gameObject.transform.parent != null)
            //     self = gameObject.transform.parent.gameObject;
            // else
            //     self = gameObject;

            // defaults to given object.
            self = gameObject;

        }
            

        // if no rigidbody was set, look for one.
        if (rigidBody == null)
            rigidBody = self.GetComponent<Rigidbody>();

        // if no rigidbody was set, add one.
        if(rigidBody == null)
            rigidBody = self.AddComponent<Rigidbody>();

        // grabs value for using gravity.
        if (rigidBody != null)
            outsideBeamGravity = rigidBody.useGravity;
    }

    // Done in the TractorBeam class.
    // // called when entering a collider.
    // private void OnCollisionStay(Collision collision)
    // {
    //     // checks if tractor beam.
    //     if (tractorBeam == null)
    //         TryEnterTractorBeam(collision.gameObject);
    // }
    // 
    // // called when exiting a collider.
    // private void OnCollisionExit(Collision collision)
    // {
    //     // checks if tractor beam
    //     if (tractorBeam != null)
    //         TryExitTractorBeam(collision.gameObject);
    // }
    // 
    // // calledd when still in the trigger.
    // private void OnTriggerStay(Collider other)
    // {
    //     // checks if tractor beam.
    //     if (tractorBeam == null)
    //         TryEnterTractorBeam(other.gameObject);
    // }
    // 
    // // called when leaving the trigger
    // private void OnTriggerExit(Collider other)
    // {
    //     // checks if tractor beam
    //     if (tractorBeam != null)
    //         TryExitTractorBeam(other.gameObject);
    // }

    // tries to enter the tractor beam.
    private void TryEnterTractorBeam(GameObject ent)
    {
        // if no tractor beam is set.
        if (tractorBeam == null)
        {
            // trys to get the tractor beam.
            TractorBeam beam;

            // goes to enter function.
            if (ent.TryGetComponent<TractorBeam>(out beam))
                OnTractorBeamEnter(beam);
        }
    }

    // called when the item is tracted by the tractor beam.
    public void OnTractorBeamEnter(TractorBeam beam)
    {
        // given beam
        tractorBeam = beam;

        // no longer using gravity since in tractor beam.
        if (rigidBody != null)
            rigidBody.useGravity = false;
    }

    // tries to exit the tractor beam.
    private void TryExitTractorBeam(GameObject ent)
    {
        // if tractor beam is set.
        if (tractorBeam != null)
        {
            // trys to get the tractor beam.
            TractorBeam beam;

            // goes to enter function.
            if (ent.TryGetComponent<TractorBeam>(out beam))
                OnTractorBeamEnter(beam);
        }
    }

    // called when leaving the tractor beam.
    public void OnTractorBeamExit()
    {
        OnTractorBeamExit(tractorBeam);
    }

    // called when the item is let go by the tractor beam.
    public void OnTractorBeamExit(TractorBeam beam)
    {
        // it's the beam the object was trapped in.
        if(beam == tractorBeam)
        {
            tractorBeam = null;

            // may now use gravity since not in tractor beam.
            if (rigidBody != null)
                rigidBody.useGravity = outsideBeamGravity;
        }
      
    }

    // called when the tractable object is absorbed.
    public void OnAbsorbtion()
    {
        Destroy(self); // destroy the object.
    }

    // Update is called once per frame
    void Update()
    {
        // moves the object upwards.
        if(isTractable && tractorBeam != null)
        {
            // the tractor beam object is enabled (i.e. it is in use)
            if(tractorBeam.isActiveAndEnabled)
            {
                Vector3 direc = tractorBeam.transform.position - self.transform.position; // movement direction
                direc.y = Mathf.Abs(direc.y); // should always be moving upwards

                // rigid body was found.
                if (rigidBody != null)
                {
                    rigidBody.AddForce(Vector3.Scale(direc.normalized, tractorBeam.TractorSpeed) * Time.deltaTime, ForceMode.Impulse);
                }
                else // no rigid body, so translate object.
                {
                    rigidBody.transform.Translate(Vector3.Scale(direc.normalized, tractorBeam.TractorSpeed) * Time.deltaTime);
                }
            }
            else // not in use, so leave effect.
            {
                tractorBeam = null;

                if(rigidBody != null)
                    rigidBody.useGravity = outsideBeamGravity; // return gravity back to normal
            }
            

        }
        else if(isTractable && tractorBeam == null) // no tractor beam set (possibly deleted).
        {
            if (rigidBody != null)
                rigidBody.useGravity = outsideBeamGravity; // return gravity back to normal
        }
    }
}
