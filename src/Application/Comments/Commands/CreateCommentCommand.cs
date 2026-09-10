using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Application.Posts;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Comments.Commands;

public record CreateCommentCommand(Guid PostId, string Content, Guid AuthorId) : IRequest<CommentResult>;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResult>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommentResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.AnyAsync(p => p.Id == request.PostId, cancellationToken);

        if(!post)
        {
            throw new PostNotFoundException(request.PostId);
        }

        var comment = new Comment
        {
            PostId = request.PostId,
            Content = request.Content,
            AuthorId = request.AuthorId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new CommentResult(comment.Id, comment.PostId, comment.Content, comment.AuthorId, comment.CreatedAt);
    }
}
