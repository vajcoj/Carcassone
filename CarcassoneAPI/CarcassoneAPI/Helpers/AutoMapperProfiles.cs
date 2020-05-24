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
            

            // get all tiles of board
            CreateMap<Tile, TilePutted>()
                .ForMember(putted => putted.ImageUrl,
                    opt => opt.MapFrom(tile => tile.TileType.ImageUrl))
                .ForMember(dest => dest.Top,
                    opt => opt.MapFrom(tile => tile.GetTerrain(TilePosition.Top)))
                .ForMember(dest => dest.Right,
                    opt => opt.MapFrom(tile => tile.GetTerrain(TilePosition.Right)))
                .ForMember(dest => dest.Bottom,
                    opt => opt.MapFrom(tile => tile.GetTerrain(TilePosition.Bottom)))
                .ForMember(dest => dest.Left,
                    opt => opt.MapFrom(tile => tile.GetTerrain(TilePosition.Left)));

            // tile to put
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
