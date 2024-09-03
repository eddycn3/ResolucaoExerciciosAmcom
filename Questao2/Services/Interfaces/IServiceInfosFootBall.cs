using Questao2.Dtos;

namespace Questao2.Services.Interfaces
{
    public interface IServiceInfosFootballMatches
    {
        Task<IEnumerable<FootballMatchDTO>> GetFootballMatches(int ano, string time);
    }
}