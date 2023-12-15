using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Linq;
//using term_project.BusinessLogic; // Assuming you have a BusinessLogic namespace
//using term_project.DataAccess;   // Assuming you have a DataAccess namespace

namespace term_project
{
    public enum RateFeeOperation
    {
        None,
        Add,
        Update,
        Delete
    }

    public enum CarrierOperation
    {
        None,
        Add,
        Update,
        Delete
    }

    public enum RouteOperation
    {
        None,
        Add,
        Update,
        Delete
    }

    public partial class AdminDashboard : Window
    {
        public string CurrentLogDirectory { get; set; } = "default_log_directory"; // Default value
        public string CurrentDbmsIP { get; set; } = "default_ip_address"; // Default value
        public int CurrentDbmsPort { get; set; } = 3306; // Default port number
        public ObservableCollection<Carrier> Carriers { get; set; }
        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<LogEntry> LogEntries { get; set; }
        public ObservableCollection<RateFee> RateFees { get; set; }


        private RateFeeOperation currentOperation = RateFeeOperation.None;
        private RateFee? selectedRateFeeForEdit;
        private CarrierOperation currentCarrierOperation = CarrierOperation.None;
        private RouteOperation currentRouteOperation = RouteOperation.None;

        private int currentPage = 1;
        private int itemsPerPage = 30;
        private int totalItems = 0;

        public AdminDashboard()
        {
            InitializeComponent();
            this.DataContext = this; // Set the data context for data binding

            Users = new ObservableCollection<User>();
            UsersDataGrid.ItemsSource = Users;

            LogEntries = new ObservableCollection<LogEntry>();
            LogsDataGrid.ItemsSource = LogEntries;

            RateFees = new ObservableCollection<RateFee>();
            RateFeeDataGrid.ItemsSource = RateFees;

            LoadSettings(); // Call this method to load initial settings
            LoadUsers(); // Call a method to load users from the database

            Carriers = new ObservableCollection<Carrier>();
            Routes = new ObservableCollection<Route>();
        }


