using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

public class ShipAuthoring : MonoBehaviour
{
    public float MaxSpeed;
    public float RotationSpeed;
    public float AccelerationSpeed;


    public class Baker : Baker<ShipAuthoring>
    {
        public override void Bake(ShipAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShipComponent{
                MaxSpeed = authoring.MaxSpeed,
                RotationSpeed = authoring.RotationSpeed,
                AccelerationSpeed = authoring.AccelerationSpeed,
                Speed = 0,
                ShouldGoForward = false,
                ShouldRotate = false
            });
        }
    }
}



public struct ShipComponent : IComponentData
{
    public float MaxSpeed;
    public float RotationSpeed;
    public float AccelerationSpeed;
    public float Speed;
    public bool ShouldRotate;
    public bool RotateRight; // ≈сли включен поворот, то эта переменна€ отвечает за лево право (если 1 - вправо, если 0 - влево)
    public bool ShouldGoForward;
}

