﻿using MoviePro.Models.Database;
using MoviePro.Models.TmDb;
using System.Threading.Tasks;

namespace MoviePro.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);
    }
}