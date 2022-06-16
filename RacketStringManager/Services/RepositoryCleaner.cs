using RacketStringManager.Services.Repository;

namespace RacketStringManager.Services;

public class RepositoryCleaner : DataRepository, IRepositoryCleaner
{
    private readonly IJobRepository _jobRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRacketRepository _racketRepository;
    private readonly IStringingRepository _stringRepository;
        
    public RepositoryCleaner(IJobRepository jobRepository, IPlayerRepository playerRepository, IRacketRepository racketRepository, IStringingRepository stringRepository)
    {
        _jobRepository = jobRepository;
        _playerRepository = playerRepository;
        _racketRepository = racketRepository;
        _stringRepository = stringRepository;
    }

    public void CleanRepository()
    {
        CleanupStrings();
        CleanupRackets();
        CleanupPlayers();
    }

    private void CleanupStrings()
    {
        var strings = _stringRepository.GetAll();
        foreach (var entity in strings)
        {
            if (_jobRepository.GetAllJobs().Any(x => x.StringId == entity.Id))
            {
                continue;
            }

            _stringRepository.Delete(entity);
        }
    }

    private void CleanupRackets()
    {
        var rackets = _racketRepository.GetAll();
        foreach (var entity in rackets)
        {
            if (_jobRepository.GetAllJobs().Any(x => x.RacketId == entity.Id))
            {
                continue;
            }

            _racketRepository.Delete(entity);
        }
    }

    private void CleanupPlayers()
    {
        var players = _playerRepository.GetAll();
        foreach (var entity in players)
        {
            if (_jobRepository.GetAllJobs().Any(x => x.PlayerId == entity.Id))
            {
                continue;
            }

            _playerRepository.Delete(entity);
        }
    }
}