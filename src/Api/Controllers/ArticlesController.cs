using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamworkApp.Application.Posts;
using TeamworkApp.Application.Posts.Commands;

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
}
