using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Storyline")]
public class Storyline : ScriptableObject
{
    [SerializeField] private StorylineID id;
    [SerializeField] private List<StorylineChapter> chapterList;
    [SerializeField] private int currentChapterID;

    public bool isRepeatable;

    public StorylineID GetID()
    {
        return id;
    }

    public StorylineChapter GetCurrentChapter()
    {
        return chapterList[currentChapterID];
    }

    public int GetCurrentChapterID()
    {
        return currentChapterID;
    }

    public void SetCurrentChapter(int value)
    {
        currentChapterID = value;
    }

    public void ResetStorylineProgress()
    {
        currentChapterID = 0;
        foreach (var chapter in chapterList)
        {
            chapter.ResetChapterProgress();
        }
    }

    public void ResetCurrentChapter()
    {
        chapterList[currentChapterID].ResetChapterProgress();
    }

    public bool IsFinished()
    {
        if (isRepeatable)
        {
            return false;
        }

        if (currentChapterID < chapterList.Count)
        {
            return false;
        }

        return true;
    }

    public void AdvanceProgress()
    {
        currentChapterID++;

        if (isRepeatable && (currentChapterID >= chapterList.Count))
        {
            ResetStorylineProgress();
        }
    }
}
