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
    public partial class AddProfile : Form
    {
        // Variable holding NFC UID.
        string UID = "";

        public AddProfile()
        {
            InitializeComponent();
        }

        // Method for closing application on form close.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Method for pressing NFC Scan button.
        private void nfcScan_Click(object sender, EventArgs e)
        {
            try
            {
                Card card = new Card();
                card.SelectDevice();
                card.establishContext();
                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Data.accdb;Jet OLEDB:Database Password=01827711125;"))
                {
                    string temp = card.searchForTag();
                    OleDbCommand command = new OleDbCommand("SELECT UID FROM Login WHERE UID = \"" + temp + "\";", connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        nfcScan.ForeColor = Color.RoyalBlue;
                        connection.Close();
                        MessageBox.Show("Sorry NFC Tag already registered!", "Failed NFC Scan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        UID = temp;
                        nfcScan.ForeColor = Color.Green;
                        connection.Close();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                nfcScan.ForeColor = Color.Black;
                MessageBox.Show("Make sure your NFC reader to connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                nfcScan.ForeColor = Color.Black;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method for returning to the previous window.
        private void Back_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        // Method for when add button is pressed.
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Checks if passwords are equal to each other.
                if (password.Text == passwordCon.Text)
                {
                    // connects to the database.
                    using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Data.accdb;Jet OLEDB:Database Password=01827711125;"))
                    {
                        // Tries to find if the profile name already exists.
                        OleDbCommand command = new OleDbCommand("SELECT Profile FROM Login WHERE Profile = \"" + profileName.Text + "\";", connection);
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // For if the profile name dose not exists.
                            MessageBox.Show("Error Adding Profile!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            connection.Close();
                            nfcScan.ForeColor = Color.RoyalBlue;
                            UID = "";
                            profileName.Text = "";
                            password.Text = "";
                            passwordCon.Text = "";
                        }
                        else
                        {
                            // Checks the UID field.
                            if (UID != "")
                            {
                                // Checks if the profile name and passwords and the correct length.
                                if (profileName.Text.Length > 2 && profileName.Text.Length < 16)
                                {
                                    if (password.Text.Length > 2 && password.Text.Length < 16)
                                    {
                                        // Dialog box asking if you want to add the profile.
                                        DialogResult result = MessageBox.Show("Are you sure you want to add " + profileName.Text + "?", "Add Name?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (result == DialogResult.Yes)
                                        {
                                            // In
                                            command = new OleDbCommand("INSERT INTO Login VALUES (\"" + profileName.Text + "\", \"" + UID + "\", \"" + password.Text + "\");", connection);
                                            command.ExecuteNonQuery();
                                            connection.Close();
                                            MessageBox.Show(profileName.Text + " Added!", "Added!", MessageBoxButtons.OK, MessageBoxIcon.None);

                                            nfcScan.ForeColor = Color.RoyalBlue;
                                            profileName.Text = "";
                                            password.Text = "";
                                            passwordCon.Text = "";
                                            UID = "";
                                        }
                                        else if (result == DialogResult.No)
                                        {
                                            connection.Close();
                                            nfcScan.ForeColor = Color.RoyalBlue;
                                            profileName.Text = "";
                                            password.Text = "";
                                            passwordCon.Text = "";
                                            UID = "";
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error Adding Profile!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            connection.Close();
                                            password.Text = "";
                                            passwordCon.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Password needs to between 3 and 15 characters long!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        password.Text = "";
                                        passwordCon.Text = "";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Profile Name needs to between 3 and 15 characters long!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    password.Text = "";
                                    passwordCon.Text = "";
                                }
                            }
                            else
                            {
                                connection.Close();
                                MessageBox.Show("Please scan NFC tag!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Passwords Do Not Match!", "Error Adding Profile!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    password.Text = "";
                    passwordCon.Text = "";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nfcScan.ForeColor = Color.Black;
                nfcScan.ForeColor = Color.RoyalBlue;
                UID = "";
                password.Text = "";
                passwordCon.Text = "";
            }
        }
    }
}
