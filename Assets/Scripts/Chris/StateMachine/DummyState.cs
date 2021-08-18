using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyState : State
{

    private StateManager stateInfo;
    private GameObject tagger;

    private bool gotTargets;
    public GameObject[] targets;
    public GameObject target;
    public float distance;

    private void Start()
    {
        stateInfo = GetComponentInParent<StateManager>();
    }

    public override State RunCurrentState(NavMeshAgent agent)
    {
        tagger = stateInfo.contestants.tagger;

        if (isIT)
        {
            if (!gotTargets)
            {
                targets = getTargets();
            }

            findClosest();
            if ((Vector3.Distance(gameObject.transform.parent.position, target.transform.position) <= 2) && stateInfo.canTag)
            {
                Debug.Log(gameObject.transform.parent.name + " tagged: " + target.name);
                if (!target.GetComponent<StateManager>().shielded)
                {
                    tagged();
                }                
            }

            distance = Vector3.Distance(gameObject.transform.parent.position, target.transform.position);
        }
        return this;
    }

    private GameObject[] getTargets()
    {
        var pseudos = stateInfo.contestants.contestants;
        var targs = new GameObject[pseudos.Count - 1];
        int targetIndex = 0;
        for (int i = 0; i < pseudos.Count; i++)
        {
            if (pseudos[i] != gameObject.transform.parent.gameObject)
            {
                targs[targetIndex] = pseudos[i];
                targetIndex++;
            }
        }
        gotTargets = true;
        return targs;
    }

    private void findClosest()
    {
        target = targets[0];

        for (int i = 1; i < targets.Length; i++)
        {
            var dist = Vector3.Distance(transform.parent.position, target.transform.position);
            if (Vector3.Distance(transform.parent.position, targets[i].transform.position) < dist)
            {
                target = targets[i];
            }
        }
    }

    private void tagged()
    {
        stateInfo.taggedAnother(target.GetComponent<StateManager>());
        target.GetComponent<StateManager>().gotTagged(stateInfo);
        stateInfo.contestants.tagger = target;
    }
}
