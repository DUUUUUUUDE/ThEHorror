using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueTree
{

    public class Dialogue
    {
        public List<DialogueNode> Nodes;

        public Dialogue()
        {
            Nodes = new List<DialogueNode>();
        }

        #region Constructors

        //Add node to Dialogue
        public void AddNode (DialogueNode node)
        {
            //if null it's an ExitNode. We Skip
            if (node == null) return;

            //Add node and Set ID
            Nodes.Add(node);
            node.NodeID = Nodes.IndexOf(node);
        }

        //Add option to node (After all nodes are created)
        public void AddOption (string text, DialogueOptionNode node, DialogueNode dest)
        {
            //If dest or node aren't in the NodeList Add them
            if (!Nodes.Contains(node))
                AddNode(node);
            if (!Nodes.Contains(dest))
                AddNode(dest);

            DialogueOption option;

            //Create Option object, if destination is null is an ExitNode (set destID to -1)
            if (dest == null)
                option = new DialogueOption(text, -1);
            else
                option = new DialogueOption(text, dest.NodeID);

            node.Options.Add(option);
        }
        #endregion

    }

    #region DIALOGUE NODES

    //BASE
    public class DialogueNode
    {

        public int NodeID = -1;

        public string Text;

    }

    //NORMAL
    public class DialogueNormalNode : DialogueNode
    {

        public int DestionationNodeID;

        public DialogueNormalNode ()
        {

        }

        public DialogueNormalNode (string text, int dest)
        {
            Text = text;
            DestionationNodeID = dest;
        }

    }

    //OPTIONS
    public class DialogueOptionNode : DialogueNode
    {

        public int Affection;

        public List<DialogueOption> Options;

        public DialogueOptionNode()
        {
            Options = new List<DialogueOption>();
        }

        public DialogueOptionNode(string text, int affection)
        {
            Text = text;
            Affection = affection;
            Options = new List<DialogueOption>();
        }

    }

    //EVENT
    public class DialogueEventNode : DialogueNode
    {

        public int DestionationNodeID;

        public UnityEvent NodeEvent;

        public DialogueEventNode ()
        {

        }

        public DialogueEventNode (string text, int dest, UnityEvent actions)
        {
            Text = text;
            DestionationNodeID = dest;
            NodeEvent = actions;
        }

        public void ExecuteEvent ()
        {
            NodeEvent.Invoke();
        }

    }

    #endregion

    public class DialogueOption
    {

        public int DestinationNodeID;

        public string Text;

        public DialogueOption ()
        {

        }

        public DialogueOption (string text, int dest)
        {
            Text = text;
            DestinationNodeID = dest;
        }
    }

}