        private void LoadUsers()
        {
            Users.Clear();
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Users;";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? null : reader.GetInt32("UserId"),
                                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString("Username"),
                                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString("FullName"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString("Role"),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString("Status"),
                                CreationDate = reader.GetDateTime("CreationDate"),
                                LastModifiedDate = reader.GetDateTime("LastModifiedDate")
                            };
                            Users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            var newUser = new User { CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now };
            DatabaseHelper.AddUser(newUser);
            Users.Add(newUser);
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                DatabaseHelper.DeleteUser(selectedUser.UserId ?? 0);
                Users.Remove(selectedUser);
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private void RefreshLogs(object sender, RoutedEventArgs e)
        {
            LogEntries.Clear();
            try
            {
                var logLines = File.ReadAllLines("logs.txt");
                foreach (var line in logLines)
                {
                    var parts = line.Split(',');
                    LogEntries.Add(new LogEntry
                    {
                        Timestamp = DateTime.Parse(parts[0]),
                        Level = parts[1].Trim(),
                        Message = parts[2].Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading logs: {ex.Message}");
            }
        }

        private void AddRateFee(object sender, RoutedEventArgs e)
        {
            // Set up UI for adding a new rate fee (e.g., clear input fields)
            currentOperation = RateFeeOperation.Add;
            // Clear the form or set up for a new entry
        }

        private void UpdateRateFee(object sender, RoutedEventArgs e)
        {
            if (RateFeeDataGrid.SelectedItem is RateFee selectedRateFee)
            {
                selectedRateFeeForEdit = selectedRateFee;
                // Load selected rate fee data into input fields for editing
                // Example: Populate form fields with selectedRateFee data
                currentOperation = RateFeeOperation.Update;
            }
            else
            {
                MessageBox.Show("Please select a rate/fee to update.");
            }
        }

        private void DeleteRateFee(object sender, RoutedEventArgs e)
        {
            if (RateFeeDataGrid.SelectedItem is RateFee selectedRateFee)
            {
                selectedRateFeeForEdit = selectedRateFee;
                currentOperation = RateFeeOperation.Delete;
            }
            else
            {
                MessageBox.Show("Please select a rate/fee to delete.");
            }
        }

        private void RefreshRateFees(object sender, RoutedEventArgs e)
        {
            RateFees.Clear();
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM RateFees;";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                         
                            var rateFee = new RateFee
                            {
                                RateFeeId = reader.GetInt32("RateFeeId"),
                                
                                Rate = reader.GetDecimal("Rate"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description")
                            };
                            RateFees.Add(rateFee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing rate fees: " + ex.Message);
            }
        }

        private void ConfirmAddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                Username = UsernameTextBox.Text,
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                Role = RoleTextBox.Text,
                Status = StatusTextBox.Text,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            DatabaseHelper.AddUser(newUser);

            Users.Add(newUser);

            // Clear text boxes or give some success message
            MessageBox.Show("User added successfully.");
        }

        private void SubmitChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentOperation)
                {
                    case RateFeeOperation.Add:
                        var newRateFee = new RateFee
                        {
                            Rate = decimal.Parse(RateTextBox.Text), // Parsing the text to decimal
                            Description = DescriptionTextBox.Text   // Directly taking the string
                        };
                        DatabaseHelper.AddRateFee(newRateFee);
                        RateFees.Add(newRateFee);
                        MessageBox.Show("Rate fee added successfully.");
                        break;

                    case RateFeeOperation.Update:
                        if (selectedRateFeeForEdit != null)
                        {
                            selectedRateFeeForEdit.Rate = decimal.Parse(RateTextBox.Text);
                            selectedRateFeeForEdit.Description = DescriptionTextBox.Text;
                            DatabaseHelper.UpdateRateFee(selectedRateFeeForEdit);
                            MessageBox.Show("Rate fee updated successfully.");
                        }
                        break;

                    case RateFeeOperation.Delete:
                        if (selectedRateFeeForEdit != null)
                        {
                            DatabaseHelper.DeleteRateFee(selectedRateFeeForEdit.RateFeeId);
                            RateFees.Remove(selectedRateFeeForEdit);
                            MessageBox.Show("Rate fee deleted successfully.");
                        }
                        break;
                }

                currentOperation = RateFeeOperation.None;
                // Reset UI as needed, e.g., clear form fields
                RateTextBox.Clear();
                DescriptionTextBox.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid format for rate. Please enter a valid decimal number.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SubmitCarrierChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentCarrierOperation)
                {
                    case CarrierOperation.Add:
                        var newCarrier = new Carrier
                        {
                            cName = CarrierNameTextBox.Text,
                            dCity = CarrierCityTextBox.Text
                        };
                        DatabaseHelper.AddCarrier(newCarrier);
                        Carriers.Add(newCarrier);
                        MessageBox.Show("Carrier added successfully.");
                        break;

                    case CarrierOperation.Update:
                        // Logic to update the selected carrier
                        break;

                    case CarrierOperation.Delete:
                        // Logic to delete the selected carrier
                        break;
                }

                currentCarrierOperation = CarrierOperation.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SubmitRouteChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentRouteOperation)
                {
                    case RouteOperation.Add:
                        var newRoute = new Route
                        {
                            // Assign properties from the form fields
                        };
                        DatabaseHelper.AddRoute(newRoute);
                        Routes.Add(newRoute);
                        MessageBox.Show("Route added successfully.");
                        break;

                    case RouteOperation.Update:
                        // Logic to update the selected route
                        break;

                    case RouteOperation.Delete:
                        // Logic to delete the selected route
                        break;
                }

                currentRouteOperation = RouteOperation.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }




        /*==========================Carrier==================================*/

        // Add Carrier
        private void AddCarrier(object sender, RoutedEventArgs e)
        {
            var newCarrier = new Carrier
            {
                cName = CarrierNameTextBox.Text,
                dCity = CarrierCityTextBox.Text
            };

            DatabaseHelper.AddCarrier(newCarrier); // This should handle adding to the database
            Carriers.Add(newCarrier); // Add to the ObservableCollection
            MessageBox.Show("Carrier added successfully.");

            // Clear the text boxes after adding
            CarrierNameTextBox.Clear();
            CarrierCityTextBox.Clear();
        }

