﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class ENodeBase
{

    //DialogueTree nodes
    public string DialogueText;


    //Visuals
    public Rect rect;
    public string Title = "Base Node";

    public float NodeWidth = 250;
    public float NodeHight;
    public float VerticalOffset = 10;
    public float HorizontalOffset = 10;
    public float TopExtraOffset = 10;

    public float TextAreaHeight = 100;


    //Player Input
    public bool isDraggable;

    //Connections
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    //NodeActions
    public System.Action<ENodeBase> OnRemoveNode;

    //GUI
    public virtual void OnGUI()
    {
        NodeHight = 0;
        Rect DialogueTextRect = RectTextArea();

        NodeHight += VerticalOffset;
        rect.height = NodeHight;

        DrawNodeHolder();

        DialogueText = GUI.TextArea(DialogueTextRect, DialogueText);

    }

    //Draw Node
    protected virtual void DrawNodeHolder()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, Title);
    }

    //Give DialogueText RECT
    protected virtual Rect RectTextArea ()
    {
        NodeHight += TextAreaHeight + VerticalOffset + TopExtraOffset;

        return new Rect
            (
            rect.x + HorizontalOffset,
            rect.y + VerticalOffset + TopExtraOffset,
            rect.size.x - HorizontalOffset * 2,
            TextAreaHeight
            );
    }
    


    
    #region OTHER STUFF

    //Node internal RightCLick menu
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    //Move Node
    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    //Remove Node
    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }

    //PlayerInput if true redraw editor window
    public bool ProcessEvents(Event e)
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
                        isDraggable = true;
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

    #endregion


}
