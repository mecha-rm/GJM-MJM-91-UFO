using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // owner of the projectile.
    public GameObject owner = null;

    // if 'true', the owner can be harmed by their own projectile.
    public bool ownerHarm = false;

    // the attack power of the projectile.
    public float attackPower = 10.0F;

    // the pool for the projectile.
    // if there is no pool, the bullet is destroyed.
    public ProjectilePool pool = null;

    [Header("Life")]
    // the life time of the projectile.
    public float lifeTime = 0.0F;

    // the maximum life time of the projectile.
    public float lifeTimeMax = 8.5F;

    // if 'true', the projectile has a life span.
    // if 'false', the projectile continues infinitely.
    public bool hasLifeSpan = true;

    [Header("Movement")]
    // the rigid body for the projectile.
    public Rigidbody rigidbody = null;

    // apply movement to the projectile.
    public bool applyMovement = true;

    // the direction of the projectile, normalized.
    public Vector3 direcNormal;

    // the speed of the projectile.
    public float speed = 60.0F;

    // if 'true', the projectile is 'ended' when it runs into something.
    // if 'pool' is not null, it returns to the pool. Otherwise, the projectile is destroyed.
    public bool endOnContact = true;

    // Start is called before the first frame update
    void Start()
    {
        // if the rigidbody is set to null, then get the component for it.
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        // sets life time
        lifeTime = lifeTimeMax;
    }

    // called when the projectile collides with something.
    private void OnCollisionEnter(Collision collision)
    {
        HitEntity(collision.gameObject);
    }

    // called when entering trigger
    private void OnTriggerEnter(Collider other)
    {
        HitEntity(other.gameObject);
    }

    // called when the projectile collides with something.
    private void HitEntity(GameObject entity)
    {
        // if the projectile hit soemthing.
        if(entity != null)
        {
            // this entity should be hurt.
            if((entity == owner && ownerHarm) || entity != owner)
            {
                // hit player
                if (entity.tag == "Player")
                {
                    // get component.
                    Player plyr = entity.GetComponent<Player>();

                    // deal damage
                    if (plyr != null)
                        plyr.DamagePlayer(attackPower);
                }
                // hit enemy
                if(entity.tag == "Enemy")
                {
                    // get component.
                    Enemy enemy = entity.GetComponent<Enemy>();

                    // deal damage
                    if (enemy != null)
                        enemy.DamageEnemy(attackPower);
                }
            }  
        }

        // pool exists, so return it to there.
        if (pool != null)
        {
            pool.ReturnProjectile(this);
        }
        else // pool does not exist, so delete.
        {
            Destroy(gameObject);
        }
    }

    // resets the projectile life time.
    public void ResetLifeTime()
    {
        lifeTime = lifeTimeMax;
    }

    // returns the pool
    public ProjectilePool GetPool()
    {
        return pool;
    }

    // sets the pool this projectile belongs to.
    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    // removes the pol this object is part of. This doesn't delete the pool.
    public void RemovePool()
    {
        pool = null;
    }

    // Update is called once per frame
    void Update()
    {
        // move projectile.
        if(applyMovement && rigidbody != null)
        {
            rigidbody.AddForce(direcNormal * speed * Time.deltaTime, ForceMode.Impulse);
        }

        // if the projectile has a life span.
        if(hasLifeSpan)
        {
            // reduce life time.
            lifeTime -= Time.deltaTime;

            // projectile should be destroyed.
            if (lifeTime <= 0.0F)
                HitEntity(null);
        }
    }
}
