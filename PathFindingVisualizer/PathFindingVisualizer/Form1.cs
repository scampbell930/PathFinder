using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFindingVisualizer
{
    public partial class Form1 : Form
    {
        AStarNode startNode;
        AStarNode endNode;
        AStarAlgorithim aStar;
        Button[,] formMap = new Button[10, 10];
        CheckBox setStart = new CheckBox();
        CheckBox setEnd = new CheckBox();
        CheckBox setWall = new CheckBox();
        int numOfStart;
        int numOfEnd;

        public Form1()
        {
            InitializeComponent();
            LoadButtons();
        }

        public void LoadButtons()
        {

            numOfStart = 0;
            numOfEnd = 0;

            // Fill board with cells
            for (int i = 0; i < formMap.GetLength(0); i++)
            {
                for (int j = 0; j < formMap.GetLength(1); j++)
                {
                    Button temp = new Button();
                    temp.BackColor = Color.White;
                    temp.Size = new Size(32, 32);
                    temp.Location = new Point(i * temp.Size.Width, j * temp.Size.Height);

                    temp.Click += NodeClickHandler;
                    formMap[i, j] = temp;
                    this.Controls.Add(formMap[i, j]);
                }
            }

            // Initialize button appearance and location
            setStart.Appearance = Appearance.Button;
            setStart.BackColor = Color.Green;
            setEnd.Appearance = Appearance.Button;
            setEnd.BackColor = Color.Red;

            setWall.Appearance = Appearance.Button;
            setWall.BackColor = Color.Gray;

            setStart.Size = new Size(100, 30);
            setEnd.Size = new Size(100, 30);
            setStart.Location = new Point(650, 100);
            setEnd.Location = new Point(650, 150);

            setWall.Size = new Size(100, 30);
            setWall.Location = new Point(650, 200);

            setStart.Text = "Set Start Node";
            setEnd.Text = "Set Goal Node";
            setWall.Text = "Set Wall Nodes";

            setStart.CheckedChanged += StartNodeHandler;
            setEnd.CheckedChanged += EndNodeHandler;
            setWall.CheckedChanged += WallNodeHandler;

            // Add button controls
            this.Controls.Add(setStart);
            this.Controls.Add(setEnd);
            this.Controls.Add(setWall);

        }

        /// <summary>
        /// Loop through closed set to trace path
        /// </summary>
        private void TracePath()
        {
            foreach (AStarNode node in aStar.ClosedSet)
            {
                formMap[node.Location[0], node.Location[1]].BackColor = Color.Blue;
            }
            formMap[startNode.Location[0], startNode.Location[1]].BackColor = Color.Green;
            formMap[endNode.Location[0], endNode.Location[1]].BackColor = Color.Red;
        }

        /// <summary>
        /// Initialize start, end and wall nodes
        /// </summary>
        private void LoadAStar()
        {
            // Initialize wall list
            List<AStarNode> walls = new List<AStarNode>();

            // Loop through form map to determine which locations have been initialized to the start and end node
            for (int row = 0; row < formMap.GetLength(0); row++)
            {
                for (int col = 0; col < formMap.GetLength(1); col++)
                {
                    int[] location = { row, col };

                    if (formMap[row, col].BackColor == Color.Green)
                    {
                        startNode = new AStarNode(false, location);
                    }
                    else if (formMap[row, col].BackColor == Color.Red)
                    {
                        endNode = new AStarNode(false, location);
                    }
                    else if (formMap[row, col].BackColor == Color.Black)
                    {
                        walls.Add(new AStarNode(true, location));
                    }
                }
            }

            // Create aStar algorithm object with given nodes
            aStar = new AStarAlgorithim(startNode, endNode, walls);

        }

        /// <summary>
        /// Handles placing start node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartNodeHandler(object sender, EventArgs e)
        {
            // Check if button is already checked and make sure other button is not checked
            if (setStart.Checked && setEnd.BackColor == Color.Red)
            {
                setStart.BackColor = Color.DarkGreen;
            }
            else
            {
                setStart.Checked = false;
                setStart.BackColor = Color.Green;
            }
        }

        /// <summary>
        /// Handles placing end node 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndNodeHandler(object sender, EventArgs e)
        {
            if (setEnd.Checked && setStart.BackColor == Color.Green)
            {
                setEnd.BackColor = Color.DarkRed;
            }
            else
            {
                setEnd.Checked = false;
                setEnd.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Handles placing wall nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WallNodeHandler(object sender, EventArgs e)
        {
            if (setWall.Checked && setWall.BackColor == Color.Gray)
            {
                setWall.BackColor = Color.DarkGray;
            }
            else
            {
                setWall.Checked = false;
                setWall.BackColor = Color.Gray;
            }
        }

        /// <summary>
        /// Handles is a placed node is clicked again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeClickHandler(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Check if either of the node setting buttons are checked
            if (setStart.Checked)
            {
                // Check if node is already a start node
                if (button.BackColor != Color.Green && numOfStart == 0)
                {
                    button.BackColor = Color.Green;
                    numOfStart = 1;
                }
                else if (button.BackColor == Color.Green && numOfStart == 1)
                {
                    button.BackColor = Color.White;
                    numOfStart = 0;
                }
            }
            else if (setEnd.Checked)
            {
                // Check if node is already a start node
                if (button.BackColor != Color.Red && numOfEnd == 0)
                {
                    button.BackColor = Color.Red;
                    numOfEnd = 1;
                }
                else if (button.BackColor == Color.Red && numOfEnd == 1)
                {
                    button.BackColor = Color.White;
                    numOfEnd = 0;
                }
            }
            else if (setWall.Checked)
            {
                if (button.BackColor == Color.Black)
                {
                    button.BackColor = Color.White;
                }
                else
                {
                    button.BackColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Runs A* algorithm on given nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoadAStar();
            aStar.Run();
            TracePath();
        }
    }
}
