using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : FSMState
{
    public EvadeState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Evading;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, destPos);
        bool closeToFriendly = npc.GetComponent<NPCTankController>().isTooCloseToFriendly();
        int health = npc.GetComponent<NPCTankController>().getHealth();

        if(!closeToFriendly) { 
            if (dist <= 100.0f && health < 100)
            {
                Debug.Log("Switch to Fleeing state");
                npc.GetComponent<NPCTankController>().SetTransition(Transition.inDanger);
            }
            else if (dist <= 300.0f)
            {
                Debug.Log("Switch to Chase State");
                npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
            }
            else
            {
                Debug.Log("Switch to Patrol");
                npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
            }
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