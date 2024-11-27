using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utilities/Random Storyline Generator")]
[Serializable]
public class RandomStorylineGenerator : RandomObjectGenerator<Storyline>
{
    [SerializeField] private StorylineCollection storylines;
    [SerializeField] private StorylineCollection repeatableStorylines;

    protected override string GetKey(Storyline entry)
    {
        return (entry == null) ? null : entry.ToString();
    }

    protected override int IndexOf(string key)
    {
        return entries.FindIndex(x => x.ToString() == key);
    }

    protected override Storyline CopyEntry(Storyline entry)
    {
        return entry.Clone();
    }

    public void Populate()
    {
        foreach (var storyline in storylines.GetStorylineList())
        {
            if (storyline.IsFinished())
            {
                continue;
            }

            AddEntry(storyline);
        }
    
        foreach (var storyline in repeatableStorylines.GetStorylineList())
        {
            if (storyline.GetCurrentChapter().randomNPC)
            {
                continue;
            }

            AddEntry(storyline);
        }
    }

    public bool hasEntries()
    {
        return entries.Count > 0;
    }
}