        // Update Carrier
        private void UpdateCarrier(object sender, RoutedEventArgs e)
        {
            if (CarriersDataGrid.SelectedItem is Carrier selectedCarrier)
            {
                selectedCarrier.cName = CarrierNameTextBox.Text;
                selectedCarrier.dCity = CarrierCityTextBox.Text;

                DatabaseHelper.UpdateCarrier(selectedCarrier); // This should handle updating the database
                MessageBox.Show("Carrier updated successfully.");

            }
            else
            {
                MessageBox.Show("Please select a carrier to update.");
            }
        }

        // Delete Carrier
        private void DeleteCarrier(object sender, RoutedEventArgs e)
        {
            if (CarriersDataGrid.SelectedItem is Carrier selectedCarrier)
            {
                DatabaseHelper.DeleteCarrier(selectedCarrier.CarrierID); // This should handle deletion from the database
                Carriers.Remove(selectedCarrier); // Remove from the ObservableCollection
                MessageBox.Show("Carrier deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a carrier to delete.");
            }
        }

        private void RefreshCarriers(object sender, RoutedEventArgs e)
        {
            try
            {
                var carriers = DatabaseHelper.GetAllCarriers();
                Carriers.Clear();
                foreach (var carrier in carriers)
                {
                    Carriers.Add(carrier);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing carriers: " + ex.Message);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                BackupDirectoryTextBox.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            string backupDirectory = BackupDirectoryTextBox.Text;
            if (string.IsNullOrEmpty(backupDirectory))
            {
                MessageBox.Show("Please select a backup directory.");
                return;
            }

            try
            {
                DatabaseHelper.BackupDatabase(backupDirectory);
                MessageBox.Show("Backup completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during backup: " + ex.Message);
            }
        }


        /*=======================Routes====================================*/

        private void AddRoute(object sender, RoutedEventArgs e)
        {
            // Input validation and conversion from string to respective data types
            if (!int.TryParse(RouteCarrierIDTextBox.Text, out int carrierId) ||
                !int.TryParse(RouteFTLATextBox.Text, out int ftla) ||
                !int.TryParse(RouteLTLATextBox.Text, out int ltla))
            {
                MessageBox.Show("Invalid input format.");
                return;
            }

            var newRoute = new Route
            {
                CarrierID = carrierId,
                FTLA = ftla,
                LTLA = ltla,
                // Add FTLRate, LTLRate, ReefCharge fields after conversion
            };

            DatabaseHelper.AddRoute(newRoute);
            Routes.Add(newRoute); // Assuming 'Routes' is an ObservableCollection<Route>
            MessageBox.Show("Route added successfully.");
        }


        private void UpdateRoute(object sender, RoutedEventArgs e)
        {
            if (RoutesDataGrid.SelectedItem is Route selectedRoute)
            {
                // Input validation and conversion from string to respective data types
                selectedRoute.FTLA = int.Parse(RouteFTLATextBox.Text);
                selectedRoute.LTLA = int.Parse(RouteLTLATextBox.Text);

                DatabaseHelper.UpdateRoute(selectedRoute);
                MessageBox.Show("Route updated successfully.");
            }
            else
            {
                MessageBox.Show("Please select a route to update.");
            }
        }


        private void DeleteRoute(object sender, RoutedEventArgs e)
        {
            if (RoutesDataGrid.SelectedItem is Route selectedRoute)
            {
                DatabaseHelper.DeleteRoute(selectedRoute.RouteID);
                Routes.Remove(selectedRoute);
                MessageBox.Show("Route deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a route to delete.");
            }
        }


        private void RefreshRoutes(object sender, RoutedEventArgs e)
        {
            var updatedRoutes = DatabaseHelper.GetAllRoutes();
            Routes.Clear();
            foreach (var route in updatedRoutes)
            {
                Routes.Add(route);
            }
        }


        /*==========================Settings=================================*/

        private void LoadSettings()
        {
            CurrentLogDirectory = "C:\\Logs";
            CurrentDbmsIP = "159.89.117.198";
            CurrentDbmsPort = 3306;

            CurrentLogDirectoryText.Text = CurrentLogDirectory;
            CurrentDbmsIpAddressText.Text = CurrentDbmsIP;
            CurrentDbmsPortText.Text = CurrentDbmsPort.ToString();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the CurrentLogDirectory property with the value from the LogFilesDirectoryTextBox
            CurrentLogDirectory = LogFilesDirectoryTextBox.Text;

            // Update the CurrentDbmsIP property with the value from the DbmsIpAddressTextBox
            CurrentDbmsIP = DbmsIpAddressTextBox.Text;

            // Validate and update the CurrentDbmsPort property with the value from the DbmsPortTextBox
            if (int.TryParse(DbmsPortTextBox.Text, out int dbmsPort))
            {
                CurrentDbmsPort = dbmsPort;
            }
            else
            {
                MessageBox.Show("Invalid DBMS port number. Please enter a valid number.");
                return; // Exit the method if the port number is not valid
            }

            MessageBox.Show("Settings saved successfully.");


            RefreshSettingsUI();
        }

        private void RefreshSettingsUI()
        {
            CurrentLogDirectoryText.Text = CurrentLogDirectory;
            CurrentDbmsIpAddressText.Text = CurrentDbmsIP;
            CurrentDbmsPortText.Text = CurrentDbmsPort.ToString();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateLogsDataGrid();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                UpdateLogsDataGrid();
            }
        }

        private void UpdateLogsDataGrid()
        {
            var logs = GetPaginatedLogs(currentPage, itemsPerPage); // Fetch paginated logs
            LogsDataGrid.ItemsSource = new ObservableCollection<LogEntry>(logs);

            PageNumberText.Text = $"Page {currentPage}"; // Update page number display
        }

        private List<LogEntry> GetPaginatedLogs(int page, int itemsPerPage)
        {
            var allLogs = GetAllLogs();
            totalItems = allLogs.Count;

            return allLogs.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        }

        private List<LogEntry> GetAllLogs()
        {
            return new List<LogEntry>();
        }



    }

