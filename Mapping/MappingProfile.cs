using AutoMapper;
using MovieBookingPro.Models;
using MovieBookingPro.DTOs;

namespace MovieBookingPro.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Movie
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieCreateDto, Movie>();
            CreateMap<MovieEditDto, Movie>().ReverseMap();

            // Theatre
            CreateMap<Theatre, TheatreDto>();
            CreateMap<TheatreCreateDto, Theatre>();
            CreateMap<TheatreEditDto, Theatre>().ReverseMap();

            // Screen
            CreateMap<Screen, ScreenDto>()
                .ForMember(dest => dest.TheatreName,
                    opt => opt.MapFrom(src => src.Theatre != null ? src.Theatre.Name : string.Empty));
            CreateMap<ScreenCreateDto, Screen>();
            CreateMap<ScreenEditDto, Screen>().ReverseMap();

            // ShowSchedule
            CreateMap<ShowSchedule, ShowScheduleDto>()
                .ForMember(dest => dest.MovieTitle,
                    opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Title : string.Empty))
                .ForMember(dest => dest.PosterUrl,
                    opt => opt.MapFrom(src => src.Movie != null ? src.Movie.PosterUrl : null))
                .ForMember(dest => dest.ScreenName,
                    opt => opt.MapFrom(src => src.Screen != null ? src.Screen.ScreenName : string.Empty))
                .ForMember(dest => dest.TheatreName,
                    opt => opt.MapFrom(src => src.Screen != null && src.Screen.Theatre != null ? src.Screen.Theatre.Name : string.Empty))
                .ForMember(dest => dest.SeatCapacity,
                    opt => opt.MapFrom(src => src.Screen != null ? src.Screen.SeatCapacity : 0))
                .ForMember(dest => dest.SeatsBooked,
                    opt => opt.MapFrom(src => src.Bookings != null
                        ? src.Bookings.Where(b => b.Status == BookingStatus.Confirmed).Sum(b => b.SeatCount)
                        : 0));
            CreateMap<ShowScheduleCreateDto, ShowSchedule>();
            CreateMap<ShowScheduleEditDto, ShowSchedule>().ReverseMap();

            // Booking
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.MovieTitle,
                    opt => opt.MapFrom(src => src.Show != null && src.Show.Movie != null ? src.Show.Movie.Title : string.Empty))
                .ForMember(dest => dest.TheatreName,
                    opt => opt.MapFrom(src => src.Show != null && src.Show.Screen != null && src.Show.Screen.Theatre != null ? src.Show.Screen.Theatre.Name : string.Empty))
                .ForMember(dest => dest.ScreenName,
                    opt => opt.MapFrom(src => src.Show != null && src.Show.Screen != null ? src.Show.Screen.ScreenName : string.Empty))
                .ForMember(dest => dest.ShowDate,
                    opt => opt.MapFrom(src => src.Show != null ? src.Show.ShowDate : default))
                .ForMember(dest => dest.ShowTime,
                    opt => opt.MapFrom(src => src.Show != null ? src.Show.ShowTime : default))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}