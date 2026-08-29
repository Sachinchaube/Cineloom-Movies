using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Controllers.Api
{
    [ApiController]
    [Route("api/theatres")]
    public class TheatresApiController : ControllerBase
    {
        private readonly ITheatreRepo _theatreRepo;
        private readonly IMapper _mapper;

        public TheatresApiController(ITheatreRepo theatreRepo, IMapper mapper)
        {
            _theatreRepo = theatreRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TheatreDto>>> GetTheatres()
        {
            var theatres = await _theatreRepo.GetTheatresAsync();
            return Ok(_mapper.Map<List<TheatreDto>>(theatres));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TheatreDto>> GetTheatreById(int id)
        {
            var theatre = await _theatreRepo.GetTheatreById(id);
            if (theatre == null)
            {
                return NotFound(new { message = $"Theatre with ID {id} not found." });
            }

            return Ok(_mapper.Map<TheatreDto>(theatre));
        }
    }
}
