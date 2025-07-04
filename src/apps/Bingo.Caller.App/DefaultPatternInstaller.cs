using System.Runtime.Versioning;
using System.Text.Json;
using Bingo.Core.Patterns;
using Bingo.Services.Patterns;

namespace Bingo.Caller.App;

public static class DefaultPatternInstaller
{
	[SupportedOSPlatform("ios13.0")]
	[SupportedOSPlatform("maccatalyst13.0")]
	[SupportedOSPlatform("android21.0")]
	public static async Task InstallPatternsIfFirstLaunchAsync(FilePatternRepository repository)
	{
		const string installKey = "ArePatternsInstalled";

		if (Preferences.Default.Get(installKey, false))
			return;

		try
		{
			await repository.InitializeAsync();

			using var stream = await FileSystem.OpenAppPackageFileAsync("patterns.json");
			using var reader = new StreamReader(stream);
			string json = await reader.ReadToEndAsync();

			var patterns = JsonSerializer.Deserialize<List<BingoPattern>>(json);
			if (patterns is null || patterns.Count == 0)
				return;

			foreach (var pattern in patterns)
			{
				if (pattern is null || string.IsNullOrWhiteSpace(pattern.Name) || pattern.Cells is null)
					continue;

				if (repository.HasPatternNamed(pattern.Name))
					continue;

				await repository.SaveAsync(pattern);
			}

			Preferences.Default.Set(installKey, true);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"⚠️ Failed to install default patterns: {ex.Message}");
		}
	}
}
