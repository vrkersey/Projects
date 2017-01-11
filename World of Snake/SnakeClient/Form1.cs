using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Network_Controller;
using System.Text.RegularExpressions;
using SnakeGame;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Model;

namespace SnakeClient
{
    public partial class Form1 : Form
    {
        // variable used to stop the update of the spectate dropdown
        private bool block = false;

        // This object represents the world.
        private World world;

        // This object is contains the server socket and any other server information
        private SocketState theServer;

        // this is to keep track of player info
        private Snake player;
        Color playerColor;

        // initializes the form
        public Form1()
        {
            InitializeComponent();
            scoreBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
            scoreBox.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
        }

        private void UpdateFrame()
        {
            // Cause the panel to redraw
            drawingPanel1.Invalidate();
        }


        // Connect button event handler
        private void connectButton_Click(object sender, EventArgs e)
        {
            string ip = serverAddress.Text;

            // disconnect from a previous socket
            if (theServer != null)
                if (theServer.connectionEstablished)
                    Networking.Disconnect(theServer);
            try
            {
                theServer = Networking.ConnectToServer(FirstContact, Networking.IPResolve(ip), 11000);
            }
            catch { }
        }

        // Connected to server, sends Name and waits
        private void FirstContact(SocketState s)
        {
            theServer.connectionEstablished = true;

            // disable controls because the game is starting
            this.Invoke(new MethodInvoker(() => connectButton.Enabled = false));
            this.Invoke(new MethodInvoker(() => colorButton.Enabled = false));
            this.Invoke(new MethodInvoker(() => serverAddress.Enabled = false));
            this.Invoke(new MethodInvoker(() => nameBox.Enabled = false));
            this.Invoke(new MethodInvoker(() => WelcomePanel.Visible = false));
            this.Invoke(new MethodInvoker(() => scoreBox.Visible = true));
            this.Invoke(new MethodInvoker(() => spectateComboBox.Visible = false));
            this.Invoke(new MethodInvoker(() => this.Focus()));
            Networking.SendMessage(s, nameBox.Text,false);
            s.CallMe = ReceiveStartup;
        }

        // receive startup data from the server
        private void ReceiveStartup(SocketState s)
        {
            //recieves your snake ID and dimentions of the world
            string startupData = s.sb.ToString();
            string[] parts = Regex.Split(startupData, @"(?<=[\n])");
            int x,y,id;
            int.TryParse(parts[0], out id); //obtain the player ID
            int.TryParse(parts[1], out x); //obtain the width of the world
            int.TryParse(parts[2], out y); //obtain the hieght of the world
            world = new World(x,y);

            this.Invoke(new MethodInvoker(()=> drawingPanel1.Size = new Size(x * World.pixelsPerCell + World.pixelsPerCell, y * World.pixelsPerCell + World.pixelsPerCell))); //resize the panel
            this.Invoke(new MethodInvoker(()=> this.Width = x*World.pixelsPerCell + 150)); //resize the form

            player = new Snake(id, nameBox.Text); // setting up the player snake
            drawingPanel1.SetWorld(world);
            s.sb.Remove(0, startupData.Length); //removing the startup data from the socketstate string builder
            s.CallMe = ReceiveWorld;
            Networking.GetData(s);
        }

        // receive World data from the server
        private void ReceiveWorld(SocketState s)
        {
            if (s.sb.ToString().ToCharArray()[s.sb.Length - 1] == '\n') //only process data when a complete message was transmitted
                processWorld(s.sb);
            Networking.GetData(s);
        }

