using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveScript : MonoBehaviour
{
    public GridLayout grid;
    public void OnMouseDown()
    {
        Solver solver = new Solver();
        List<Node> l = solver.solveGraph();

        ShuffleScript.maxStepsLast = l.Count;

        Debug.Log("Steps " + l.Count);

        foreach(Node n in l)
        {
            Debug.Log(n);
        }

        grid.moves = l.Count;
    }
}
