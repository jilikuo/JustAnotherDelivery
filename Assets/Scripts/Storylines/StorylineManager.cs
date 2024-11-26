using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorylineManager : MonoBehaviour, ISaveable
{
    [SerializeField] private StorylineCollection storylineCollection;
    [SerializeField] private StorylineCollection repeatableStorylineCollection;
    private GameManager gameManager;
    private Storyline activeStoryline;
    private StorylineChapter activeChapter;
    private NavigationMenuHandler navMenuHandler;

    public StorylineState state;

    public static StorylineManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (storylineCollection == null)
        {
            Debug.Log("No STORYLINE COLLECTION was found at STORYLINE MANAGER");
        }

        if (repeatableStorylineCollection == null)
        {
            Debug.Log("No >REPEATABLE< STORYLINE COLLECTION was found at STORYLINE MANAGER");
        }

        gameManager = GameManager.instance;
    }

    public void Save(GameData gameData)
    {
        var data = gameData.storylineManagerStorylines;
        foreach (Storyline story in storylineCollection.GetStorylineList())
        {
            data.Add(story.GetCurrentChapterID().ToString());
        }

        data = gameData.storylineManagerRepeatableStorylines;
        foreach (Storyline story in repeatableStorylineCollection.GetStorylineList())
        {
            data.Add(story.GetCurrentChapterID().ToString());
        }
    }

    public bool Load(GameData gameData)
    {
        var data = gameData.storylineManagerStorylines;
        var storylines = storylineCollection.GetStorylineList();
        if (data.Count != storylines.Count)
        {
            return false;
        }

        for (int i = 0; i < storylines.Count; i++)
        {
            storylines[i].SetCurrentChapter(Convert.ToInt32(data[i]));
        }

        data = gameData.storylineManagerRepeatableStorylines;
        storylines = repeatableStorylineCollection.GetStorylineList();
        if (data.Count != storylines.Count)
        {
            return false;
        }

        for (int i = 0; i < storylines.Count; i++)
        {
            storylines[i].SetCurrentChapter(Convert.ToInt32(data[i]));
        }
        return true;
    }

    public void ResetDailyProgress()
    {
        SaveSystem.DataManager.instance.Load(this);
    }

    public StorylineID GetActiveStorylineID()
    {
        return activeStoryline.GetID();
    }

    public Storyline GetStorylineByID(StorylineID iD)
    {
        return storylineCollection.GetStorylineByID(iD) ?? repeatableStorylineCollection.GetStorylineByID(iD);
    }

    public void ResetAllStorylines()
    {
        foreach (var story in storylineCollection.GetStorylineList())
        {
            story.ResetStorylineProgress();
        }

        foreach (var story in repeatableStorylineCollection.GetStorylineList())
        {
            story.ResetStorylineProgress();
        }
    }

    public void SetActiveStoryline(Storyline story)
    {
        activeStoryline = story;
    }

    public void UpdateActiveChapter()
    {
        activeChapter = activeStoryline.GetCurrentChapter();
    }

    public void PlayScriptedStoryline(StorylineID ID)
    {
        UpdateNavMenuHandler();
        SetActiveStoryline(storylineCollection.GetStorylineByID(ID));
        UpdateActiveChapter();
        StartNextChapter();
    }

    public void PlayRandomRepeatableStoryline()
    {
        UpdateNavMenuHandler();
        SetActiveStoryline(repeatableStorylineCollection.GetRandomRepeatableStoryline());
        UpdateActiveChapter();
        if (activeChapter.randomNPC)
        {
            SetupRandomNPC();
        }
        StartNextChapter();
    }
    
    public void ResetCurrentChapterProgress()
    {
        activeStoryline.ResetCurrentChapter();
        state = StorylineState.Inactive;
    }

    private void StartNextChapter()
    {
        UpdateNavMenuHandler();
        state = StorylineState.WaitingInput;
        UpdateInteractionButton();
        AdvanceInteraction();
    }

    public void FinishChapter()
    {
        ShowInteraction(activeChapter.GetLastInteraction());
        state = StorylineState.Inactive;
        UpdateInteractionButton();
        activeStoryline.AdvanceProgress();
    }

    public void AdvanceInteraction()
    {
        UpdateActiveChapter();
        ShowInteraction(activeChapter.GetNextInteraction());
        activeChapter.AdvanceChapterProgress();

        if (activeChapter.IsLastInteraction())
        {
            state = StorylineState.WaitingDelivery;
        }

        UpdateInteractionButton();
    }

    private void ShowInteraction(Interaction interaction)
    {
        UpdateNavMenuHandler();
        navMenuHandler.SetActiveCharacter(interaction.GetNPC());
        navMenuHandler.SetMessage(interaction.GetContent());
    }

    private void SetupRandomNPC()
    {
        List<Interaction> interactions = activeChapter.GetInteractionList();
        Characters NPC = navMenuHandler.GetRandomNPC();

        if (NPC == null)
        {
            Debug.Log("No npc found to setup repeatable storyline");
            return;
        }

        foreach (var interaction in interactions)
        {
            interaction.SetNPC(NPC);
        }
    }

    private void UpdateNavMenuHandler()
    {
        if (navMenuHandler != null)
        {
            return;
        }
        
        navMenuHandler = FindAnyObjectByType<NavigationMenuHandler>();
        UpdateInteractionButton();
    }

    public void UpdateInteractionButton()
    {
        navMenuHandler.UpdateInteractionButton();
    }

    public StorylineCollection GetLinearStorylines()
    {
        return storylineCollection;
    }


    public StorylineCollection GetRepeatableStorylines()
    {
        return repeatableStorylineCollection;
    }

}
