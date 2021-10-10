using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Game : MonoBehaviour
{
    public static float width;
    public static float height;
    public static float deltaWidth;
    public static float deltaHeight;
    public static Vector2 resolution;

    public Shader line;

    public GameObject nodePrefab;
    public static List<Node> nodes = new List<Node>();
    public static int levelNumber;
    public Material LineMaterial;
    public Vector2[] originalPosition;
    public int gridWidth;
    public int gridHeight;
    public float nodeSize;
    public Material material;
    //SpawnNode

   

    void Awake()
    {
        //Set resolution

        
    }

    // Start is called before the first frame update
    void Start()
    {
        width = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;
        deltaWidth = width / 510;
        deltaHeight = height / 510;
        resolution = new Vector2(width, height);
        originalPosition = new Vector2[]{Vector2.zero - new Vector2(0, Game.deltaHeight * 30), new Vector2(Game.deltaWidth * 350, Game.deltaWidth * 350)};
        
        LineMaterial = material;

        levelNumber = 1;  
    }


    // Update is called once per frame
    void Update()
    {
        foreach(Node n in nodes)
        {
            n.setSize(nodeSize);
        }
    }
    public Node spawnNode(int x, int y)
    {
        GameObject go = (GameObject)Instantiate(nodePrefab);
        go.transform.SetParent(gameObject.transform);
        Node n = go.GetComponent<Node>();
        n.setSize(nodeSize);
        nodes.Add(n);
        return n;
    }


}