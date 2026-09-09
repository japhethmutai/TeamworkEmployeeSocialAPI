using MediatR;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Posts.Commands;

public record CreateGifCommand(string Url, string Caption, Guid AuthorId) : IRequest<GifResult>;

public class CreateGifCommandHandler : IRequestHandler<CreateGifCommand, GifResult>
{
    private readonly IApplicationDbContext _context;

    public CreateGifCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GifResult> Handle(CreateGifCommand request, CancellationToken cancellationToken)
    {
        var gif = new Gif
        {
            Url = request.Url,
            Caption = request.Caption,
            AuthorId = request.AuthorId
        };

        _context.Gifs.Add(gif);
        await _context.SaveChangesAsync(cancellationToken);

        return new GifResult(gif.Id, gif.Url, gif.Caption, gif.AuthorId, gif.CreatedAt);
    }
}