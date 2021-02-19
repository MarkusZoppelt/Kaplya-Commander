using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBillboard : MonoBehaviour
{
    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 targetPosition = targetCamera.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition, Vector3.up);
    }
}
