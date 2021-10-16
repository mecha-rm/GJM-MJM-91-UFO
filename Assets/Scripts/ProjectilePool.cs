using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pool for projectiles
// this should be a template class, but it's fine for the sake of this project.
public class ProjectilePool: MonoBehaviour
{
    // the name of the pool
    public string poolName;

    // the projectile prefab
    public string prefab = "";

    // list of projectiles.
    private Queue<Projectile> projPool = new Queue<Projectile>();

    // Start is called before the first frame update
    void Start()
    {
        // checks for the prefab
        Object obj = Resources.Load(prefab);

        if (obj == null)
            Debug.LogWarning("No prefab provided, or prefab was not found.");
    }

    // generates projectiles that are put into the pool immediately.
    public void GenerateProjectiles(int amount)
    {
        // loads up the prefab
        Object obj = Resources.Load(prefab);

        // prefab not loaded.
        if (obj == null)
        {
            Debug.LogError("Prefab could not be loaded.");
            return;
        }

        // object does not have component
        if (((GameObject)obj).GetComponent<Projectile>() == null)
        {
            Debug.LogError("The object does not have a Projectile component.");
            return;
        }

        // generates projectiles
        for (int i = 1; i <= amount; i++)
        {
            GameObject go = Instantiate((GameObject)Resources.Load(prefab));
            Projectile proj = go.GetComponent<Projectile>();

            // deactvates the projectile.
            proj.gameObject.SetActive(false);

            // add to pool queue
            projPool.Enqueue(proj);
        }
    }

    // gets the projectile from the pool.
    public Projectile GetProjectile()
    {
        // object
        Projectile proj = null;

        // got projectile
        bool gotProj = false;

        // while no projectile has been recieved.
        while(!gotProj)
        {
            if (projPool.Count != 0) // projectile available
            {
                // grabs a projectile fom the pool.
                proj = projPool.Dequeue();

                // if the projectile was null (such as if it was deleted), then pull from the pool again.
                if (proj == null)
                    continue;

                // activate projectile, and say you got it.
                proj.gameObject.SetActive(true);
                gotProj = true;
            }
            else // no projectile, so make a new one.
            {
                GameObject go = Instantiate((GameObject)Resources.Load(prefab));

                // checks to see if the game object was generated.
                if (go != null)
                {
                    proj = go.GetComponent<Projectile>();
                    proj.gameObject.SetActive(true);
                }
                else // instantiate could not exist.
                {
                    Debug.LogError("Projectile prefab could not be loaded.");
                    return null;
                }

                // got projectile
                gotProj = true;
            }
        }
        
        return proj;
    }

    // returns to the projectile to the pool.
    public void ReturnProjectile(Projectile proj)
    {
        // no projectile.
        if (proj == null)
        {
            Debug.LogError("Projectile provided was null.");
            return;
        }

        // resets the projectile's life time.
        proj.ResetLifeTime();

        // no one owns this projectile anymore.
        proj.owner = null;

        // deactivates the projectile.
        proj.gameObject.SetActive(false);

        // add to pool queue
        projPool.Enqueue(proj);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
