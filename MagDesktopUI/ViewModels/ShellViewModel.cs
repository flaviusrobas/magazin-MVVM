using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MagDesktopUI.EventsModel;
using MagDesktopUI.Views;

namespace MagDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
       private LoginViewModel _loginVM;
       private IEventAggregator _events;
       private SalesViewModel _salesVM;
       private SimpleContainer _container;

        public ShellViewModel(LoginViewModel loginVM, IEventAggregator events, SalesViewModel salesVM,
            SimpleContainer container)
        {
            _events = events;
            _loginVM = loginVM;
            _salesVM = salesVM;
            _container = container;

            _events.SubscribeOnPublishedThread(this);
            // Subscribe to the LogOnEvent so that we can handle it when it is published
            
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_salesVM);
            return Task.CompletedTask;
        }
       
    }

    
}
