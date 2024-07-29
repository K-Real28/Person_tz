using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Person_tz
{
    public partial class Form1 : Form
    {
        DataBase dataBase = new DataBase();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "person1DataSet.personBase". При необходимости она может быть перемещена или удалена.
            this.personBaseTableAdapter.Fill(this.person1DataSet.personBase);

        }

        private void button1_Click(object sender, EventArgs e) // Кнопка добавления person
        {
            AddPerson add = new AddPerson();
            add.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // Кнопка изменения person
        {
            EditPerson edit = new EditPerson();
            edit.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) // Кнопка сохранение в файл
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получить выбранную запись
                var selectedRow = dataGridView1.SelectedRows[0];
                var record = new Record
                {
                    identity = (string)selectedRow.Cells["idDataGridViewTextBoxColumn"].Value,
                    LastName = (string)selectedRow.Cells["lastNameDataGridViewTextBoxColumn"].Value,
                    firstName = (string)selectedRow.Cells["firstNameDataGridViewTextBoxColumn"].Value,
                    surname = (string)selectedRow.Cells["surnameDataGridViewTextBoxColumn"].Value,
                    birthDate = ConvertToString(selectedRow.Cells["DateofB"].Value),
                    email = (string)selectedRow.Cells["Email"].Value,
                    phoneNumber = (string)selectedRow.Cells["PhoneNumber"].Value
                };

                // Выбрать путь для сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    SaveRecordToFile(record, filePath);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int ConvertToInt(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }

        private string ConvertToString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        private void SaveRecordToFile(Record record, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Record));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, record);
                }
                MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) //Кнопка Удаления
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получить выбранную строку
                var selectedRow = dataGridView1.SelectedRows[0];
                string recordId = selectedRow.Cells["idDataGridViewTextBoxColumn"].Value.ToString();


                // Подтверждение удаления
                var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить эту запись?",
                                                     "Подтверждение удаления",
                                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Удалить запись из базы данных
                    DeleteRecordFromDatabase(recordId);

                    // Удалить строку из DataGridView
                    dataGridView1.Rows.Remove(selectedRow);

                    MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteRecordFromDatabase(string recordId)
        {
            string connectionString = (@"Data Source=HOME-PC\DB_FOR_PERSON;Initial Catalog=person1;Integrated Security=True");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM personBase WHERE Id = @Id", connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = recordId;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
