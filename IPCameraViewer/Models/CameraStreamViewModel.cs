using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IPCameraViewer.Services;
using Microsoft.Maui.Controls;

namespace IPCameraViewer.Models
{
    public class CameraStreamViewModel : INotifyPropertyChanged
    {
        private int id;
        private string cameraName = string.Empty;
        private string url = string.Empty;
        private bool isRunning;
        private ImageSource? currentFrame;
        private string metrics = "Metrics: -";
        private string motionStatus = "Motion: idle";
        private Color motionColor = Colors.Gray;
        private float lastRatio;
        private double motionThreshold = 1.5; // Default 1.5%

        public int Id
        {
            get => this.id;
            set => SetProperty(ref this.id, value);
        }

        public string CameraName
        {
            get => this.cameraName;
            set => SetProperty(ref this.cameraName, value);
        }

        public string Url
        {
            get => this.url;
            set => SetProperty(ref this.url, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => SetProperty(ref this.isRunning, value);
        }

        public ImageSource? CurrentFrame
        {
            get => this.currentFrame;
            set => SetProperty(ref this.currentFrame, value);
        }

        public string Metrics
        {
            get => this.metrics;
            set => SetProperty(ref this.metrics, value);
        }

        public string MotionStatus
        {
            get => this.motionStatus;
            set => SetProperty(ref this.motionStatus, value);
        }

        public Color MotionColor
        {
            get => this.motionColor;
            set => SetProperty(ref this.motionColor, value);
        }

        public float LastRatio
        {
            get => this.lastRatio;
            set => SetProperty(ref this.lastRatio, value);
        }

        public double MotionThreshold
        {
            get => this.motionThreshold;
            set => SetProperty(ref this.motionThreshold, value);
        }

        public ObservableCollection<string> DetectionLogs { get; } = new();

        public MjpegStreamer? Streamer { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}