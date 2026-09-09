using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Common;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Posts.Queries.GetArticles;

public record GetArticlesQuery(string? Cursor, int PageSize) : IRequest<PagedResult<ArticleResult>>;

public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, PagedResult<ArticleResult>>
{
    private readonly IApplicationDbContext _context;
    public GetArticlesQueryHandler(IApplicationDbContext context)
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

    public async Task<PagedResult<ArticleResult>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Article> query = _context.Articles
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.Id);

        if (!string.IsNullOrEmpty(request.Cursor))
        {
            var (cursorCreatedAt, cursorId) = DecodeCursor(request.Cursor);

            query = query.Where(a => a.CreatedAt < cursorCreatedAt || (a.CreatedAt == cursorCreatedAt && a.Id < cursorId));
        }

        var articles = await query.Take(request.PageSize + 1).ToListAsync(cancellationToken);

        var hasMore = articles.Count > request.PageSize;
        var items = hasMore ? articles.Take(request.PageSize).ToList() : articles;

        string? nextCursor = hasMore ? EncodeCursor(items[^1].CreatedAt, items[^1].Id) : null;

        var results = items.Select(a => new ArticleResult(a.Id, a.Title, a.Content, a.AuthorId, a.CreatedAt)).ToList();

        return new PagedResult<ArticleResult>(results, nextCursor, hasMore);
    }
}
