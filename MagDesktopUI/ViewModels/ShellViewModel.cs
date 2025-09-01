using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MagDesktopUI.EventsModel;
using MagDesktopUI.Library.Models;
using MagDesktopUI.Views;

namespace MagDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;


        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;

            _events.SubscribeOnPublishedThread(this);
            
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }      


        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }


        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
            return Task.CompletedTask;
        }


    }


}
