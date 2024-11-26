namespace app.enchantedair.api.Controllers
{
    using Dapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class GenericController<TId, TModel> : IGenericController<TId, TModel>
    {
        private readonly IDbConnection _dbConnection;

        // Constructor receives IConfiguration to fetch the connection string.
        public GenericController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("enchantedair");
            _dbConnection = new MySqlConnection(connectionString);
            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open(); 
        }

        public async Task<IActionResult> CreateAsync([FromBody] TModel model)
        {
            try
            {

                var result = await _dbConnection.InsertAsync(model);
                return result != null
                    ? new OkObjectResult(result)
                    : new BadRequestObjectResult("Failed to create the record.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteSync(TId id)
        {
            try
            {
                var result = await _dbConnection.DeleteAsync<TModel>(id);
                return result > 0
                    ? new OkResult()
                    : new NotFoundObjectResult($"Record with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var models = await _dbConnection.GetListAsync<TModel>();
                return models != null && models.AsList().Count > 0
                    ? new OkObjectResult(models)
                    : new NotFoundObjectResult("No records found.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }


        public async Task<IActionResult> GetAsync(TId id)
        {
            try
            {
                var model = await _dbConnection.GetAsync<TModel>(id);
                return model != null
                    ? new OkObjectResult(model)
                    : new NotFoundObjectResult($"Record with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateSync(TId id, [FromBody] TModel model)
        {
            try
            {
                var updated = await _dbConnection.UpdateAsync(model);
                return updated > 0
                    ? new OkResult()
                    : new NotFoundObjectResult($"Failed to update the record with ID {id}.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }

        
    }


}
