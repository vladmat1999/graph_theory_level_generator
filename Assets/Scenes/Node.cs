using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static void connect(Node n1, Node n2)
    {
        n1.addConnection(n2);
        n2.addConnection(n1);

        n2.drawLine(n1);
    }

    public static void connectNoGraphics(Node n1, Node n2)
    {
        n1.addConnection(n2);
        n2.addConnection(n1);
    }

    public GameObject getColourNode()
    {
        return gameObject.transform.GetChild(0).gameObject;
    }

    public void Start()
    {
        if(parent == null)
            parent = GameObject.Find("LineParent");
    }

    public static GameObject parent;

    public Shader line;
    public Vector2 gridPosition;
    public int number;
    public HashSet<Node> connections = new HashSet<Node>();
    public Dictionary<Node, GameObject> lines = new Dictionary<Node, GameObject>();
    public HashSet<Node> allNodes = new HashSet<Node>(Game.nodes);
    public HashSet<Node> newConnections = new HashSet<Node>();
    public static bool isPretty = false;
    public bool isThisPretty = false;

    //Constructors

    public Node() { }

    //Methods

        //Set position

    public void setPosition(Vector3 newPos)
    {
        gameObject.GetComponent<RectTransform>().localPosition = newPos;
    }

    public void setPosition(float x, float y)
    {
        setPosition(new Vector2(x, y));
    }
    public void setPosition(float x, float y, float z)
    {
        setPosition(new Vector3(x, y, z));
    }

    public Vector2 getPosition()
    {
        return gameObject.GetComponent<RectTransform>().localPosition;
    }

    public Vector2 getWorldPosition()
    {
        return gameObject.GetComponent<RectTransform>().transform.position;
    }

    public void addLine(Node n, GameObject l)
    {
        try
        {
        lines.Add(n, l);
        }
        catch(System.Exception)
        {}
    }

    //Get Grid Coordinates

    public int getGridX()
    {
        return (int)gridPosition.x;
    }

    public int getGridY()
    {
        return (int)gridPosition.y;
    }

    public void setGrid(int x, int y)
    {
        gridPosition = new Vector2(x, y);
    }

    //Set connections

    void setConnections(HashSet<Node> newConnections)
    {
        connections = newConnections;
    }

    void addConnection(Node n)
    {
        connections.Add(n);
    }

    //Draw 

    public void drawNode()
    {

    }

    public void drawLine(Node other)
    {
        StartCoroutine(DrawLineEnum(this, other, new Color(0.8313f, 0.8131f, 0.8313f, 1)));
    }

    public IEnumerator DrawLineEnum(Node n, Node other, Color color)
    {
        Vector3 start = n.getWorldPosition();
        Vector3 end = other.getWorldPosition();

        start.z = 0;
        end.z = 0;

        GameObject myLine = new GameObject();
        myLine.transform.position = Vector3.zero;
        myLine.AddComponent<LineRenderer>();
        myLine.transform.parent = gameObject.transform.parent.transform;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.material = GameObject.Find("Canvas").GetComponent<Game>().LineMaterial;
        lr.startColor = (Color)color;
        lr.endColor = (Color)color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.numCapVertices = 10;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.gameObject.transform.localPosition = new Vector3(0,0,-1);
        
        this.addLine(other, myLine);
        other.addLine(this, myLine);

        yield return null;
    }

    public void retractLine(Node other)
    {
        try
        {
            GameObject line = lines[other];
            StartCoroutine(retractLineEnum(line, other));
        }
        catch(System.Exception){}
    }

    IEnumerator retractLineEnum(GameObject line, Node other)
    {
        LineRenderer lr = line.GetComponent<LineRenderer>();

        Vector3 end = this.getWorldPosition();
        Vector3 start = other.getWorldPosition();

        lr.SetPosition(0, end);
        lr.SetPosition(1, start);

        GameObject.Destroy(line);

        yield return null;
    }

    public void deleteConnection(Node n)
    {
        connections.Remove(n);
        lines.Remove(n);
    }

    public void deleteThisConnection(Node n)
    {
        connections.Remove(n);
    }

    public void disconnectAll()
    {
        HashSet<Node> tmp  = new HashSet<Node>(connections);
        foreach(Node n in tmp)
        {
            retractLine(n);
            n.deleteConnection(this);
        }

        HashSet<Node> temp = new HashSet<Node>(allNodes);
        temp.ExceptWith(connections);

        lines = new Dictionary<Node, GameObject>();

        connections = new HashSet<Node>(temp);

        foreach(Node n in connections)
        {
            connect(this, n);
        }
    }
    public void disconnectAllTrue()
    {
        HashSet<Node> tmp  = new HashSet<Node>(connections);
        foreach(Node n in tmp)
        {
            retractLine(n);
            n.deleteConnection(this);
        }

        lines = new Dictionary<Node, GameObject>();

        connections = new HashSet<Node>();
    }

    public void switchConnection()
    {
        HashSet<Node> tmp  = new HashSet<Node>(connections);
        
        foreach(Node n in tmp)
        {
            n.deleteThisConnection(this);
        }

        HashSet<Node> temp = new HashSet<Node>(allNodes);
        temp.ExceptWith(connections);

        connections = new HashSet<Node>(temp);

        foreach(Node n in connections)
        {
            connectNoGraphics(this, n);
        }
    }

    public void setPretty(bool pretty)
    {
        if(pretty)
        {
            Animator prettyAnim = gameObject.transform.GetChild(2).GetComponent<Animator>();
            isPretty = true;
            isThisPretty = true;
            prettyAnim.SetBool("isPretty", isPretty);
        }
        else
        {
            Animator prettyAnim = gameObject.transform.GetChild(2).GetComponent<Animator>();
            isPretty = false;
            isThisPretty = false;   
            prettyAnim.SetBool("isPretty", isPretty);
        }
    }

    public bool isConnectedToAll()
    {
        return connections.SetEquals(allNodes);
    }

    public bool isConnectedTo(Node n)
    {
        return connections.Contains(n);
    }

    public bool isAllNodes(Node n)
    {
        return allNodes.Contains(n);
    }

    public void addNewConnection(Node n)
    {
        newConnections.Add(n);
    }

    public void submitConnection()
    {
        HashSet<Node> diff = new HashSet<Node>(connections);
        diff.ExceptWith(newConnections);

        foreach(Node n in diff)
        {
            n.retractLine(this);
            n.deleteConnection(this);
        }

        diff = new HashSet<Node>(newConnections);
        diff.ExceptWith(connections);

        foreach(Node n in diff)
        {
            Node.connect(this, n);
        }

        connections = new HashSet<Node>(newConnections);
    }

    public void emitParticles()
    {
        ParticleSystem particles = gameObject.GetComponentInChildren<ParticleSystem>();
        particles.Play();
    }

    public void setSize(float f)
    {
        gameObject.transform.localScale = new Vector3(60, 60, 60) * f;
    }

    //Overriden

    override
    public int GetHashCode()
    {   
        return (int) (gridPosition.x * 100 + gridPosition.y);
    }

    override
    public bool Equals(object other)
    {
        try
        {
            return ((Node)this).GetHashCode() == ((Node)other).GetHashCode();
        }
        catch(System.Exception)
        {
            return false;
        }
    }

    override
    public string ToString()
    {
        return "" + gridPosition.x + " " + gridPosition.y;
    }

}
