namespace MillionLuxury.WebApi.Controllers.Owners;

#region Usings
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillionLuxury.Application.Owners.CreateOwner;
using MillionLuxury.Application.Owners.DeleteOwner;
using MillionLuxury.Application.Owners.Dtos;
using MillionLuxury.Application.Owners.GetAllOwners;
#endregion

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/owners")]
public class OwnersController : ControllerBase
{
    #region Private Members
    private readonly ISender sender;
    #endregion

    public OwnersController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    /// Creates a new owner
    /// </summary>
    /// <param name="request">Owner creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created owner</returns>
    [HttpPost]
    public async Task<IActionResult> CreateOwner(
        [FromBody] CreateOwnerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOwnerCommand(request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Created($"owners/{result.Value}", new { Id = result.Value });
    }

    /// <summary>
    /// Gets all owners
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all owners</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllOwners(CancellationToken cancellationToken)
    {
        var query = new GetAllOwnersQuery();
        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes an owner by ID
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or failure result</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOwner(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteOwnerCommand(id);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}
