using Bingo.Core.Models;
using Bingo.Services.Patterns;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bingo.AppServices.Patterns;

public class DefaultPatternRepository : IPatternRepository
{
    private readonly List<BingoPattern> _cache;

    public DefaultPatternRepository(ILogger<DefaultPatternRepository> logger)
    {
        using var stream = typeof(DefaultPatternRepository)
            .Assembly
            .GetManifestResourceStream("Bingo.AppServices.Resources.patterns.json")!;

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var parsed = JsonSerializer.Deserialize<List<JsonPattern>>(json);

        _cache = parsed?
            .Where(p =>
            {
                logger.LogInformation("Checking pattern: \"{PatternName}\". Size={Rows}x{Cols}, HasActiveCells={HasAny}",
                p.PatternName ?? "<unnamed>",
                p.Pattern?.Length,
                p.Pattern?.FirstOrDefault()?.Length ?? 0,
                p.Pattern?.Any(r => r.Any(cell => cell)) ?? false);

                bool isValid = !string.IsNullOrWhiteSpace(p.PatternName) &&
                               p.Pattern?.Length == 5 &&
                               p.Pattern.All(r => r.Length == 5) &&
                               p.Pattern.Any(r => r.Any(cell => cell));

                if (!isValid)
                    logger.LogWarning("Discarded invalid pattern: \"{PatternName}\" (missing name, bad grid, or no active cells)", p.PatternName ?? "<unnamed>");

                return isValid;
            })
            .Select(p => new BingoPattern
            {
                Name = p.PatternName,
                Cells = ToCellSet(p.Pattern)
            }).ToList()
            ?? [];
    }

    public DefaultPatternRepository(string rawJson, ILogger<DefaultPatternRepository> logger)
    {
        var parsed = JsonSerializer.Deserialize<List<JsonPattern>>(rawJson);

        _cache = parsed?
            .Where(p =>
            {
                logger.LogInformation("Checking pattern: \"{PatternName}\". Size={Rows}x{Cols}, HasActiveCells={HasAny}",
                    p.PatternName ?? "<unnamed>",
                    p.Pattern?.Length,
                    p.Pattern?.FirstOrDefault()?.Length ?? 0,
                    p.Pattern?.Any(r => r.Any(cell => cell)) ?? false);

                bool isValid = !string.IsNullOrWhiteSpace(p.PatternName) &&
                               p.Pattern?.Length == 5 &&
                               p.Pattern.All(r => r.Length == 5) &&
                               p.Pattern.Any(r => r.Any(cell => cell));

                if (!isValid)
                    logger.LogWarning("Discarded invalid pattern: \"{PatternName}\" (missing name, bad grid, or no active cells)",
                        p.PatternName ?? "<unnamed>");

                return isValid;
            })
            .Select(p => new BingoPattern
            {
                Name = p.PatternName,
                Cells = ToCellSet(p.Pattern)
            })
            .ToList()
            ?? [];
    }

    public Task<IEnumerable<BingoPattern>> GetAllAsync() => Task.FromResult<IEnumerable<BingoPattern>>(_cache);

    public Task<BingoPattern?> GetByNameAsync(string name)
        => Task.FromResult(_cache.FirstOrDefault(p => p.Name == name));

    public Task SaveAsync(BingoPattern pattern) => Task.CompletedTask; // Immutable
    public Task DeleteAsync(string name) => Task.CompletedTask;        // Immutable

    private static HashSet<(int, int)> ToCellSet(bool[][] grid)
    {
        var set = new HashSet<(int, int)>();
        for (int row = 0; row < grid.Length; row++)
            for (int col = 0; col < grid[row].Length; col++)
                if (grid[row][col])
                    set.Add((row, col));
        return set;
    }

    private record JsonPattern(string PatternName, bool[][] Pattern);
}
