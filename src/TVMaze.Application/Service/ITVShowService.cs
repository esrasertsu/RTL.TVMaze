using TVMaze.Core;

namespace TVMaze.Application
{
    public interface ITVShowService
    {
        Task<IEnumerable<TvShowWithCast>> GetTvShows(int page);
        Task<TvShowWithCast> GetTvShow(int id);
        Task<bool> AddTvShow(TvShowWithCast show);
        Task<bool> DeleteTvShow(int id);


    }
}
