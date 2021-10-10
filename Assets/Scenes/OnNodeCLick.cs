using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnNodeCLick : MonoBehaviour
{
    public void OnMouseDown()
    {
        GridLayout grid = GameObject.Find("Grid").GetComponent<GridLayout>();

        grid.removeNode(gameObject.GetComponent<Node>());
    }

    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Node n = gameObject.GetComponent<Node>();
            n.disconnectAll();
        }
    }
}
