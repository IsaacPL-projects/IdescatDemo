using Microsoft.AspNetCore.Mvc;
using PruebaBackend.Model.DTO;
using PruebaBackend.Core.Interface;

namespace PruebaBackend.Controllers
{
    [ApiController]
    [Route("api/v1/data/region")]
    public class IdescatController
    {
        private readonly IInhabitantCore _inhabitantCore;

        public IdescatController(IInhabitantCore inhabitantCore)
        {
            _inhabitantCore = inhabitantCore;
        }

        [HttpGet]
        public async Task<JsonResult> GetInhabitants()
        {
            try
            {
                var collection = await _inhabitantCore.GetInhabitants();

                var result = collection.GroupBy(item => item.region)
                .Select(group => new
                {
                    Region = group.Key,
                    Data = group.Select(item => new
                    {
                        sex = item.gender,
                        value = item.value
                    })
                });

                JsonResult json = new JsonResult(result);

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("gender/{gender}")]
        public async Task<JsonResult> GetInhabitantsByGender(string gender)
        {
            try
            {
                var collection = await _inhabitantCore.GetInhabitantsByGender(gender);

                var result = collection.GroupBy(item => item.region)
                .Select(group => new
                {
                    Region = group.Key,
                    Data = group.Select(item => new
                    {
                        sex = item.gender,
                        value = item.value
                    })
                });

                JsonResult json = new JsonResult(result);

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task GetIdescatData()
        {
            try
            {
                await _inhabitantCore.GetIdescatData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}