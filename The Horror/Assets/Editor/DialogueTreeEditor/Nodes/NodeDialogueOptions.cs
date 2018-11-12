using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENodeDialogueOptions : ENodeBase
{
    //Connection POints
    public ConnectionPoint outPoint01;
    public ConnectionPoint outPoint02;

    //Visuals
    public float OptionBoxHight = 60;
    public float OptionButtonHight = 20;

    public string OptionText01;
    public string OptionText02;
    public string OptionText03;

    Rect OptionBox01Rect;
    Rect OptionText01Rect;
    Rect OptionBox02Rect;
    Rect OptionText02Rect;
    Rect OptionBox03Rect;
    Rect OptionText03Rect;

    //options buttons
    public Rect AddOptionRect;
    public Rect EraseOptionRect;
    System.Action<ConnectionPoint> eraseConnection;

    int options = 1;

    public ENodeDialogueOptions
        (
        Vector2 position,
        GUIStyle inPointStyle,
        GUIStyle outPointStyle,
        System.Action<ConnectionPoint> OnClickInPoint,
        System.Action<ConnectionPoint> OnClickOutPoint,
        System.Action<ENodeBase> OnClickRemoveNode,
        System.Action<ConnectionPoint> RemoveConnection
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
        eraseConnection = RemoveConnection;

        #endregion
    }

    public override void OnGUI()
    {
        NodeHight = 0;
        Rect DialogueTextRect = RectTextArea();

        AddOptionRect = AddOptionButton();
        EraseOptionRect = AddOptionButton();

        //Option1 Rects
        if (options > 0)
        {
            OptionBox01Rect = GetOptionBoxRect();
            OptionText01Rect = GetOptionTextRect();
        }

        if (options > 1)
        {
            OptionBox02Rect = GetOptionBoxRect();
            OptionText02Rect = GetOptionTextRect();
        }

        if (options > 2)
        {
            OptionBox03Rect = GetOptionBoxRect();
            OptionText03Rect = GetOptionTextRect();
        }

        NodeHight += VerticalOffset;
        rect.height = NodeHight;

        DrawNodeHolder();

        DialogueText = GUI.TextArea(DialogueTextRect, DialogueText);

        //BUttons
        GUI.Box(AddOptionRect,"+");
        GUI.Box(EraseOptionRect, "-");

        if (options > 0)
        {
            //Option1
            GUI.Box(OptionBox01Rect, "Option 1");
            OptionText01 = GUI.TextArea(OptionText01Rect, OptionText01);
            outPoint.Place(OptionBox01Rect);
        }

        if (options > 1)
        {
            //Option2
            GUI.Box(OptionBox02Rect, "Option 2");
            OptionText02 = GUI.TextArea(OptionText02Rect, OptionText02);
            outPoint01.Place(OptionBox02Rect);
        }

        if (options > 2)
        {
            //Option2
            GUI.Box(OptionBox03Rect, "Option 3");
            OptionText03 = GUI.TextArea(OptionText03Rect, OptionText03);
            outPoint02.Place(OptionBox03Rect);
        }

    }

    protected Rect AddOptionButton()
    {

        Rect ret = new Rect
            (
            rect.x + HorizontalOffset,
            rect.y + NodeHight + VerticalOffset,
            rect.size.x - HorizontalOffset * 2 ,
            OptionButtonHight
            );

        NodeHight += OptionButtonHight + VerticalOffset / 2;

        return ret;
    }

    protected Rect GetOptionBoxRect ()
    {
        Rect ret = new Rect
            (
            rect.x + HorizontalOffset,
            rect.y + NodeHight + VerticalOffset,
            rect.size.x - HorizontalOffset * 2,
            OptionBoxHight
            );

        NodeHight += OptionBoxHight + VerticalOffset;

        return ret;
    }

    protected Rect GetOptionTextRect ()
    {
        Rect ret = new Rect
           (
           rect.x + HorizontalOffset * 2,
           rect.y + NodeHight + VerticalOffset + BoxTitleOffset - OptionBoxHight,
           rect.size.x - HorizontalOffset * 4,
           OptionBoxHight - HorizontalOffset * 2 - BoxTitleOffset
           );

        return ret;
    }

    protected override void DrawNodeHolder()
    {
        inPoint.Place(rect);
        GUI.Box(rect, Title);
    }

    


    //PlayerInput if true redraw editor window
    public override bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:

                //Left click
                if (e.button == 0)
                {
                    //Select
                    if (rect.Contains(e.mousePosition))
                    {
                        if (AddOptionRect.Contains(e.mousePosition))
                        {
                            if (options < 3)
                                options++;

                        }
                        else if (EraseOptionRect.Contains(e.mousePosition))
                        {
                            if (options > 1)
                                options--;

                            if (options < 2)
                                eraseConnection(outPoint01);

                                eraseConnection(outPoint02);
                        }
                        else
                        {
                            isDraggable = true;
                        }
                        GUI.changed = true;
                    }
                    //Fail to select
                    else
                    {
                        GUI.changed = true;
                    }
                }

                //Right click
                if (e.button == 1 && rect.Contains(e.mousePosition))
                {
                    //Open Node Menu
                    ProcessContextMenu();
                    e.Use();
                }

                break;

            case EventType.MouseUp:
                //Deselect Node
                isDraggable = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDraggable)
                {
                    //Move Node
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

}
