using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;
public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;
    NavMeshAgent agent;
    
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Node goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Node goToBackDoor = new Leaf("Go To BackDoor", GoToBackDoor);
        Node goToFrontDoor = new Leaf("Go To FrontDoor", GoToFrontDoor);
        Node goToVan = new Leaf("Go To Van", GoToVan);
        Selector opendoor = new Selector("Open Door");

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);
        opendoor.AddChild(goToFrontDoor);

        steal.AddChild(goToBackDoor);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);    
        tree.AddChild(steal);

        tree.PrintTree();

        

        
    }

    public Node.Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }
    public Node.Status GoToBackDoor()
    {
        return GoToLocation(backdoor.transform.position);
    }
    public Node.Status GoToFrontDoor()
    {
        return GoToLocation(frontdoor.transform.position);
    }
    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    private Node.Status GoToLocation(UnityEngine.Vector3 position)
    {
        throw new NotImplementedException();
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = UnityEngine.Vector3.Distance(destination, this.transform.position);
        if(state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (UnityEngine.Vector3.Distance(agent.pathEndPosition, destination)  >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }
    // Update is called once per frame
    void Update()
    {
        if(treeStatus == Node.Status.RUNNING)
            treeStatus = tree.Process();
        
        
    }
}
