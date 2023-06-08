using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using AGV_Dashboards.Models;
using Xamarin.Forms;

namespace AGV_Dashboards
{
    public partial class MainPage
    {
        //Get the average usuage of resources in a given day from the api and create two pie charts for the memory and a cpu consumption
        private async void LoadFilteredResources(DateTime filterDate)
        {
            // Get data from the API endpoint
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://10.0.2.2:3000/filter_resources?filterdate={filterDate:yyyy-MM-dd}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<ResourcesApiData>(responseBody);

                // Create a pie series for memory data
                var memorySeries = new PieSeries
                {
                    StrokeThickness = 2,
                    InsideLabelPosition = 0.5,
                    AngleSpan = 360,
                    StartAngle = 0,
                    InsideLabelFormat = "{1}: {0:#.#}MB",
                    Slices =
                {
                    new PieSlice("Memory Consumed", data.Memory.Consumed)
                    {
                        Fill = OxyColor.FromRgb(0, 136, 204),
                    },
                    new PieSlice("Memory Available", data.Memory.Available)
                    {
                        Fill = OxyColor.FromRgb(0, 204, 136)
                    }
                }
                };

                // Set the font size and font weight for the memory plot model title
                var memoryPlotModel = new PlotModel
                {
                    TitleFontSize = 24,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    TitleColor = OxyColor.FromRgb(51, 51, 51),
                    Title = "Memory Usage"
                };

                // Add the memory series to the memory plot model
                memoryPlotModel.Series.Add(memorySeries);

                // Set the memory plot model as the binding context for the memory chart view
                memoryChart.Model = memoryPlotModel;

                // Calculate available CPU
                var availableCpu = 100 - data.CPU.Utilization;

                // Create a pie series for CPU data
                var cpuSeries = new PieSeries
                {
                    StrokeThickness = 2,
                    InsideLabelPosition = 0.5,
                    AngleSpan = 360,
                    StartAngle = 0,
                    InsideLabelFormat = "{1}: {0:#.#}%",
                    Slices =
                {
                    new PieSlice("CPU Utilization", data.CPU.Utilization)
                    {
                        Fill = OxyColor.FromRgb(255, 102, 102)
                    },
                    new PieSlice("CPU Available", availableCpu)
                    {
                        Fill = OxyColor.FromRgb(102, 255, 102)
                    }
                }
                };

                // Set the font size and font weight for the CPU plot model title
                var cpuPlotModel = new PlotModel
                {
                    TitleFontSize = 24,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    TitleColor = OxyColor.FromRgb(51, 51, 51),
                    Title = "CPU Utilization"
                };

                // Add the CPU series to the CPU plot model
                cpuPlotModel.Series.Add(cpuSeries);

                // Set the CPU plot model as the binding context for the CPU chart view
                cpuChart.Model = cpuPlotModel;
            }
        }

