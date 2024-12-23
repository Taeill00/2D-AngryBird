using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBird : BaseBird
{
    [Header("Bomb Status")]
    [SerializeField] private GameObject boomParticleEffect;
    [SerializeField] private float explosionForce = 3f;
    [SerializeField] private float explosionRadius = 1f;

    private Collider2D[] ObjInExplosionRadius;

    private SpriteRenderer spriteRenderer;
    private float boomTime = 2f;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();  
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent contact before shooting
        if (!collision.gameObject.CompareTag("Stone"))
        {
            isBumped = true;
            
            StartCoroutine(BoomCo());
        }
    }

    private IEnumerator BoomCo()
    {
        float time = 0f;

        while (time < boomTime)
        {
            time += 0.2f;

            if(spriteRenderer.material.color == Color.white)
            {
                spriteRenderer.material.color = Color.red;
            }
            else
            {
                spriteRenderer.material.color = Color.white;
            }
            
            yield return new WaitForSeconds(0.2f);
        }

        GameObject boomParticleClone = Instantiate(boomParticleEffect, transform.position, Quaternion.identity);
        ExplosionPhysicsEffect();
        SFXManager.instance.PlaySFX("Boom");

        Destroy(boomParticleClone, 1f);

        birdSpawner.birdCount--;
        birdSpawner.canShoot = true;
        gameObject.SetActive(false);
    }

    private void ExplosionPhysicsEffect()
    {
        ObjInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach(Collider2D obj in ObjInExplosionRadius)
        {
            Rigidbody2D objRigid = obj.GetComponent<Rigidbody2D>();

            if(objRigid != null)
            {
                Vector2 distaceVec = obj.transform.position - transform.position;

                if(distaceVec.magnitude > 0)
                {
                    // Apply different explosion force according to the distance from the transform 

                    float eachExplosionForce = explosionForce / distaceVec.magnitude;
                    Vector2 force = distaceVec.normalized * eachExplosionForce;

                    objRigid.AddForce(force);

                    if(force.magnitude >= 7)
                    {
                        obj.GetComponent<Wood>()?.DestroyFunc();
                        obj.GetComponent<Pig>()?.Destroyfunc();
                    }
                }
            }
        }
    }
}
