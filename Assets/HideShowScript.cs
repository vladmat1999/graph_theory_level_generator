using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowScript : MonoBehaviour
{
    public GridLayout grid;

    public void OnMouseDown()
    {
        grid.toggleHideGraph();
    }
}
