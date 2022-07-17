using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRTest
{
    public partial class Form1 : Form
    {

        private HubConnection connection;
        string urlSignalR = "";

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            urlSignalR = "https://localhost:7073/chathub/?username=testCSClient";

            connection = new HubConnectionBuilder().WithUrl(urlSignalR).Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string, string>("GetThatMessageDude", (user, message) =>
            {
                this.Invoke((Action)(() =>
                {
                    //if (to == txtFrom.Text)
                    //{
                    //    var newMessage = $"{user}: {message}";
                    //    listBox1.Items.Add(newMessage);
                    //}
                    var newMessage = $"{user}: {message}";
                    listBox1.Items.Add(newMessage);

                }));
            });

            try
            {
                await connection.StartAsync();
                listBox1.Items.Add("Connection started");
                //connectButton.IsEnabled = false;
                //btSend.IsEnabled = true;
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                await connection.InvokeAsync("AddMessageToChat", "testCSClient", textBox1.Text);
                listBox1.Items.Add($"{"test"}:{textBox1.Text}");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }
    }
}
