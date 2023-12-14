using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace term_project
{
    public partial class AdminDashboard : Window
    {
        public ObservableCollection<User> Users { get; set; }

        public AdminDashboard()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            UsersDataGrid.ItemsSource = Users;
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            var newUser = new User { CreationDate = DateTime.Now, LastModifiedDate = DateTime.Now };
            Users.Add(newUser);
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                Users.Remove(selectedUser);
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
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
}
