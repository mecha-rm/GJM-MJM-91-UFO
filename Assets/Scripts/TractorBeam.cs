using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    // the speed of the objects moving in the tractor beam.
    private float tractorSpeed = 100.0F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // called when something enters the tractor beam
    private void OnTriggerEnter(Collider other)
    {
        // grabs component to check.
        Tractable tbl = other.gameObject.GetComponent<Tractable>();

        // in beam
        if (tbl != null)
        {
            // if the component is enabled.
            if(tbl.enabled)
                tbl.OnTractorBeamEnter(this);
        }
            
    }

    // called when something leaves the tractor beam
    private void OnTriggerExit(Collider other)
    {
        // checks component.
        Tractable tbl = other.gameObject.GetComponent<Tractable>();

        // out of beam
        if (tbl != null)
        {
            // if the component is enabled.
            if(tbl.enabled)
                tbl.OnTractorBeamExit(this);
        }
            

    }

    // the strength of the tractor beam's pull
    public float TractorSpeed
    {
        get
        {
            return tractorSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
