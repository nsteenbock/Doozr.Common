using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;
using Doozr.Common.Application;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Doozr.Common.Logging.Autofac
{
	public class LoggingModule : global::Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(container =>
			{
				var applicationProperties = container.Resolve<IApplicationProperties>();

				var logLibraryDirectory = Path.Combine(applicationProperties.AppDataDirectory, "Logging");

				if (Directory.Exists(logLibraryDirectory))
				{
					var loggingDlls = Directory.GetFiles(logLibraryDirectory, "*.dll");
					foreach(var loggingDll in loggingDlls)
					{
						var assembly = Assembly.LoadFile(loggingDll);
						var logManagerType = assembly.GetTypes().Where(x => x.IsAssignableTo<ILogManager>()).SingleOrDefault();
						if (logManagerType != null)
						{
							return Activator.CreateInstance(logManagerType);
						}
					}
				}

				return new NullLogManager();
			}).As<ILogManager>().SingleInstance();
		}

		protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
		{
			registration.PipelineBuilding += (sender, pipeline) =>
			{
				pipeline.Use(PipelinePhase.Activation, MiddlewareInsertionMode.EndOfPhase, (c, next) =>
				{
					next(c);
					Registration_Activated(c);
				});

				pipeline.Use(PipelinePhase.ParameterSelection, MiddlewareInsertionMode.EndOfPhase, (c, next) =>
				{
					next(c);
					Registration_Preparing(c);
				});
			};
			
		}

		private void Registration_Activated(ResolveRequestContext context)
		{
			var instanceType = context.Instance.GetType();

			var properties = instanceType
			  .GetProperties(BindingFlags.Public | BindingFlags.Instance)
			  .Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);


			if (properties.Any())
			{
				var logManager = context.Resolve<ILogManager>();

				foreach (var propToSet in properties)
				{
					propToSet.SetValue(context.Instance, logManager.GetLogger(instanceType), null);
				}
			}
		}

		private void Registration_Preparing(ResolveRequestContext context)
		{
			context.ChangeParameters(context.Parameters.Union(
				new[]
				{
					new ResolvedParameter(
						(p, i) => p.ParameterType == typeof(ILogger),
						(p, i) => i.Resolve<ILogManager>().GetLogger(p.Member.DeclaringType))
				}));
		}
	}
}
