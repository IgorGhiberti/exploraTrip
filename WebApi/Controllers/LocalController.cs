using Application.DTOs.LocalDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocalController : ControllerBase
{
    private readonly ILocalService _localService;

    public LocalController(ILocalService localService)
    {
        _localService = localService;
    }

    /// <summary>
    /// Create a new local.
    /// </summary>
    /// <param name="localDto">Local body with required data.</param>
    /// <returns>Local</returns>
    /// <response code="201">Created</response>
    /// <response code="400">BadRequest</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocal(CreateLocalDTO localDto)
    {
        var result = await _localService.AddLocal(localDto);
        if (!result.IsSuccess)
            return result.ToBadRequestResult();
        return result.ToCreateResult($"api/local/{result.Data!.LocalId}");
    }

    /// <summary>
    /// Get all locals related to a trip.
    /// </summary>
    /// <param name="tripId">Trip identifier</param>
    /// <returns>List of locals</returns>
    /// <response code="200">Ok</response>
    /// <response code="400">BadRequest</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLocal(Guid tripId)
    {
        var result = await _localService.ListLocalByTrip(tripId);
        if (!result.IsSuccess)
            return result.ToBadRequestResult();
        return result.ToOkResult();
    }

    /// <summary>
    /// Delete a local.
    /// </summary>
    /// <param name="localID">Local identifier</param>
    /// <returns>No content</returns>
    /// <response code="204">NoContent</response>
    /// <response code="400">BadRequest</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteLocal(Guid localID)
    {
        var result = await _localService.DeleteLocal(localID);
        if (!result.IsSuccess)
            return result.ToBadRequestResult();
        return result.ToNoContentResult();
    }

    /// <summary>
    /// Update an existing local.
    /// </summary>
    /// <param name="localDto">Local update body</param>
    /// <returns>Updated local</returns>
    /// <response code="200">Ok</response>
    /// <response code="400">BadRequest</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLocal(UpdateLocalDTO localDto)
    {
        var result = await _localService.UpdateLocal(localDto);
        if (!result.IsSuccess)
            return result.ToBadRequestResult();
        return result.ToOkResult();
    }
}