using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MagDesktopUI.EventsModel;
using MagDesktopUI.Views;

namespace MagDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
       private IEventAggregator _events;
       private SalesViewModel _salesVM;
      

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {
            _events = events;
            _salesVM = salesVM;
            

            _events.SubscribeOnPublishedThread(this);
            // Subscribe to the LogOnEvent so that we can handle it when it is published

            //ActivateItemAsync(_container.GetInstance<LoginViewModel>());
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_salesVM);
            return Task.CompletedTask;
        }
       
    }

    
}
