using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    [Header("TNT Status")]
    [SerializeField] private GameObject boomParticleEffect;
    [SerializeField] private GameObject boomAnimationEffect;

    [SerializeField] private float explosionForce = 250f;
    [SerializeField] private float explosionRadius = 10f;

    private Collider2D[] ObjInExplosionRadius;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Vector2 relativeVelocity = collision.relativeVelocity; // 상대속도
        float collsionMass = collision.gameObject.GetComponent<Rigidbody2D>().mass; // 질량

        float impactPower = relativeVelocity.magnitude * collsionMass;

        if (impactPower > 7)
        {
            SFXManager.instance.PlaySFX("Boom");

            Destroy(gameObject);

            ExplosionPhysicsEffect();
            GameObject boomEffectClone = Instantiate(boomParticleEffect, transform.position, Quaternion.identity);
            GameObject boomAniClone = Instantiate(boomAnimationEffect, transform.position, Quaternion.identity);

            GameManager.Instance.score += 100;
            Destroy(boomEffectClone, 1f);
            Destroy(boomAniClone, 1f);
        }
    }


    private void ExplosionPhysicsEffect()
    {
        ObjInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D obj in ObjInExplosionRadius)
        {
            Rigidbody2D objRigid = obj.GetComponent<Rigidbody2D>();

            if (objRigid != null)
            {
                Vector2 distaceVec = obj.transform.position - transform.position;

                if (distaceVec.magnitude > 0)
                {
                    // Apply different explosion force according to the distance from the transform 

                    float eachExplosionForce = explosionForce / distaceVec.magnitude;
                    Vector2 force = distaceVec.normalized * explosionForce;

                    objRigid.AddForce(force);

                    if (force.magnitude >= 7)
                    {
                        obj.GetComponent<Wood>()?.DestroyFunc();
                        obj.GetComponent<Pig>()?.Destroyfunc();
                    }
                }
            }
        }
    }
}
