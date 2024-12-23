using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseBird : MonoBehaviour
{
    [Header("Script Reference")]
    protected SlingShotController slingShotController;
    protected BirdSpawner birdSpawner;

    [Header("Bird")]
    public int shootingForce = 100;
    protected bool isRightPosClicked;
    protected Rigidbody2D rigid;

    public bool isShooted = false;
    public bool isBumped = false;
    public Vector2 shootingVelocity;
    public int birdIndex = 0;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.simulated = false;
    }

    public void SetUp(SlingShotController refSript, BirdSpawner refScript2, int birdIndex)
    {
        slingShotController = refSript;
        birdSpawner = refScript2;
        this.birdIndex = birdIndex;
    }

    protected virtual void Update()
    {
        // 1. Click Check
        if (Mouse.current.leftButton.wasPressedThisFrame && slingShotController.slingShotArea.IsWithinSlingShotArea())
        {
            isRightPosClicked = true; 
        }

        // 2. Shoot Bird
        if (Mouse.current.leftButton.isPressed && isRightPosClicked && !isShooted)
        {
            transform.position = slingShotController.slingShotLineLimit; // Set start position limit
            CalculateBirdVelocity();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && isRightPosClicked && !isShooted)
        {
            SFXManager.instance.PlaySFX("Shoot");
            ShootBird();
        }

        // 3. During Flying
        if (isShooted && !isBumped)
        {
            transform.right = rigid.velocity;
        }
    }

    protected void CalculateBirdVelocity()
    {
        Vector2 shootingDir = -(transform.position - slingShotController.centerTransform.position);
        shootingVelocity = shootingDir * shootingForce;
    }

    protected virtual void ShootBird()
    {
        // the button of the bird that was shooted set interactable false
        birdSpawner.birdSelectBtns[birdIndex].interactable = false;
        birdSpawner.birdSelectBtns[birdIndex].image.color = Color.red;


        rigid.simulated = true;
        isShooted = true;
        rigid.velocity = shootingVelocity;

        birdSpawner.canShoot = false;
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent contact before shooting
        if (!collision.gameObject.CompareTag("Stone"))
        {
            isBumped = true;
            StartCoroutine(DestroyCo());
        }
    }

    protected IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(2f);

        birdSpawner.birdCount--;
        birdSpawner.canShoot = true;
        gameObject.SetActive(false);
    }
}
