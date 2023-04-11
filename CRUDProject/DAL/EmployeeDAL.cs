using CRUDProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUDProject.DAL
{
    public class EmployeeDAL
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;
        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                //Import System.Data
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[uspGetEmployees]";
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();
                while (dr.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);
                    employeeList.Add(employee);
                }
                _connection.Close();
            }
            return employeeList;
        }
        //insert
        public bool Insert(Employee model)
        {
            int id = 0;
            using(_connection = new SqlConnection(GetConnectionString())) 
            { 
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[uspInsertGetEmployees]";
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0 ? true : false;
        }

        //update get by Id
        public Employee GetById(int id)
        {
            Employee employee = new Employee();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                //Import System.Data
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[uspGetEmployeesById]";
                _command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();
                while (dr.Read())
                {
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);

                }
                _connection.Close();
            }
            return employee;
        }
        //Update
        public bool Update(Employee model)
        {
            int deleteRowCount = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[uspUpdateGetEmployees]";
                _command.Parameters.AddWithValue("@Id", model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Salary", model.Salary);

                _connection.Open();
                deleteRowCount = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return deleteRowCount > 0 ? true : false;
        }
        //Delete
        public bool Delete(int id)
        {
            int idd = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[uspDeleteGetEmployees]";
                _command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                idd = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return idd > 0 ? true : false;
        }
    }
}
