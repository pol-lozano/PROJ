using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour {

    [SerializeField] private LineRenderer lineRendererPrefab;

    private int width;
    private int height;
    
    private List<Node> closestNodes = new List<Node>();
    private Stack<LineObject> lineRenderers = new Stack<LineObject>();
    private Node currentNode;

    private string solution;

    //Needs method for clearing the puzzle. FOR WINNERS AND FOR LOSERS

    public string GetSolution() { return solution; }
    
    
    private void Awake() {

        StartGrid();
        
    }

    private void StartGrid()
    {
        List<Node> allNodes = new List<Node>();


        allNodes.AddRange(transform.GetComponentsInChildren<Node>());

        Debug.Log(allNodes.Count);
        Node startNode = currentNode = FindStartNode(ref allNodes);

        width = CalculateWidth(ref allNodes);
        height = allNodes.Count / width;

        foreach (Node node in allNodes)
            node.gameObject.SetActive(false);

        startNode.gameObject.SetActive(true);


        //AddSelectedNode(startNode);
    }

    private Node FindStartNode(ref List<Node> nodes) {
        foreach(Node node in nodes)
            if (node.startNode)
            {
                node.OnNodeSelected += AddSelectedNode;
                return node;
            }
                

        
        Debug.LogError("No start node selected");
        return null;
    }
    
    private void AddSelectedNode(Node node) 
    {
        LineObject newLine = new LineObject(node);
        
        if (lineRenderers.Count > 0 && lineRenderers.Peek().CompareLastLine(newLine))
        {
            //Checks if this was the last line that was drawn, if so delete that line (eraser)
            LineObject oldLine = lineRenderers.Pop();
            foreach (Node n in currentNode.neighbours.Keys)
            {
                n.gameObject.SetActive(false);
            }

            //REMOVE LAST CHAR IN SOLUTION OR CALCULATE EVERYTHING AFTERWARDS
            node.RemoveLineToNode(currentNode);
            currentNode.RemoveLineToNode(node);
            currentNode = oldLine.originNode;
            Destroy(oldLine.line);
            return;
        }

        else
        {
            if (lineRenderers.Count > 1)
            {
                //Checks if there exists a line between these nodes already, if so it destroys the line that was created
                if (node.HasLineToNode(currentNode))
                {
                    Debug.Log("This line already exists");
                    return;
                }
            }

            LineRenderer newLineRenderer = Instantiate(lineRendererPrefab, transform);
            newLineRenderer.SetPosition(0, currentNode.transform.position);
            newLineRenderer.SetPosition(1, node.transform.position);
            LineObject line = new LineObject(currentNode, newLineRenderer);

            //ADD LINE
            node.AddLineToNode(currentNode);
            currentNode.AddLineToNode(node);
            lineRenderers.Push(line);
        }

        //This is the input in a comparable string. This needs to connect to the puzzles solution
        if (lineRenderers.Count > 1) 
            solution += PuzzleHelper.TranslateInput(node, currentNode); 

        currentNode = node;
        ActivateNode(node);
    }

    private int CalculateWidth(ref List<Node> nodes) {
        
        float currentHeight = nodes[0].transform.position.y;
        
        int width = 0;
        int index = 0;

        while (nodes[index++].transform.position.y <= currentHeight)
            width++;

        return width;
    }

    private void ActivateNode(Node node) {
        node.gameObject.SetActive(true);

        if (closestNodes.Count > 0) {
            foreach(Node closestNode in closestNodes)
                closestNode.ClearSelectable();
        }
        
        foreach(Node n in node.neighbours.Keys)
        {
            closestNodes.Add(n);
        }
        
        foreach (Node neighbour in node.neighbours.Keys) {
            neighbour.gameObject.SetActive(true);
            neighbour.OnNodeSelected += AddSelectedNode;
        } 
    }

}

public class LineObject
{
    //Object that can compare lines between nodes, stored in a stack in the grid
    public Node originNode;
    public LineRenderer line;

    public LineObject(Node a, LineRenderer lineRen)
    {
        originNode = a;
        line = lineRen;
    }
    public LineObject(Node a)
    {
        originNode = a;
    }
    public LineObject(LineRenderer lineRen)
    {
        line = lineRen;
    }
    public bool CompareLastLine(LineObject other)
    {
        return originNode == other.originNode;
    }
    public bool CheckIfLineExists(LineObject other)
    {
        //THIS IS NOT ACCURATE ENOUGH compare the nodes instead!
        /*
        List<Vector3> thisPos = new List<Vector3>();
        for(int i = 0; i < 2; i++)
        {
            thisPos.Add(line.GetPosition(i));
        }

        List<Vector3> otherPos = new List<Vector3>();
        for (int i = 0; i < 2; i++)
        {
            otherPos.Add(other.line.GetPosition(i));
        }

        if(thisPos.Contains(otherPos[0]) && thisPos.Contains(otherPos[1]))
        {
            return true;
        }
        */






        return false;
    }

}