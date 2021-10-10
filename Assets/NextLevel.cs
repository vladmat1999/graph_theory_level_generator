using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GridLayout grid;

    public void OnMouseDown()
    {
        grid.number++;
        grid.loadLevel(grid.number);
    }
}
