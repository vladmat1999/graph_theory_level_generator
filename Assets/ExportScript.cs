using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportScript : MonoBehaviour
{
    public GridLayout grid;

    void Start()
    {
        //grid.number = PlayerPrefs.GetInt("GridGenLevelNumber");
    }

    public void OnMouseDown()
    {
        grid.export();
        PlayerPrefs.SetInt("GridGenLevelNumber", grid.number);
        
    }
}
