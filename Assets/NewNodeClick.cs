using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNodeClick : MonoBehaviour
{
    public Vector2 gridPos;
    public GridLayout grid;
    public GameObject node;
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
        GameObject n = Instantiate(node, Vector3.zero, Quaternion.identity);
        n.transform.SetParent(transform.parent);
        n.GetComponent<Node>().gridPosition = gridPos;
        GridLayout.nodes.Add(n.GetComponent<Node>());
        grid.pack();
        grid.connectNodes();
        gameObject.SetActive(false);
    }
}
