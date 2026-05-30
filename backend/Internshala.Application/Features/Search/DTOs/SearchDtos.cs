namespace Internshala.Application.Features.Search.DTOs;

public sealed record SearchResultDto(
    string EntityType,
    int Id,
    string Title,
    string? Subtitle,
    string? BadgeLabel,
    string NavigateUrl);

public sealed record GlobalSearchResult(
    IReadOnlyList<SearchResultDto> Items,
    int TotalCount,
    string Query);
