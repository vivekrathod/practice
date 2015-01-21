using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sybase.Data.AseClient;

namespace SybaseADOClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Execute_Click_1(object sender, EventArgs e)
        {
            ResultGrid.Rows.Clear();
            ResultGrid.Columns.Clear();

            string connString = string.Format("Data Source={0};Port={1};database={2};uid={3};pwd={4};CharSet={5};CodePageType={6};EnableTracing=true", Server.Text, Port.Text, Database.Text, Username.Text, Password.Text, CharSet.Text, CodePageType.Text);
            //string.Format("Data Source={0};Port={1};database={2};uid={3};pwd={4};CharSet={5};Language={6}", Server.Text, Port.Text, Database.Text, Username.Text, Password.Text, CharSet.Text, Language.Text);
            if (CharSet.Text == "")
                connString = string.Format("Data Source={0};Port={1};database={2};uid={3};pwd={4};EnableTracing=true", Server.Text, Port.Text, Database.Text, Username.Text, Password.Text);

            try
            {
                using (AseConnection connn = new AseConnection(connString))
                {
                    connn.Open();
                    using (AseCommand cmd = connn.CreateCommand())
                    {
                        cmd.CommandText = SQLText.Text;
                        //cmd.ExecuteNonQuery();
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            int columnCount = reader.FieldCount;
                            ResultGrid.ColumnCount = columnCount;
                            while (reader.Read())
                            {
                                string[] row = new string[columnCount];
                                for (int i = 0; i < columnCount; i++)
                                {
                                    ResultGrid.Columns[i].Name = reader.GetName(i);
                                    row[i] = Convert.ToString(reader[i]);
                                }
                                ResultGrid.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (AseException ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
        }
    }
}
