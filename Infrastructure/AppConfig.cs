namespace MauiUkraine.UITests.Infrastructure;

public static class AppConfig
{
	public const string ProcessName = "MauiControlsShowcase";

	/// <summary>
	/// Path to the unpackaged Windows build of MauiControlsShowcase.
	/// Override with env var MAUI_UKRAINE_APP_PATH when needed.
	/// </summary>
	public static string AppPath
	{
		get
		{
			var fromEnv = Environment.GetEnvironmentVariable("MAUI_UKRAINE_APP_PATH");
			if (!string.IsNullOrWhiteSpace(fromEnv))
			{
				return Path.GetFullPath(fromEnv);
			}

			var defaultPath = Path.GetFullPath(Path.Combine(
				TestContext.CurrentContext.TestDirectory,
				"..", "..", "..", "..",
				"MauiUkraine",
				"bin", "Debug", "net10.0-windows10.0.19041.0", "win-x64",
				"MauiControlsShowcase.exe"));

			return defaultPath;
		}
	}

	public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(30);
	public static TimeSpan PollInterval { get; } = TimeSpan.FromMilliseconds(250);
}
