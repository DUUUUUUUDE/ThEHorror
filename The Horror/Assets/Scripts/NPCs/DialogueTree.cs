using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueTree
{
    public class Dialogue
    {
        public List<DialogueNode> Nodes;

        public Dialogue()
        {
            Nodes = new List<DialogueNode>();
        }

        public void AddNode (DialogueNode node)
        {
            //if null it's an ExitNode. We Skip
            if (node == null) return;

            //Add node and Set ID
            Nodes.Add(node);
            node.NodeID = Nodes.IndexOf(node);
        }

        public void AddOption (string text, DialogueNode node, DialogueNode dest)
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

    }

    public class DialogueNode
    {

        public int NodeID = -1;

        public string Text;

        public List <DialogueOption> Options;

        public DialogueNode ()
        {
            Options = new List<DialogueOption>();
        }

        public DialogueNode(string text)
        {
            Text = text;
            Options = new List<DialogueOption>(); 
        }
    }

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
