using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float gravity = -9.81f;
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
        movementDirection = new Vector3(inputMovement.x, 0f, inputMovement.y);
    }

    public void OnTargetedAction(InputAction.CallbackContext value)
    {
        // TODO
    }
    #endregion

    private void Move()
    {
        Vector3 movement = movementDirection * speed * Time.deltaTime;
        movement.y = gravity;
        controller.Move(movement);
    }

    /// <summary>
    /// Determines the target of the action and calls either the throw- or callback-method
    /// </summary>
    private void HandleTargetedAction()
    {
        // TODO: Determine where we clicked/tapped and decide whether to call back all blobs or throw them
    }
}
