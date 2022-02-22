using PruebaBackend.DataAccess.Interface;
using PruebaBackend.Model.Context;
using MySql.Data.MySqlClient;
using Dapper;
using Newtonsoft.Json.Linq;

namespace PruebaBackend.DataAccess.Repository
{
    public class InhabitantRepository : IInhabitantRepository
    {
        private readonly string? _connectionString;
        private readonly string? _tableName;
        private string? _sql;
        private static readonly HttpClient _client = new HttpClient();

        public InhabitantRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
            _tableName = configuration.GetValue<string>("DBInfo:TableName");
        }

        public async Task<IEnumerable<Inhabitant>> GetInhabitants()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    _sql = $"SELECT * FROM {_tableName}";

                    var result = await conn.QueryAsync<Inhabitant>(_sql);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Inhabitant>> GetInhabitantsByGender(string gender)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    _sql = $"SELECT * FROM {_tableName} WHERE gender = '{gender}'";

                    return await conn.QueryAsync<Inhabitant>(_sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetIdescatData()
        {
            try
            {
                List<List<Inhabitant>> inhabitantNestedList = new List<List<Inhabitant>>();

                string responseBody = await _client.GetStringAsync("https://api.idescat.cat/pob/v1/geo.json?tipus=com&lang=es");

                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }

        private List<List<Inhabitant>> ReturnPopulatedInhabitantList()
        {
            try
            {
                List<List<Inhabitant>> inhabitantNestedList = new List<List<Inhabitant>>();
                JObject data = JObject.Parse(GetIdescatData().Result);

                var entries =
                    from d in data["feed"]["entry"]
                    select d["cross:DataSet"];

                var regionTitles =
                    from r in data["feed"]["entry"]
                    select r["title"];

                int index = 0;

                foreach (var entry in entries)
                {
                    var inhabitantEntries =
                        from e in entry["cross:Section"]["cross:Obs"]
                        select e;

                    List<Inhabitant> innerHabitantList = new List<Inhabitant>();


                    foreach (var inhabitantEntry in inhabitantEntries)
                    {
                        Inhabitant inhabitant = new Inhabitant()
                        {
                            region = (string)regionTitles.ElementAt(index),
                            gender = (char)inhabitantEntry["SEX"],
                            value = (int)inhabitantEntry["OBS_VALUE"]
                        };

                        innerHabitantList.Add(inhabitant);
                    }

                    inhabitantNestedList.Add(innerHabitantList);
                    index++;
                }

                return inhabitantNestedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SerializeIdescatDataBBDD()
        {
            try
            {
                var list = ReturnPopulatedInhabitantList();

                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    CleanTable(conn);

                    foreach (var item in list)
                    {
                        foreach (var innerItem in item)
                        {
                            _sql = $"INSERT INTO {_tableName} VALUES (null, @region, @gender, @value)";
                            await conn.ExecuteAsync(_sql, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void CleanTable(MySqlConnection connection)
        {
            // _sql = $"SELECT 1 FROM {_tableName} LIMIT 1";
            _sql = $"SELECT COUNT(*) FROM {_tableName}";
            var result = await connection.QueryAsync(_sql);

            if (result.Count() > 0)
            {
                _sql = $"DELETE FROM {_tableName}";
                await connection.ExecuteAsync(_sql);
            }
        }
    }
}