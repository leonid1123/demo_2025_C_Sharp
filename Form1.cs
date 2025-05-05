using MySqlConnector;

namespace demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connection = new MySqlConnection();
            try
            {
                connection = new MySqlConnection(
                                    "Server=localhost;User ID=pk32;Password=1234;Database=partners"
                                    );
                connection.Open();
                MessageBox.Show("подключение к БД успешно.");
            }
            catch
            {
                MessageBox.Show("Проблемы с БД");
                return;

            }


            var command = new MySqlCommand(
                "SELECT partners.Name, partners.Country, SUM(sales.quantity) FROM `partners` JOIN `sales` ON partners.Id=sales.id_partner GROUP BY partners.Id LIMIT 10",
                connection
            );
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var skidka = Skidka(reader.GetInt32(2));
                listBox1.Items.Add(
                    $"Название:{reader.GetString(0)}, страна:{reader.GetString(1)}, Скидка:{skidka}");
            }
        }

        private int Skidka(int _prod)
        {
            if (_prod < 300)
            {
                return 0;
            }
            else if (_prod > 300 & _prod < 600)
            {
                return 10;
            }
            else if (_prod > 600 & _prod < 900)
            {
                return 20;
            }
            else
            {
                return 30;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Добро пожаловать! \nНажмите большую кнопку, чтобы получить список партнеров.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }
    }
}