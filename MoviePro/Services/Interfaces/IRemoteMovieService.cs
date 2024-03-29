﻿using MoviePro.Enums;
using MoviePro.Models.TmDb;
using System.Threading.Tasks;

namespace MoviePro.Services.Interfaces
{
    public interface IRemoteMovieService
    {
        Task<MovieDetail> MovieDetailAsync(int id);
        Task<MovieSearch> SearchMoviesAsync(MovieCategory category, int count);
        Task<ActorDetail> ActorDetailAsync(int id);
    }
}
