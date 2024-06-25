using Assets;
using Assets.Components;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Answer = Assets.Answer;

public class NPCDialogueUI : MonoBehaviour
{
    public DialogueSO Dialogue;
    public NPCComponent NPCComponent;
    public TMP_Text Text;

    public List<Button> Buttons;

    private void OnAnswerClick(Answer answer)
    {
        switch (answer.AnswerType)
        {
            case Assets.Components.EAnswerType.Exit:
                Exit();
                break;
            case Assets.Components.EAnswerType.Next:
                SetNext(answer);
                break;
            case Assets.Components.EAnswerType.Trade:
                StartTrade();
                break;
            case Assets.Components.EAnswerType.Quest:
                StartQuest();
                break;
        }
        Debug.Log(answer.Text);
    }

    private void SetNext(Answer answer)
    {
        Dialogue.CurrentMessageID = answer.NextMessageGuid;
        UpdateMessage();
    }

    private void Exit()
    {
    }



    private void StartTrade()
    {
        throw new NotImplementedException();
    }

    private void StartQuest()
    {
        throw new NotImplementedException();
    }


    private void UpdateMessage()
    {
        var currentMessage = Dialogue.Messages.Find(x => x.Guid == Dialogue.CurrentMessageID);
        Debug.Log(currentMessage.Answers.Count);
        Debug.Log(Buttons.Count);
        for (int i = 0; i < Buttons.Count; i++)
        {
            var capturedIndex = i;
            Buttons[i].onClick.RemoveAllListeners();
            Buttons[i].onClick.AddListener(delegate
            {
                OnAnswerClick(currentMessage.Answers[capturedIndex]);
            });
        }
        Text.text = currentMessage.Text;
        for (int i = 0; i < currentMessage.Answers.Count; i++)
        {
            var textField = Buttons[i].GetComponentInChildren<TMP_Text>();
            textField.text = currentMessage.Answers[i].Text;
        }
    }

    private void Start()
    {
        UpdateMessage();
    }
}