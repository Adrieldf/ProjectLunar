using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField]
    private GrapplingGun grapplingGun;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (grapplingGun.IsPlayerDead)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos);
    }


    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gameObject.transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
