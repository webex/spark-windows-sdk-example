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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenSink
{
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand LogoutCMD { get; set; }
        public RelayCommand VedioAudioSetupCMD { get; set; }
        public RelayCommand InitialCallCMD { get; set; }
        public RelayCommand CallCMD { get; set; }
        public RelayCommand SendFeedBackCMD { get; set; }
        public RelayCommand WaitingCallCMD { get; set; }


        private string loginInfo = string.Empty;
        public string LoginInfo
        {
            get { return this.loginInfo; }
            set
            {
                if (value != loginInfo)
                {
                    this.loginInfo = value;
                    OnPropertyChanged("LoginInfo");
                }
            }
        }

        private string connInfo = string.Empty;
        public string ConnectionInfo
        {
            get { return this.connInfo; }
            set
            {
                if (value != connInfo)
                {
                    this.connInfo = value;
                    OnPropertyChanged("ConnectionInfo");
                }
            }
        }

        public MainViewModel()
        {
            LogoutCMD = new RelayCommand(Logout, IfNotInCall);
            VedioAudioSetupCMD = new RelayCommand(VedioAudioSetup, IfNotInCall);
            InitialCallCMD = new RelayCommand(InitialCall, IfNotInCall);
            SendFeedBackCMD = new RelayCommand(SendFeedBack, IfNotInCall);
            WaitingCallCMD = new RelayCommand(WaitingCall, IfNotInCall);
            ApplicationController.Instance.StateInfoReceived += StateInfoReceived;

            GetUserInfo();
            RegistPhone();
        }

        private void GetUserInfo()
        {
            ApplicationController.Instance.PublishStateInfo("fetching user profile...", InfoType.LoginInfo);
            var sparkManager = ApplicationController.Instance.CurSparkManager;
            sparkManager.CurSpark.People.GetMe(r =>
            {
                if (r.IsSuccess)
                {
                    sparkManager.CurUser = (SparkSDK.Person)r.Data;
                    ApplicationController.Instance.PublishStateInfo("login as: " + sparkManager.CurUser.DisplayName, InfoType.LoginInfo);
                }
                else
                {
                    ApplicationController.Instance.PublishStateInfo("Fetch user profile failed", InfoType.LoginInfo);
                }
            });
        }

        private void RegistPhone()
        {
            ApplicationController.Instance.PublishStateInfo("spark cloud connecting...", InfoType.ConnectionInfo);
            var sparkManager = ApplicationController.Instance.CurSparkManager;
            sparkManager.CurSpark.Phone.Register(result =>
            {
                if (result.IsSuccess == true)
                {
                    ApplicationController.Instance.PublishStateInfo("spark cloud connected", InfoType.ConnectionInfo);
                }
                else
                {
                    ApplicationController.Instance.PublishStateInfo("spark cloud failed", InfoType.ConnectionInfo);
                }
            });
        }

        private void StateInfoReceived(object sender, EventArgs<Tuple<InfoType, string>> e)
        {
            if (e.Value.Item1 == InfoType.ConnectionInfo)
            {
                this.ConnectionInfo = e.Value.Item2;
            }
            if (e.Value.Item1 == InfoType.LoginInfo)
            {
                this.LoginInfo = e.Value.Item2;
            }
        }

        private void WaitingCall(object o)
        {
            ApplicationController.Instance.ChangeState(State.WaitingCall);
        }

        private void SendFeedBack(object o)
        {
            ApplicationController.Instance.ChangeState(State.SendFeedBack);
        }



        private void InitialCall(object o)
        {
            ApplicationController.Instance.ChangeState(State.IntiateCall);
        }

        private void VedioAudioSetup(object o)
        {
            ApplicationController.Instance.ChangeState(State.VideoAudioSetup);
        }
        private bool IfNotInCall(object o)
        {
            return true;
        }

        private void Logout(object o)
        {
            var sparkManager = ApplicationController.Instance.CurSparkManager;
            sparkManager.CurAuthenticator.Deauthorize();
            ApplicationController.Instance.ChangeState(State.PreLogin);
        }

    }
}
