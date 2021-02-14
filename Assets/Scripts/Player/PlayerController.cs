using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float speed = 3f;
    #endregion

    private Vector3 movementDirection;

    #region Unity Methods
    private void Update()
    {
        Move();
    }
    #endregion

    #region Input Events
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Debug.Log(inputMovement);
        movementDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnTargetedAction(InputAction.CallbackContext value)
    {
        // TODO
    }
    #endregion

    private void Move()
    {
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    /// <summary>
    /// Determines the target of the action and calls either the throw- or callback-method
    /// </summary>
    private void HandleTargetedAction()
    {
        // TODO: Determine where we clicked/tapped and decide whether to call back all blobs or throw them
    }
}
