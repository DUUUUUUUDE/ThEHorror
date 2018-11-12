using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENodeDialogueOptions : ENodeBase
{
    //Connection POints
    public ConnectionPoint outPoint01;
    public ConnectionPoint outPoint02;

    //Visuals
    public float OptionLabelHight = 40;


    public string OptionText01;
    public string OptionText02;
    public string OptionText03;

    public ENodeDialogueOptions
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
        outPoint01 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        outPoint02 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        //Remove node function
        OnRemoveNode = OnClickRemoveNode;

        #endregion
    }

    public override void OnGUI()
    {
        NodeHight = 0;
        Rect DialogueTextRect = RectTextArea();
        Rect OptionLabelRect = GetLabelRect();

        NodeHight += VerticalOffset;
        rect.height = NodeHight;

        DrawNodeHolder();

        DialogueText = GUI.TextArea(DialogueTextRect, DialogueText,);

        GUI.Box(OptionLabelRect, "Option 1");

        OptionText01 = GUI.TextField(,);

    }

    protected Rect GetLabelRect ()
    {
        Rect ret = new Rect
            (
            rect.x + HorizontalOffset,
            rect.y + NodeHight + VerticalOffset,
            rect.size.x - HorizontalOffset * 2,
            OptionLabelHight
            );

        NodeHight += OptionLabelHight + VerticalOffset;


        return ret;
    }

    protected override void DrawNodeHolder()
    {
        inPoint.Place();
        GUI.Box(rect, Title);
    }
}
