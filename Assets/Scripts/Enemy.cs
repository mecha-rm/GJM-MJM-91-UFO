using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // enemy's health
    public float health = 5.0F;

    // enemy's max health
    public float maxHealth = 5.0F;

    // player target
    public Player target;

    // aim distance.
    public float aimDistance = 100.0F;

    // the amount of damage an enemy does on contact.
    public float contactAttackPower = 5.0F;

    // projectiles.
    [Header("Projectiles")]

    // the list of the projectiles
    public ProjectilePool projPool;

    // if 'true', the nemey fires projectiles.
    public bool fireProjectiles = false;

    // the offset for shooting the projectile in the calculated direction.
    public float fireOffset = 2.5F;

    // the shot cool down time.
    public float shotCoolDown = 0.0F;

    // the maximum shot cool down time.
    public float shotCoolDownMax = 10.0F;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        // finds the player as a target.
        if (target == null)
            target = FindObjectOfType<Player>();
    }

    // kills the enemy.
    public void DamageEnemy(float amount)
    {
        health -= amount;
    }

    // Update is called once per frame
    void Update()
    {
        // if the enemy dies, so destroy them.
        if (health <= 0.0F)
            Destroy(gameObject);

        // should be firing projectiles
        if(fireProjectiles && projPool != null)
        {
            if(shotCoolDown <= 0.0F) // time to fire a shot.
            {
                // finds player if not set.
                if (target == null)
                    target = FindObjectOfType<Player>();

                // target set.
                if (target != null)
                {
                    Vector3 direc = target.transform.position - transform.position;
                    direc.Normalize();

                    // getting the projectile.
                    Projectile proj = projPool.GetProjectile();

                    // set projectile in direction.
                    if (proj != null)
                    {
                        proj.owner = gameObject;
                        proj.transform.position = transform.position + direc * fireOffset;
                        proj.direcNormal = direc;

                        // shot cool down
                        shotCoolDown = shotCoolDownMax;
                    }

                }
            }
            else // reduce timer.
            {
                shotCoolDown -= Time.deltaTime;
            }
        }
    }
}
