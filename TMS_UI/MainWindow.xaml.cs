using System.Windows;
using System;

namespace term_project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAdminDashboard(object sender, RoutedEventArgs e)
        {
            if (AdminPasswordBox.Password == "admin")
            {
                AdminDashboard adminWindow = new AdminDashboard();
                adminWindow.Show();
                FeedbackText.Text = "";
            }
            else
            {
                FeedbackText.Text = "Incorrect password for Admin Dashboard.";
            }
        }

        private void OpenBuyerDashboard(object sender, RoutedEventArgs e)
        {
            if (BuyerPasswordBox.Password == "buyer")
            {
                BuyerDashboard buyerWindow = new BuyerDashboard();
                buyerWindow.Show();
                FeedbackText.Text = "";
            }
            else
            {
                FeedbackText.Text = "Incorrect password for Buyer Dashboard.";
            }
        }

        private void OpenPlannerDashboard(object sender, RoutedEventArgs e)
        {
            if (PlannerPasswordBox.Password == "planner")
            {
                PlannerDashboard plannerWindow = new PlannerDashboard();
                plannerWindow.Show();
                FeedbackText.Text = "";
            }
            else
            {
                FeedbackText.Text = "Incorrect password for Planner Dashboard.";
            }
        }
    }
}
