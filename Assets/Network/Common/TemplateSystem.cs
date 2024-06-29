using Unity.Entities;

namespace Assets.Network.Common
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TemplateSystem : ISystem
    {
        void OnCreate(ref SystemState state)
        {

        }
        void OnUpdate(ref SystemState state)
        {

        }
    }
}