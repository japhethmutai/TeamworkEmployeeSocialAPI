using MediatR;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Posts.Commands;

public record CreateArticleCommand(string Title, string Content, Guid AuthorId) : IRequest<ArticleResult>;

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleResult>
{
    private readonly IApplicationDbContext _context;
    public CreateArticleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArticleResult> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync(cancellationToken);

        return new ArticleResult(article.Id, article.Title, article.Content, article. AuthorId, article.CreatedAt);
    }
}
