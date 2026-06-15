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
    public partial class EditUserForm : Form
    {
        public int currentUserId;
        public EditUserForm(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            LoadData();
        }
        public void LoadData()
        {
            DB db = new DB();
            string selQuery = "SELECT * FROM users WHERE idusers = @currentUserId";
            var selParameters = new Dictionary<string, object>
            {
                { "@currentUserId", currentUserId }
            };
            DataTable user = db.Select(selQuery, selParameters);
            tbLogin.Text = user.Rows[0][1].ToString();
            tbPassword.Text = user.Rows[0][2].ToString();
            tbRole.Text = user.Rows[0][3].ToString();
            if (Convert.ToInt32(user.Rows[0][4]) == 0)
            {
                lblBlockedStatus.Text = "Нет";
            }
            else
            {
                lblBlockedStatus.Text = "Да";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = tbLogin.Text;
            string password = tbPassword.Text;
            string role = tbRole.Text;

            if (username == null || password == null || role == null)
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение");
                return;
            }
            DB db = new DB();
            string query = "UPDATE users SET (username = @username, password = @password, role = @role) WHERE idusers = @currentUserId";
            var parameters = new Dictionary<string, object>
            {
                { "@currentUserId", currentUserId },
                { "@username", username },
                { "@password", password },
                { "@role", role }
            };
            db.Execute(query, parameters);
        }

        private void btnChangeBlocked_Click(object sender, EventArgs e)
        {
            string updQuery = "UPDATE users SET is_blocked = 0 WHERE idusers = @currentUserId";
            var updParameters = new Dictionary<string, object>
            {
                { "@currentUserId", currentUserId }
            };
            DB db = new DB();
            db.Execute(updQuery, updParameters);
            lblBlockedStatus.Text = "Нет";
        }
    }
}
