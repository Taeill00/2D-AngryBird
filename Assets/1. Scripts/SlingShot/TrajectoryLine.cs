using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private BaseBird baseBird;
    private LineRenderer lineRenderer;

    [Header("Trajectory Component")]
    private Vector2[] segments;
    private float timeStep = 0.1f;
    [SerializeField] private int segmentsCount = 80;
    
    [HideInInspector] public bool isAreaClicked;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentsCount;

        segments = new Vector2[segmentsCount];
    }

    public void SetUp(BaseBird bird)
    {
        baseBird = bird;
    }

    private void Update()
    {
        if (baseBird != null)
        {
            if (isAreaClicked && !baseBird.isShooted)
            {
                lineRenderer.enabled = true;
                DrawTrajectoryLine();
            }
            else if (baseBird.isShooted)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void DrawTrajectoryLine()
    {
        // Set lineRenderer start position 
        segments[0] = baseBird.transform.position;
        lineRenderer.SetPosition(0, segments[0]);

        // StartVelocity
        Vector2 birdShootingVelocity = baseBird.shootingVelocity;

        // Bird Mass
        float mass = baseBird.GetComponent<Rigidbody2D>().mass;

        for (int i = 1; i < segmentsCount; i++)
        {
            float t = i * timeStep;
            Vector2 pos = segments[0] + birdShootingVelocity * t + 0.5f * (Physics2D.gravity/mass) * t * t; // Gravity OffSet considering birdmass
            segments[i] = pos;

            lineRenderer.SetPosition(i, segments[i]);
        }
    }

}
