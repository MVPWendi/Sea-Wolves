using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue/Dialogue", order = 1)]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        public List<Message> Messages;
        [SerializeField]
        public string CurrentMessageID;
    }
}
