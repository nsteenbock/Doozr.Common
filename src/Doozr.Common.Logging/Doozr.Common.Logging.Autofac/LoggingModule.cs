using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using System.Linq;
using System.Reflection;

namespace Doozr.Common.Logging.Autofac
{
	public class LoggingModule : global::Autofac.Module
	{
		protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
		{
			registration.Preparing += Registration_Preparing;
			registration.Activated += Registration_Activated;
		}

		private void Registration_Activated(object sender, ActivatedEventArgs<object> e)
		{
			var instanceType = e.Instance.GetType();

			var properties = instanceType
			  .GetProperties(BindingFlags.Public | BindingFlags.Instance)
			  .Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);


			var logManager = e.Context.Resolve<ILogManager>();

			foreach (var propToSet in properties)
			{
				propToSet.SetValue(e.Instance, logManager.GetLogger(instanceType), null);
			}
		}

		private void Registration_Preparing(object sender, PreparingEventArgs e)
		{
			e.Parameters = e.Parameters.Union(
				new[]
				{
					new ResolvedParameter(
						(p, i) => p.ParameterType == typeof(ILogger),
						(p, i) => i.Resolve<ILogManager>().GetLogger(p.Member.DeclaringType))
				});
		}
	}
}
