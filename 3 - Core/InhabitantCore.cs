using PruebaBackend.Core.Interface;
using PruebaBackend.DataAccess.Interface;
using PruebaBackend.Model.DTO;
using PruebaBackend.Model.Context;
using System.Linq;
using AutoMapper;
using System.Linq;

namespace PruebaBackend.Core
{
    public class InhabitantCore : IInhabitantCore
    {
        private IInhabitantRepository _habitantRepository;
        private readonly IMapper _mapper;

        public InhabitantCore(IMapper mapper, IInhabitantRepository habitantRepository)
        {
            _habitantRepository = habitantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InhabitantDTO>> GetInhabitants()
        {
            try
            {
                var collection = await _habitantRepository.GetInhabitants();
                var result = _mapper.Map<IEnumerable<InhabitantDTO>>(collection);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<InhabitantDTO>> GetInhabitantsByGender(string gender)
        {
            try
            {
                var collection = await _habitantRepository.GetInhabitantsByGender(gender.ToUpper());
                var result = _mapper.Map<IEnumerable<InhabitantDTO>>(collection);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task GetIdescatData()
        {
            try
            {
                await _habitantRepository.SerializeIdescatDataBBDD();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}