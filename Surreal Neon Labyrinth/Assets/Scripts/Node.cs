using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public int state; //0 - unwalkable, 1 - obstructed(by dedstructible block), 2 - walkable;

    public Node(Vector3 _worldPosition, int _state)
    {
        worldPosition = _worldPosition;
        state = _state;
    }

}
