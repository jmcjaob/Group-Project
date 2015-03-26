﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Group_Project
{
    public partial class passwordLogin : Form
    {
        public passwordLogin()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Data.accdb;Jet OLEDB:Database Password=01827711125;"))
                {
                    OleDbCommand command = new OleDbCommand("SELECT userPassword FROM Login WHERE Profile = \"" + textBox1.Text + "\";", connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader.GetString(0) == textBox2.Text)
                        {
                            Encrpt en = new Encrpt();
                            en.profileName = textBox1.Text;
                            en.setFiles();
                            en.label1.Text = "Hello " + en.profileName;
                            this.Hide();
                            en.Show();
                            connection.Close();
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sorry there was an error logging in!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            connection.Close();
                            textBox2.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry there was an error logging in!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        connection.Close();
                        textBox2.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}