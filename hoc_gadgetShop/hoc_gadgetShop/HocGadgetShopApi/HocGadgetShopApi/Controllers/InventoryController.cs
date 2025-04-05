using HocGadgetShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace HocGadgetShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public ActionResult SaveInventoryData(InventoryRequestDto requestDto)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString= "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_SaveInventoryData",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            command.Parameters.AddWithValue("@ProductId", requestDto.ProductId);
            command.Parameters.AddWithValue("@ProductName", requestDto.ProductName);
            command.Parameters.AddWithValue("@AvailableQty", requestDto.AvailableQty);
            command.Parameters.AddWithValue("@ReOrderPoint", requestDto.ReOrderPoint);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }
        [HttpGet]
        public ActionResult GetInventoryData()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_GetInventoryData",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();

            List<InventoryDto> respone = new List<InventoryDto>();

            using (SqlDataReader sqlDataReader = command.ExecuteReader())
            {
                while(sqlDataReader.Read())
                {                   
                    InventoryDto inventoryDto = new InventoryDto();
                    inventoryDto.ProductId = Convert.ToInt32(sqlDataReader["ProductId"]);
                    inventoryDto.ProductName = Convert.ToString(sqlDataReader["ProductName"]);
                    inventoryDto.AvailableQty = Convert.ToInt32(sqlDataReader["AvailableQty"]);
                    inventoryDto.ReOrderPoint = Convert.ToInt32(sqlDataReader["ReOrderPoint"]);

                    respone.Add(inventoryDto);
                }
            }

            connection.Close();

            return Ok(JsonConvert.SerializeObject(respone));
        }

        [HttpDelete]
        public ActionResult DeleteInventoryData(int productId)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_DeleteInventoryDetails",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();

            command.Parameters.AddWithValue("@ProductId", productId);

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }
        [HttpPut]
        public ActionResult UpdateInventoryData(InventoryRequestDto inventoryRequest)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_UpdateInventoryData",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();

            command.Parameters.AddWithValue("@ProductId", inventoryRequest.ProductId);
            command.Parameters.AddWithValue("@ProductName", inventoryRequest.ProductName);
            command.Parameters.AddWithValue("@AvailableQty", inventoryRequest.AvailableQty);
            command.Parameters.AddWithValue("@ReOrderPoint", inventoryRequest.ReOrderPoint);

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }
    }
}