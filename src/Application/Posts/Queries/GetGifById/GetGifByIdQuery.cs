using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Persistence;

namespace TeamworkApp.Application.Posts.Queries.GetGifById;

public record GetGifByIdQuery(Guid GifId) : IRequest<GifResult>;

public class GetGifByIdQueryHandler : IRequestHandler<GetGifByIdQuery, GifResult>
{
    private readonly IApplicationDbContext _context;

    public GetGifByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GifResult> Handle(GetGifByIdQuery request, CancellationToken cancellationToken)
    {
        var gif = await _context.Gifs.FirstOrDefaultAsync(g => g.Id == request.GifId, cancellationToken);

        if(gif == null)
        {
            throw new GifNotFoundException(request.GifId);
        }

        return new GifResult(gif.Id, gif.Url, gif.Caption, gif.AuthorId, gif.CreatedAt);
    }
}

