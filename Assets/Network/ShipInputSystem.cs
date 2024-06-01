using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.CharacterController;
using Unity.NetCode;
using Assets.Network;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class ShipInputsSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<NetworkTime>();
        RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ShipComponent, ShipInputs>().Build());
    }

    protected override void OnUpdate()
    {

        foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<ShipInputs>, RefRW<ShipComponent>>())
        {
            //Debug.Log("YES");
            //Debug.Log("Name: " + player.ValueRO.Name);
            playerInputs.ValueRW.MoveInput = new float2
            {
                x = (Input.GetKey(KeyCode.D) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f),
                y = (Input.GetKey(KeyCode.W) ? 1f : 0f) + (Input.GetKey(KeyCode.S) ? -1f : 0f),
            };


            player.ValueRW.SailPressed = default;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ValueRW.SailPressed.Set();
                player.ValueRW.Name = "Fuck you";
            }
            //Debug.Log("Name2: " + player.ValueRO.Name);
            //Debug.Log("Name22: " + player.ValueRO.SailPressed.IsSet);
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a variable rate
/// </summary>
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
[UpdateAfter(typeof(PredictedFixedStepSimulationSystemGroup))]
[BurstCompile]
public partial struct ShipVariableStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkTime>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ShipComponent, ShipInputs>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        NetworkInputUtilities.GetCurrentAndPreviousTick(SystemAPI.GetSingleton<NetworkTime>(), out NetworkTick currentTick, out NetworkTick previousTick);

        foreach (var (playerInputsBuffer, player) in SystemAPI.Query<DynamicBuffer<InputBufferData<ShipInputs>>, ShipComponent>().WithAll<Simulate>())
        {
            NetworkInputUtilities.GetCurrentAndPreviousTickInputs(playerInputsBuffer, currentTick, previousTick, out ShipInputs currentTickInputs, out ShipInputs previousTickInputs);
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a fixed rate.
/// It is necessary to handle this as part of the fixed step group, in case your framerate is lower than the fixed step rate.
/// </summary>
[UpdateInGroup(typeof(PredictedFixedStepSimulationSystemGroup), OrderFirst = true)]
[BurstCompile]
public partial struct ShipFixedStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkTime>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ShipComponent, ShipInputs>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInputs, player) in SystemAPI.Query<ShipInputs, ShipComponent>().WithAll<Simulate>())
        {
            //Debug.Log("JUMP2: " + player.Name);
            //Debug.Log("JUMP22: " + player.SailPressed.IsSet);
        }
    }
}