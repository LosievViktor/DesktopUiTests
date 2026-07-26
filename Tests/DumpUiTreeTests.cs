using System.Diagnostics;
using System.Text;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Tests;

/// <summary>
/// Diagnostic helper for inspecting AutomationIds in the running WinUI tree.
/// </summary>
[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class DumpUiTreeTests
{
	[Test]
	[Explicit("Diagnostic only")]
	public void DumpUiTree()
	{
		foreach (var process in Process.GetProcessesByName(AppConfig.ProcessName))
		{
			try { process.Kill(entireProcessTree: true); } catch { /* ignored */ }
			process.Dispose();
		}

		Assert.That(File.Exists(AppConfig.AppPath), Is.True, $"Missing app: {AppConfig.AppPath}");

		using var automation = new UIA3Automation();
		using var app = Application.Launch(AppConfig.AppPath);
		app.WaitWhileMainHandleIsMissing(TimeSpan.FromSeconds(30));
		Thread.Sleep(3000);

		var window = app.GetMainWindow(automation, TimeSpan.FromSeconds(10));
		Assert.That(window, Is.Not.Null, "Main window was null");

		var sb = new StringBuilder();
		sb.AppendLine($"Window Name='{SafeName(window!)}'");
		CollectIds(window!, sb);

		var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "ui-tree-dump.txt");
		File.WriteAllText(path, sb.ToString());
		Console.WriteLine($"Wrote dump to {path}");
		Console.WriteLine(sb.ToString());

		app.Close();
	}

	private static void CollectIds(AutomationElement root, StringBuilder sb)
	{
		var stack = new Stack<AutomationElement>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			var current = stack.Pop();
			var id = SafeAutomationId(current);
			if (!string.IsNullOrWhiteSpace(id))
			{
				sb.AppendLine($"AutomationId='{id}' Name='{Truncate(SafeName(current), 80)}' Type={SafeType(current)}");
			}

			try
			{
				foreach (var child in current.FindAllChildren())
				{
					stack.Push(child);
				}
			}
			catch
			{
				// ignored
			}
		}
	}

	private static string SafeAutomationId(AutomationElement element)
	{
		try
		{
			return element.Properties.AutomationId.TryGetValue(out var value) ? value ?? "" : "";
		}
		catch { return ""; }
	}

	private static string SafeName(AutomationElement element)
	{
		try
		{
			return element.Properties.Name.TryGetValue(out var value) ? value ?? "" : "";
		}
		catch { return ""; }
	}

	private static string SafeType(AutomationElement element)
	{
		try
		{
			return element.Properties.ControlType.TryGetValue(out var value) ? value.ToString() : "";
		}
		catch { return ""; }
	}

	private static string Truncate(string value, int max)
		=> value.Length <= max ? value : value[..max] + "...";
}
