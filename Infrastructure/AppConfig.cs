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
		{	// Set real path to your MauiUkraine Application	
			return "C:\\repository\\MauiUkraine\\bin\\x64\\Debug\\net10.0-windows10.0.19041.0\\win-x64\\MauiControlsShowcase.exe";
		}
	}

	public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(30);
	public static TimeSpan PollInterval { get; } = TimeSpan.FromMilliseconds(250);
}
