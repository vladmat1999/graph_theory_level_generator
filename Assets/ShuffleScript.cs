using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleScript : MonoBehaviour
{
    public GridLayout grid;
    public static int maxStepsLast;

    public void OnMouseDown()
    {
        List<Node> maxpath = new List<Node>();

        for(int i=0; i<40; i++)
        {
            grid.shuffle();

            Solver solver = new Solver();
            List<Node> l = solver.solveGraph();
            int steps = l.Count;

            if(steps > maxStepsLast)
                {
                    maxStepsLast = steps;
                    maxpath = l;
                    break;
                }
        }

        Debug.Log("Steps " + maxpath.Count);
        grid.moves = maxpath.Count;

        foreach(Node n in maxpath)
        {
            Debug.Log(n);
        }
    }
}
