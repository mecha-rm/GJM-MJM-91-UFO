using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gets the gameplay variables
public class GameplayPhysics
{
    // the instance
    private static GameplayPhysics instance = null;

    // drag variables
    private float airDrag = 0.95F;
    private float waterDrag = 0.90F;

    // value used to determine if the entity should stop or not.
    private float zeroedVel = 0.001F;

    
    //

    // constructor
    private GameplayPhysics()
    {
        Start();
    }

    // gets the instance
    public static GameplayPhysics GetInstance()
    {
        // no instance generated
        if (instance == null)
        {
            // generates instance
            instance = new GameplayPhysics();
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

    // rotates to face the mouse.
    public static void RotateToFaceMouse2D(Camera cam, Transform tform)
    {
        // mouse world position
        Vector3 mouseWpos = cam.ScreenToWorldPoint(Input.mousePosition);

        // move to be relative to world origin
        Vector3 fromOrigin = mouseWpos - tform.position;
        fromOrigin.z = 0.0F;

        // rotation value
        float theta = Vector3.Angle(Vector3.up, fromOrigin);

        // rotation direction
        float direc = (mouseWpos.x < tform.position.x) ? 1 : -1;

        // reset to base rotation
        Vector2 rotXY = new Vector2(tform.rotation.x, tform.rotation.y); // save x and y
        tform.rotation = Quaternion.identity;

        // rotates to face camera
        tform.Rotate(0.0F, 0.0F, theta * direc);

        // give back x and y rotations
        Quaternion objectRot = tform.rotation;
        objectRot.x = rotXY.x;
        objectRot.y = rotXY.x;
        tform.rotation = objectRot;
    }

    // generates a normal that faces the mouse in 3D
    // fromPos: geneate normal from the provided position.
    // cam: the camera the mouse is being viewed through.
    public static Vector3 NormalTowardsMouse3D(Vector3 fromPos, Camera cam)
    {
        // both ortho and perspective cameras have far planes, but using the focal length for a perspective camera is more accurate.
        // as such, the focal length is used for perspective, and far clip plane is used for the orthographic.
        // however, all that matters is that the z-value is positive.
        
        if(!cam.orthographic) // perspective camera
            return NormalTowardsMouse3D(fromPos, cam, cam.focalLength);
        else // orthographic camera.
            return NormalTowardsMouse3D(fromPos, cam, cam.farClipPlane);
    }

    // generates a normal that faces the mouse in 3D
    // fromPos: geneate normal from the provided position.
    // mouseZ: is the mouse's z-axis position. This must be positive.
    // cam: the camera the mouse is being viewed through.
    public static Vector3 NormalTowardsMouse3D(Vector3 fromPos, Camera cam, float mouseZ)
    {
        // the center of the screen is (0, 0, 0).
        // get mouse position in world space.
        Vector3 camWPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mouseZ)));

        // target
        Vector3 target = camWPos - fromPos;

        // direction to face
        Vector3 direc = target.normalized;

        return direc;
    }

    // euler rotation (2D)
    public static Vector2 RotateEuler(Vector2 v, float angle, bool inDegrees)
    {
        // re-uses rotation calculation for 3D with z = 0.
        Vector3 result = RotateEuler(new Vector3(v.x, v.y, 0.0F), Vector3.forward, angle, inDegrees);
        return new Vector2(result.x, result.y);
    }

    // euler rotation (3D)
    private static Vector3 RotateEuler(Vector3 v, Vector3 axis, float angle, bool inDegrees)
    {
        // angle conversion
        float radAngle = (inDegrees) ? Mathf.Deg2Rad * angle : angle;

        // sin and cos angle
        float sinAngle = Mathf.Sin(radAngle);
        float cosAngle = Mathf.Cos(radAngle);

        // the three rows (set to identity by default)
        Vector3 r0 = Vector3.one;
        Vector3 r1 = Vector3.one;
        Vector3 r2 = Vector3.one;

        // set rotation values
        if(axis == Vector3.right) // x-axis
        {
            r0 = new Vector3(1.0F, 0.0F, 0.0F);
            r1 = new Vector3(0.0f, cosAngle, -sinAngle);
            r2 = new Vector3(0.0F, sinAngle, cosAngle);
        }
        else if(axis == Vector3.up) // y-axis
        {
            r0 = new Vector3(cosAngle, 0.0F, sinAngle);
            r1 = new Vector3(0.0f, 1.0F, 0.0F);
            r2 = new Vector3(-sinAngle, 0.0F, cosAngle);
        }
        else if(axis == Vector3.forward) // z-axis
        {
            r0 = new Vector3(cosAngle, -sinAngle, 0.0F);
            r1 = new Vector3(sinAngle, cosAngle, 0.0F);
            r2 = new Vector3(0.0F, 0.0F, 1.0F);
        }
        
        // calculation (modelled after matrix multiplication)
        Vector3 result = Vector3.zero;
        result.x = Vector3.Dot(r0, v);
        result.y = Vector3.Dot(r1, v);
        result.z = Vector3.Dot(r2, v);

        return result;
    }

    // rotates along the x-axis
    public static Vector3 RotateEulerX(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.right, angle, inDegrees);
    }

    // rotates along the y-axis
    public static Vector3 RotateEulerY(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.up, angle, inDegrees);
    }

    // rotates along the z-axis
    public static Vector3 RotateEulerZ(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.forward, angle, inDegrees);
    }
}
