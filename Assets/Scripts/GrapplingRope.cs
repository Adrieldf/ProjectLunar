using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class GrapplingRope : MonoBehaviour
{
    [HideInInspector]
    public bool IsGrappling = true;
    [HideInInspector]
    public bool IsTooFar = false;

    [SerializeField]
    private GrapplingGun grapplingGun;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int precision;
    [SerializeField]
    private AnimationCurve ropeAnimationCurve;
    [SerializeField]
    private AnimationCurve ropeProgressionCurve;
    [SerializeField]
    [Range(0.01f, 4)]
    private float StartWaveSize = 2;
    [SerializeField]
    [Range(1, 50)]
    private float ropeProgressionSpeed = 1;
    [SerializeField]
    [Range(0, 20)]
    private float straightenLineSpeed = 5;
    [SerializeField]
    private float grapplingTime = 0.5f;

    [SerializeField]
    private Animator animator;


    private float moveTime;
    private float waveSize = 0;

    private void OnEnable()
    {
        moveTime = 0;
        lineRenderer.positionCount = precision;
        waveSize = StartWaveSize;
        LinePointsToFirePoint();

        lineRenderer.enabled = true;
        animator.SetBool("RopeBreak", true);

    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        IsGrappling = false;
        IsTooFar = false;
        animator.SetBool("RopeBreak", false);
    }


    void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();

        if (moveTime > grapplingTime)
        {
            grapplingGun.DisableRope();
        }
    }

    void DrawRope()
    {
        if (!IsGrappling)
        {
            if (!IsTooFar)
            {
                grapplingGun.Grapple();
            }
            IsGrappling = true;
        }
        if (waveSize > 0)
        {
            waveSize -= Time.deltaTime * straightenLineSpeed;
            DrawRopeWaves();
        }
        else
        {
            waveSize = 0;

            if (lineRenderer.positionCount != 2)
            {
                lineRenderer.positionCount = 2;
            }

            DrawRopeNoWaves();
        }
    }


    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < 50; i++)
        {
            lineRenderer.SetPosition(i, grapplingGun.FirePoint.position);
        }
    }
    void DrawRopeWaves()
    {
        for (int i = 0; i < precision; i++)
        {
            float delta = (float)i / ((float)precision - 1f);
            Vector2 offset = Vector2.Perpendicular(grapplingGun.GrappleDistance).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.FirePoint.position, grapplingGun.GrapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.FirePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        lineRenderer.SetPosition(0, grapplingGun.FirePoint.position);
        lineRenderer.SetPosition(1, grapplingGun.GrapplePoint);
    }
}
