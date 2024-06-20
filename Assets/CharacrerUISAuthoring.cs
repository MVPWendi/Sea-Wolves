using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CharacrerUISAuthoring : MonoBehaviour
{
    public HealthBarUI HealthBar;


    public class Baker : Baker<CharacrerUISAuthoring>
    {
        public override void Bake(CharacrerUISAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new CharacterUIS { HealthBar = authoring.HealthBar });
        }
    }
}

public class CharacterUIS : IComponentData
{
    public HealthBarUI HealthBar;
}