        // helper method for processing world data from the server
        private void processWorld(StringBuilder data)
        {
            string[] parts = Regex.Split(data.ToString(), @"(?<=[\n])"); //split the string into parts

            foreach (string s in parts)
            {
                if (s != "")
                {
                    JObject obj = JObject.Parse(s.Replace("\n", ""));
                    JToken snakeProp = obj["vertices"];
                    JToken foodProp = obj["loc"];

                    if (snakeProp != null) // if the part is a snake
                    {
                        Snake snake = JsonConvert.DeserializeObject<Snake>(s.Replace("\n", ""));
                        if (snake.Id == player.Id) // if the snake being processed is the player we want to do things a little differently
                        {
                            drawingPanel1.playerHead = snake.Head();
                            drawingPanel1.zoom = snake.Length*2;
                            
                            world.addSnake(snake, true, playerColor);
                            if (snake.Verts[0].X == -1)
                                playerDied();
                        }
                        else
                            world.addSnake(snake, false, playerColor);
                    }
                    else if (foodProp != null) // if the part is food
                    {
                        Food food = JsonConvert.DeserializeObject<Food>(s.Replace("\n", ""));
                        world.addFood(food);
                    }
                    data.Remove(0, s.Length); //remove the processed part from the stringbuilder
                }

            }

            //update the scorebox and spectatorbox
            lock (world)
            {
                System.Threading.Thread.Sleep(50);
                this.Invoke(new MethodInvoker(() => scoreBox.Items.Clear()));
                if (!block)
                    this.Invoke(new MethodInvoker(() => spectateComboBox.Items.Clear()));
                foreach (Snake s in world.getSnakes())
                {
                    this.Invoke(new MethodInvoker(() => scoreBox.Items.Add(new MyCustomItem(s.getColor(), s.Name + " " + s.Length))));
                    if (!block)
                    {
                        MyCustomItem i = new MyCustomItem(s.getColor(), s.Name);
                        i.ID = s.Id;
                        this.Invoke(new MethodInvoker(() => spectateComboBox.Items.Add(i)));
                    }
                }
                spectateComboBox.Invalidate();
            }
            UpdateFrame(); //redraw after all parts have been processed
        }
        // Helper Method for what happens when the player dies
        private void playerDied()
        {
            this.Invoke(new MethodInvoker(() => connectButton.Enabled = true));
            this.Invoke(new MethodInvoker(() => colorButton.Enabled = true));
            this.Invoke(new MethodInvoker(() => nameBox.Enabled = true));
            this.Invoke(new MethodInvoker(() => connectButton.Text = "Play Again"));
            this.Invoke(new MethodInvoker(() => colorButton.Text = "Change Color"));
            this.Invoke(new MethodInvoker(() => spectateComboBox.Visible = true));
            this.Invoke(new MethodInvoker(() => spectateComboBox.Text = "Spectate"));
        }

        // Event Handler for Key presses
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            //Moving around the World
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                Networking.SendMessage(theServer, "(4)\n",false);
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                Networking.SendMessage(theServer, "(2)\n", false);
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                Networking.SendMessage(theServer, "(1)\n", false);
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                Networking.SendMessage(theServer, "(3)\n", false);
            }
        }

        // Event Handler for closing the Form
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (theServer != null)
                if (theServer.connectionEstablished)
                    Networking.Disconnect(theServer);
        }

        // Event Handler for Color Button
        private void colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                colorButton.BackColor = colorDialog1.Color;
                playerColor = colorDialog1.Color;
            }
        }

        // Drawing a custom Listbox Item
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                MyCustomItem item = (MyCustomItem) scoreBox.Items[e.Index]; // Get the current item and cast it to MyListBoxItem
                    e.Graphics.DrawString(item.Message, scoreBox.Font, new SolidBrush(item.ItemColor), 0, e.Index * scoreBox.ItemHeight);
              
            }
        }

        // event handler to grab the selected spectate player
        private void spectateComboBox_SelectionChanged(object sender, EventArgs e)
        {
            int id =(int)spectateComboBox.SelectedIndex;
            if (id>=0)
            {
                MyCustomItem i = (MyCustomItem)spectateComboBox.Items[id];
                if (world.findSnake(i.ID) != null)
                    player = world.findSnake(i.ID);
            }
            block = false;
        }

        // Method to update block so that the combobox does not keep getting updated while the user tries to select a player
        private void spectateComboBox_DropDown(object sender, EventArgs e)
        {
            block = true;
        }
    }

    /// <summary>
    /// Custom ListBox Item which includes text color
    /// </summary>
    public class MyCustomItem
    {
        /// <summary>
        /// Initialize a new Custom ListBox Item
        /// </summary>
        /// <param name="c">Text Color</param>
        /// <param name="m">Message</param>
        public MyCustomItem(Color c, string m)
        {
            ItemColor = c;
            Message = m;
        }
        /// <summary>
        /// Getting or setting the Color of the Item
        /// </summary>
        public Color ItemColor { get; set; }
        /// <summary>
        /// Getting or setting the Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// used to hold on to player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// used to display the player's name in the spectate box
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Message;
        }
    }
}