﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SimpleRemoteCommandSample
{
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.Extensibility;
	using Microsoft.VisualStudio.Extensibility.Commands;
	using Microsoft.VisualStudio.Extensibility.Definitions;
	using Microsoft.VisualStudio.Extensibility.Shell;
	using Microsoft.VisualStudio.RpcContracts.Commands;

	[CommandIcon(KnownMonikers.QuestionMark, IconSettings.IconAndText)]
	[Command(CommandId, "User Prompt", "Click for a prompt", CommandFlags.None, placement: CommandPlacement.ToolsMenu)]
	public class SampleCommand : Command
	{
		private const string CommandId = "SimpleRemoteCommandSample.Command";

		public SampleCommand(VisualStudioExtensibility extensibility, string name)
			: base(extensibility, name)
		{
		}

		public enum TokenThemeResult
		{
			None,
			Solarized,
			OneDark,
			GruvBox,
		}

		public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken ct)
		{
			// Asking the user to confirm an operation.
			if (!await context.ShowPromptAsync("Continue with executing the command?", PromptOptions.OKCancel, ct))
			{
				return;
			}

			// Asking the user to confirm a dangerous operation.
			if (!await context.ShowPromptAsync("Continue with executing the command?", PromptOptions.OKCancel.WithCancelAsDefault(), ct))
			{
				return;
			}

			// OK-only prompt
			await context.ShowPromptAsync("The extension must reload.", PromptOptions.OK, ct);

			// Custom prompt
			var themeResult = await context.ShowPromptAsync(
				"Which theme should be used for the generated output?",
				new PromptOptions<TokenThemeResult>
				{
					Choices =
					{
				{ "Solarized Is Awesome", TokenThemeResult.Solarized },
				{ "OneDark Is The Best", TokenThemeResult.OneDark },
				{ "GruvBox Is Groovy", TokenThemeResult.GruvBox },
					},
					DismissedReturns = TokenThemeResult.None,
					DefaultChoiceIndex = 2,
				},
				ct);

			Debug.WriteLine($"Selected Token Theme: {themeResult}");
		}
	}
}
