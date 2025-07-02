namespace Bingo.Core.Domain;

public record GameSessionSnapshot<T>(
	IReadOnlyList<T> History,
	int Pointer,
	DateTime StartedAt,
	string? SessionId = null
);
