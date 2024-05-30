using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public class NPC : MonoBehaviour
    {
        public float MaxHp;
        public float CurrentHp;
        public float ShiftSpeed;
        public float MoveSpeed;
        public NavMeshAgent controller;
        public GameObject[] PointsToWalk;
        public Vector3 WalpPoint()
        {
            Vector3 result = PointsToWalk[(int)UnityEngine.Random.Range(0, PointsToWalk.Length - 1)].transform.position;
            return result;
        }

        public void Stop() 
        {
            controller.speed = 0;
        }

        public void Run()
        {
            controller.speed = ShiftSpeed;
        }
        
        public void Go()
        {
            controller.speed = MoveSpeed;
        }

        public void Walk()
        {
            Vector3 Point = WalpPoint();
            controller.SetDestination(Point);

        }
        public void Update()
        {
            if (Vector3.Distance(gameObject.transform.position, controller.destination)<= 1)
            {
                Walk();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Run();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Go();
            }
            if(Input.GetKeyDown(KeyCode.S)) 
            { 
                Stop(); 
            }
            
        }


        public void Start()
        {
            Walk();
        }


    }
}
