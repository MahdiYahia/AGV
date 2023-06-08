using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AGV_Dashboards.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace AGV_Dashboards
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadResources();
            LoadWheelsData();
            LoadLineTrackerData();
            LoadImages();
            LoadObstaclesImages();
            LoadUltrasonicData();
        }
        private void OnFilterClicked(object sender, EventArgs e)
        {
            if (filteringDatePicker.IsVisible == false)
            {
                filteringDatePicker.IsVisible = true;
                labelDatePicker.IsVisible = true;
            }
            else
            {
                filteringDatePicker.IsVisible = false;
                labelDatePicker.IsVisible = false;
            }
        }
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            var selectedDate = e.NewDate;
            // Use the selected date for filtering
            LoadFilteredWheelsData(selectedDate);
            LoadFilteredResources(selectedDate);
            LoadFilteredLineTrackerData(selectedDate);
            LoadFilteredUltrasonicData(selectedDate);
            LoadFilteredImages(selectedDate);
            LoadFilteredObstaclesImages(selectedDate);
        }
    }
}
