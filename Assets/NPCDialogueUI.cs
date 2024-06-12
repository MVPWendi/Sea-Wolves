using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class NPCDialogueUI : MonoBehaviour
{
    public TextMeshPro NPCName;
}
public struct NPCDialogueRefering : ICleanupComponentData
{
    public NPCDialogueUI UI;

}