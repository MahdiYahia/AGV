using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Mock_Client_UI_Load(object sender, EventArgs e)
        {

        }

        private void camera_Click(object sender, EventArgs e)
        {
            ImageFromMqttTopicToMongoDB();
            ImageToTopicThroughMQQTT();
        }

        private void obstacles_detected_Click(object sender, EventArgs e)
        {
            ObstacleFromMqttTopicToMongoDB();
            ImageToTopicThroughMQQTT();
        }

        private void wheels_sensor_Click(object sender, EventArgs e)
        {
            WheelsDataFromMqttTopicToMongoDB();
            WheelsDataToTopicThroughMQQTT();
        }

        private void ultrasonic_sensor_Click(object sender, EventArgs e)
        {
            UltrasonicFromMqttTopicToMongoDB();
            UltrasonicToTopicThroughMQQTT();
        }

        private void line_tracker_sensor_Click(object sender, EventArgs e)
        {
            LineTrackerStatusFromMqttTopicToMongoDB();
            LineTrackerStatusToTopicThroughMQQTT();
        }

        private void Resources(object sender, EventArgs e)
        {
            ResourcesFromMqttTopicToMongoDB();
            ResourcesToTopicThroughMQQTT();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
