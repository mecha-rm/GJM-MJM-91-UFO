using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gets the gameplay variables
public class GameplaySingleton
{
    // the instance
    private static GameplaySingleton instance = null;

    // drag variables
    private float airDrag = 0.95F;
    private float waterDrag = 0.90F;

    // value used to determine if the entity should stop or not.
    private float zeroedVel = 0.001F;

    // constructor
    private GameplaySingleton()
    {
        Start();
    }

    // gets the instance
    public static GameplaySingleton GetInstance()
    {
        // no instance generated
        if (instance == null)
        {
            // generates instance
            instance = new GameplaySingleton();
        }

        return instance;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // gets the air drag
    public float AirDrag
    {
        get
        {
            return airDrag;
        }
    }

    // gets the water drag
    public float WaterDrag
    {
        get
        {
            return waterDrag;
        }
    }

    // gets zeroed velocity value. This is the standard value.
    public float ZeroedVelocity
    {
        get
        {
            return zeroedVel;
        }
    }
}
