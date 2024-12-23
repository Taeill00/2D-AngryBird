using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SlingShotController : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform")]
    [SerializeField] private Transform leftStartTransform;
    [SerializeField] private Transform rightStartTransform;
    [SerializeField] private Transform idleTransform;
    public Transform centerTransform;

    [Header("SlingShot")]
    [SerializeField] private float maxDistance = 4f;

    [Header("Scripts Reference")]
    public SlingShotArea slingShotArea;
    public TrajectoryLine trajectoryLine;

    [HideInInspector] public Vector2 slingShotLineLimit;

    private bool isClickedWithinArea;

    private void Start()
    {
        InitLinePos();
    }

    private void Update()
    {
        // Return true when mouse once pressed
        if(Mouse.current.leftButton.wasPressedThisFrame && slingShotArea.IsWithinSlingShotArea())
        {
            isClickedWithinArea = true;
            trajectoryLine.isAreaClicked = true;
            SFXManager.instance.PlaySFX("Load");
        }

        // Return true when mouse is being pressed
        if (Mouse.current.leftButton.isPressed && isClickedWithinArea)
        {
            DrawSlingShotLine();
        }

        // Return true when mouse is released after pressing
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isClickedWithinArea = false;
            trajectoryLine.isAreaClicked = false;
            InitLinePos();
        }
    }

    private void DrawSlingShotLine()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ClampMagnitude => Returns a vector that does not exceed maxDistance.
        // Since a vector only has magnitude and direction properties but no positional value, you need to add the center position value to it.
        slingShotLineLimit = centerTransform.position + Vector3.ClampMagnitude(mouseWorldPos - centerTransform.position, maxDistance);

        SetLine(slingShotLineLimit);
    }

    private void SetLine(Vector2 position)
    {
        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartTransform.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartTransform.position);
    }

    private void InitLinePos()
    {
        leftLineRenderer.SetPosition(0, centerTransform.position);
        leftLineRenderer.SetPosition(1, leftStartTransform.position);

        rightLineRenderer.SetPosition(0, centerTransform.position);
        rightLineRenderer.SetPosition(1, rightStartTransform.position);
    }
}