        //Get the speed and direction in a given day from the api and create two line graphs
        private async void LoadFilteredWheelsData(DateTime filterDate)
        {
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = $"http://10.0.2.2:3000/filter_wheels?filterdate={filterDate:yyyy-MM-dd}";

            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            JArray speedData = (JArray)data["speed_data"];
            JArray directionData = (JArray)data["direction_data"];

            // Create the speed graph
            PlotModel speedModel = new PlotModel() { TitleFontSize = 24, TitleFontWeight = OxyPlot.FontWeights.Bold, TitleColor = OxyColor.FromRgb(51, 51, 51), Title = "Speed" };
            LinearAxis speedYAxis = new LinearAxis { Position = AxisPosition.Left };
            DateTimeAxis speedXAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
            LineSeries speedSeries = new LineSeries();
            foreach (JObject point in speedData)
            {
                double speed = (double)point["speed"];
                DateTime timestamp = (DateTime)point["timestamp"];
                speedSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), speed));
            }
            speedModel.Axes.Add(speedYAxis);
            speedModel.Axes.Add(speedXAxis);
            speedModel.Series.Add(speedSeries);
            speedPlot.Model = speedModel;

            // Create the direction graph
            PlotModel directionModel = new PlotModel() { TitleFontSize = 24, TitleFontWeight = OxyPlot.FontWeights.Bold, TitleColor = OxyColor.FromRgb(51, 51, 51), Title = "Direction" };
            LinearAxis directionYAxis = new LinearAxis { Position = AxisPosition.Left };
            DateTimeAxis directionXAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
            LineSeries directionSeries = new LineSeries();
            foreach (JObject point in directionData)
            {
                double direction = (double)point["direction"];
                DateTime timestamp = (DateTime)point["timestamp"];
                directionSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), direction));
            }
            directionModel.Axes.Add(directionYAxis);
            directionModel.Axes.Add(directionXAxis);
            directionModel.Series.Add(directionSeries);
            directionPlot.Model = directionModel;
        }

        //Get the data about the line tracker status in a given day from the api and create a line graph
        private async void LoadFilteredLineTrackerData(DateTime filterDate)
        {
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = $"http://10.0.2.2:3000/filter_line_tracker?filterdate={filterDate:yyyy-MM-dd}";
            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            JArray lineTrackerData = (JArray)data["line_status_data"];

            // Create the graph
            PlotModel lineTrackerModel = new PlotModel() { TitleFontSize = 24, TitleFontWeight = OxyPlot.FontWeights.Bold, TitleColor = OxyColor.FromRgb(51, 51, 51), Title = "Line Tracker Status" };
            LinearAxis lineTrackerYAxis = new LinearAxis { Position = AxisPosition.Left };
            DateTimeAxis lineTrackerXAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
            LineSeries lineTrackerSeries = new LineSeries();
            foreach (JObject point in lineTrackerData)
            {
                int lineTrackerStatus = (int)point["line_status"];
                DateTime timestamp = (DateTime)point["timestamp"];
                lineTrackerSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), lineTrackerStatus));
            }
            lineTrackerModel.Axes.Add(lineTrackerYAxis);
            lineTrackerModel.Axes.Add(lineTrackerXAxis);
            lineTrackerModel.Series.Add(lineTrackerSeries);
            lineTrackerPlot.Model = lineTrackerModel;
        }

        //Get the images taken by the camera in a given day from the api and add them to an horizontal scroll view
        private async void LoadFilteredImages(DateTime filterDate)
        {
            // clear the image stack from old omages to only have the needed ones
            ImageStack.Children.Clear();
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = $"http://10.0.2.2:3000/filter_camera?filterdate={filterDate:yyyy-MM-dd}";
            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            JArray binaryData = (JArray)data["images_data"];

            foreach (JObject imageData in binaryData)
            {


                byte[] imageDataBytes = Convert.FromBase64String(imageData["data"].ToString());

                {
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"{Guid.NewGuid()}.jpg");

                    // Create the directory if it doesn't exist
                    string directory = Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write the image data bytes to a file on the device's storage
                    File.WriteAllBytes(fileName, imageDataBytes);

                    ImageSource imageSource = ImageSource.FromFile(fileName);

                    Image image = new Image
                    {
                        Source = imageSource,
                        WidthRequest = 300,
                        HeightRequest = 550
                    };
                    Frame frame = new Frame
                    {
                        Padding = new Thickness(5),
                        Margin = new Thickness(10),
                        Content = image
                    };


                    ImageStack.Children.Add(frame);
                }
            }
        }

        //Get the images of obstacles detected in a given day from the api and add them to an horizontal scroll view
        private async void LoadFilteredObstaclesImages(DateTime filterDate)
        {
            // clear the image stack from old omages to only have the needed ones
            ObstaclesImageStack.Children.Clear();
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = $"http://10.0.2.2:3000/filter_obstacles_detected?filterdate={filterDate:yyyy-MM-dd}";
            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            JArray binaryData = (JArray)data["images_data"];

            foreach (JObject imageData in binaryData)
            {


                byte[] imageDataBytes = Convert.FromBase64String(imageData["data"].ToString());

                {
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"{Guid.NewGuid()}.jpg");

                    // Create the directory if it doesn't exist
                    string directory = Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write the image data bytes to a file on the device's storage
                    File.WriteAllBytes(fileName, imageDataBytes);

                    ImageSource imageSource = ImageSource.FromFile(fileName);

                    Image image = new Image
                    {
                        Source = imageSource,
                        WidthRequest = 300,
                        HeightRequest = 550
                    };
                    Frame frame = new Frame
                    {
                        Padding = new Thickness(5),
                        Margin = new Thickness(10),
                        Content = image
                    };


                    ObstaclesImageStack.Children.Add(frame);
                }
            }
        }

        //Get the data of the four ultrasonic sensors in a given day from the api and create a graph with four lines
        private async void LoadFilteredUltrasonicData(DateTime filterDate)
        {
            // Call the API to fetch the data
            HttpClient client = new HttpClient();
            string apiUrl = $"http://10.0.2.2:3000/filter_ultrasonic?filterdate={filterDate:yyyy-MM-dd}";
            string response = await client.GetStringAsync(apiUrl);

            // Parse the data from the API response
            JObject data = JObject.Parse(response);
            JArray leftDistance = (JArray)data["left_distance_data"];
            JArray rightDistance = (JArray)data["right_distance_data"];
            JArray backDistance = (JArray)data["back_distance_data"];
            JArray frontDistance = (JArray)data["front_distance_data"];

            PlotModel ultrasonicModel = new PlotModel() { TitleFontSize = 24, TitleFontWeight = OxyPlot.FontWeights.Bold, TitleColor = OxyColor.FromRgb(51, 51, 51), Title = "Distance To the Sides" };
            LinearAxis ultrasonicYAxis = new LinearAxis { Position = AxisPosition.Left };
            DateTimeAxis ultrasonicXAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
            LineSeries leftDistanceSeries = new LineSeries() { Title = "Distance to the left" };
            LineSeries rightDistanceSeries = new LineSeries() { Title = "Distance to the right" };
            LineSeries backDistanceSeries = new LineSeries() { Title = "Distance to the back" };
            LineSeries frontDistanceSeries = new LineSeries() { Title = "Distance to the front" };
            foreach (JObject point in leftDistance)
            {
                double leftDistanceDouble = (double)point["left_distance"];
                DateTime timestamp = (DateTime)point["timestamp"];
                leftDistanceSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), leftDistanceDouble));
            }
            foreach (JObject point in rightDistance)
            {
                double rightDistanceDouble = (double)point["right_distance"];
                DateTime timestamp = (DateTime)point["timestamp"];
                rightDistanceSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), rightDistanceDouble));
            }
            foreach (JObject point in backDistance)
            {
                double backDistanceDouble = (double)point["back_distance"];
                DateTime timestamp = (DateTime)point["timestamp"];
                backDistanceSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), backDistanceDouble));
            }
            foreach (JObject point in frontDistance)
            {
                double frontDistanceDouble = (double)point["front_distance"];
                DateTime timestamp = (DateTime)point["timestamp"];
                frontDistanceSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), frontDistanceDouble));
            }
            ultrasonicModel.Axes.Add(ultrasonicYAxis);
            ultrasonicModel.Axes.Add(ultrasonicXAxis);
            ultrasonicModel.Series.Add(backDistanceSeries);
            ultrasonicModel.Series.Add(frontDistanceSeries);
            ultrasonicModel.Series.Add(rightDistanceSeries);
            ultrasonicModel.Series.Add(leftDistanceSeries);
            ultrasonicPlot.Model = ultrasonicModel;
        }

    }

}


