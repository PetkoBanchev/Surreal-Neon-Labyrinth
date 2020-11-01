using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterControllerMovement : MonoBehaviour, IMovement
{
    private PlayerScript PS;
    private CharacterController CC;
    private Vector3 playerVelocityVector;

    private void Start()
    {
        PS = GetComponent<PlayerScript>();
        CC = GetComponent<CharacterController>();
    }
    private void Update()
    {
        CC.Move(playerVelocityVector * Time.deltaTime * PS.speed);
    }

    public void SetPlayerVelocityVector(Vector3 velocityVector)
    {
        playerVelocityVector = velocityVector;
    }
}
