using System;
using System.Collections.Generic;
using System.Text;

namespace PathFindingVisualizer
{
    class AStarAlgorithim
    {

        private AStarNode[,] map = new AStarNode[10, 10];               // Map of all nodes to calculate path in
        private List<AStarNode> openSet = new List<AStarNode>();     // List of nodes to be checked
        private List<AStarNode> closedSet = new List<AStarNode>();     // List of checked nodes
        private AStarNode startNode;                                    // Node where path will start
        private AStarNode endNode;                                      // Node where path will end
        private AStarNode currentNode;                                  // Node currently being looked at
        private bool pathFound = false;
        private int algorithmCount = 0;

        public AStarNode[,] Map { get => map; set => map = value; }
        public List<AStarNode> OpenSet { get => openSet; set => openSet = value; }
        public List<AStarNode> ClosedSet { get => closedSet; set => closedSet = value; }
        public AStarNode StartNode { get => startNode; set => startNode = value; }
        public AStarNode EndNode { get => endNode; set => endNode = value; }
        public AStarNode CurrentNode { get => currentNode; set => currentNode = value; }

        public AStarAlgorithim(AStarNode startNode, AStarNode endNode, List<AStarNode> walls)
        {
            // Initialize map CURRENTLY SETTING EVERY NODE TO LEGAL ------ NEED TO ADD ADDITIONAL FUNCTIONALLITY LATER
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int[] location = { i, j };
                    foreach (AStarNode node in walls)
                    {
                        if (node.Location[0] == i && node.Location[1] == j)
                        {
                            Map[i, j] = node;
                            break;
                        }
                    }
                    Map[i, j] = new AStarNode(true, location);
                }
            }

            this.StartNode = startNode;
            this.EndNode = endNode;

            map[startNode.Location[0], startNode.Location[1]] = startNode;
            map[endNode.Location[0], endNode.Location[1]] = endNode;
        }

        public void Run()
        {
            while (!pathFound)
            {
                // Check if algorithm has started yet
                if (algorithmCount == 0)
                {
                    // Set current node to start node
                    currentNode = startNode;

                    // Start algorithm at initial step
                    StartAlgorithm();
                }
                else
                {
                    // Continue default algorithm
                    Algorithm();
                }

                // Increment algorithm count every step
                algorithmCount++;
            }

        }

        /// <summary>
        /// Starts the A* algorithm from the start node
        /// </summary>
        /// <param name="node"></param>
        private void StartAlgorithm()
        {
            // Start by setting node's total movement cost to 0 and adding start node to open set
            currentNode.TotalCost = 0;
            openSet.Add(currentNode);
        }

        private void Algorithm()
        {
            // Check if no path can be found
            if (openSet.Count == 0)
            {
                pathFound = true;
                return;
            }

            // Initialize new current node by removing node with smallest total cost from open set
            currentNode = PathNode();

            // Initialize currentNode neighbors
            List<AStarNode> neighbors = InitializeNeighbors(currentNode);

            // Initialize neighbors parent node and add them to the open list
            foreach (AStarNode node in neighbors)
            {
                node.Parent = currentNode;
                CalculateTotalMovement(node);
                openSet.Add(node);
            }

            // Add current node to closed set and remove from open set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Check if endNode has been reached
            if (closedSet.Contains(endNode))
            {
                pathFound = true;
                Console.WriteLine("Reached Goal!!!!!!!!!!!!");
                return;
            }
        }

        /// <summary>
        /// Determines which node in the open set has the smallest total movement cost.
        /// Returns node
        /// </summary>
        /// <returns></returns>
        private AStarNode PathNode()
        {
            // Initialize return node
            AStarNode result = null;
            double bigCost = 10000;

            // Loop through open set and determine which node has the smallest total cost
            foreach (AStarNode node in openSet)
            {
                if (node.TotalCost < bigCost)
                {
                    result = node;
                    bigCost = node.TotalCost;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates and returns the nodes surrounding the current node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<AStarNode> InitializeNeighbors(AStarNode node)
        {
            // Return List
            List<AStarNode> neighbors = new List<AStarNode>();

            // Loop around current node to inialize neighbors
            for (int row = node.Location[0] - 1; row <= node.Location[0] + 1; row++)
            {
                for (int col = node.Location[1] - 1; col <= node.Location[1] + 1; col++)
                {
                    // Initialize current neighbor
                    if ((row < 0 || row >= 10) || (col < 0 || col >= 10))
                    {
                        continue;
                    }

                    AStarNode neighbor = map[row, col];

                    // Check if node is at a diagonal as I am currently not allowing this
                    if ((row == node.Location[0] - 1 && col == node.Location[1] - 1) ||
                        (row == node.Location[0] + 1 && col == node.Location[1] - 1) ||
                        (row == node.Location[0] - 1 && col == node.Location[1] + 1) ||
                        (row == node.Location[0] + 1 && col == node.Location[1] + 1))
                    {
                        continue;
                    }

                    // Check if neighbor is a legal node
                    if (neighbor.Illegal)
                    {
                        continue;
                    }

                    // Check if neighbor has already been initialized
                    if (openSet.Contains(neighbor) || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // Node is a valid neighbor
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;

        }

        /// <summary>
        /// Calculates the total movement cost to move from parent node to the node passed as the argument
        /// </summary>
        /// <param name="node"></param>
        private void CalculateTotalMovement(AStarNode node)
        {
            // Initializing initial movement cost to 10 as currently only supporting horizontal and vertical movement
            node.MovementCost = 10;

            // Calculate heuristic cost
            node.Heuristic = (Math.Abs(node.Location[0] - endNode.Location[0])
                             + Math.Abs(node.Location[1] - endNode.Location[1]))
                             * 10;

            // Calculate total
            node.TotalCost = node.MovementCost + node.Heuristic;
        }

        /// <summary>
        /// Checks if a node location is contained within the closed list for form display purposes
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public bool closedSetContains(int i, int j)
        {
            foreach (AStarNode node in closedSet)
            {
                if (node.Location[0] == i && node.Location[1] == j)
                {
                    return true;
                }
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////
        /// PRINTING METHOD FOR CONSOLE TESTING
        public void PrintMap()
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                Console.Write('|');
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == startNode)
                    {
                        Console.Write("S" + "|");
                    }
                    else if (map[row, col] == endNode)
                    {
                        Console.Write("E" + "|");
                    }
                    else if (closedSet.Contains(map[row, col]))
                    {
                        Console.Write("C" + "|");
                    }
                    else
                    {
                        Console.Write(" |");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
