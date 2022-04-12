using TVMaze.Core;

namespace TVMaze.Application.Repository
{
    public interface ITVShowReposiory
    {
        Task<IEnumerable<TvShowWithCast>> GetTvShows(int page);
        Task<bool> AddTvShow(TvShowWithCast show);
        Task<bool> DeleteTvShow(int id);

        Task<TvShowWithCast> GetTvShow(int showId);
    }
}
