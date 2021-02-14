using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    #region Inspector
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -15f);
    [SerializeField, Range(0.01f, 5f)] private float smoothTime = 1f;
    #endregion

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
