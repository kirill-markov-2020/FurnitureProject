﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace Furniture
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private SqlConnection GetDatabaseConnection()
        {
            connection = new SqlConnection("Server=KIRILL-MARKOV;Database=Furniture;Integrated Security=True;");
            return connection;
        }

        private void TextBoxInputLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxInputLogin.Text == "Введите логин")
            {
                TextBoxInputLogin.Text = "";
                TextBoxInputLogin.Foreground = Brushes.Black;
            }
        }
        private void TextBoxInputLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxInputLogin.Text))
            {
                TextBoxInputLogin.Text = "Введите логин";
                TextBoxInputLogin.Foreground = Brushes.Gray;
            }
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordHintText.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordHintText.Visibility = Visibility.Visible; 
            }
        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxInputLogin.Text == "ManagerLogin" && PasswordBox.Password == "ManagerPassword")
            {
                AuthorizationPanel.Visibility = Visibility.Collapsed;

                ManagerPanel.Visibility = Visibility.Visible;   
                AdministratorPanel.Visibility = Visibility.Collapsed;
                ConsultantPanel.Visibility = Visibility.Collapsed;
            }
            else if (TextBoxInputLogin.Text == "AdministratorLogin" && PasswordBox.Password == "AdministratorPassword")
            {
                AuthorizationPanel.Visibility = Visibility.Collapsed;

                ManagerPanel.Visibility = Visibility.Collapsed;
                AdministratorPanel.Visibility = Visibility.Visible;
                ConsultantPanel.Visibility = Visibility.Collapsed;
            }
            else if (TextBoxInputLogin.Text == "ConsultantLogin" && PasswordBox.Password == "ConsultantPassword")
            {
                AuthorizationPanel.Visibility = Visibility.Collapsed;

                ManagerPanel.Visibility = Visibility.Collapsed;
                AdministratorPanel.Visibility = Visibility.Collapsed;
                ConsultantPanel.Visibility = Visibility.Visible;
                LoadCategories();
            }
            else if (TextBoxInputLogin.Text == "Введите логин" && PasswordBox.Password == "")
            {
                MessageBox.Show("Введите логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordTextBox.Text = PasswordBox.Password;    
            }
            else
            {
                PasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Password = PasswordTextBox.Text;    
            }
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;  
        }

        private void LoadCategories()
        {
            List<CategoryFurniture> categories = new List<CategoryFurniture>();

            try
            {
                using (SqlConnection connection = GetDatabaseConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM CategoryFurniture";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        categories.Add(new CategoryFurniture
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Name = row["name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }

            DataGridConsult.ItemsSource = categories;
        }


        public class CategoryFurniture
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }










        private SqlConnection connection;

    }
}
