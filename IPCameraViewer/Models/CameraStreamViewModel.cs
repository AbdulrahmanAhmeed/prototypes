using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IPCameraViewer.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace IPCameraViewer.Models
{
    public class CameraStreamViewModel : INotifyPropertyChanged
    {
        private const string DefaultMetricsText = "Metrics: -";
        private const string DefaultMotionIdleText = "Motion: idle";
        private const double DefaultMotionThresholdPercent = 1.5;
        private const double DefaultSoundVolume = 1.0;
        private const bool DefaultSoundEnabled = true;
        private const string SoundVolumeKeyFormat = "Camera_{0}_SoundVolume";
        private const string SoundEnabledKeyFormat = "Camera_{0}_SoundEnabled";
        private const string SoundFilePathKeyFormat = "Camera_{0}_SoundFilePath";

        private int id;
        private string cameraName = string.Empty;
        private string url = string.Empty;
        private bool isRunning;
        private ImageSource? currentFrame;
        private string metrics = CameraStreamViewModel.DefaultMetricsText;
        private string motionStatus = CameraStreamViewModel.DefaultMotionIdleText;
        private Color motionColor = Colors.Gray;
        private float lastRatio;
        private double motionThresholdPercent = CameraStreamViewModel.DefaultMotionThresholdPercent;
        private double soundVolume = CameraStreamViewModel.DefaultSoundVolume;
        private bool soundEnabled = CameraStreamViewModel.DefaultSoundEnabled;
        private string? soundFilePath;

        public int Id
        {
            get => this.id;
            set
            {
                if (this.SetProperty(ref this.id, value))
                {
                    // Load persisted sound settings when ID is set
                    this.LoadSoundSettings();
                }
            }
        }

        private void LoadSoundSettings()
        {
            string volumeKey = string.Format(CameraStreamViewModel.SoundVolumeKeyFormat, this.Id);
            this.soundVolume = Preferences.Get(volumeKey, CameraStreamViewModel.DefaultSoundVolume);
            this.OnPropertyChanged(nameof(this.SoundVolume));

            string enabledKey = string.Format(CameraStreamViewModel.SoundEnabledKeyFormat, this.Id);
            this.soundEnabled = Preferences.Get(enabledKey, CameraStreamViewModel.DefaultSoundEnabled);
            this.OnPropertyChanged(nameof(this.SoundEnabled));

            string filePathKey = string.Format(CameraStreamViewModel.SoundFilePathKeyFormat, this.Id);
            this.soundFilePath = Preferences.Get(filePathKey, null);
            this.OnPropertyChanged(nameof(this.SoundFilePath));
            this.OnPropertyChanged(nameof(this.SoundFileName));
        }

        public string CameraName
        {
            get => this.cameraName;
            set => this.SetProperty(ref this.cameraName, value);
        }

        public string Url
        {
            get => this.url;
            set => this.SetProperty(ref this.url, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetProperty(ref this.isRunning, value);
        }

        public ImageSource? CurrentFrame
        {
            get => this.currentFrame;
            set => this.SetProperty(ref this.currentFrame, value);
        }

        public string Metrics
        {
            get => this.metrics;
            set => this.SetProperty(ref this.metrics, value);
        }

        public string MotionStatus
        {
            get => this.motionStatus;
            set => this.SetProperty(ref this.motionStatus, value);
        }

        public Color MotionColor
        {
            get => this.motionColor;
            set => this.SetProperty(ref this.motionColor, value);
        }

        public float LastRatio
        {
            get => this.lastRatio;
            set => this.SetProperty(ref this.lastRatio, value);
        }

        public double MotionThresholdPercent
        {
            get => this.motionThresholdPercent;
            set => this.SetProperty(ref this.motionThresholdPercent, value);
        }

        public double SoundVolume
        {
            get => this.soundVolume;
            set
            {
                if (this.SetProperty(ref this.soundVolume, value))
                {
                    // Persist volume setting
                    string key = string.Format(CameraStreamViewModel.SoundVolumeKeyFormat, this.Id);
                    Preferences.Set(key, value);
                }
            }
        }

        public bool SoundEnabled
        {
            get => this.soundEnabled;
            set
            {
                if (this.SetProperty(ref this.soundEnabled, value))
                {
                    // Persist enabled setting
                    string key = string.Format(CameraStreamViewModel.SoundEnabledKeyFormat, this.Id);
                    Preferences.Set(key, value);
                }
            }
        }

        public string? SoundFilePath
        {
            get => this.soundFilePath;
            set
            {
                if (this.SetProperty(ref this.soundFilePath, value))
                {
                    // Persist file path setting
                    string key = string.Format(CameraStreamViewModel.SoundFilePathKeyFormat, this.Id);
                    if (!string.IsNullOrEmpty(value))
                    {
                        Preferences.Set(key, value);
                    }
                    else
                    {
                        Preferences.Remove(key);
                    }
                    this.OnPropertyChanged(nameof(this.SoundFileName));
                }
            }
        }

        public string SoundFileName
        {
            get
            {
                if (string.IsNullOrEmpty(this.soundFilePath))
                {
                    return "No file selected";
                }
                return System.IO.Path.GetFileName(this.soundFilePath);
            }
        }

        public ObservableCollection<string> DetectionLogs { get; } = new();

        public MjpegStreamer? Streamer { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            bool result = true;
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                result = false;
            }
            else
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
            return result;
        }
    }
}