using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private IMovement movementScript;

    private void Awake()
    {
        movementScript = GetComponent<IMovement>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        movementScript.SetPlayerVelocityVector(move);
    }
}
