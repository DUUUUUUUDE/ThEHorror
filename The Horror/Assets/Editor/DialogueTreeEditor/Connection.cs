using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public System.Action<Connection> OnClickRemoveConnection;

    //Constructor
    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, System.Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    //Draw
    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
        );

        //Make connection
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
    }
}
