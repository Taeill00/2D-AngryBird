using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private GameObject boomEffectPrefab;
    [SerializeField] private Sprite pigHurtSprite;
    private int hurtCount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 relativeVelocity = collision.relativeVelocity; 
        float collsionMass = collision.gameObject.GetComponent<Rigidbody2D>().mass; 
        float impactPower = relativeVelocity.magnitude * collsionMass; 

        if(impactPower > 6)
        {
            Destroyfunc();
        }
        else if (impactPower > 4)
        {
            if(hurtCount == 1)
            {
                GetComponent<SpriteRenderer>().sprite = pigHurtSprite;
                hurtCount--;
            }
            else if(hurtCount == 0)
            {
                Destroyfunc();
            }
        }
    }

    public void Destroyfunc()
    {
        SFXManager.instance.PlaySFX("Pop");

        Destroy(gameObject);
        GameObject boomEffectClone = Instantiate(boomEffectPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.score += 500;
        Destroy(boomEffectClone, 1f);
    }
}
