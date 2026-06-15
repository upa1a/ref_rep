using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace @ref.Forms
{
    public partial class MainForm : Form
    {
        public string currentTable;
        public MainForm()
        {
            InitializeComponent();
        }
        public void LoadFromDB()
        {
            DB db = new DB();
            string query = $"SELECT * FROM {currentTable}";
            DataTable table = db.Select(query);
            dgvInfo.DataSource = table;
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            currentTable = "clients";
            LoadFromDB();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            currentTable = "orders";
            LoadFromDB();
        }

        private void btnMaterials_Click(object sender, EventArgs e)
        {
            currentTable = "materials";
            LoadFromDB();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            currentTable = "products";
            LoadFromDB();
        }

        private void btnAPI_Click(object sender, EventArgs e)
        {
            Hide();
            APIForm apiForm = new APIForm();
            apiForm.ShowDialog();
        }
    }
}
