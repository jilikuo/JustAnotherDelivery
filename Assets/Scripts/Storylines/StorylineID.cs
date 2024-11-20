using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StorylineID
{
    None,
    Main_Story,
    Stamp_Delivery,
    World_Diversity
}

public enum StorylineState
{
    Inactive,
    WaitingDelivery,
    WaitingInput
}