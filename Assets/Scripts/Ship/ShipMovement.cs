using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

namespace Assets
{
    internal class ShipMovement       
    {
        public float RotateSpeed; //скорость поворота
        public float Speed;
        public float AccelerationSpeed; // усрокрение
        public Sail Sails; // паруса
        public float MaxSpeed;

        public int GetInt()
        {
            return 1;
        }


        public void UpdateMove(Transform transform, CharacterController controller, Sail sail)
        {
            Vector3 move = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
            {
                sail.IsSailsUp = !sail.IsSailsUp;
                sail.SwitchStatus();
            }

            if (sail.IsSailsUp == true)
            {
                Speed += AccelerationSpeed * Time.deltaTime;
            }
            else
            {
                Speed = Mathf.Clamp(Speed -= AccelerationSpeed * 3 * Time.deltaTime, 0, MaxSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(-transform.up * RotateSpeed * Time.deltaTime);

            }
            if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(transform.up * RotateSpeed * Time.deltaTime);

            }
            move = transform.forward * Speed;
            controller.Move(move);

        }
        public void Initialize(ShipStats stats) 
        {
          MaxSpeed = stats.MaxSpeed;
         RotateSpeed = stats.MaxRotateSpeed;
            AccelerationSpeed = stats.MaxAccelerationSpeed;
            Speed = 0;
        }
    }
}
