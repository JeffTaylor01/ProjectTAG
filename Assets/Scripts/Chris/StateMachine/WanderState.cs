using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    public FleeState fleeState;
    public ChaseState chaseState;
    public FindItemState itemState;

    public float xMax = 10;
    public float xMin = -10;
    public float zMax = 10;
    public float zMin = -10;
    private Vector3 destination;

    private StateManager stateInfo;
    private GameObject tagger;
    public Material wanderColor;

    public float itemUseChance;
    private float chanceTimer = 0;

    private void Start()
    {
        stateInfo = GetComponentInParent<StateManager>();
        destination = transform.parent.position;
    }
    public override State RunCurrentState(NavMeshAgent agent)
    {
        runChance();
        tagger = stateInfo.contestants.tagger;

        if (isIT)
        {
            return chaseState;
        }
        else
        {
            if (checkMaterial())
            {
                GetComponentInParent<MeshRenderer>().material = wanderColor;
            }
            if (stateInfo.heldItem == null && stateInfo.itemInRange)
            {
                return itemState;
            }
            if (canSeeIT() && !stateInfo.ignoreIT)
            {
                return fleeState;
            }
            else
            {
                if (stateInfo.heldItem != null)
                {
                    bool useItem = false;

                    if (stateInfo.heldItem.tag.Equals("SpeedBoostItem"))
                    {
                        if (itemUseChance > 0.8)
                        {
                            Debug.Log("Pseudo Used Item");
                            useItem = true;
                        }
                        var item = stateInfo.heldItem.GetComponent<SpeedBoost>();
                        item.user = transform.parent.gameObject;

                        if (useItem && !stateInfo.itemUsed)
                        {
                            item.UseItem();
                        }
                        item.RunTimer();
                    }
                    else if (stateInfo.heldItem.tag.Equals("ShieldItem"))
                    {
                        if (itemUseChance > 0.95)
                        {
                            Debug.Log("Pseudo Used Item");
                            useItem = true;
                        }
                        var item = stateInfo.heldItem.GetComponent<Shield>();
                        item.user = transform.parent.gameObject;

                        if (useItem && !stateInfo.itemUsed)
                        {
                            item.UseItem();
                        }
                        item.RunTimer();
                    }
                    else if (stateInfo.heldItem.tag.Equals("StunItem"))
                    {
                        if (itemUseChance > 0.5)
                        {
                            Debug.Log("Pseudo Used Item");
                            useItem = true;
                        }
                        var item = stateInfo.heldItem.GetComponent<Stun>();
                        item.user = transform.parent.gameObject;
                        item.startPos = stateInfo.gameObject.transform.position;                        

                        if (useItem && !stateInfo.itemUsed)
                        {
                            item.aiming = true;
                            item.Aim();
                            item.UseItem();
                        }
                        item.RunTimer();
                    }
                }
                else
                {
                    itemUseChance = 0;
                }
                //Debug.Log("Wandering");
                if (Vector3.Distance(transform.parent.position, destination) < 10)
                {
                    destination = newWayPoint(agent);
                }
                agent.SetDestination(destination);
                return this;
            }
        }
                
    }

    private Vector3 newWayPoint(NavMeshAgent agent)
    {
        var dest = Vector3.zero; //default
        Vector3 point = new Vector3(Random.Range(xMin, xMax), transform.parent.position.y, Random.Range(zMin, zMax));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 25f, NavMesh.GetAreaFromName("Walkable")))
        {
            dest = hit.position;
        }
        else
        {
            dest = point;
        }
        return dest;
    }

    private bool canSeeIT()
    {
        if (Vector3.Distance(transform.position, tagger.transform.position) <= 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkMaterial()
    {
        if (GetComponentInParent<MeshRenderer>().material != wanderColor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void runChance()
    {
        if (chanceTimer >= 3 && stateInfo.heldItem != null)
        {
            itemUseChance = Random.value;
            chanceTimer = 0;
        }

        if (stateInfo.heldItem != null)
        {
            chanceTimer += Time.deltaTime;
            Debug.Log(chanceTimer);
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination, 1f);
    }
}
