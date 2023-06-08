using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Drawing;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;

namespace AGV_Mock_Client
{
    class MainClass
    {
        private static void Main(string[] args)
        {
            WheelsDataFromMqttTopicToMongoDB();
            WheelsDataToTopicThroughMQQTT();
            LineTrackerStatusFromMqttTopicToMongoDB();
            LineTrackerStatusToTopicThroughMQQTT();
            //..
        }
        static async void LoadImages()
        {
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = "https://us-east-1.aws.data.mongodb-api.com/app/agvdashboards-jvtgj/endpoint/camera";
            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            
            
            JArray binaryData = (JArray)data["images_data"];
            
              foreach (JObject imageData in binaryData)
              {
               
               //byte[] imageDataBytes = (byte[])imageData["data"]["Data"];
               
                
                byte[] imageDataBytes = Convert.FromBase64String(imageData["data"]["Data"].ToString());
                Console.WriteLine(BitConverter.ToString(imageDataBytes));
                Console.WriteLine("hello");
                // Create a MemoryStream from the byte array
                using (MemoryStream ms = new MemoryStream(imageDataBytes))
                {
                    // Load the image from the MemoryStream using the System.Drawing.Image class
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                    // Save the image to a file
                    string filePath = Path.Combine("C:\\Users\\Lenovo\\OneDrive\\Documents\\University Projects", $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.jpg");
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
        static void ImageToTopicThroughMQQTT() 
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            byte[] imageBytes = File.ReadAllBytes("image.jpg");
            Thread.Sleep(500);
            mqttClient.Publish("image_topic", imageBytes);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void ImageFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_camera");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "image_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                //..
                BsonDocument document = new BsonDocument
                {
                    { "data", new BsonBinaryData(e.Message) },
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
               collection.InsertOne(document);
            };
        }

        static void ObstacleToTopicThroughMQQTT()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            byte[] imageBytes = File.ReadAllBytes("image.jpg");
            Thread.Sleep(500);
            mqttClient.Publish("image_topic", imageBytes);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void ObstacleFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_obstacles_detected");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "image_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                //..
                BsonDocument document = new BsonDocument
                {
                    { "data", new BsonBinaryData(e.Message) },
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
                collection.InsertOne(document);
            };
        }
        static void LineTrackerStatusToTopicThroughMQQTT()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            Random random = new Random();
            int line_status = random.Next(0, 2);
            byte[] payload = BitConverter.GetBytes(line_status);
            mqttClient.Publish("line_status_topic", payload);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void LineTrackerStatusFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_line_tracker");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "line_status_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                //..
                BsonDocument document = new BsonDocument
                {
                    { "line_status", BitConverter.ToInt32(e.Message,0) },
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
                collection.InsertOne(document);
            };
        }
        static void ResourcesToTopicThroughMQQTT()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            Random random = new Random();
            int memoryConsumed = random.Next(0, 400);
            int cpuUtilization = random.Next(0, 100);
            int memoryAvailable = 400 - memoryConsumed;
            int[] values = {memoryConsumed, memoryAvailable, cpuUtilization};
            byte[] payload = values.SelectMany(BitConverter.GetBytes).ToArray();
            mqttClient.Publish("resources_topic", payload);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void ResourcesFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_resources");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "resources_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                byte[] payload = e.Message;
                int[] values = new int[payload.Length/sizeof(int)];
                for (int i=0; i<values.Length; i++)
                {
                    values[i] = BitConverter.ToInt32(payload, i * sizeof(int));
                }
                BsonDocument document = new BsonDocument
                {
                    { "memoryConsumed", values[0]},
                    { "memoryAvailable", values[1]},
                    { "cpuUtilization", values[2]},
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
                collection.InsertOne(document);
            };
        }
        static void UltrasonicToTopicThroughMQQTT()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            Random random = new Random();
            int left_distance = random.Next(0, 100);
            int right_distance = random.Next(0, 100);
            int back_distance = random.Next(0, 100);
            int front_distance = random.Next(0, 100); 
            int[] values = { left_distance, right_distance, back_distance, front_distance };
            byte[] payload = values.SelectMany(BitConverter.GetBytes).ToArray();
            mqttClient.Publish("ultrasonic_topic", payload);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void UltrasonicFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_ultrasonic");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "ultrasonic_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                byte[] payload = e.Message;
                int[] values = new int[payload.Length / sizeof(int)];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = BitConverter.ToInt32(payload, i * sizeof(int));
                }
                BsonDocument document = new BsonDocument
                {
                    { "left_distance", values[0]},
                    { "right_distance", values[1]},
                    { "back_distance", values[2]},
                    { "front_distance", values[3]},
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
                collection.InsertOne(document);
            };
        }
        static void WheelsDataToTopicThroughMQQTT()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client");
            Random random = new Random();
            int direction = random.Next(0, 360);
            int speed = random.Next(0, 50);
            
            int[] values = { direction, speed };
            byte[] payload = values.SelectMany(BitConverter.GetBytes).ToArray();
            mqttClient.Publish("wheels_topic", payload);
            Thread.Sleep(500);
            // Disconnect from the MQTT broker
            mqttClient.Disconnect();
        }
        static void WheelsDataFromMqttTopicToMongoDB()
        {
            // Connect to the MQTT broker
            MqttClient mqttClient = new MqttClient("localhost");
            mqttClient.Connect("my-client2");

            // Set up the MongoDB client
            MongoClient mongoClient = new MongoClient("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = mongoClient.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("agv_wheels");

            // Subscribe to the image_topic topic
            mqttClient.Subscribe(new string[] { "wheels_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            // Handle the message received event
            mqttClient.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => {
                byte[] payload = e.Message;
                int[] values = new int[payload.Length / sizeof(int)];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = BitConverter.ToInt32(payload, i * sizeof(int));
                }
                BsonDocument document = new BsonDocument
                {
                    { "direction", values[0]},
                    { "speed", values[1]},
                    {"timestamp", DateTime.UtcNow}
                };
                //..
                // Insert the document into the MongoDB collection
                collection.InsertOne(document);
            };
        }
        static void UploadImage()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://AGVUser:AGVPassword@agvcluster.g7tyzlu.mongodb.net/?retryWrites=true&w=majority");
            var client = new MongoClient(settings);
            IMongoDatabase database = client.GetDatabase("AGV");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("agv_camera");

            byte[] imageBytes = File.ReadAllBytes("image.jpg");
            BsonDocument document = new BsonDocument
            {
                { "data", new BsonBinaryData(imageBytes) },
                {"timestamp", DateTime.UtcNow}
            };
            Console.WriteLine(imageBytes);

            // collection.InsertOne(document);
        }

    }
}


