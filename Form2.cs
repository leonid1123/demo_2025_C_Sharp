﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace demo
{
    public partial class Form2 : Form
    {
        List<int> id_list = new List<int>();
        int i = -1;
        int selectedPartnerID = -1;
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text.Trim();
            var country = textBox2.Text.Trim();
            var type = comboBox1.Text;
            if (name.Length > 0 & country.Length > 0)
            {
                var connection = new MySqlConnection();
                try
                {
                    connection = new MySqlConnection(
                                        "Server=localhost;User ID=pk32;Password=1234;Database=partners"
                                        );
                    connection.Open();
                    MessageBox.Show("подключение к БД успешно.");
                    var command = new MySqlCommand();
                    command.CommandText = "INSERT INTO partners(name,country,type) VALUES(@name, @country,@type)";
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("country", country);
                    command.Parameters.AddWithValue("type", type);
                    var res = command.ExecuteNonQuery();
                    if (res != 0)
                    {
                        MessageBox.Show("Запрос прошел успешно");
                        get_partners();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка запроса");
                    }
                }
                catch
                {
                    MessageBox.Show("Проблемы с БД");
                    return;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connection = new MySqlConnection();
            var name = textBox1.Text.Trim();
            var country = textBox2.Text.Trim();
            var type = comboBox1.Text.Trim();
            try
            {
                connection = new MySqlConnection(
                                    "Server=localhost;User ID=pk32;Password=1234;Database=partners"
                                    );
                connection.Open();
                var command = new MySqlCommand();
                command.CommandText = "UPDATE partners SET name=@name, country=@country, type=@type WHERE ID=@id ";
                command.Connection = connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("country", country);
                command.Parameters.AddWithValue("type", type);
                command.Parameters.AddWithValue("id", selectedPartnerID);
                command.ExecuteNonQuery();

                get_partners();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void get_partners()
        {
            var connection = new MySqlConnection();
            try
            {
                connection = new MySqlConnection(
                                    "Server=localhost;User ID=pk32;Password=1234;Database=partners"
                                    );
                connection.Open();
                MessageBox.Show("подключение к БД успешно.");
                var command = new MySqlCommand();
                command.CommandText = "SELECT * FROM partners";
                command.Connection = connection;
                MySqlDataReader reader = command.ExecuteReader();
                listBox1.Items.Clear();
                while (reader.Read())
                {
                    id_list.Add(reader.GetInt32("ID"));
                    listBox1.Items.Add($"Название:{reader.GetString(1)}, страна:{reader.GetString(2)}");
                }
            }
            catch
            {
                MessageBox.Show("Проблемы с БД");
                return;
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            get_partners();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            i = listBox1.SelectedIndex;
            selectedPartnerID = id_list[i];
            var connection = new MySqlConnection();
            try
            {
                connection = new MySqlConnection(
                                    "Server=localhost;User ID=pk32;Password=1234;Database=partners"
                                    );
                connection.Open();
                var command = new MySqlCommand();
                command.CommandText = "SELECT * FROM partners WHERE ID=@id";
                command.Connection = connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("id", selectedPartnerID);
                command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetString(1);
                    textBox2.Text = reader.GetString(2);
                    comboBox1.Text = reader.GetString(3);
                }
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
            }

        }
    } 
}
