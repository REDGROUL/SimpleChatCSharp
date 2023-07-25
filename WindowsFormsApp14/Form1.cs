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
using WebSocketSharp;

namespace WindowsFormsApp14
{
    public partial class Form1 : Form
    {
        WebSocket ws = new WebSocket("ws://127.0.0.1:8888");

        private string username { get; set; }
        public Form1()
        {
            NameInput frm2 = new NameInput();
            DialogResult dr = frm2.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                username = frm2.getText();
                frm2.Close();
            }

            InitializeComponent();




            Thread t = new Thread(() => {

               
                ws.Connect();
               // MessageBox.Show(ws.Ping().ToString());

                ws.OnMessage += (sender, e) =>
                    {
                        Action a = () =>
                        {
                            listBox1.Items.Add(e.Data);
                        };


                        if (InvokeRequired)
                        {
                            Invoke(a);
                        }
                        else
                        {
                            a();
                        }
                    };
                    

                
            });
            t.Start();

            Thread PingPong = new Thread(() =>
            {
                while (true)
                {
                    if (ws.Ping())
                    {
                        Action a = () =>
                        {
                            toolStripStatusLabel1.Text = "Connected";
                        };

                        Invoke(a);
                    }
                    else
                    {
                        Action a = () =>
                        {
                            toolStripStatusLabel1.Text = "Not Connected";
                        };
                        Invoke(a);

                    }
                    Thread.Sleep(5000);
                }
                
            });
            PingPong.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ws.Send(username+": "+textBox1.Text);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ws.Close();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
