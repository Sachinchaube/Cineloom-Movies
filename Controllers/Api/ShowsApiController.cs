using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Controllers.Api
{
    [ApiController]
    [Route("api/shows")]
    public class ShowsApiController : ControllerBase
    {
        private readonly IShowScheduleRepo _showRepo;
        private readonly IMapper _mapper;

        public ShowsApiController(IShowScheduleRepo showRepo, IMapper mapper)
        {
            _showRepo = showRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShowScheduleDto>>> GetAllShows([FromQuery] int? movieId)
        {
            var shows = movieId.HasValue 
                ? await _showRepo.GetShowsByMovie(movieId.Value)
                : await _showRepo.GetShowsAsync();

            var dtoList = _mapper.Map<List<ShowScheduleDto>>(shows);
            return Ok(dtoList);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShowScheduleDto>> GetShowById(int id)
        {
            var show = await _showRepo.GetShowById(id);
            if (show == null)
            {
                return NotFound(new { message = $"Show with ID {id} not found." });
            }

            return Ok(_mapper.Map<ShowScheduleDto>(show));
        }
    }
}
