using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChuckBird : BaseBird
{
    [Header("Chuck Status")]
    [SerializeField] private float chuckRushSpeed = 12f;
    [SerializeField] private Sprite dashSprite;

    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }


    protected override void Update()
    {
        // 1. Click Check
        if (Mouse.current.leftButton.wasPressedThisFrame && slingShotController.slingShotArea.IsWithinSlingShotArea())
        {
            isRightPosClicked = true;
        }

        // 2. Shoot Bird
        if (Mouse.current.leftButton.isPressed && isRightPosClicked && !isShooted)
        {
            transform.position = slingShotController.slingShotLineLimit; //  Set start position limit
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

            DashToMousePos();
        }
    }

    private void DashToMousePos()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SFXManager.instance.PlaySFX("Dash");

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 mouseWorldPos2D = new Vector3(mouseWorldPos.x, mouseWorldPos.y);
            Vector2 dirVec = (mouseWorldPos2D - transform.position).normalized;

            trailRenderer.enabled = true;
            spriteRenderer.sprite = dashSprite;
            rigid.velocity = dirVec * chuckRushSpeed;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent contact before shooting
        if (!collision.gameObject.CompareTag("Stone"))
        {
            isBumped = true;
            spriteRenderer.sprite = originalSprite;

            StartCoroutine(DestroyCo());
        }
    }
}
