using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamworkApp.Application.Comments;
using TeamworkApp.Application.Comments.Commands;
using TeamworkApp.Application.Comments.Queries;
using TeamworkApp.Application.Common;

namespace TeamworkApp.Api.Controllers;

public record CreateCommentRequest(string Content);

[ApiController]
[Route("api/posts/{postId:guid}/comments")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentResult>> Create(Guid postId, CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateCommentCommand(postId, request.Content, authorId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<CommentResult>>> GetByPostId(
        Guid postId,
        [FromQuery] string? cursor,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default

    )
    {
        var clampedPageSize = Math.Clamp(pageSize, 1, 50);
        var query = new GetCommentsByPostQuery(postId, cursor, clampedPageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
