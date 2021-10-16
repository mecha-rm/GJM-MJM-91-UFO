using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the gameplay manager
public class GameplayManager : MonoBehaviour
{
    // the player in the game.
    public Player player;

    // the final score
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        // if the player isn't set, go find it.
        if (player == null)
            player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
