using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class DialogueTreeEditor : EditorWindow
 {

    private float menuBarHeight = 20f;
    private Rect menuBar;


    //List of nodes in window
    private List<ENodeBase> Nodes = new List<ENodeBase>();
    //List of connections in window
    private List<Connection> connections;

    //Styles
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    //connection points
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    //Drag
    private Vector2 drag;

    //Grid
    private Vector2 offset;

    //OPEN
    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        DialogueTreeEditor window = GetWindow<DialogueTreeEditor>();
        window.titleContent = new GUIContent("Dialogue Editor");
    }

    //ENALBE
    private void OnEnable()
    {
        SetUpConnectionStyle();
    }

    //GUI
    private void OnGUI()
    {
        //Grid
        DrawGrid(25, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        //Menu
        DrawMenuBar();

        //Draw Nodes and connections
        DrawNodes();
        DrawConnections();
        DrawConnectionLine(Event.current);

        //Move Nodes // create nodes
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    //right click menu
    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Normal Node"), false, () => OnClickAddNormalDialogueNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Option Node"), false, () => OnClickAddOptionDialogueNode(mousePosition));

        genericMenu.ShowAsContext();
    }

    //create node
    private void OnClickAddNormalDialogueNode(Vector2 mousePosition)
    {
        if (Nodes == null)
        {
            Nodes = new List<ENodeBase>();
        }

        Nodes.Add(new ENodeDialogueNormal
            (
            mousePosition, 
            inPointStyle, 
            outPointStyle, 
            OnClickInPoint, 
            OnClickOutPoint, 
            OnClickRemoveNode
            ));
    }

    private void OnClickAddOptionDialogueNode(Vector2 mousePosition)
    {
        if (Nodes == null)
        {
            Nodes = new List<ENodeBase>();
        }

        Nodes.Add(
            new ENodeDialogueOptions
            (
            mousePosition, 
            inPointStyle, 
            outPointStyle, 
            OnClickInPoint,
            OnClickOutPoint, 
            OnClickRemoveNode, 
            RemoveOptionConnections
            ));
    }

    #region NODES

    //setup connections style
    private void SetUpConnectionStyle()
    {
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    //Remove Node
    private void OnClickRemoveNode(ENodeBase node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        Nodes.Remove(node);
    }
    //Remove option connections
    protected void RemoveOptionConnections(ConnectionPoint point)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].outPoint == point)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }
        }
    }

    //Draw nodes
    private void DrawNodes()
    {
        if (Nodes != null)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].OnGUI ();
            }
        }
    }
    
    //Draw Cnonections
    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    //Have the nodes Move?  Gui.Changed = true (redraw)
    private void ProcessNodeEvents(Event e)
    {
        if (Nodes != null)
        {
            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = Nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }
    #endregion


    #region INPUT

    //player input
    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    //Drag Window
    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (Nodes != null)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }


    #region Connections

    //Draw connection vefore conection happens
    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    // click IN connection point
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.ParentNode != selectedInPoint.ParentNode)
            {
                if (selectedOutPoint.ParentNode is ENodeDialogueNormal)
                {
                    ((ENodeDialogueNormal)selectedOutPoint.ParentNode).DestinationNode = selectedInPoint.ParentNode;
                }

                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    // click OUT connection point
    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.ParentNode != selectedInPoint.ParentNode)
            {
                if (selectedOutPoint.ParentNode is ENodeDialogueNormal)
                {
                    ((ENodeDialogueNormal)selectedOutPoint.ParentNode).DestinationNode = selectedInPoint.ParentNode;
                }

                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    // click Remove connection
    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    // In & Out selected
    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    // clear connection points
    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    #endregion

    #endregion


    //Window Grid
    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }


    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            Save();
        }
        GUILayout.Space(5);
        if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            Load();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void Save()
    {
        XMLOp.Serialize(Nodes, "Assets/Resources/nodes.xml");
        XMLOp.Serialize(connections, "Assets/Resources/connections.xml");
    }

    private void Load()
    {
        var nodesDeserialized = XMLOp.Deserialize<List<ENodeBase>>("Assets/Resources/nodes.xml");
        var connectionsDeserialized = XMLOp.Deserialize<List<Connection>>("Assets/Resources/connections.xml");

        Nodes = new List<ENodeBase>();
        connections = new List<Connection>();

        foreach (var node in nodesDeserialized)
        {
            if (node is ENodeDialogueOptions)
            {
                Nodes.Add(new ENodeDialogueOptions(
                    node.rect.position,
                    inPointStyle,
                    outPointStyle,
                    OnClickInPoint,
                    OnClickOutPoint,
                    OnClickRemoveNode,
                    RemoveOptionConnections,
                    node.inPoint.id,
                    node.outPoint.id,
                    ((ENodeDialogueOptions)node).outPoint01.id,
                    ((ENodeDialogueOptions)node).outPoint02.id
                    )
                );
            }else if (node is ENodeDialogueNormal)
            {
                Nodes.Add(new ENodeDialogueNormal(
                    node.rect.position,
                    inPointStyle,
                    outPointStyle,
                    OnClickInPoint,
                    OnClickOutPoint,
                    OnClickRemoveNode,
                    node.inPoint.id,
                    node.outPoint.id
                    )
                );
            }
        }

        foreach (var connectionDeserialized in connectionsDeserialized)
        {
            var inPoint = Nodes.First(n => n.inPoint.id == connectionDeserialized.inPoint.id).inPoint;
            var outPoint = Nodes.First(n => n.outPoint.id == connectionDeserialized.outPoint.id).outPoint;
            connections.Add(new Connection(inPoint, outPoint, OnClickRemoveConnection));
        }
    }
}

public class XMLOp
{
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }

    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
}
