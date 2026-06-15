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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            DB db = new DB();
            string query = "SELECT * FROM users";
            DataTable users = db.Select(query);
            dgvInfo.DataSource = users;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.ShowDialog();
            LoadData();
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(dgvInfo.CurrentRow.Cells[0].Value);
            EditUserForm editUserForm = new EditUserForm(userId);
            editUserForm.ShowDialog();
            LoadData();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить пользователя? Это действие невозможно отменить", "Предупреждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            int idusers = Convert.ToInt32(dgvInfo.CurrentRow.Cells[0].Value);
            string query = "DELETE FROM users WHERE idusers = @idusers";
            var parameters = new Dictionary<string, object>
            {
                { "@idusers", idusers }
            };
            DB db = new DB();
            db.Execute(query, parameters);
            LoadData();
        }
    }
}
