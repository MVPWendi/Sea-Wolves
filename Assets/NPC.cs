using Assets.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;
using Unity.Physics;
using Unity.Physics.Hybrid;
using Unity.Physics.Authoring;
using Unity.AI.Navigation;
namespace Assets
{
    public class NPC : MonoBehaviour
    {
        public NavMeshAgent controller;


        public void Update()
        {
            controller.SetDestination(new Vector3(100, 0, 100));
        }
    }
    public struct NavMeshAgentComponent : IComponentData
    {

    }

    public partial class Baker : Baker<NPC>
    {
        public override void Bake(NPC authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add NavMeshAgent component with provided controller reference
            AddComponentObject<NavMeshAgent>(entity, authoring.controller);

            // Add a new component data for NavMeshAgentComponent
            var navMeshAgentComponentData = new NavMeshAgentComponent();
            AddComponent(entity, navMeshAgentComponentData);
        }
    }
    
    public partial class TestSytem : SystemBase
    {
        protected override void OnUpdate()
        {
            Debug.Log("Test System");
            float3 targetPosition = float3.zero;
            foreach (var (agent, entity) in SystemAPI.Query <RefRW<NavMeshAgentComponent>>().WithEntityAccess())
            {
                Debug.Log("Test System2");
                var agent1 = EntityManager.GetComponentObject<NavMeshAgent>(entity);
                Debug.Log(agent1);
                agent1.SetDestination(new Vector3(100, 0, 100));
            }
        }
    }
}
