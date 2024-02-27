

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SwadeshiApp.DTO;
using System.ComponentModel.DataAnnotations;

namespace SaleCRMApp.Models
{
    public class UserEntity 
    {

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string PinCode { get; set; }

        public UserProfile GetUserDetail(string email)
        {

            UserProfile userEntity = new UserProfile();

            string connectionString = "server=LAPTOP-1UGATPN9; database=SwadeshiApp; Trusted_Connection=True;TrustServerCertificate=True; Integrated Security=true; Encrypt=true;";

            // SQL query to execute
            string sqlQuery = "SELECT FirstName, LastName, MobileNo, Gender, Address, city, PinCode FROM AspNetUsers WHERE Email = @Email";

            try
            {
                // Create a connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a command
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        // Execute the query
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows
                            if (reader.HasRows)
                            {
                                // Iterate through the rows
                                while (reader.Read())
                                {
                                    // Access columns by name or index
                                   // UserEntity userEntity = new UserEntity();
                                   userEntity.FirstName= reader["FirstName"].ToString();
                                    userEntity.LastName = reader["LastName"].ToString();
                                    userEntity.MobileNo = reader["MobileNo"].ToString();
                                    userEntity.Gender = reader["Gender"].ToString();
                                    userEntity.city = reader["city"].ToString();
                                    userEntity.PinCode = reader["PinCode"].ToString();
                                    userEntity.Address = reader["Address"].ToString();
                                    

                                   
                                }
                                return userEntity;
                            }
                            else
                            {
                                Console.WriteLine("No data found.");
                                return userEntity;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
            return userEntity;
        }

        //public UserProfile SaveUserDetail(string email)
        //{

        //    UserProfile userEntity = new UserProfile();

        //    string connectionString = "server=DESKTOP-E4GKT8M\\MSSQLSERVER01; database=SwadeshiApp; Trusted_Connection=True;TrustServerCertificate=True; Integrated Security=true; Encrypt=true;";

        //    // SQL query to execute
        //    string sqlQuery = "UPDATE AspNetUsers SET FirstName=@FirstName, LastName=@LastName, MobileNo=@MobileNo, Gender=@Gender, Address=@Address, city=@city, PinCode=@PinCode  WHERE Email = @Email";

        //    try
        //    {
        //        // Create a connection
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            // Open the connection
        //            connection.Open();

        //            // Create a command
        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.AddWithValue("@Email", email);
        //                // Execute the query
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    // Check if there are rows
        //                    if (reader.HasRows)
        //                    {
        //                        // Iterate through the rows
        //                        while (reader.Read())
        //                        {
        //                            // Access columns by name or index
        //                            // UserEntity userEntity = new UserEntity();
        //                            userEntity.FirstName = reader["FirstName"].ToString();
        //                            userEntity.LastName = reader["LastName"].ToString();
        //                            userEntity.MobileNo = reader["MobileNo"].ToString();
        //                            userEntity.Gender = reader["Gender"].ToString();
        //                            userEntity.city = reader["city"].ToString();
        //                            userEntity.PinCode = reader["PinCode"].ToString();
        //                            userEntity.Address = reader["Address"].ToString();



        //                        }
        //                        return userEntity;
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("No data found.");
        //                        return userEntity;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }

        //    Console.ReadLine();
        //    return userEntity;
        //}

        public List<UserProfile> GetSellerList()
        {
            List<UserProfile> userList = new List<UserProfile>();
            string role = "Seller";
            string connectionString = "server=LAPTOP-1UGATPN9; database=SwadeshiApp; Trusted_Connection=True;TrustServerCertificate=True; Integrated Security=true; Encrypt=true;";

            // SQL query to execute
            string sqlQuery = @"
        SELECT 
            U.FirstName, 
            U.LastName, 
            U.MobileNo, 
            U.Gender, 
            U.Address, 
            U.city, 
            U.PinCode 
        FROM 
            AspNetUsers U
        JOIN 
            AspNetUserRoles UR ON U.Id = UR.UserId
        JOIN 
            AspNetRoles R ON UR.RoleId = R.Id
        WHERE 
            R.Name = @role";

            try
            {
                // Create a connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a command
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@role", role);
                        // Execute the query
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows
                            while (reader.Read())
                            {
                                // Create a new UserProfile object for each row
                                UserProfile userEntity = new UserProfile();
                                // Access columns by name and populate the UserProfile object
                                userEntity.FirstName = reader["FirstName"].ToString();
                                userEntity.LastName = reader["LastName"].ToString();
                                userEntity.MobileNo = reader["MobileNo"].ToString();
                                userEntity.Gender = reader["Gender"].ToString();
                                userEntity.city = reader["city"].ToString();
                                userEntity.PinCode = reader["PinCode"].ToString();
                                userEntity.Address = reader["Address"].ToString();

                                userList.Add(userEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Handle exception or log error as needed
            }

            return userList;
        }



        public UserProfile SaveUserDetail(string email, UserProfile updatedUser)
        {
            string connectionString = "server=LAPTOP-1UGATPN9; database=SwadeshiApp; Trusted_Connection=True;TrustServerCertificate=True; Integrated Security=true; Encrypt=true;";

            // SQL query to update user details
            string updateQuery = "UPDATE AspNetUsers SET FirstName=@FirstName, LastName=@LastName, MobileNo=@MobileNo, Gender=@Gender, Address=@Address, city=@City, PinCode=@PinCode  WHERE Email = @Email";

            // SQL query to fetch updated user details
            string selectQuery = "SELECT FirstName, LastName, MobileNo, Gender, Address, city, PinCode FROM AspNetUsers WHERE Email = @Email";

            try
            {
                // Create a connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Update user details
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Email", email);
                        updateCommand.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
                        updateCommand.Parameters.AddWithValue("@LastName", updatedUser.LastName);
                        updateCommand.Parameters.AddWithValue("@MobileNo", updatedUser.MobileNo);
                        updateCommand.Parameters.AddWithValue("@Gender", updatedUser.Gender);
                        updateCommand.Parameters.AddWithValue("@Address", updatedUser.Address);
                        updateCommand.Parameters.AddWithValue("@City", updatedUser.city);
                        updateCommand.Parameters.AddWithValue("@PinCode", updatedUser.PinCode);

                        // Execute the update query
                        updateCommand.ExecuteNonQuery();
                    }

                    // Fetch updated user details
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Email", email);

                        // Execute the query
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            // Check if there are rows
                            if (reader.HasRows)
                            {
                                // Iterate through the rows
                                while (reader.Read())
                                {
                                    // Access columns by name or index
                                    updatedUser.FirstName = reader["FirstName"].ToString();
                                    updatedUser.LastName = reader["LastName"].ToString();
                                    updatedUser.MobileNo = reader["MobileNo"].ToString();
                                    updatedUser.Gender = reader["Gender"].ToString();
                                    updatedUser.city = reader["City"].ToString();
                                    updatedUser.PinCode = reader["PinCode"].ToString();
                                    updatedUser.Address = reader["Address"].ToString();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found.");
                            }
                        }
                    }
                }

                return updatedUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return updatedUser; // or handle the error as needed
            }
        }

    }
}
