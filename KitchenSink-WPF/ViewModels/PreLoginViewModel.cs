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

namespace KitchenSink
{
    public class PreLoginViewModel : ViewModelBase
    {
        public RelayCommand SelectAuthCMD { get; private set; }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                if (value != this.isBusy)
                {
                    this.isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        public PreLoginViewModel()
        {
            this.SelectAuthCMD = new RelayCommand(SelectAuthMethod);
        }

        public void SelectAuthMethod(object o)
        {
            AuthMethod am = (AuthMethod)o;
            if (am == AuthMethod.AppID)
            {
                TryToLoginByJWT();
            }
            else if (am == AuthMethod.SparkID)
            {
                TryToLoginByOAuth();
            }
        }
        void TryToLoginByOAuth()
        {
            this.IsBusy = true;
            string clientId = "C6b13dd866c63ad282d89902953afd5c511a935cfe7cda688ed72efb53ba7c54e";
            string clientSecret = "07ad817f035d80a6e250d8706b16a0d0f30c1aad70bf4c314b48fc3e2a5537b8";
            string redirectUri = "KitchenSink://response/";
            string scope = "spark:all";

            var auth = new OAuthAuthenticator(clientId, clientSecret, scope, redirectUri);
            var sparkManager = new SparkManager(auth);
            ApplicationController.Instance.CurSparkManager = sparkManager;
         
            auth.Authorized(r =>
            {
                this.IsBusy = false;
                if (r.IsSuccess)
                {
                    ApplicationController.Instance.ChangeState(State.Main);
                }
                else
                {
                    ApplicationController.Instance.ChangeState(State.LoginByOAuth);
                }
            });
        }

        void TryToLoginByJWT()
        {
            this.IsBusy = true;
            var auth = new JWTAuthenticator();
            var sparkManager = new SparkManager(auth);
            ApplicationController.Instance.CurSparkManager = sparkManager;
            auth.Authorized(r =>
            {
                this.IsBusy = false;
                if (r.IsSuccess)
                {
                    ApplicationController.Instance.ChangeState(State.Main);
                }
                else
                {
                    ApplicationController.Instance.ChangeState(State.LoginByJWT);
                }
            });
        }
    }
}
