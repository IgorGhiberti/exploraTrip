using Application.DTOs.TripDTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripServices _tripServices;
        public TripController(ITripServices tripServices)
        {
            _tripServices = tripServices;
        }

        /// <summary>
        /// Create a new trip.
        /// </summary>
        /// <param name="tripDto">Trip body</param>
        /// <returns>Trip</returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTrip(CreateTripDTO tripDto)
        {
            var result = await _tripServices.AddTrip(tripDto);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToCreateResult($"api/trip/{result.Data!.Id}");
        }

        /// <summary>
        /// Get the trip with users relateds
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Trip</returns>
        /// /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripById(Guid id)
        {
            var result = await _tripServices.GetTripById(id);
            if (!result.IsSuccess)
                return result.ToNotFoundResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Update the current trip
        /// </summary>
        /// <param name="id">Trip id</param>
        /// <param name="tripDto">Trip update body</param>
        /// <returns>Trip</returns>
        /// /// <response code="20">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTrip(Guid id, UpdateTripDTO tripDto)
        {
            var result = await _tripServices.UpdateTrip(id, tripDto);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Delete a trip.
        /// </summary>
        /// <param name="id">Trip id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var result = await _tripServices.DeleteTrip(id);
            if (!result.IsSuccess)
                return result.ToNotFoundResult();
            return result.ToNoContentResult();
        }
        
        /// <summary>
        /// Get all trips for user
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <returns>All trips that the user has access.</returns>
        [HttpGet("userTrips/{email}")]
        public async Task<IActionResult> GetAllTripsByUserEmail(string email)
        {
            var result = await _tripServices.GetAllTripsByUserEmail(email);
            if (!result.IsSuccess)
                return result.ToNotFoundResult();
            return result.ToOkResult();
        }
        
    }
}