using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue/Message", order = 1)]
    public class Message : ScriptableObject
    {
        public string Guid;
        public string Text;
        public List<Answer> Answers;
    }
}
