using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Components
{
    public struct NPCComponent : IComponentData
    {
        public FixedString128Bytes Name;
        public Dialogue Dialogue;
    }
    
    public struct TraderTag : IComponentData
    {

    }

    public struct Dialogue
    {
        public Message CurrentMessage;
        public BlobArray<Message> Messages;
    }


    public struct Message
    {
        public FixedString128Bytes Text;
        public BlobArray<Answer> Answers;
    }


    public struct Answer
    {
        public FixedString128Bytes Text;
        public EAnswerType AnswerType;
    }


    public enum EAnswerType
    {
        Exit,
        Next,
        Quest,
        Trade
    }
}
