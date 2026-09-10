using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Common;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Application.Posts;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Comments.Queries;

public record GetCommentsByPostQuery(Guid PostId, string? Cursor, int PageSize) : IRequest<PagedResult<CommentResult>>;

public class GetCommentsByPostQueryHandler : IRequestHandler<GetCommentsByPostQuery, PagedResult<CommentResult>>
{
    private readonly IApplicationDbContext _context;

    public GetCommentsByPostQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    private static string EncodeCursor(DateTime createdAt, Guid id) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{createdAt.Ticks}|{id}"));

    private static (DateTime CreatedAt, Guid Id) DecodeCursor(string cursor)
    {
        var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
        var parts = raw.Split('|');
        return (new DateTime(long.Parse(parts[0]), DateTimeKind.Utc), Guid.Parse(parts[1]));
    }

    public async Task<PagedResult<CommentResult>> Handle(GetCommentsByPostQuery request, CancellationToken cancellationToken)
    {
        var postExists = await _context.Posts.AnyAsync(p => p.Id == request.PostId, cancellationToken);

        if(!postExists)
        {
            throw new PostNotFoundException(request.PostId);
        }

        IQueryable<Comment> query = _context.Comments
            .Where(c => c.PostId == request.PostId)
            .OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id);

        if (!string.IsNullOrEmpty(request.Cursor))
        {
            var (cursorCreatedAt, cursorId) = DecodeCursor(request.Cursor);

            query = query.Where(c => c.CreatedAt < cursorCreatedAt || (c.CreatedAt == cursorCreatedAt && c.Id < cursorId));
        }
        var comments = await query.Take(request.PageSize + 1).ToListAsync(cancellationToken);

        var hasMore = comments.Count > request.PageSize;
        var items = hasMore ? comments.Take(request.PageSize).ToList() : comments;

        string? nextCursor = hasMore ? EncodeCursor(items[^1].CreatedAt, items[^1].Id) : null;

        var results = items.Select(a => new CommentResult(a.Id, a.PostId, a.Content, a.AuthorId, a.CreatedAt)).ToList();

        return new PagedResult<CommentResult>(results, nextCursor, hasMore);
    }
}
