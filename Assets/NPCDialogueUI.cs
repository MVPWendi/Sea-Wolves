using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class NPCDialogueUI : MonoBehaviour
{
    public TMP_Text NPCName;
}

public class UIS : IComponentData
{
    public NPCDialogueUI DialogueUI;
}