using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public static GameObject testSquare;

    private class Node
    {
        public bool blocked;

        public float g;
        public float h;
        public float F { get { return g + h; } }

        public int x;
        public int y;

        public Node parent;
    }

    public static List<Vector2> FindPathToDestination(Vector2 start, Vector2 destination, int size)
    {
        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        Node startNode = null;
        Node destinationNode = null;

        Vector2 mapOrigin = new Vector2(-14, -10);

        start.x = Mathf.RoundToInt(start.x);
        start.y = Mathf.RoundToInt(start.y);
        destination.x = Mathf.RoundToInt(destination.x);
        destination.y = Mathf.RoundToInt(destination.y);

        // Initialize the grid
        Node[,] grid = new Node[28, 20];

        for (int x = 0; x < grid.GetLength(0) - (size - 1); x++)
        {
            for (int y = 0; y < grid.GetLength(1) - (size - 1); y++)
            {
                grid[x, y] = new Node();
                grid[x, y].blocked = Physics2D.OverlapBox(mapOrigin + new Vector2(x, y), new Vector2(size - 0.05f, size - 0.05f), 0, Utils.layerMaskBlock);

                // We don't worry if the start is blocked
                if (x == start.x - mapOrigin.x && y == start.y - mapOrigin.y)
                {
                    startNode = grid[x, y];
                    grid[x, y].blocked = false;
                }
                // We don't worry if the destination is blocked
                if (x == destination.x - mapOrigin.x && y == destination.y - mapOrigin.y)
                {
                    destinationNode = grid[x, y];
                    grid[x, y].blocked = false;
                }

                if (!grid[x, y].blocked)
                {
                    grid[x, y].x = x;
                    grid[x, y].y = y;
                    grid[x, y].h = Mathf.Pow(destination.x - mapOrigin.x - x, 2) + Mathf.Pow(destination.y - mapOrigin.y - y, 2);
                }
            }
        }

        int safetyButton = 2000;

        Node currentNode = startNode;
        openNodes.Add(currentNode);
        while (openNodes.Count > 0)
        {
            currentNode = GetLowestCostFromList(openNodes);
            if (currentNode == destinationNode)
                break;
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            AddNodesToOpenList(openNodes, closedNodes, grid, currentNode);

            safetyButton--;
            if (safetyButton <= 0)
            {
                Debug.Log("Safety button activated");
                break;
            }
        }
        safetyButton = 2000;

        List<Vector2> path = new List<Vector2>();
        while (currentNode != startNode)
        {
            Vector2 direction = new Vector2(currentNode.x, currentNode.y) - new Vector2(currentNode.parent.x, currentNode.parent.y);
            path.Add(direction);

            currentNode = currentNode.parent;

            safetyButton--;
            if (safetyButton <= 0)
            {
                Debug.Log("Safety button activated");
                break;
            }
        }
        path.Reverse();

        return path;
    }

    private static void AddNodesToOpenList(List<Node> openList, List<Node> closedList, Node[,] grid, Node currentNode)
    {
        Node node;
        if (currentNode.x - 1 >= 0 && currentNode.x - 1 < grid.GetLength(0) &&
            currentNode.y >= 0 && currentNode.y < grid.GetLength(1))
        {
            node = grid[currentNode.x - 1, currentNode.y];
            CheckNewNode(openList, closedList, node, currentNode);
        }
        if (currentNode.x + 1 >= 0 && currentNode.x + 1 < grid.GetLength(0) &&
            currentNode.y >= 0 && currentNode.y < grid.GetLength(1))
        {
            node = grid[currentNode.x + 1, currentNode.y];
            CheckNewNode(openList, closedList, node, currentNode);
        }
        if (currentNode.x >= 0 && currentNode.x < grid.GetLength(0) &&
            currentNode.y - 1 >= 0 && currentNode.y - 1 < grid.GetLength(1))
        {
            node = grid[currentNode.x, currentNode.y - 1];
            CheckNewNode(openList, closedList, node, currentNode);
        }
        if (currentNode.x >= 0 && currentNode.x < grid.GetLength(0) &&
            currentNode.y + 1 >= 0 && currentNode.y + 1 < grid.GetLength(1))
        {
            node = grid[currentNode.x, currentNode.y + 1];
            CheckNewNode(openList, closedList, node, currentNode);
        }
    }

    private static void CheckNewNode(List<Node> openList, List<Node> closedList, Node node, Node currentNode)
    {
        if (!closedList.Contains(node) && !node.blocked)
        {
            if (!openList.Contains(node))
            {
                node.parent = currentNode;
                node.g = currentNode.g + 1;
                openList.Add(node);
            }
            else
            {
                if (node.g > currentNode.g + 1)
                {
                    node.parent = currentNode;
                    node.g = currentNode.g + 1;
                }
            }
        }
    }

    private static Node GetLowestCostFromList(List<Node> list)
    {
        Node lowestCostNode = list[0];
        foreach (Node node in list)
        {
            if (node.F < lowestCostNode.F)
                lowestCostNode = node;
        }

        return lowestCostNode;
    }
}
