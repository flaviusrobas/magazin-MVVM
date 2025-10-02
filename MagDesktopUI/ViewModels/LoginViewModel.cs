using Caliburn.Micro;
using MagDesktopUI.EventsModel;
using MagDesktopUI.Helpers;
using MagDesktopUI.Library.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username = "rbs@test.com"; // Default username for testing
        private string _password = "Admin123#"; // Default password for testing
        private readonly IAPIHelper _apiHelper;
        private readonly IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);                
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }           

        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {                
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }


        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty; // Clear previous error message
                var result = await _apiHelper.Authenticate(UserName, Password);

                //Caption more information about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEvent(), new CancellationToken());
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                ErrorMessage = ex.Message;
            }
            
        }       

    }
}
