#region License
// Copyright (c) 2016-2018 Cisco Systems, Inc.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenSink
{
    public class VideoAudioSetupViewModel : ViewModelBase
    {
        private SparkSDK.Spark spark;

        private IntPtr pbHandle;
        private Action refreshViewMtd;

        public List<string> DefaultVideoBandWidths { get; set; }
        public List<string> DefaultShareBandWidths { get; set; }
        public List<string> DefaultAudioBandWidths { get; set; }

        public DeviceData DeviceData
        {
            get;
            set;
        }

        public IList<SparkSDK.LogLevel> LogLevels
        {
            get
            {
                // Will result in a list
                return Enum.GetValues(typeof(SparkSDK.LogLevel)).Cast<SparkSDK.LogLevel>().ToList<SparkSDK.LogLevel>();
            }
        }
        public SparkSDK.LogLevel LogLevel
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.ConsoleLogger;
            }
            set
            {
                ApplicationController.Instance.CurSparkManager.CurSpark.ConsoleLogger = value;
            }
        }

        public string VideoBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.VideoMaxBandwidth.ToString();
            }
            set
            {
                ApplicationController.Instance.CurSparkManager.CurSpark.Phone.VideoMaxBandwidth = Convert.ToUInt32(value);
            }
        }
        public string NewVideoBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.VideoMaxBandwidth.ToString();
            }
            set
            {
                if (value.Length > 0)
                {
                    ApplicationController.Instance.CurSparkManager.CurSpark.Phone.VideoMaxBandwidth = Convert.ToUInt32(value);
                }                
            }
        }
        public string ShareBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.ShareMaxBandwidth.ToString();
            }
            set
            {
                ApplicationController.Instance.CurSparkManager.CurSpark.Phone.ShareMaxBandwidth = Convert.ToUInt32(value);
            }
        }
        public string NewShareBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.ShareMaxBandwidth.ToString();
            }
            set
            {
                if (value.Length > 0)
                {
                    ApplicationController.Instance.CurSparkManager.CurSpark.Phone.ShareMaxBandwidth = Convert.ToUInt32(value);
                }
            }
        }

        public string AudioBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.AudioMaxBandwidth.ToString();
            }
            set
            {
                ApplicationController.Instance.CurSparkManager.CurSpark.Phone.AudioMaxBandwidth = Convert.ToUInt32(value);
            }
        }
        public string NewAudioBandWidth
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.CurSpark.Phone.AudioMaxBandwidth.ToString();
            }
            set
            {
                if (value.Length > 0)
                {
                    ApplicationController.Instance.CurSparkManager.CurSpark.Phone.AudioMaxBandwidth = Convert.ToUInt32(value);
                }
            }
        }

        private bool ifClosePreview = false;
        public bool IfClosePreview
        {
            get
            {
                return this.ifClosePreview;
            }
            set
            {
                if (value != ifClosePreview)
                {
                    ifClosePreview = value;
                    this.SwitchPreview();
                    OnPropertyChanged("IfClosePreview");
                }
            }
        }

        private SparkSDK.AVIODevice selectedCamera;
        public SparkSDK.AVIODevice SelectedCamera
        {
            get { return this.selectedCamera; }
            set
            {
                if (this.selectedCamera != value)
                {
                    this.selectedCamera = value;
                    spark.Phone.SelectAVIODevice(value);
                    OnPropertyChanged("SelectedCamera");
                }
            }
        }

        private SparkSDK.AVIODevice selectedRinger;
        public SparkSDK.AVIODevice SelectedRinger
        {
            get { return this.selectedRinger; }
            set
            {
                if (this.selectedRinger != value)
                {
                    this.selectedRinger = value;
                    spark.Phone.SelectAVIODevice(value);
                    OnPropertyChanged("SelectedRinger");
                }
            }
        }

        private SparkSDK.AVIODevice selectedSpeaker;
        public SparkSDK.AVIODevice SelectedSpeaker
        {
            get { return this.selectedSpeaker; }
            set
            {
                if (this.selectedSpeaker != value)
                {
                    this.selectedSpeaker = value;
                    spark.Phone.SelectAVIODevice(value);
                    OnPropertyChanged("SelectedSpeaker");
                }
            }
        }

        private SparkSDK.AVIODevice selectedMircoPhone;
        public SparkSDK.AVIODevice SelectedMircoPhone
        {
            get { return this.selectedMircoPhone; }
            set
            {
                if (this.selectedMircoPhone != value)
                {
                    this.selectedMircoPhone = value;
                    spark.Phone.SelectAVIODevice(value);
                    OnPropertyChanged("SelectedMircoPhone");

                }
            }
        }

        public VideoAudioSetupViewModel()
        {
            this.spark = ApplicationController.Instance.CurSparkManager.CurSpark;
            DeviceData = new DeviceData(this.spark);

            DefaultAudioBandWidths = new List<string>();
            DefaultAudioBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidthAudio).ToString());

            DefaultVideoBandWidths = new List<string>();
            DefaultVideoBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidth90p).ToString());
            DefaultVideoBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidth180p).ToString());
            DefaultVideoBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidth360p).ToString());
            DefaultVideoBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidth720p).ToString());
            DefaultVideoBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidth1080p).ToString());

            DefaultShareBandWidths = new List<string>();
            DefaultShareBandWidths.Add(((uint)SparkSDK.Phone.DefaultBandwidth.MaxBandwidthSession).ToString());
        }

        public void OnViewClosed()
        {
            spark.Phone.StopPreview(this.pbHandle);
        }

        public void OnViewReady(IntPtr pbCameraPreviewHandle, Action refreshViewFunc)
        {
            this.pbHandle = pbCameraPreviewHandle;
            spark.Phone.StartPreview(pbCameraPreviewHandle);
            this.refreshViewMtd = refreshViewFunc;
        }

        public void ResetPreviewWindow()
        {
            this.spark.Phone.UpdatePreview(this.pbHandle);
        }

        private void SwitchPreview()
        {
            if (this.ifClosePreview)
            {
                spark.Phone.StopPreview(this.pbHandle);
                this.refreshViewMtd();
            }
            else
            {
                spark.Phone.StartPreview(this.pbHandle);
            }
        }

    }
}
