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
        public bool IsSailsUp; // паруса
        public float MaxSpeed;

        public void UpdateMove(Transform transform, CharacterController controller)
        {
            Vector3 move = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
            {
                IsSailsUp = !IsSailsUp;
            }

            if (IsSailsUp == true)
            {
                Speed += AccelerationSpeed * Time.deltaTime;
            }
            else
            {
                Speed = Mathf.Clamp(Speed -= AccelerationSpeed * Time.deltaTime, 0, MaxSpeed);
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
            IsSailsUp = true;
            Speed = 0;
        }
    }
}
