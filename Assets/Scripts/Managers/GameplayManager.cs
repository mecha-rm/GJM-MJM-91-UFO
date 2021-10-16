using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// the gameplay manager
public class GameplayManager : MonoBehaviour
{
    // the player in the game.
    public Player player;

    // TODO: replace timer with fuel guage.

    // the player's health bar
    public Slider healthBar;

    // the player's attack bar
    public Slider attackBar;

    [Header("Timer")]
    // timer is active.
    public bool activeTimer = true;

    // timer value.
    public float timer = 120.0F; // 2 minutes

    // start of the timer.
    public float timeStart = 120.0F;

    // the timer text
    public TMPro.TextMeshProUGUI timerText;

    // DEATH //
    // death planes (minimum, maximum)
    private static Vector2 deathPlanesX = new Vector2(-500.0F, 500.0F); // x-plane
    private static Vector2 deathPlanesY = new Vector2(-100.0F, 500.0F); // y-plane
    private static Vector2 deathPlanesZ = new Vector2(-500.0F, 500.0F); // z-plane

    // Start is called before the first frame update
    void Start()
    {
        // if the player isn't set, go find it.
        if (player == null)
            player = FindObjectOfType<Player>();
    }

    // checks to see if the object has hit a death plane.
    public static bool InDeathPlane(Vector3 position)
    {
        // result variable
        bool result = false;

        // checking the x
        result = InDeathPlaneX(position.x);

        if (result)
            return true;

        // checking the y
        result = InDeathPlaneY(position.y);

        if (result)
            return true;

        // checking the z
        result = InDeathPlaneZ(position.z);

        return result;
    }

    // checks to see if there is death along the x-axis
    public static bool InDeathPlaneX(float x)
    {
        return (x <= deathPlanesX.x) || (x >= deathPlanesX.y);
    }

    // checks to see if there is death along the y-axis
    public static bool InDeathPlaneY(float y)
    {
        return (y <= deathPlanesY.x) || (y >= deathPlanesY.y);
    }

    // checks to see if there is death along the z-axis
    public static bool InDeathPlaneZ(float z)
    {
        return (z <= deathPlanesZ.x) || (z >= deathPlanesZ.y);
    }

    // instantiate the game info
    public GameEndInfo InstantiateGameEndInfo()
    {
        // grabs prefab for game info, and gets the component.
        Object prefab = Resources.Load("Prefabs/Game End Info");

        // object for game info component. Checks to see if one already exists.
        GameObject geiObject = GameObject.Find("Game End Info");

        // component for round info
        GameEndInfo geiComp;

        // prefab not found, so instantiate object.
        if (prefab == null && geiObject == null) // create new object
        {
            // game end
            geiObject = new GameObject("Game End Info");
        }
        else if (prefab != null && geiObject == null) // instantiate prefab
        {
            // instantiate from prefab.
            geiObject = (GameObject)Instantiate(prefab);
        }

        // grabs component
        geiComp = geiObject.GetComponent<GameEndInfo>();

        // prefab did not have component, so add it.
        if (geiComp == null)
        {
            geiComp = geiObject.AddComponent<GameEndInfo>();
        }

        // returns round component.
        return geiComp;
    }

    // game has finished.
    public void GameOver()
    {
        // instantiates the game end info.
        InstantiateGameEndInfo();

        // change the scene helper value
        SceneHelper.ChangeScene("End");
    }

    // Update is called once per frame
    void Update()
    {
        // TIMER / FUEL //
        // if the timer is active.
        if (activeTimer)
        {
            // reduce timer
            timer -= Time.deltaTime;

            // if the time runs out, the player missed.
            if (timer <= 0)
            {
                // timer over
                timer = 0.0F;

                // calls game over screen.
                Debug.Log("Time Over");
                GameOver();
            }
        }

        // HEALTH, ATTACK //
        if(player != null)
        {
            // health bar is not equal to null.
            if (healthBar != null)
            {
                // health value
                float h = (player.MaxHealth != 0) ? player.Health / player.MaxHealth : 0.0F;

                // health bar
                healthBar.value = h;
            }

            // attack bar is not equal to null.
            if (attackBar != null)
            {
                // health value
                float a = (player.AttackPower != 0) ? player.AttackPower / player.maxAttackPower : 0.0F;

                // health bar
                attackBar.value = a;
            }
        }


        // timer text is set.
        if (timerText != null)
        {
            timerText.text = "TIME: " + timer.ToString("F3");
        }

    }
}
