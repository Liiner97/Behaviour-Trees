using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(String n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status chieldstatus = children[currentChild].Process();
        if (chieldstatus == Status.RUNNING) return Status.RUNNING;

        if (chieldstatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}

