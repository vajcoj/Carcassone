using AutoMapper;
using CarcassoneAPI.DTOs;
using CarcassoneAPI.Models;

namespace CarcassoneAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TilePuttedDTO, Tile>();
            

            // get all tiles of board
            CreateMap<Tile, TilePuttedDTO>()
                .ForMember(putted => putted.ImageUrl,
                    opt => opt.MapFrom(tile => tile.TileType.ImageUrl))
                .ForMember(dest => dest.Top,
                    opt => opt.MapFrom(tile => tile.GetTerrainAt(TilePosition.Top)))
                .ForMember(dest => dest.Right,
                    opt => opt.MapFrom(tile => tile.GetTerrainAt(TilePosition.Right)))
                .ForMember(dest => dest.Bottom,
                    opt => opt.MapFrom(tile => tile.GetTerrainAt(TilePosition.Bottom)))
                .ForMember(dest => dest.Left,
                    opt => opt.MapFrom(tile => tile.GetTerrainAt(TilePosition.Left)));

            // tile to put
            CreateMap<TileType, TileToPutDTO>()
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
