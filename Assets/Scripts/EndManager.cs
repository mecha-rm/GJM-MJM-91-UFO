using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    private int genericPickups = 0;

    // Start is called before the first frame update
    void Start()
    {
        // finished game information.
        GameEndInfo gameInfo = FindObjectOfType<GameEndInfo>();

        if (gameInfo != null)
            genericPickups = gameInfo.genericPickups;

        // destroys the game object.
        if (gameInfo != null)
            Destroy(gameInfo.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
