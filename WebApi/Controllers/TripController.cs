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
    }
}