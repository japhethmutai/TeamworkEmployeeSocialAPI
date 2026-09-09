using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamworkApp.Application.Common;
using TeamworkApp.Application.Posts;
using TeamworkApp.Application.Posts.Commands;
using TeamworkApp.Application.Posts.Queries.GetGifById;
using TeamworkApp.Application.Posts.Queries.GetGifs;

namespace TeamworkApp.Api.Controllers;

public record CreateGifRequest(string Url, string Caption);

[ApiController]
[Route("api/[controller]")]
public class GifsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GifsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GifResult>> Create(CreateGifRequest request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateGifCommand(request.Url, request.Caption, authorId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<GifResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetGifByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<GifResult>>> GetGifs(
        [FromQuery] string? cursor,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default
    )
    {
        var clampedPageSize = Math.Clamp(pageSize, 1, 50);
        var query = new GetGifsQuery(cursor, clampedPageSize);
        var results = await _mediator.Send(query, cancellationToken);
        return Ok(results);
    }
}
