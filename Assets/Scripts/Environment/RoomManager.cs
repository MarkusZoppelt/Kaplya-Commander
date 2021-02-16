using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PneumaticTube tube;

    public PneumaticTube Tube { get { return tube; } }

    private void OnCollisionEnter(Collision collision)
    {
        var carryObject = collision.gameObject.GetComponent<CarryObject>();
        if (carryObject != null)
        {
            carryObject.Room = this;
        }
    }
}
