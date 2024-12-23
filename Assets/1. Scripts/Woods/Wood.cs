using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private GameObject boomEffectPrefab;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Vector2 relativeVelocity = collision.relativeVelocity; // 상대속도
        float collsionMass = collision.gameObject.GetComponent<Rigidbody2D>().mass; // 질량

        float impactPower = relativeVelocity.magnitude * collsionMass;

        if (impactPower > 5)
        {
            DestroyFunc();
        }
    }

    public void DestroyFunc()
    {
        SFXManager.instance.PlaySFX("Pop");

        Destroy(gameObject);
        GameObject boomEffectClone = Instantiate(boomEffectPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.score += 100;
        Destroy(boomEffectClone, 1f);
    }
}
