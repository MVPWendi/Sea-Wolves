using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue/Answer", order = 1)]
    public class Answer : ScriptableObject
    {
        public string Text;
        public string NextMessageGuid;
        public EAnswerType AnswerType;
    }
}
