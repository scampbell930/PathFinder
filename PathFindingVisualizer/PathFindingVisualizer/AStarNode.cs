using System;
using System.Collections.Generic;
using System.Text;

namespace PathFindingVisualizer
{
    class AStarNode
    {
        // Data fields stored in every node
        private double heuristic;
        private double movementCost;
        private double totalCost;
        private int[] location = new int[2];            // Node location on map
        private AStarNode parent;                       // Node that comes prior in possible path
        private bool illegal;

        // Node constructor
        public AStarNode(bool illegal, int[] location)
        {
            this.location = location;
            this.Illegal = illegal;
        }

        // Properties
        public double Heuristic { get => heuristic; set => heuristic = value; }
        public double MovementCost { get => movementCost; set => movementCost = value; }
        public double TotalCost { get => totalCost; set => totalCost = value; }
        public int[] Location { get => location; set => location = value; }
        public bool Illegal { get => illegal; set => illegal = value; }
        public AStarNode Parent { get => parent; set => parent = value; }
    }
}
