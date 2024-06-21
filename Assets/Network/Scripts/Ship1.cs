using Assets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Ship1 : MonoBehaviour
{
    [SerializeField]
    ShipMovement shipMovement;
    [SerializeField]
    public ShipStats shipStats;
    private CharacterController characterController;

    [SerializeField]
    private GameObject[] Sails;
    // Start is called before the first frame update
    void Start()
    {
        shipMovement = new ShipMovement();
        shipMovement.Sails = new Sail(Sails[0], Sails[1]);
        shipStats = new ShipStats
        {
            MaxSpeed = 5,
            MaxRotateSpeed = 40,
            MaxAccelerationSpeed = 0.01f,

        };
        shipMovement.Initialize(shipStats);
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        shipMovement.UpdateMove(gameObject.transform,characterController, shipMovement.Sails);
    }
}
