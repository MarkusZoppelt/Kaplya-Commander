using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBladeTrack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;
    [SerializeField] private Transform bladeTransform;
    [SerializeField] private SawBlade blade;
    [Header("Settings")]
    [SerializeField] private float bladeSpeed = 2f;
    [SerializeField] private float stoppingTime = 2.5f;

    private Coroutine movementRoutine;
    private Transform targetTransform;

    private void Awake()
    {
        StartMoving();
    }

    public void StartMoving()
    {
        movementRoutine = StartCoroutine(MoveBlade());
        blade.enabled = true;
    }

    public void StopMoving()
    {
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
            movementRoutine = null;
            blade.enabled = false;
        }
    }

    private IEnumerator MoveBlade()
    {
        float movementTimer;
        Vector3 movementDirection;
        while (true)
        {
            if(targetTransform == positionA)
            {
                targetTransform = positionB;
            }
            else
            {
                targetTransform = positionA;
            }

            float movementDistance = Vector3.Distance(bladeTransform.position, targetTransform.position);
            movementTimer = movementDistance / bladeSpeed;
            movementDirection = (targetTransform.position - bladeTransform.position).normalized;

            while(movementTimer > 0f)
            {
                movementTimer -= Time.deltaTime;
                bladeTransform.position += movementDirection * bladeSpeed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(stoppingTime);
        }
    }
}
