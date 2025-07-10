using Bingo.Core.Patterns;
using Bingo.Services.Patterns;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text.Json;

namespace Bingo.Caller.App;

public static class DefaultPatternInstaller
{
	[SupportedOSPlatform("ios13.0")]
	[SupportedOSPlatform("maccatalyst13.0")]
	[SupportedOSPlatform("android21.0")]
	[SupportedOSPlatform("Windows10.0.17763.0")]
	public static async Task InstallPatternsIfFirstLaunchAsync(FilePatternRepository repository)
	{
		const string installKey = "ArePatternsInstalled";

		if (Preferences.Default.Get(installKey, false))
			return;				
		try
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync("patterns.json");
			using var reader = new StreamReader(stream);
			string json = await reader.ReadToEndAsync();

			var dtos = JsonSerializer.Deserialize<List<PatternJsonModel>>(json);
			if (dtos == null || dtos.Count == 0)
			{
				Debug.WriteLine("[Installer] No patterns found in JSON.");
				return;
			}
			
			foreach (var dto in dtos)
			{
				var pattern = dto.ToDomain();

				if (string.IsNullOrWhiteSpace(pattern.Name) || pattern.Cells is null || pattern.Cells.Count == 0)
				{
					Debug.WriteLine($"[Installer] Skipping invalid pattern: {dto.PatternName}");
					continue;
				}

				if (repository.HasPatternNamed(pattern.Name))
				{
					Debug.WriteLine($"[Installer] Pattern already exists: {pattern.Name}");
					continue;
				}

				await repository.SaveAsync(pattern);
				//Debug.WriteLine($"[Installer] Saved pattern: {pattern.Name}");
			}

			Preferences.Default.Set(installKey, true);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[Installer] Failed to install default patterns: {ex.Message}");
		}
	}
}
