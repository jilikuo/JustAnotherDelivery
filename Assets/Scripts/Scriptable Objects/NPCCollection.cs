using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This Collection must store each individual waypoint in the game.
/// For each waypoint, it will check the NPCs that have been assigned to it, 
/// after validating if the npc has a valid prefab and if they are not duplicated, 
/// it will add them to the list, assigning the waypoint to the NPC.
/// 
/// You should not have to manually assign waypoints to the NPC, but the other way around.
/// 
/// If you need to change a NPC address(waypoint), make sure to delete its waypoint first, then delete the NPC 
/// from the waypoint residents list. After that, you can assign the NPC to a new waypoint, then refresh the Collection.
/// </summary>
[CreateAssetMenu(menuName = "Interaction System/NPC Collection")]
[Serializable]
public class NPCCollection : ScriptableObject
{
    public List<Waypoints> Waypoints;   
    public List<Characters> NPCList;
    public bool refreshCollection;

    private void OnValidate()
    {
        foreach (var wp in Waypoints)
        {
            if (wp.isDirty)
            {
                if (wp.isValid && (wp.residents != null))
                {
                    foreach (var npc in wp.residents)
                    {
                        if (ValidateResident(npc, wp))
                        {
                            if (!npc.hasBeenAssigned || npc.waypoint == null)
                            {
                                SetUpNPCWaypoint(npc, wp);
                            }

                            NPCList.Add(npc);
                        }
                    }
                }

                wp.isDirty = false;
            }
        }

        foreach (var npc in NPCList)
        {
            if (npc.waypoint == null)
            {
                Debug.LogError("Character " + npc.fullName + " has no waypoint assigned");
            }
        }

        NPCList = NPCList.Distinct().ToList();
        NPCList.Sort((x, y) => string.Compare(x.fullName, y.fullName));
        refreshCollection = false;
    }

    private bool ValidateResident(Characters npc, Waypoints wp)
    {
        if (npc.characterPrefab == null)
        {
            Debug.LogError("Character prefab is not set for " + npc.fullName);
            return false;
        }

        if (NPCList.Contains(npc))
        {   
            if (npc.waypoint == null)
            {
                Debug.LogWarning("Character " + npc.fullName + " has no waypoint assigned, but is already in the NPC list, their addres will be changed");
                return true;
            }

            if (npc.waypoint != wp)
            {
                    Debug.LogError("Character " + npc.fullName + " is already assigned to " + npc.waypoint.GetCurrentAddress() + " Could not assign them to " + wp.GetCurrentAddress());
            }
            return false;
        }

        return true;
    }

    private void SetUpNPCWaypoint(Characters npc, Waypoints wp)
    {
        npc.waypoint = wp;
        npc.hasBeenAssigned = true;
        Debug.Log("Assigned " + npc.fullName + " to " + wp.GetCurrentAddress() + ".");
    }
}
