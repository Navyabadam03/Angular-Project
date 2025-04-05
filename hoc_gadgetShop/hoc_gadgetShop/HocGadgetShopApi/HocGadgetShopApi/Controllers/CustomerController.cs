using HocGadgetShopApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace HocGadgetShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        public ActionResult SaveCustomerData(CustomerRequestDto requestDto)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_SaveCustomerDetails",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            command.Parameters.AddWithValue("@CustomerId", requestDto.CustomerId);
            command.Parameters.AddWithValue("@FirstName", requestDto.FirstName);
            command.Parameters.AddWithValue("@LastName", requestDto.LastName);
            command.Parameters.AddWithValue("@Email", requestDto.Email);
            command.Parameters.AddWithValue("@Phone", requestDto.Phone);
            command.Parameters.AddWithValue("@RegistrationDate", requestDto.RegistrationDate);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }

        [HttpGet]
        public ActionResult GetCustomerData()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_GetCustomerDetails",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();

            List<CustomerDto> customers = new List<CustomerDto>();

            using(SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CustomerDto customerDto = new CustomerDto();
                    customerDto.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    customerDto.FirstName = Convert.ToString(reader["FirstName"]);
                    customerDto.LastName = Convert.ToString(reader["LastName"]);
                    customerDto.Phone = Convert.ToString(reader["Phone"]);
                    customerDto.Email = Convert.ToString(reader["Email"]);
                    customerDto.RegistrationDate = Convert.ToString(reader["RegistrationDate"]);

                    customers.Add(customerDto);
                }
            }

            connection.Close();

            return Ok(JsonConvert.SerializeObject(customers));
        }

        [HttpDelete]
        public ActionResult DeleteCustomerData(int customerId)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_DeleteCustomerDetails",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            command.Parameters.AddWithValue("@CustomerId", customerId);
            
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }
        [HttpPut]
        public ActionResult UpdateCustomerData(CustomerRequestDto customerRequest)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "some DB connection string"
            };

            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_UpdateCustomerDetails",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();

            command.Parameters.AddWithValue("@CustomerId", customerRequest.CustomerId);
            command.Parameters.AddWithValue("@FirstName", customerRequest.FirstName);
            command.Parameters.AddWithValue("@LastName", customerRequest.LastName);
            command.Parameters.AddWithValue("@Email", customerRequest.Email);
            command.Parameters.AddWithValue("@Phone", customerRequest.Phone);
            command.Parameters.AddWithValue("@RegistrationDate", customerRequest.RegistrationDate);

            command.ExecuteNonQuery();

            connection.Close();

            return Ok();
        }
    }
}
