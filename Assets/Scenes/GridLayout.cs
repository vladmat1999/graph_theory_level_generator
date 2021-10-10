using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridLayout : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(5,5);
    public static List<Node> nodes = new List<Node>();
    public List<GameObject> emptyNodes = new List<GameObject>();
    public GameObject emptyNode;
    public float nodeSize;
    public float lineSize;
    public bool isHidden = false;
    public int number;
    public int moves;
    public GameObject nodePrefab;
    void Start()
    {
    }
    void Awake()
    {
        /*
        for(;number <= 100;)
        {
            loadLevel(number);
            Solver solver = new Solver();
            List<Node> l = solver.solveGraph();
            moves = l.Count;
            number--;
            export();
            number++;
            Debug.Log(number-1);
        }
        */
    }
    
    void Update()
    {
        foreach(Node n in nodes)
        {
            n.setSize(nodeSize);
            foreach(KeyValuePair<Node, GameObject> kv in n.lines)
            {
                LineRenderer lr = kv.Value.GetComponent<LineRenderer>();
                lr.startWidth = 0.18f * lineSize;
                lr.endWidth = 0.18f * lineSize;
            }
        }

        if(Input.GetKey(KeyCode.Q))
            nodeSize -= 0.01f;
        if(Input.GetKey(KeyCode.W))
            nodeSize += 0.01f;
        if(Input.GetKey(KeyCode.A))
            lineSize -= 0.01f;
        if(Input.GetKey(KeyCode.S))
            lineSize += 0.01f;

        if(Input.GetKeyDown(KeyCode.Z))
        {
            number--;
            loadLevel(number);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            number++;
            loadLevel(number);
        }

    }
    public void setSize(int x, int y)
    {
        gridSize = new Vector2(x, y);
    }

    public void add(Node n)
    {
        nodes.Add(n);
    }

    public void pack()
    {
        Vector2 center = Vector2.zero - new Vector2(0, Game.deltaHeight * 30);
        Vector2 resolution = new Vector2(Game.deltaWidth * 350, Game.deltaWidth * 350);
        Vector2 interval = new Vector2(resolution.x / gridSize.x, resolution.y / gridSize.y);
        Vector2 topLeftCorner = new Vector2(center.x - resolution.x / 2, center.y + resolution.y / 2);

        Debug.Log(topLeftCorner);
        foreach(Node node in nodes)
        {
            node.setPosition(topLeftCorner.x + node.getGridX() * interval.x,
                             topLeftCorner.y - node.getGridY() * interval.y, -1);
        }
    }

    public void repack()
    {
        nodes = new List<Node>();
        emptyNodes = new List<GameObject>();
        foreach(Transform go in transform)
        {
            Destroy(go.gameObject);
        }
        
        Vector2 center = Vector2.zero - new Vector2(0, Game.deltaHeight * 30);
        Vector2 resolution = new Vector2(Game.deltaWidth * 350, Game.deltaWidth * 350);
        Vector2 interval = new Vector2(resolution.x / gridSize.x, resolution.y / gridSize.y);
        Vector2 topLeftCorner = new Vector2(center.x - resolution.x / 2, center.y + resolution.y / 2);

        for(int i=0; i<=gridSize.x; i++)
        {
            for(int j=0; j<=gridSize.y; j++)
            {
                GameObject en = spawnEmptyNode(i,j);
                 en.GetComponent<RectTransform>().localPosition = new Vector2(topLeftCorner.x + i * interval.x,
                             topLeftCorner.y - j * interval.y);
            }
        }
    }

    public GameObject spawnEmptyNode(int i, int j)
    {
        GameObject en = Instantiate(emptyNode, transform.position, Quaternion.identity);
        en.GetComponent<NewNodeClick>().grid = this;
        en.transform.SetParent(transform);
        en.GetComponent<NewNodeClick>().gridPos = new Vector2(i, j);
        emptyNodes.Add(en);
        return en;
    }

    public void connectNodes()
    {
        //Foreach Node
        foreach(Node n1 in nodes)
        {
            //Create a new set for the upcoming connections
            HashSet<Node> connections = new HashSet<Node>();

            //Foreach Pair of lines
            foreach(Node n2 in nodes)
            {
                if(!n1.Equals(n2))
                {
                    //Get the implicit equation and set the variables
                    Vector3 equation = getEquation(n1, n2);

                    if(n1.gridPosition.x != n2.gridPosition.x)
                    {
                        NodeAndDistance closestLeft = null;
                        NodeAndDistance closestRight = null;

                        //------------------------------------------//

                        //Get the side of the line n2 is on

                        if(n2.gridPosition.x > n1 .gridPosition.x)
                        {
                            closestRight = new NodeAndDistance(n2, getSquaredDistance(n1, n2));
                        }
                        else
                        {
                            closestLeft = new NodeAndDistance(n2, getSquaredDistance(n1, n2));
                        }

                        //-----------------------------------------//

                        foreach(Node n3 in nodes)
                        {
                            //If it belongs to the line
                            if(!n1.Equals(n3) && !n2.Equals(n3) && isOnLine(n3, equation) && n1.gridPosition.x != n3.gridPosition.x)
                            {
                                int newD = getSquaredDistance(n1, n3);

                                if(closestLeft == null && n3.gridPosition.x < n1.gridPosition.x)
                                {
                                    closestLeft = new NodeAndDistance(n3, newD);
                                }
                                else if(closestRight == null && n3.gridPosition.x > n1.gridPosition.x)
                                {
                                    closestRight = new NodeAndDistance(n3, newD);
                                }
                                else if(closestLeft != null && newD < closestLeft.d && n3.gridPosition.x < n1.gridPosition.x)
                                {
                                    closestLeft = new NodeAndDistance(n3, newD);
                                }
                                else if(closestRight != null && newD < closestRight.d && n3.gridPosition.x > n1.gridPosition.x)
                                {
                                    closestRight = new NodeAndDistance(n3, newD);
                                }
                            }
                        }

                        //-------------------------------------------//

                        //Add the nodes to the hash set

                        if(closestLeft != null)
                            connections.Add(closestLeft.n);
                        if(closestRight != null)
                            connections.Add(closestRight.n);
                    }

                    else
                    {
                        NodeAndDistance up = null;
                        NodeAndDistance down = null;

                        //------------------------------------------//

                        //Get the side of the line n2 is on

                        if(n2.gridPosition.y > n1 .gridPosition.y)
                        {
                            up = new NodeAndDistance(n2, getSquaredDistance(n1, n2));
                        }
                        else
                        {
                            down = new NodeAndDistance(n2, getSquaredDistance(n1, n2));
                        }

                        //-----------------------------------------//

                        foreach(Node n3 in nodes)
                        {
                            //If it belongs to the line
                            if(!n1.Equals(n3) && !n2.Equals(n3) && isOnLine(n3, equation) && n1.gridPosition.x == n3.gridPosition.x)
                            {
                                int newD = getSquaredDistance(n1, n3);

                                if(up == null && n3.gridPosition.y > n1.gridPosition.y)
                                {
                                    up = new NodeAndDistance(n3, newD);
                                }
                                else if(down == null && n3.gridPosition.y < n1.gridPosition.y)
                                {
                                    down = new NodeAndDistance(n3, newD);
                                }
                                else if(up != null && newD < up.d && n3.gridPosition.y > n1.gridPosition.y)
                                {
                                    up = new NodeAndDistance(n3, newD);
                                }
                                else if(down != null && newD < down.d && n3.gridPosition.y < n1.gridPosition.y)
                                {
                                    down = new NodeAndDistance(n3, newD);
                                }
                            }
                        }

                        //-------------------------------------------//

                        //Add the nodes to the hash set

                        if(up != null)
                            connections.Add(up.n);
                        if(down != null)
                            connections.Add(down.n);
                    }
                }
            }

            n1.allNodes = connections;
        }

        foreach(Node n in nodes)
        {
            n.disconnectAll();

            foreach(Node n2 in n.allNodes)
            {
                if(!n.isConnectedTo(n2))
                Node.connect(n, n2);
            }
        }
    }

    public static int getSquaredDistance(Node n1, Node n2)
    {
        return (n1.getGridX() - n2.getGridX()) * (n1.getGridX() - n2.getGridX()) + (n1.getGridY() - n2.getGridY()) * (n1.getGridY() - n2.getGridY());
    }

    public static bool isOnLine(Node n, Vector3 eq)
    {
        float x = n.gridPosition.x;
        float y = n.gridPosition.y;

        if((int)((eq.x * x + eq.y * y + eq.z) * 1000) == 0)
        {
            return true;
        }
        else
            return false;
    }

    public static Vector3 getEquation(Node n1, Node n2)
    {
        if(n1.gridPosition.x - n2.gridPosition.x != 0)
        {
            float m = (n1.gridPosition.y - n2.gridPosition.y) / (n1.gridPosition.x - n2.gridPosition.x);
            float y = n1.gridPosition.y - m * n1.gridPosition.x;
            return new Vector3(-m, 1, -y);
        }
        else
        {
            return new Vector3(1, 0, -n1.gridPosition.x);
        }
    }

    public void toggleHideGraph()
    {
        isHidden = !isHidden;

        foreach(GameObject n in emptyNodes)
        {
            n.GetComponent<SpriteRenderer>().enabled = !isHidden;
        }
    }

    public void removeNode(Node n)
    {
        int x = (int)n.gridPosition.x;
        int y = (int)n.gridPosition.y;

        n.disconnectAll();
        nodes.Remove(n);

        foreach(Node n2 in nodes)
        {
            try{
            n2.allNodes.Remove(n);
            }catch(System.Exception){};
        }

        foreach(GameObject go in emptyNodes)
        {
            NewNodeClick nn = go.GetComponent<NewNodeClick>();

            if(nn.gridPos.x == x && nn.gridPos.y == y)
            {
                go.SetActive(true);
            }
        }

        Destroy(n.gameObject);
    }

    public void shuffle()
    {
        System.Random rnd = new System.Random();

        for(int i=0; i<100; i++)
            foreach(Node n in nodes)
            {
                int a = rnd.Next(0,5);
                if(a < 1)   
                {
                    n.disconnectAll();
                }
            }
    }

    public void export()
    {
        List<Node> nodelist = new List<Node>(nodes);

        using(System.IO.StreamWriter f = new System.IO.StreamWriter("Assets/Levels/lvl" + number + ".lv"))
        {
            Debug.Log(countLines());
            f.WriteLine("" + nodelist.Count + " " + countLines());
            Debug.Log(countLines());
            f.WriteLine("" + nodeSize + " " + lineSize);
            f.WriteLine("" + moves * moves);
            f.WriteLine("" + (int)gridSize.x + " " + (int)gridSize.y);
            f.WriteLine("" + nodes.Count);

            foreach(Node n in nodelist)
            {
                f.WriteLine("" + (int)n.gridPosition.x + " " + (int)n.gridPosition.y);
            }

            foreach(Node n in nodelist)
            {
                string s = "";
                foreach(Node c in n.allNodes)
                {
                    s += "" + (nodelist.IndexOf(c) + 1) + " ";
                }

                if(s.EndsWith(" "))
                    s = s.Remove(s.Length - 1);

                f.WriteLine(s);
            }

            List<NodeConn> ncl = new List<NodeConn>();

            foreach(Node n in nodelist)
            {
                foreach(Node n2 in n.connections)
                {
                    if(!ncl.Contains(new NodeConn(nodelist.IndexOf(n), nodelist.IndexOf(n2))))
                        ncl.Add(new NodeConn(nodelist.IndexOf(n), nodelist.IndexOf(n2)));
                }
            }

            ncl = ncl.Distinct().ToList();

            foreach(NodeConn nc in ncl)
            {
                f.WriteLine("" + (nc.n1 + 1) + " " + (nc.n2 + 1));
            }
        }
    }

    public int countLines()
    {
        int n = 0;
        foreach(Transform t in transform)
        {
            if(t.gameObject.GetComponent<LineRenderer>() != null)
                n++;
        }
        return n;
    }

    public void loadLevel(int number)
    {
        repack();
        System.IO.StreamReader reader = new System.IO.StreamReader("Assets/Levels/lvl" + number + ".lv");
        string[] line = reader.ReadLine().Split(' ');

        float waitTimeNodes = 1 / float.Parse(line[0]);
        float waitTimeLines = 0.5f / float.Parse(line[1]);

        line = reader.ReadLine().Split(' ');

        nodeSize = float.Parse(line[0]);
        lineSize = float.Parse(line[1]);

        moves = int.Parse(reader.ReadLine());

        line = reader.ReadLine().Split(' ');

        gridSize = new Vector2(int.Parse(line[0]), int.Parse(line[1]));

        repack();

        int n = int.Parse(reader.ReadLine());
        
        for(int i=0; i<n; i++)
        {
            line = reader.ReadLine().Split(' ');

            Node node = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity).GetComponent<Node>();
            node.gameObject.transform.SetParent(transform);
            node.transform.localPosition = Vector3.zero;
            node.gridPosition = new Vector2(int.Parse(line[0]), int.Parse(line[1]));
 
            add(node);
            pack();
        }

        foreach(Node node in nodes)
        {
            line = reader.ReadLine().Split(' ');
            HashSet<Node> tmp = new HashSet<Node>();
            foreach(string nb in line)
                tmp.Add(nodes[int.Parse(nb) - 1]);
            node.allNodes = tmp;
        }

        string ln;
        int i1, i2;

        while((ln = reader.ReadLine()) != null)
        {
            line = ln.Split(' ');
            i1 = int.Parse(line[0]) - 1;
            i2 = int.Parse(line[1]) - 1;

            Node.connect(nodes[i1], nodes[i2]);
        }

        pack();
        reader.Close();
    }
}

class NodeAndDistance
{
    public NodeAndDistance(Node n, int d)
    {
        this.n = n;
        this.d = d;
    }
    public Node n;
    public int d;
}