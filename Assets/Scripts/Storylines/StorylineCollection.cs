using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Collections/Storyline Collection")]
public class StorylineCollection : ScriptableObject
{
    [SerializeField] private List<Storyline> stories;

    public List<Storyline> GetStorylineList()
    {
        return stories;
    }

    public Storyline GetStorylineByID(StorylineID ID)
    {
        return stories.Find(story => story.GetID() == ID);
    }

    public Storyline GetRandomRepeatableStoryline()
    {
        List<Storyline> tempStoryList = stories.FindAll(story => story.isRepeatable);
        return tempStoryList[Random.Range(0, stories.Count)];
    }
}