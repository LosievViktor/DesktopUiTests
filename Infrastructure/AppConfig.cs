namespace MauiUkraine.UITests.Infrastructure;

public static class AppConfig
{
	public static string ProcessName => Require("ProcessName");

	public static string AppPath => Require("AppPath");

	public static TimeSpan DefaultTimeout => TimeSpan.FromSeconds(int.Parse(Require("DefaultTimeout")));

	public static TimeSpan PollInterval => TimeSpan.FromMilliseconds(int.Parse(Require("PollInterval")));

	private static string Require(string name)
	{
		var value = TestContext.Parameters[name];
		if (string.IsNullOrWhiteSpace(value))
			throw new InvalidOperationException(
				$"Missing '{name}' in .runsettings. Pass --settings .runsettings when running tests.");

		return value;
	}
}
