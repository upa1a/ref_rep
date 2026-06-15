using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Json;

namespace @ref.Forms
{
    public partial class APIForm : Form
    {
        private HttpClient _httpClient;
        public APIForm()
        {
            InitializeComponent();
        }
        //При загрузке формы (2 нажатия по самой форме) происходит подключение к API
        private void Form1_Load(object sender, EventArgs e)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:4444/TransferSimulator/fullName")
            };
        }

        //Кнопка получения ФИО
        private async void Button1_Click(object sender, EventArgs e)
        {
            var responce = await _httpClient.GetAsync("fullName");
            var result = await responce.Content.ReadFromJsonAsync<Message>();

            lblData.Text = result.value;
        }

        //Кнопка валидации ФИО
        private void Button2_Click(object sender, EventArgs e)
        {
            string value = lblData.Text;

            if (Regex.IsMatch(value, @"[^а-яА-я \-]"))
            {
                lblResult.Text = "ФИО содержит запрещенные символы";
            }
            else
                lblResult.Text = "ФИО не содержит запрещенные символы";
        }
    }
    // класс - обертка для получения ФИО 
    class Message { public string value { get; set; } }
}