using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Chapter")]
public class StorylineChapter : ScriptableObject
{
    [SerializeField] private List<Interaction> interactions;
    [SerializeField] private Characters recipientNPC;
    [SerializeField] private GameObject package;
    [SerializeField] private int nextInteractionID;
    public bool randomNPC = true;

    public int GetNextInteractionID()
    {
        return nextInteractionID;
    }

    public Interaction GetNextInteraction()
    {
        return interactions[nextInteractionID];
    }

    public Characters GetRecipient()
    {
        if (recipientNPC == null)
        {
            return GetLastInteraction().GetNPC();
        }
        return recipientNPC;
    }

    public Interaction GetLastInteraction()
    {
        return interactions[interactions.Count - 1];
    }

    public List<Interaction> GetInteractionList()
    {
        return interactions;
    }

    public void AdvanceChapterProgress()
    {
        nextInteractionID++;
    }

    public void ResetChapterProgress()
    {
        nextInteractionID = 0;
    }

    public bool IsLastInteraction()
    {
        return (nextInteractionID == interactions.Count - 1);
    }

    public GameObject GetPackage()
    {
        return package;
    }
}

[Serializable]
public class Interaction
{
    [SerializeField] private Characters NPC;
    [SerializeField] private string Content;

    public void SetNPC(Characters tempNPC)
    {
        NPC = tempNPC;
    }

    public Characters GetNPC()
    {
        return NPC;
    }

    public string GetContent()
    {
        return Content;
    }
}