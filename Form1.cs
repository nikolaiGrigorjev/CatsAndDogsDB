using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatsAndDogsDB
{
    public partial class Form1 : Form
    {
        string conStr;
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            conStr = ConfigurationManager.ConnectionStrings["CatsAndDogsDB.Properties.Settings.petsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulatePetsTable();
        }
        private void PopulatePetsTable()
        {
            using (connection = new SqlConnection(conStr))
            using (SqlDataAdapter adapter = new SqlDataAdapter("select * from PetType", connection))
            {
                DataTable petTable = new DataTable();
                adapter.Fill(petTable);

                listBox1.DisplayMember = "PetTypeName";
                listBox1.ValueMember = "Id";
                listBox1.DataSource = petTable;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePetNames();
        }
        private void PopulatePetNames()
        {
            string query = "select Pet.Name from petType inner join Pet on Pet.TypeID = petType.Id where PetType.Id = @TypeId";
            using (connection = new SqlConnection(conStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@TypeId", listBox1.SelectedValue);
                DataTable petNameTable = new DataTable();
                adapter.Fill(petNameTable);
                
                listBox2.DisplayMember = "Name";
                listBox2.ValueMember = "Id";
                listBox2.DataSource = petNameTable;
            }
        }
    }
}
