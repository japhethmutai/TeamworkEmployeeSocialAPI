using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Persistence;

namespace TeamworkApp.Application.Posts.Queries.GetArticleById;

public record GetArticleByIdQuery(Guid ArticleId) : IRequest<ArticleResult>;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleResult>
{
    private readonly IApplicationDbContext _context;

    public GetArticleByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArticleResult> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == request.ArticleId, cancellationToken);

        if (article == null)
        {
            throw new ArticleNotFoundException(request.ArticleId);
        }

        return new ArticleResult(article.Id, article.Title, article.Content, article.AuthorId, article.CreatedAt);
    }
}
