using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ENodeDialogueNormal : ENodeBase
{
    public ENodeBase DestinationNode;

    //constructor
    public ENodeDialogueNormal
        (
        Vector2 position,
        GUIStyle inPointStyle,
        GUIStyle outPointStyle,
        System.Action<ConnectionPoint> OnClickInPoint,
        System.Action<ConnectionPoint> OnClickOutPoint,
        System.Action<ENodeBase> OnClickRemoveNode
        )
    {

        //Node size + style
        rect = new Rect(position.x, position.y, NodeWidth, NodeHight);


        #region Node Actions

        //Create conneciton points
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        //Remove node function
        OnRemoveNode = OnClickRemoveNode;

        #endregion
    }

   

}
