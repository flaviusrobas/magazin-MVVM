using Caliburn.Micro;
using MagDesktopUI.Helpers;
using MagDesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MagDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
            // Register the PasswordBoxHelper to handle the PasswordBox's BoundPasswordProperty
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");
        }

        /*Caliburn.Micro NuGet
              IWindowManager - interfata pentru managerul de ferestre
              SimpleContainer - container de dependente
              IEventAggregator - interfata pentru agregatorul de evenimente*/
        protected override void Configure()
        {
            //Initialize the container -this is where we register our services and view models
            _container.Instance(_container);

            // Register the services and view models
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
            // Register the APIHelper as a singleton
                .Singleton<IAPIHelper, APIHelper>();
            // putem sa cerem apiHelper oriunde ne aflam in aplicatie si vom obtine aceeasi instanta

            // Register all ViewModels in the assembly that end with "ViewModel"
            GetType().Assembly.GetTypes()
                .Where(type =>type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(ViewModelType => _container.RegisterPerRequest(
                    ViewModelType, ViewModelType.ToString(), ViewModelType));

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}
