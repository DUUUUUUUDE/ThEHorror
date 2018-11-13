using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

[XmlInclude(typeof(ENodeDialogueNormal))]
[XmlInclude(typeof(ENodeDialogueOptions))]
public abstract class ENodeBase
{

    //DialogueTree nodes
    public string DialogueText;

    //Visuals
    public Rect rect;
    [XmlIgnore] public string Title = "Base Node";

    [XmlIgnore] public float NodeWidth = 200;
    public float NodeHight;
    [XmlIgnore] public float VerticalOffset = 10;
    [XmlIgnore] public float HorizontalOffset = 10;
    [XmlIgnore] public float BoxTitleOffset = 10;

    [XmlIgnore] public float TextAreaHeight = 100;


    //Player Input
    [XmlIgnore] public bool isDraggable;

    //Connections
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    //NodeActions
    [XmlIgnore] public System.Action<ENodeBase> OnRemoveNode;

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
        inPoint.Place(rect);
        outPoint.Place(rect);
        GUI.Box(rect, Title);
    }

    //Give DialogueText RECT
    protected virtual Rect RectTextArea ()
    {
        NodeHight += TextAreaHeight + VerticalOffset * 2 + BoxTitleOffset;

        return new Rect
            (
            rect.x + HorizontalOffset,
            rect.y + VerticalOffset + BoxTitleOffset,
            rect.size.x - HorizontalOffset * 2,
            TextAreaHeight
            );
    }
    


    
    #region OTHER STUFF

    //Node internal RightCLick menu
    protected void ProcessContextMenu()
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
    public virtual bool ProcessEvents(Event e)
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
