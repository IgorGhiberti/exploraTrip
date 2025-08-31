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

        [HttpPost]
        public async Task<IActionResult> AddTrip(CreateTripDTO tripDto)
        {
            var result = await _tripServices.AddTrip(tripDto);
            return result.ToOkResult();
        }
    }
}