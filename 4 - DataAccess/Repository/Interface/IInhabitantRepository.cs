using PruebaBackend.Model.Context;

namespace PruebaBackend.DataAccess.Interface
{
    public interface IInhabitantRepository
    {
        Task<IEnumerable<Inhabitant>> GetInhabitants();
        Task<IEnumerable<Inhabitant>> GetInhabitantsByGender(string gender);
        Task SerializeIdescatDataBBDD();
    }
}