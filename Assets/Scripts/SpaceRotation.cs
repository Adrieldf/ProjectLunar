using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRotation : MonoBehaviour
{
    private float roationAmount;

    private void Start()
    {
        roationAmount = Random.Range(-0.08f, 0.08f);
    }
    void Update()
    {
        gameObject.transform.Rotate(0f, 0f, roationAmount);
    }
}
