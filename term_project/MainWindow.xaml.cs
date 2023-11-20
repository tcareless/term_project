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
            AdminDashboard adminWindow = new AdminDashboard();
            adminWindow.Show();
        }

        private void OpenBuyerDashboard(object sender, RoutedEventArgs e)
        {
            BuyerDashboard buyerWindow = new BuyerDashboard();
            buyerWindow.Show();
        }

        private void OpenPlannerDashboard(object sender, RoutedEventArgs e)
        {
            PlannerDashboard plannerWindow = new PlannerDashboard();
            plannerWindow.Show();
        }
    }
}
