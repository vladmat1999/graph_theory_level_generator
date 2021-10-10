﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGrid : MonoBehaviour
{
    public GridLayout grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        grid.repack();
        ShuffleScript.maxStepsLast = 0;
        grid.nodeSize = 1;
        grid.lineSize = 1;
    }
}
