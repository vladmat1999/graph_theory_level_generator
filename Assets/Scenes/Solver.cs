using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver
{
    private List<Node> nodes;
    private int total;
    private int smallestSol = 100000000;
    private List<Node> goodSol = new List<Node>();
    private List<Node> smallestSolList;
    private int stepsSoFar = 0;
    public Solver()
    {
        this.nodes = new List<Node>(GridLayout.nodes);
        total = nodes.Count;
    }

    public List<Node> solveGraph()
    {
        solve(0);
        return smallestSolList;
    }

    public void solve(int i)
    {
        if(i == total)
            return;

        Node n = nodes[i];

        if(test())
        {
            if(stepsSoFar < smallestSol)
            {
                smallestSol = stepsSoFar;
                smallestSolList = new List<Node>(goodSol);
            }
        }

        solve(i+1);

        n.switchConnection();
        goodSol.Add(n);
        stepsSoFar++;

        if(test())
        {
            if(stepsSoFar < smallestSol)
            {
                smallestSol = stepsSoFar;
                smallestSolList = new List<Node>(goodSol);
            }
        }

        solve(i+1);

        n.switchConnection();
        goodSol.Remove(n);
        stepsSoFar--;
    }

    public static bool test()
    {
        foreach(Node n in GridLayout.nodes)
        {
            if(!n.isConnectedToAll())
                return false;
        }

        return true;
    }
}
