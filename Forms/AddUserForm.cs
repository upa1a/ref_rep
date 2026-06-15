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
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string username = tbLogin.Text;
            string password = tbPassword.Text;
            string role = cbRole.Text;

            if (username == null || password == null || role == null)
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение");
                return;
            }

            DB db = new DB();
            try
            {
                string query = "INSERT INTO users VALUES (null, @username, @password, @role, 0)";
                var parameters = new Dictionary<string, object>
                {
                    { "@username", username },
                    { "@password", password },
                    { "@role", role }
                };
                db.Execute(query, parameters);
                MessageBox.Show("Пользователь успешно добавлен");
                Close();
            }
            catch
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Предупреждение");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}
