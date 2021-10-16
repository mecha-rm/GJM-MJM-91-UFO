using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// information for when a game ends
public class GameEndInfo : MonoBehaviour
{
    // the final score (this should be 
    public int genericPickups = 0;

    // Start is called before the first frame update
    void Start()
    {
        // don't destory this object
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
