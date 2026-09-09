using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Common;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Posts.Queries.GetGifs;

public record GetGifsQuery(string? Cursor, int PageSize) : IRequest<PagedResult<GifResult>>;

public class GetGifsQueryHandler : IRequestHandler<GetGifsQuery, PagedResult<GifResult>>
{
    private readonly IApplicationDbContext _context;

    public GetGifsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    private static string EncodeCursor(DateTime createdAt, Guid id) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{createdAt.Ticks}|{id}"));

    private static (DateTime CreatedAt, Guid Id) DecodeCursor (string cursor)
    {
        var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
        var parts = raw.Split('|');
        return (new DateTime(long.Parse(parts[0]), DateTimeKind.Utc), Guid.Parse(parts[1]));
    }

    public async Task<PagedResult<GifResult>> Handle(GetGifsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Gif> query = _context.Gifs
            .OrderByDescending(g => g.CreatedAt)
            .ThenByDescending(g => g.Id);

        if (!string.IsNullOrEmpty(request.Cursor))
        {
            var (cursorCreatedAt, cursorId) = DecodeCursor(request.Cursor);

            query = query.Where(g => g.CreatedAt < cursorCreatedAt || (g.CreatedAt == cursorCreatedAt && g.Id < cursorId));
        }

        var gifs = await query.Take(request.PageSize + 1).ToListAsync(cancellationToken);

        var hasMore = gifs.Count > request.PageSize;
        var items = hasMore ? gifs.Take(request.PageSize).ToList() : gifs;

        string? nextCursor = hasMore ? EncodeCursor(items[^1].CreatedAt, items[^1].Id) : null;

        var results = items.Select(g => new GifResult(g.Id, g.Url, g.Caption, g.AuthorId, g.CreatedAt)).ToList();

        return new PagedResult<GifResult>(results, nextCursor, hasMore);
    }
}
