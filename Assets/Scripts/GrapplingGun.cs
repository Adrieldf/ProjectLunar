using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform FirePoint;
    public bool IsPlayerDead = false;

    [HideInInspector]
    public Vector2 GrapplePoint;
    [HideInInspector]
    public Vector2 GrappleDistance;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private SpriteRenderer gunSprite;
    [SerializeField]
    private GameObject gunPivot;
    [SerializeField]
    private GrapplingRope rope;
    [SerializeField]
    private SpringJoint2D springJoint;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float launchSpeed;
    [SerializeField]
    private GameObject crosshairPivot;

    private Transform parentTranform;

    void Start()
    {
        rope.enabled = false;
        springJoint.enabled = false;
        parentTranform = gameObject.GetComponentInParent<Transform>();
    }

    void Update()
    {
        if (IsPlayerDead)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (rope.enabled)
            {
                RotateGun(GrapplePoint);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DisableRope();
        }
        else
        {
            RotateGun(mousePos);
        }

        if (crosshairPivot != null)
        {
            RotateCrosshair(mousePos);
        }
    }

    public void DisableRope()
    {
        rope.enabled = false;
        springJoint.enabled = false;
    }


    private void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gunPivot.transform.rotation = rotation;

        if (angle < -90 || angle > 90)
        {
            playerSprite.flipX = true;
            gunSprite.flipY = true;
        }
        else
        {
            playerSprite.flipX = false;
            gunSprite.flipY = false;
        }
    }
    private void RotateCrosshair(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        crosshairPivot.transform.rotation = rotation;


    }

    private void SetGrapplePoint()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - parentTranform.position).normalized;
        RaycastHit2D _hit = Physics2D.Raycast(FirePoint.position, direction);

        // Debug.Log(_hit ? _hit.collider.gameObject.name : "No hit");

        if (_hit)
        {
            if (_hit.transform.gameObject.CompareTag("Grappable"))
            {
                if (Vector2.Distance(_hit.point, FirePoint.position) > maxDistance)
                {
                    rope.IsTooFar = true;
                    Vector2 dir = (_hit.point - (Vector2)FirePoint.position).normalized;
                    Vector2 res = new Vector2(dir.x * maxDistance, dir.y * maxDistance);
                    GrapplePoint = (Vector2)FirePoint.position + res;
                }
                else
                {
                    GrapplePoint = _hit.point;
                }
                GrappleDistance = GrapplePoint - (Vector2)gunPivot.transform.position;
                rope.enabled = true;
            }
            else
            {
                rope.IsTooFar = true;
                GrapplePoint = _hit.point;
                GrappleDistance = GrapplePoint - (Vector2)gunPivot.transform.position;
                rope.enabled = true;
            }
            //se acertar um objeto que nao seja grappable tem que mostrar a linha tbm e talvez fazer um animação de cair o grapple sei la
        }
        else
        {
            rope.IsTooFar = true;
            Vector2 res = new Vector2(direction.x * maxDistance, direction.y * maxDistance);
            GrapplePoint = (Vector2)FirePoint.position + res;
            GrappleDistance = GrapplePoint - (Vector2)gunPivot.transform.position;
            rope.enabled = true;
        }
    }

    public void Grapple()
    {
        springJoint.autoConfigureDistance = false;
        springJoint.connectedAnchor = GrapplePoint;
        Vector2 distanceVector = FirePoint.position - playerTransform.position;
        springJoint.distance = distanceVector.magnitude;
        springJoint.frequency = launchSpeed;
        springJoint.enabled = true;

        //case LaunchType.Transform_Launch:
        //    m_rigidbody.gravityScale = 0;
        //    m_rigidbody.velocity = Vector2.zero;
        //    break;
    }
}
