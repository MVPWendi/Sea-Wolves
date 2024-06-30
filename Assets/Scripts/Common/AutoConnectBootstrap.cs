using Assets.Network.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
[UnityEngine.Scripting.Preserve]
public class AutoConnectBootstrap : ClientServerBootstrap
{
    public override bool Initialize(string defaultWorldName)
    {
        //AutoConnectPort = 7777;
        //CreateDefaultClientServerWorlds();
        return false;
    } 


}



public struct GoInGameRequest : IRpcCommand { }
