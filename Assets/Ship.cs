using Assets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipMovement shipMovement;
    public ShipStats shipStats;
    private CharacterController characterController;
        
    // Start is called before the first frame update
    void Start()
    {
        shipMovement = new ShipMovement();
        shipStats = new ShipStats
        {
            MaxSpeed = 10,
            MaxRotateSpeed = 20,
            MaxAccelerationSpeed = 0.03f,

        };
        shipMovement.Initialize(shipStats);
       characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        shipMovement.UpdateMove(gameObject.transform,characterController);
    }
}
