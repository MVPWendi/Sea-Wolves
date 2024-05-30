using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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


        private bool IsStopped;
        private DateTime WhenStopped;
        public Vector3 GetRandomWayPoint()
        {
            Vector3 newWaypoint = gameObject.transform.position;
            int xOffset, zOffset;
            xOffset = Random.Range(-20, 20);
            zOffset = Random.Range(-20, 20);
            newWaypoint.x += xOffset;
            newWaypoint.z += zOffset;
            return newWaypoint;
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
            Vector3 waypoint = GetRandomWayPoint();
            
            controller.SetDestination(waypoint);
            if(controller.pathStatus==NavMeshPathStatus.PathInvalid)
            {
                Walk();
            }
        }

        private void ProcessStop()
        {
            int shouldStop = Random.Range(0, 2);
            Debug.Log(shouldStop);
            if (shouldStop == 1)
            {
                IsStopped = true;
                WhenStopped = DateTime.Now;
                Stop();
            }
            else
            {
                Go();
                Walk();
            }
        }
        public void Update()
        {
            if (Vector3.Distance(gameObject.transform.position, controller.destination)<= 0.5f)
            {
                if (!IsStopped)
                {
                    ProcessStop();
                }
                if (IsStopped && (DateTime.Now-WhenStopped).TotalSeconds>=2f)
                {
                    IsStopped = false;
                    Go();
                    Walk();
                }
                
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
