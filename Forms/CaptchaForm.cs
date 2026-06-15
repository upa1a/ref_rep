using @ref;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace @ref.Forms
{
    public partial class CaptchaForm : Form
    {
        PictureBox[] slots;
        PictureBox firstPicture = null;
        int fails = 0;
        int currentUserId;

        public CaptchaForm(int userId)
        {
            InitializeComponent();

            currentUserId = userId;

            slots = new PictureBox[]
            {
                pb1,
                pb2,
                pb3,
                pb4
            };

            this.Load += CaptchaForm_Load;

            foreach (PictureBox pb in slots)
            {
                pb.Click += Slot_Click;
            }
        }

        private void CaptchaForm_Load(object sender, EventArgs e)
        {
            // Запоминаем правильный порядок
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Tag = i;
            }

            Shuffle();
        }

        private void Shuffle()
        {
            Random random = new Random();

            for (int i = 0; i < slots.Length; i++)
            {
                int j = random.Next(0, slots.Length);

                Image tempImage = slots[i].Image;
                object tempTag = slots[i].Tag;

                slots[i].Image = slots[j].Image;
                slots[i].Tag = slots[j].Tag;

                slots[j].Image = tempImage;
                slots[j].Tag = tempTag;
            }
        }

        private void Slot_Click(object sender, EventArgs e)
        {
            PictureBox current = (PictureBox)sender;

            // Первая картинка
            if (firstPicture == null)
            {
                firstPicture = current;
                current.BorderStyle = BorderStyle.Fixed3D;
                return;
            }

            // Вторая картинка
            if (current != firstPicture)
            {
                Image tempImage = firstPicture.Image;
                object tempTag = firstPicture.Tag;

                firstPicture.Image = current.Image;
                firstPicture.Tag = current.Tag;

                current.Image = tempImage;
                current.Tag = tempTag;
            }

            firstPicture.BorderStyle = BorderStyle.None;
            firstPicture = null;
        }

        private void btnCaptchaCheck_Click(object sender, EventArgs e)
        {
            bool success = true;

            for (int i = 0; i < slots.Length; i++)
            {
                if ((int)slots[i].Tag != i)
                {
                    success = false;
                }
            }

            if (success == true)
            {
                MessageBox.Show("Капча пройдена");

                DialogResult = DialogResult.OK;

                Close();

                return;
            }

            fails++;

            if (fails == 3)
            {
                DB db = new DB();

                string query = "UPDATE users SET is_blocked = 1 WHERE idusers = @id";

                var parameters = new Dictionary<string, object>()
                {
                    { "@id", currentUserId }
                };

                db.Execute(query, parameters);

                //MessageBox.Show("Вы заблокированы");

                DialogResult = DialogResult.No;

                Close();
            }
            else
            {
                MessageBox.Show("Неверно. Осталось попыток: " + (3 - fails));
            }
        }
    }
}