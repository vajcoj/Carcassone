using AutoMapper;
using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;

namespace CarcassoneAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TilePutted, Tile>();

            CreateMap<Tile, TilePutted>()
                .ForMember(putted => putted.ImageUrl,
                    opt => opt.MapFrom(tile => tile.TileType.ImageUrl))
                .ForMember(dest => dest.Top,
                    opt => opt.MapFrom(tile => tile.TileType.Top()))
                .ForMember(dest => dest.Right,
                    opt => opt.MapFrom(tile => tile.TileType.Right()))
                .ForMember(dest => dest.Bottom,
                    opt => opt.MapFrom(tile => tile.TileType.Bottom()))
                .ForMember(dest => dest.Left,
                    opt => opt.MapFrom(tile => tile.TileType.Left()));

            CreateMap<TileType, TileToPut>()
                .ForMember(dest => dest.Top,
                    opt => opt.MapFrom(tile => tile.Top()))
                .ForMember(dest => dest.Right,
                    opt => opt.MapFrom(tile => tile.Right()))
                .ForMember(dest => dest.Bottom,
                    opt => opt.MapFrom(tile => tile.Bottom()))
                .ForMember(dest => dest.Left,
                    opt => opt.MapFrom(tile => tile.Left()));

        }
    }
}
