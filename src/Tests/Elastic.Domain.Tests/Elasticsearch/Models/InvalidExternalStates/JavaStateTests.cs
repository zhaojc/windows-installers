﻿using System;
using Elastic.Installer.Domain.Properties;
using Elastic.Installer.Domain.Tests.Elasticsearch.Configuration.Mocks;
using Xunit;

namespace Elastic.Installer.Domain.Tests.Elasticsearch.Models.InvalidExternalStates
{
	public class JavaStateTests : InstallationModelTestBase
	{
		private InstallationModelTester EmptyJavaModel(Func<TestSetupStateProvider, TestSetupStateProvider> selector) => WithValidPreflightChecks(s => selector(s
			.Java(j => j
				.JavaHomeMachineVariable(null)
				.JavaHomeUserVariable(null)
				.JdkRegistry64(null)
			)));

		[Fact] public void DefaultStateNeedsJava() => EmptyJavaModel(s=>s)
			.HasPrerequisiteErrors(errors => errors
				.ShouldHaveErrors(TextResources.NoticeModelValidator_JavaInstalled)
			);

		/// <summary>
		/// User home will be lifted to a machine level environment variable during install
		/// </summary>
		[Fact] public void JavaUserEnvironmentVariable() => EmptyJavaModel(s=>s
				.Java(j => j.JavaHomeUserVariable(@"C:\Java"))
			)
			.IsValidOnFirstStep();

		[Fact] public void JavaHomeMachineVariable() => EmptyJavaModel(s=>s
				.Java(j => j.JavaHomeMachineVariable(@"C:\Java"))
			)
			.IsValidOnFirstStep();

		[Fact] public void MachineAndUserVariableSame() => EmptyJavaModel(s=>s
				.Java(j => j.JavaHomeMachineVariable(@"C:\Java").JavaHomeUserVariable(@"C:\Java"))
			)
			.IsValidOnFirstStep();

		[Fact] public void MachineAndUserVariableNOTSame() => EmptyJavaModel(s=>s
				.Java(j => j.JavaHomeMachineVariable(@"C:\Java").JavaHomeUserVariable(@"C:\JavaX"))
			)
			.HasPrerequisiteErrors(errors => errors
				.ShouldHaveErrors(TextResources.NoticeModelValidator_JavaMisconfigured)
			);

		[Fact] public void MachineAndRegistryNOTSame() => EmptyJavaModel(s=>s
				.Java(j => j.JavaHomeMachineVariable(@"C:\Java").JdkRegistry64(@"C:\JavaX"))
			)
			.IsValidOnFirstStep();
		
		[Fact] public void Jre64IsValid() => EmptyJavaModel(s=>s
				.Java(j => j.JreRegistry64(@"C:\Java"))
			)
			.IsValidOnFirstStep();
		
		[Fact] public void Jdk64IsValid() => EmptyJavaModel(s=>s
				.Java(j => j.JdkRegistry64(@"C:\Java"))
			)
			.IsValidOnFirstStep();

	}
}
