using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{
    //IN/OUT connection
    public ConnectionPointType Type;

    //Visuals
    public Rect rect;
    public GUIStyle Style;
    
    //Parent Node
    public ENodeBase ParentNode;

    //Point Action
    public System.Action<ConnectionPoint> OnClickConnectionPoint;

    //Constructor
    public ConnectionPoint
        (
        ENodeBase node, 
        ConnectionPointType type, 
        GUIStyle style, 
        System.Action <ConnectionPoint> onClickConnectionPoint
        )
    {
        //connection type
        Type = type;

        //visuals
        rect = new Rect(0, 0, 10f, 20f);
        Style = style;

        //parent node
        ParentNode = node;

        //Point action
        OnClickConnectionPoint = onClickConnectionPoint;
    }

    //Place connection point
    public void Place(Rect buttonRect)
    {
        //node position Y
        rect.y = buttonRect.y + (buttonRect.height * 0.5f) - rect.height * 0.5f;

        //node position X
        switch (Type)
        {
            case ConnectionPointType.In:
                rect.x = buttonRect.x - rect.width;
                break;

            case ConnectionPointType.Out:
                rect.x = buttonRect.x + buttonRect.width;
                break;
        }

        //Click visual effect
        if (GUI.Button(rect, "", Style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
