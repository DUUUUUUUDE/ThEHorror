using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{

    public string id;

    //IN/OUT connection
    [XmlIgnore] public ConnectionPointType Type;

    //Visuals
    [XmlIgnore] public Rect rect;
    [XmlIgnore] public GUIStyle Style;

    //Parent Node
    [XmlIgnore] public ENodeBase ParentNode;

    //Point Action
    [XmlIgnore] public Action<ConnectionPoint> OnClickConnectionPoint;

    //Constructor
    public ConnectionPoint () { }
    public ConnectionPoint
        (
        ENodeBase node, 
        ConnectionPointType type, 
        GUIStyle style, 
        Action <ConnectionPoint> onClickConnectionPoint,
        string id = null
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

        this.id = id ?? Guid.NewGuid().ToString();
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
