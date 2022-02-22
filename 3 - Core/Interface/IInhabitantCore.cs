using PruebaBackend.Model.DTO;

namespace PruebaBackend.Core.Interface
{
    public interface IInhabitantCore
    {
        Task<IEnumerable<InhabitantDTO>> GetInhabitants();
        Task<IEnumerable<InhabitantDTO>> GetInhabitantsByGender(string gender);
        Task GetIdescatData();
    }
}