    public static class DatabaseHelper
    {
        private static string connectionString = "server=localhost;database=termproject;uid=root;pwd=Jackass12!;";


        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void AddUser(User user)
        {
            var query = "INSERT INTO Users (Username, FullName, Email, Role, Status, CreationDate, LastModifiedDate) VALUES (@Username, @FullName, @Email, @Role, @Status, @CreationDate, @LastModifiedDate)";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Status", user.Status);
                command.Parameters.AddWithValue("@CreationDate", user.CreationDate);
                command.Parameters.AddWithValue("@LastModifiedDate", user.LastModifiedDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public static void DeleteUser(int userId)
        {
            var query = "DELETE FROM Users WHERE UserId = @UserId";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateUser(User user)
        {
            var query = "UPDATE Users SET Username = @Username, FullName = @FullName, Email = @Email, Role = @Role, Status = @Status, LastModifiedDate = @LastModifiedDate WHERE UserId = @UserId";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", user.UserId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }




        public static void AddRateFee(RateFee rateFee)
        {
            var query = "INSERT INTO RateFees (Rate, Description) VALUES (@Rate, @Description)";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Rate", rateFee.Rate);
                command.Parameters.AddWithValue("@Description", rateFee.Description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateRateFee(RateFee rateFee)
        {
            var query = "UPDATE RateFees SET Rate = @Rate, Description = @Description WHERE RateFeeId = @RateFeeId";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RateFeeId", rateFee.RateFeeId);
                command.Parameters.AddWithValue("@Rate", rateFee.Rate);
                command.Parameters.AddWithValue("@Description", rateFee.Description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteRateFee(int rateFeeId)
        {
            var query = "DELETE FROM RateFees WHERE RateFeeId = @RateFeeId";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RateFeeId", rateFeeId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        // Method to add a carrier to the database
        public static void AddCarrier(Carrier carrier)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO CarriersAdmin (cName, dCity) VALUES (@cName, @dCity)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cName", carrier.cName);
                    command.Parameters.AddWithValue("@dCity", carrier.dCity);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to update a carrier in the database
        public static void UpdateCarrier(Carrier carrier)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE CarriersAdmin SET cName = @cName, dCity = @dCity WHERE CarrierID = @CarrierID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cName", carrier.cName);
                    command.Parameters.AddWithValue("@dCity", carrier.dCity);
                    command.Parameters.AddWithValue("@CarrierID", carrier.CarrierID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete a carrier from the database
        public static void DeleteCarrier(int carrierId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM CarriersAdmin WHERE CarrierID = @CarrierID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarrierID", carrierId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Carrier> GetAllCarriers()
        {
            var carriers = new List<Carrier>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM CarriersAdmin;";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var carrier = new Carrier
                        {
                            CarrierID = reader.GetInt32("CarrierID"),
                            cName = reader.GetString("cName"),
                            dCity = reader.GetString("dCity")
                        };
                        carriers.Add(carrier);
                    }
                }
            }
            return carriers;
        }

        public static void BackupDatabase(string backupDirectory)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Ensure the backupDirectory ends with a backslash
                backupDirectory = backupDirectory.EndsWith("\\") ? backupDirectory : backupDirectory + "\\";

                string file = backupDirectory + "tms_backup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".sql";

                using (var command = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(command))
                    {
                        command.Connection = connection;
                        mb.ExportToFile(file);
                    }
                }
            }
        }


        /*=============================================================*/

        public static void AddRoute(Route route)
        {
            var query = "INSERT INTO Routes (CarrierID, FTLA, LTLA, FTLRate, LTLRate, ReefCharge) VALUES (@CarrierID, @FTLA, @LTLA, @FTLRate, @LTLRate, @ReefCharge)";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CarrierID", route.CarrierID);
                command.Parameters.AddWithValue("@FTLA", route.FTLA);
                command.Parameters.AddWithValue("@LTLA", route.LTLA);
                command.Parameters.AddWithValue("@FTLRate", route.FTLRate);
                command.Parameters.AddWithValue("@LTLRate", route.LTLRate);
                command.Parameters.AddWithValue("@ReefCharge", route.ReefCharge);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateRoute(Route route)
        {
            var query = "UPDATE Routes SET CarrierID = @CarrierID, FTLA = @FTLA, LTLA = @LTLA, FTLRate = @FTLRate, LTLRate = @LTLRate, ReefCharge = @ReefCharge WHERE RouteID = @RouteID";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteID", route.RouteID);
                command.Parameters.AddWithValue("@CarrierID", route.CarrierID);
                command.Parameters.AddWithValue("@FTLA", route.FTLA);
                command.Parameters.AddWithValue("@LTLA", route.LTLA);
                command.Parameters.AddWithValue("@FTLRate", route.FTLRate);
                command.Parameters.AddWithValue("@LTLRate", route.LTLRate);
                command.Parameters.AddWithValue("@ReefCharge", route.ReefCharge);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteRoute(int routeId)
        {
            var query = "DELETE FROM Routes WHERE RouteID = @RouteID";
            using (var connection = GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RouteID", routeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static List<Route> GetAllRoutes()
        {
            var routes = new List<Route>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Routes;";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var route = new Route
                        {
                            RouteID = reader.GetInt32("RouteID"),
                            CarrierID = reader.GetInt32("CarrierID"),
                            FTLA = reader.GetInt32("FTLA"),
                            LTLA = reader.GetInt32("LTLA"),
                            FTLRate = reader.GetDecimal("FTLRate"),
                            LTLRate = reader.GetDecimal("LTLRate"),
                            ReefCharge = reader.GetDecimal("ReefCharge")
                        };
                        routes.Add(route);
                    }
                }
            }
            return routes;
        }

    }

    public class User
    {
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; }
        public string? Message { get; set; }
    }



    public class RateFee
    {
        public int RateFeeId { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }

    }

    public class Carrier
    {
        public int CarrierID { get; set; }
        public string? cName { get; set; }
        public string? dCity { get; set; }

    }

    public class Route
    {
        public int RouteID { get; set; }
        public int CarrierID { get; set; }
        public int FTLA { get; set; }
        public int LTLA { get; set; }
        public decimal FTLRate { get; set; }
        public decimal LTLRate { get; set; }
        public decimal ReefCharge { get; set; }

    }
}
