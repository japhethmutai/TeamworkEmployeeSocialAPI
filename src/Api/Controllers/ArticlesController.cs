using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamworkApp.Application.Common;
using TeamworkApp.Application.Posts;
using TeamworkApp.Application.Posts.Commands;
using TeamworkApp.Application.Posts.Queries.GetArticleById;
using TeamworkApp.Application.Posts.Queries.GetArticles;

namespace TeamworkApp.Api.Controllers;

public record CreateArticleRequest(string Title, string Content);

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ArticleResult>> Create(CreateArticleRequest request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateArticleCommand(request.Title, request.Content, authorId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ArticleResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetArticleByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<ArticleResult>>> GetArticles(
        [FromQuery] string? cursor,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default
    )
    {
        var clampedPageSize = Math.Clamp(pageSize, 1, 50);
        var query = new GetArticlesQuery(cursor, clampedPageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
