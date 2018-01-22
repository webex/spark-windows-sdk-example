#region License
// Copyright (c) 2016-2017 Cisco Systems, Inc.

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

using SparkSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KitchenSink
{
    class CallViewModel : ViewModelBase
    {
        #region Fields

        SparkSDK.Spark spark;
        SparkSDK.Call currentCall
        {
            get
            {
                return ApplicationController.Instance.CurSparkManager.currentCall;
            }
            set
            {
                ApplicationController.Instance.CurSparkManager.currentCall = value;
            }
        }
        CallView curCallView;

        #endregion

        #region Properties

        public RelayCommand BackCommand { get; set; }
        public RelayCommand EndCallCMD { get; set; }
        public RelayCommand KeyboardCMD { get; set; }

        public RelayCommand DtmfCMD { get; set; }

        private double aspectRatioLocalVedio = 16.0 / 9.0;
        public double AspectRatioLocalVedio
        {
            get
            {
                return aspectRatioLocalVedio;
            }
            set
            {
                if(value != aspectRatioLocalVedio)
                {
                    aspectRatioLocalVedio = value;
                    OnPropertyChanged("AspectRatioLocalVedio");
                }
            }
        }

        private double aspectRatioRemoteVedio = 16.0 / 9.0;
        public double AspectRatioRemoteVedio
        {
            get
            {
                return aspectRatioRemoteVedio;
            }
            set
            {
                if (value != aspectRatioRemoteVedio)
                {
                    aspectRatioRemoteVedio = value;
                    OnPropertyChanged("AspectRatioRemoteVedio");
                }
            }
        }

        private double aspectRatioShareVedio = 16.0 / 9.0;
        public double AspectShareScreenVideo
        {
            get
            {
                return aspectRatioShareVedio;
            }
            set
            {
                if (value != aspectRatioShareVedio)
                {
                    aspectRatioShareVedio = value;
                    OnPropertyChanged("AspectShareScreenVideo");
                }
            }
        }

        public bool IfSendAudio
        {
            get
            {
                if (this.currentCall != null)
                {
                    return this.currentCall.IsSendingAudio;
                }
                return false;
            }
            set
            {
                if (this.currentCall != null && value != this.currentCall.IsSendingAudio)
                {
                    this.currentCall.IsSendingAudio = value;
                    OnPropertyChanged("IfSendAudio");
                }
            }
        }

        public bool IfSendVedio
        {
            get
            {
                if (this.currentCall != null)
                {
                    return this.currentCall.IsSendingVideo;
                }
                return false;
            }
            set
            {
                if (this.currentCall != null && value != this.currentCall.IsSendingVideo)
                {
                    this.currentCall.IsSendingVideo = value;
                    OnPropertyChanged("IfSendVedio");
                }                
            }
        }

        public bool IfReceiveVedio
        {
            get
            {
                if (this.currentCall != null)
                {
                    return this.currentCall.IsReceivingVideo;
                }
                return false;
            }
            set
            {
                if (this.currentCall != null && value != this.currentCall.IsReceivingVideo)
                {
                    this.currentCall.IsReceivingVideo = value;
                    OnPropertyChanged("IfReceiveVedio");
                }
            }
        }

        public bool IfReceiveAudio
        {
            get
            {
                if (this.currentCall != null)
                {
                    return this.currentCall.IsReceivingAudio;
                }
                return false;
            }
            set
            {
                if (this.currentCall != null&&value != this.currentCall.IsReceivingAudio)
                {
                    this.currentCall.IsReceivingAudio = value;
                    OnPropertyChanged("IfReceiveAudio");
                }
            }
        }

        private bool ifShareScreen = false;
        public bool IfShareSreen
        {
            get
            {
                return this.ifShareScreen;
            }
            set
            {
                if (value != ifShareScreen)
                {
                    this.ifShareScreen = value;
                    OnPropertyChanged("IfShareScreen");
                }
            }
        }

        private bool ifIncludeLog = false;
        public bool IfIncludeLog
        {
            get
            {
                return this.ifIncludeLog;
            }
            set
            {
                this.ifIncludeLog = value;
                OnPropertyChanged("IfIncludeLog");
            }
        }
        public string CallStatus
        {
            get
            {
                if (this.currentCall != null)
                {
                    return "Call Status: " + this.currentCall.Status.ToString();
                }
                return null;
            }
        }

        private string inputKey;
        public string InputKey
        {
            get
            {
                return this.inputKey;
            }
            set
            {
                this.inputKey = value;
                OnPropertyChanged("InputKey");
            }
        }

        private string eventInfo;
        public string EventInfo
        {
            get
            {
                return this.eventInfo;
            }
            set
            {
                this.eventInfo = value;
                OnPropertyChanged("EventInfo");
            }
        }

        #region Rating

        public RelayCommand HideRatingViewCMD { get; set; }
        public RelayCommand SendFeedBackCMD { get; set; }

        private int ratingValue = int.MinValue;
        public int RatingValue
        {
            get {
                return this.ratingValue;
            }
            set
            {
                if (value != this.ratingValue)
                {
                    this.ratingValue = value;
                    OnPropertyChanged("RatingValue");
                }
            }
        }

        private string comment = string.Empty;

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                if (value != this.comment)
                {
                    this.comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private bool ifShowRatingView = false;
        public bool IfShowRatingView
        {
            get
            {
                return this.ifShowRatingView;
            }
            set
            {
                if (value != ifShowRatingView)
                {
                    this.ifShowRatingView = value;
                    OnPropertyChanged("IfShowRatingView");
                }
            }
        }

        private bool ifShowkeyboard = false;
        public bool IfShowkeyboard
        {
            get
            {
                return this.ifShowkeyboard;
            }
            set
            {
                if (value != ifShowkeyboard)
                {
                    this.ifShowkeyboard = value;
                    OnPropertyChanged("IfShowkeyboard");
                }
            }
        }
        
        #endregion

        #endregion

        public CallViewModel(CallView callView)
        {
            curCallView = callView;
            EndCallCMD = new RelayCommand(EndCall);
            BackCommand = new RelayCommand(BackToMain);
            HideRatingViewCMD = new RelayCommand(HideRatingView);
            SendFeedBackCMD = new RelayCommand(this.SendFeedBack,this.CanSendFeedBack);
            KeyboardCMD = new RelayCommand(ShowHideKeyboard);
            DtmfCMD = new RelayCommand(SendDTMF);
            spark = ApplicationController.Instance.CurSparkManager.CurSpark;
        }

        #region method
        public void Dial(string calleeAddress)
        {
            spark?.Phone.Dial(calleeAddress, MediaOption.AudioVideoShare(curCallView.LocalViewHandle, curCallView.RemoteViewHandle, curCallView.RemoteShareViewHandle), result =>
            {
                if (result.IsSuccess)
                {
                    currentCall = result.Data;
                    RegisterCallEvent();
                    this.curCallView.RefreshViews();
                }
                else
                {
                    output($"Error: {result.Error?.ErrorCode.ToString()} {result.Error?.Reason}");
                }
            });
        }

        public void Answer()
        {
            if (currentCall == null)
            {
                return;
            }
            RegisterCallEvent();
            currentCall.Answer(MediaOption.AudioVideoShare(curCallView.LocalViewHandle, curCallView.RemoteViewHandle, curCallView.RemoteShareViewHandle), result =>
            {
                if (!result.IsSuccess)
                {
                    output($"Error: {result.Error?.ErrorCode.ToString()} {result.Error?.Reason}");
                }
            });
        }
        public void Reject()
        {
            if (currentCall == null)
            {
                return;
            }
            RegisterCallEvent();
            currentCall.Reject(result =>
            {
                if (!result.IsSuccess)
                {
                    output($"Error: {result.Error?.ErrorCode.ToString()} {result.Error?.Reason}");
                }
            });
        }

        private void EndCall(object o)
        {
            if (this.currentCall != null)
            {
                currentCall.Hangup(result =>
                {
                    if (!result.IsSuccess)
                    {

                    }
                });
            }
            ApplicationController.Instance.CurCallView = null;
        }

        public void UpdateLocalVideoView()
        {
            if (currentCall == null)
            {
                return;
            }
            currentCall.UpdateLocalView(curCallView.LocalViewHandle);
        }
        public void UpdateRemoteVideoView()
        {
            if (currentCall == null)
            {
                return;
            }
            currentCall.UpdateRemoteView(curCallView.RemoteViewHandle);
        }
        public void UpdateRemoteShareVideoView()
        {
            if (currentCall == null)
            {
                return;
            }
            currentCall.UpdateRemoteShareView(curCallView.RemoteShareViewHandle);
        }

        private void RefreshAudioVideoCtrlView()
        {
            OnPropertyChanged("IfSendAudio");
            OnPropertyChanged("IfSendVedio");
            OnPropertyChanged("IfReceiveVedio");
            OnPropertyChanged("IfReceiveAudio");
        }
        private void RefreshCallStatusView()
        {
            OnPropertyChanged("CallStatus");
        }

        private void output(String format, params object[] args)
        {
            EventInfo = string.Format(format, args);
        }

        private void UpdateRecentContactsStore()
        {
            if (currentCall == null
                || currentCall.To == null
                || currentCall.From == null)
            {
                return;
            }

            if (currentCall.Direction == Call.CallDirection.Outgoing)
            {
                ApplicationController.Instance.CurSparkManager?.RecentContacts?.AddRecentContactsStore(currentCall.To.PersonId);
            }
            else
            {
                ApplicationController.Instance.CurSparkManager.RecentContacts.AddRecentContactsStore(currentCall.From.PersonId);
            }
        }

        private void BackToMain(object o)
        {
            ApplicationController.Instance.CurCallView = null;
            ApplicationController.Instance.ChangeState(State.Main);
        }
        #endregion

        #region CallEvents

        private void RegisterCallEvent()
        {
            if (currentCall == null)
            {
                return;
            }
            currentCall.OnRinging += CurrentCall_onRinging;

            currentCall.OnConnected += CurrentCall_onConnected;

            currentCall.OnDisconnected += CurrentCall_onDisconnected;

            currentCall.OnMediaChanged += CurrentCall_onMediaChanged;

            currentCall.OnCapabilitiesChanged += CurrentCall_onCapabilitiesChanged;

            currentCall.OnCallMembershipChanged += CurrentCall_onCallMembershipChanged;
            
        }

        private void unRegisterCallEvent()
        {
            if (currentCall == null)
            {
                return;
            }
            currentCall.OnRinging -= CurrentCall_onRinging;

            currentCall.OnConnected -= CurrentCall_onConnected;

            currentCall.OnDisconnected -= CurrentCall_onDisconnected;

            currentCall.OnMediaChanged -= CurrentCall_onMediaChanged;

            currentCall.OnCapabilitiesChanged -= CurrentCall_onCapabilitiesChanged;

            currentCall.OnCallMembershipChanged -= CurrentCall_onCallMembershipChanged;
        }

        private void CurrentCall_onRinging(SparkSDK.Call call)
        {
            RefreshCallStatusView();
        }
        private void CurrentCall_onConnected(SparkSDK.Call call)
        {
            RefreshCallStatusView();
        }

        private void CurrentCall_onDisconnected(CallDisconnectedEvent reason)
        {
            RefreshCallStatusView();
            UpdateRecentContactsStore();        
            unRegisterCallEvent();
            output("call is disconnectd for " + reason?.GetType().Name);
            this.curCallView.RefreshViews();
            this.IfShowRatingView = true;
            currentCall = null;
            ApplicationController.Instance.CurSparkManager.CurCalleeAddress = null;
        }

        private void CurrentCall_onCallMembershipChanged(CallMembershipChangedEvent obj)
        {
            if (obj is CallMembershipJoinedEvent)
            {
                output($"{obj.CallMembership.Email} joined");
            }
            else if (obj is CallMembershipLeftEvent)
            {
                output($"{obj.CallMembership.Email} left");
            }
            else if (obj is CallMembershipDeclinedEvent)
            {
                output($"{obj.CallMembership.Email} decline");
            }
            else if (obj is CallMembershipSendingAudioEvent)
            {
                if (obj.CallMembership.IsSendingAudio)
                {
                    output($"{obj.CallMembership.Email} unmute audio");
                }
                else
                {
                    output($"{obj.CallMembership.Email} mute audio");
                }
                
            }
            else if (obj is CallMembershipSendingVideoEvent)
            {
                if (obj.CallMembership.IsSendingVideo)
                {
                    output($"{obj.CallMembership.Email} unmute video");
                }
                else
                {
                    output($"{obj.CallMembership.Email} mute video");
                }
            }
            else if (obj is CallMembershipSendingShareEvent)
            {
                if (obj.CallMembership.IsSendingVideo)
                {
                    output($"{obj.CallMembership.Email} sending share");
                }
                else
                {
                    output($"{obj.CallMembership.Email} stop share");
                }
            }
            else
            {

            }


        }

        private void CurrentCall_onMediaChanged(MediaChangedEvent mediaChgEvent)
        {
            RefreshAudioVideoCtrlView();

            if (mediaChgEvent is RemoteVideoReadyEvent)
            {
                //if dial API not set view handle, you can set them on these VideoReady events.
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    currentCall.setRemoteView(curCallView.RemoteViewHandle);
                //});  
            }
            else if (mediaChgEvent is LocalVideoReadyEvent)
            {
                //if dial API not set view handle, you can set them on these VideoReady events.
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    currentCall.setLocalView(curCallView.LocalViewHandle);
                //});
            }
            else if (mediaChgEvent is LocalVideoViewSizeChangedEvent)
            {
                output($"remote video size: width[{mediaChgEvent.Call.LocalVideoViewSize.Width}] height[{mediaChgEvent.Call.LocalVideoViewSize.Height}]");
                this.AspectRatioLocalVedio = mediaChgEvent.Call.LocalVideoViewSize.Width / (double)mediaChgEvent.Call.LocalVideoViewSize.Height;
            }
            else if (mediaChgEvent is RemoteVideoViewSizeChangedEvent)
            {
                output($"remote video size: width[{mediaChgEvent.Call.RemoteVideoViewSize.Width}] height[{mediaChgEvent.Call.RemoteVideoViewSize.Height}]");
                this.AspectRatioRemoteVedio = mediaChgEvent.Call.RemoteVideoViewSize.Width / (double)mediaChgEvent.Call.RemoteVideoViewSize.Height;
            }
            else if (mediaChgEvent is RemoteShareViewSizeChangedEvent)
            {
                output($"remote share size: width[{mediaChgEvent.Call.RemoteShareViewSize.Width}] height[{mediaChgEvent.Call.RemoteShareViewSize.Height}]");
                this.AspectShareScreenVideo = mediaChgEvent.Call.RemoteShareViewSize.Width / (double)mediaChgEvent.Call.RemoteShareViewSize.Height;
            }
            else if (mediaChgEvent is RemoteSendingVideoEvent)
            {
                this.curCallView?.RefreshRemoteViews();

                var remoteSendingVideoEvent = mediaChgEvent as RemoteSendingVideoEvent;
                if (remoteSendingVideoEvent.IsSending)
                {
                    output("remote unmute video");
                }
                else
                {
                    output("remote mute video");
                }
            }
            else if (mediaChgEvent is RemoteSendingAudioEvent)
            {
                var remoteSendingAudioEvent = mediaChgEvent as RemoteSendingAudioEvent;
                if (remoteSendingAudioEvent.IsSending)
                {
                    output("remote unmute audio");
                }
                else
                {
                    output("remote mute audio");
                }
            }
            else if (mediaChgEvent is RemoteSendingShareEvent)
            {
                this.curCallView?.RefreshShareViews();

                var remoteSendingShareEvent = mediaChgEvent as RemoteSendingShareEvent;
                if (remoteSendingShareEvent.IsSending)
                {
                    output("remote start share");
                    curCallView.SwitchShareViewWithRemoteView(true);
                }
                else
                {
                    output("remote stop share");
                    curCallView.SwitchShareViewWithRemoteView(false);
                }
            }
            else if (mediaChgEvent is SendingVideoEvent)
            {
                this.curCallView?.RefreshLocalViews();

                var sendingVideoEvent = mediaChgEvent as SendingVideoEvent;
                if (sendingVideoEvent.IsSending)
                {
                    output("local unmute video");
                }
                else
                {
                    output("local mute video");
                }
            }
            else if (mediaChgEvent is SendingAudioEvent)
            {
                var sendingAudioEvent = mediaChgEvent as SendingAudioEvent;
                if (sendingAudioEvent.IsSending)
                {
                    output("local unmute audio");
                }
                else
                {
                    output("local mute audio");
                }
            }
            else if (mediaChgEvent is ReceivingVideoEvent)
            {
                this.curCallView?.RefreshRemoteViews();

                var receivingVideoEvent = mediaChgEvent as ReceivingVideoEvent;
                if (receivingVideoEvent.IsReceiving)
                {
                    output("local restore remote video");
                }
                else
                {
                    output("local close remote video");
                }
            }
            else if (mediaChgEvent is ReceivingAudioEvent)
            {
                var receivingAudioEvent = mediaChgEvent as ReceivingAudioEvent;
                if (receivingAudioEvent.IsReceiving)
                {
                    output("local restore remote audio");
                }
                else
                {
                    output("local close remote audio");
                }
            }
            else if (mediaChgEvent is ReceivingShareEvent)
            {
                this.curCallView?.RefreshShareViews();

                var receivingShareEvent = mediaChgEvent as ReceivingShareEvent;
                if (receivingShareEvent.IsReceiving)
                {
                    output("local restore remote share");
                }
                else
                {
                    output("local close remote share");
                }
            }
            else if (mediaChgEvent is CameraSwitchedEvent)
            {
                var cameraSwitchedEvent = mediaChgEvent as CameraSwitchedEvent;
                output($"switch camera to {cameraSwitchedEvent.Camera.Name}");
            }
            else if (mediaChgEvent is SpeakerSwitchedEvent)
            {
                var speakerSwitchedEvent = mediaChgEvent as SpeakerSwitchedEvent;
                output($"switch camera to {speakerSwitchedEvent.Speaker.Name}");
            }
            else
            {
            }
        }

        private void CurrentCall_onCapabilitiesChanged(Capabilities capability)
        {   
            if (capability is CapabilitieDTMF)
            {
                CapabilitieDTMF dtmf = capability as CapabilitieDTMF;
                if (dtmf.IsEnabled)
                {
                    output($"DTMF Capability Enable");
                }
                else
                {
                    output($"DTMF Capability Disable");
                }
            }
        }
        #endregion

        #region Rating
        private void HideRatingView(object o)
        {
            this.IfShowRatingView = false;
        }

        private bool CanSendFeedBack(object o)
        {
            return !string.IsNullOrEmpty(this.Comment) || this.ratingValue != int.MinValue;
        }
        private void SendFeedBack(object o)
        {
            this.IfShowRatingView = false;
            currentCall?.SendFeedbackWith(this.RatingValue, this.Comment, this.IfIncludeLog);
        }
        #endregion

        #region keyboard
        private void ShowHideKeyboard(object o)
        {
            this.IfShowkeyboard = !IfShowkeyboard;
        }


        private void SendDTMF(object o)
        {
            string key = o as string;
            if (key == null
                || key.Length != 1)
            {
                return;
            }
            InputKey += key;
            if (currentCall == null)
            {
                return;
            }
            if (!currentCall.IsSendingDTMFEnabled)
            {
                output("Current call not support sending DTMF.");
                return;
            }
            currentCall.SendDtmf(key, r =>
            {
                if (r.IsSuccess)
                {
                    output($"Send DTMF[{key}] Success!");
                }
                else
                {
                    output($"Send DTMF[{key}] Fail!");
                }
            });
        }
        #endregion
    }
}
