namespace MillionLuxury.WebApi.Controllers.Properties;

#region Usings
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillionLuxury.Application.Properties.AddImage;
using MillionLuxury.Application.Properties.ChangePrice;
using MillionLuxury.Application.Properties.CreateProperty;
using MillionLuxury.Application.Properties.Dtos;
using MillionLuxury.Application.Properties.GetProperty;
using MillionLuxury.Application.Properties.RemoveImage;
using MillionLuxury.Application.Properties.SearchProperties;
using MillionLuxury.Application.Properties.UpdateProperty;
#endregion

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/properties")]
public class PropertiesController : ControllerBase
{
    #region Private Members
    private readonly ISender sender;
    #endregion

    public PropertiesController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    /// Creates a new property
    /// </summary>
    /// <param name="request">Property creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created property</returns>
    [HttpPost]
    public async Task<IActionResult> CreateProperty(
        [FromBody] CreatePropertyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePropertyCommand(request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(GetProperty),
            new { id = result.Value },
            new { Id = result.Value });
    }

    /// <summary>
    /// Gets a property by ID
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Property details</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProperty(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetPropertyQuery(id);
        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Updates an existing property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="request">Property update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or failure result</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProperty(
        Guid id,
        [FromBody] UpdatePropertyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePropertyCommand(id, request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Changes the price of a property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="request">Price change data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or failure result</returns>
    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> ChangePrice(
        Guid id,
        [FromBody] ChangePriceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePriceCommand(id, request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Adds an image to a property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="request">Image data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created image</returns>
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> AddImage(
        Guid id,
        [FromBody] AddImageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddImageCommand(id, request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Created($"images/{result.Value}", new { Id = result.Value });
    }

    /// <summary>
    /// Removes an image from a property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="imageId">Image ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or failure result</returns>
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    public async Task<IActionResult> RemoveImage(
        Guid id,
        Guid imageId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveImageCommand(id, imageId);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Searches properties with filters
    /// </summary>
    /// <param name="request">Search filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of properties</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchProperties(
        [FromQuery] SearchPropertiesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchPropertiesQuery(request);
        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
