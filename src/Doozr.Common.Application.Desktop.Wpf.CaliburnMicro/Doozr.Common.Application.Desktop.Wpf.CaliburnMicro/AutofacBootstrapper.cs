using Autofac;
using Caliburn.Micro;
using Doozr.Common.Logging.Aspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Doozr.Common.Application.Desktop.Wpf.CaliburnMicro
{
	[Log(LogLevel = Logging.LogLevel.Debug)]
	public abstract class AutofacBootstrapper: BootstrapperBase
	{
		private List<Assembly> uiAssemblies = new List<Assembly>();

		public AutofacBootstrapper()
		{
			PrepareRegistration();
			Initialize();
		}

		protected IContainer container;

		protected void RegisterUiAssembly(Assembly uiAssembly)
		{
			uiAssemblies.Add(uiAssembly);
			AssemblySource.Instance.Add(uiAssembly);
		}

		protected void RegisterUiAssemblyContaining<T>()
		{
			RegisterUiAssembly(typeof(T).Assembly);
		}

		protected virtual void PrepareRegistration()
		{
			RegisterUiAssembly(GetType().Assembly);
		}

		protected override void Configure()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
			builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

			foreach (var uiAssembly in uiAssemblies)
			{
				uiAssembly.GetTypes()
					.Where(type => type.IsClass)
					.Where(type => type.Name.EndsWith("ViewModel"))
					.ToList()
					.ForEach(viewModelType => builder.RegisterType(viewModelType));
			}

			ConfigureContainer(builder);

			container = builder.Build();
		}

		protected abstract void ConfigureContainer(ContainerBuilder builder);

		protected override object GetInstance(Type service, string key)
		{
			if (service == null)
			{
				return container.Resolve(Type.GetType(key));
			}

			if (string.IsNullOrWhiteSpace(key))
			{
				if (container.IsRegistered(service))
					return container.Resolve(service);
			}
			throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
		}

		protected override void BuildUp(object instance)
		{
			container.InjectProperties(instance);
		}
	}
}
