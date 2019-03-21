using UnityEngine;
using System.Collections;

public class PatrolState : FSMState
{
    public PatrolState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Patrolling;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Check the distance with player tank
        //When the distance is near, transition to chase state
        float dist = Vector3.Distance(npc.position, destPos);
        int health = npc.GetComponent<NPCTankController>().getHealth();

        if (npc.GetComponent<NPCTankController>().isTooCloseToFriendly())
        {
            Debug.Log("Switch to Evading State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.toCloseToFriendly);
        }
        else if (dist <= 100.0f && health < 100)
        {
            Debug.Log("Switch to Fleeing state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.inDanger);
        }
        else if (dist <= 300.0f)
        {
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //Find another random patrol point if the current point is reached
		
        if (Vector3.Distance(npc.position, destPos) <= 100.0f)
        {
            Debug.Log("Reached to the destination point\ncalculating the next point");
            FindNextPoint();
        }

        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}