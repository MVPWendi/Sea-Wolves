using Assets;
using Assets.Components;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class NPCAuthoring : MonoBehaviour
{
    public string Name;
    public DialogueSO Dialogue;
    public class Baker : Baker<NPCAuthoring>
    {

        public override void Bake(NPCAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NPCComponent
            {
                Name = new FixedString128Bytes(authoring.Name),
            });



        }
    }
}