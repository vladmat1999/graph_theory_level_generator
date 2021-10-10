using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConn
{
    public int n1;
    public int n2;
    public NodeConn(int n1, int n2)
    {
        this.n1 = n1;
        this.n2 = n2;
    }

    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        try
        {
            NodeConn c = (NodeConn) obj;
            if(c.n1 == n1 && c.n2 == n2)
                return true;
            else if(c.n2 == n1 && c.n1 == n2)
                return true;
            return false;
        }catch(System.Exception)
        {
            return false;
        }
    }
}
