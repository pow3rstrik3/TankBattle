using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : FSMState
{
    public FleeingState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Fleeing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, destPos);
        if (npc.GetComponent<NPCTankController>().isTooCloseToFriendly())
        {
            Debug.Log("Switch to Evading State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.toCloseToFriendly);
        }
        else if (dist > 100.0f && dist <= 300.0f)
        {
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }
        else
        {
            Debug.Log("Switch to Patrolling State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //Rotate to the target point
        destPos = -player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}
