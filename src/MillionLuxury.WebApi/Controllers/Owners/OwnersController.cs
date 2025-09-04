namespace MillionLuxury.WebApi.Controllers.Owners;

#region Usings
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillionLuxury.Application.Owners.CreateOwner;
using MillionLuxury.Application.Owners.Dtos;
using MillionLuxury.Application.Owners.GetAllOwners;
using MillionLuxury.Application.Owners.UpdateOwnerPhoto;
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
    /// Adds an image to a owner
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <param name="request">Image data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created image</returns>
    [HttpPost("{id:guid}/photo")]
    public async Task<IActionResult> AddPhotoToOwner(
        Guid id,
        [FromBody] UpdateOwnerPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateOwnerPhotoCommand(id, request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Created();
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
}
