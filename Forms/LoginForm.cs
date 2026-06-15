using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace @ref.Forms
{
    public partial class LoginForm : Form
    {
        public int fails = 0;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbLogin.Text;
            string password = tbPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Заполнены не все поля", "Предупреждение");
                return;
            }

            DB db = new DB();
            string query = @"SELECT * FROM users WHERE username = @username AND password = @password";
            var parameters = new Dictionary<string, object>
            {
                { "@username", username},
                { "@password", password }
            };
            DataTable user = db.Select(query, parameters);
            if (user.Rows.Count == 0)
            {
                fails++;
                MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные", "Предупреждение");
                return;
            }
            int userId = Convert.ToInt32(user.Rows[0][0]);
            if (fails == 3)
            {
                string queryUpd = "UPDATE users SET is_blocked = 1 WHERE idusers = @userId";
                var parametersUpd = new Dictionary<string, object>
                {
                    { "@userId", userId }
                };
                db.Execute(queryUpd, parametersUpd);
                MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Предупреждение");
                return;
            }
            if (Convert.ToInt32(user.Rows[0][4]) == 1)
            {
                MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Предупреждение");
                return;
            }
            string role = user.Rows[0][3].ToString();
            if (role == "user")
            {
                CaptchaForm captchaForm = new CaptchaForm(userId);
                //captchaForm.ShowDialog();
                if (captchaForm.ShowDialog() == DialogResult.No)
                {
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Предупреждение");
                    return;
                }
                Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
            }
            else if (role == "admin")
            {
                Hide();
                AdminForm adminForm = new AdminForm();
                adminForm.ShowDialog();
            }
            Show();
        }
    }
}
