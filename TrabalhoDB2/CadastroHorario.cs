using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using static TrabalhoDB2.CadastroHorario;

namespace TrabalhoDB2
{
    public partial class CadastroHorario : Form
    {
        public CadastroHorario()
        {
            InitializeComponent();
            CarregarClientes();
            CarregarServicos();
            CarregarFuncionarios();
            CarregarHorarios();
        }

        private void LimparCampos()
        {
            txtId.Text = string.Empty;
            dtpDataAgendamento.Value = DateTime.Now;
            dtpHorario.Value = DateTime.Now;
            cbCliente.SelectedIndex = -1;
            cbProduto.SelectedIndex = -1;
            cbFuncionario.SelectedIndex = -1;
        }

        private void CarregarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome FROM cliente";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        clientes.Add(new Cliente
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar clientes: " + ex.Message);
                }
            }

            cbCliente.DataSource = clientes;
            cbCliente.DisplayMember = "Nome";
            cbCliente.ValueMember = "Id";
        }

        private void CarregarServicos()
        {
            List<Servico> servicos = new List<Servico>();

            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome FROM servico";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        servicos.Add(new Servico
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar serviços: " + ex.Message);
                }
            }

            cbProduto.DataSource = servicos;
            cbProduto.DisplayMember = "Nome";
            cbProduto.ValueMember = "Id";
        }

        private void CarregarFuncionarios()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome FROM funcionario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        funcionarios.Add(new Funcionario
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar funcionários: " + ex.Message);
                }
            }

            cbFuncionario.DataSource = funcionarios;
            cbFuncionario.DisplayMember = "Nome";
            cbFuncionario.ValueMember = "Id";
        }

        private void CarregarHorarios()
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, data_agendamento, horario, cliente_id, servico_id, funcionario_id FROM horario";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvHorarios.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar horários: " + ex.Message);
                }
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpHorarios_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (@ID, @DATA_AGENDAMENTO, @HORARIO, @CLIENTE_ID, @SERVICO_ID, @FUNCIONARIO_ID)";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                Random random = new Random();
                int idRandom = random.Next(0, 99999);

                cmd.Parameters.AddWithValue("@ID", idRandom);
                cmd.Parameters.AddWithValue("@DATA_AGENDAMENTO", dtpDataAgendamento.Value);
                cmd.Parameters.AddWithValue("@HORARIO", dtpHorario.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@CLIENTE_ID", (int)cbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@SERVICO_ID", (int)cbProduto.SelectedValue);
                cmd.Parameters.AddWithValue("@FUNCIONARIO_ID", (int)cbFuncionario.SelectedValue);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Horário incluído com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro de conexão com o banco de dados. " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            LimparCampos();
            CarregarHorarios();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "UPDATE horario SET data_agendamento = @DATA_AGENDAMENTO, horario = @HORARIO, cliente_id = @CLIENTE_ID, servico_id = @SERVICO_ID, funcionario_id = @FUNCIONARIO_ID WHERE id = @ID";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@DATA_AGENDAMENTO", dtpDataAgendamento.Value);
                cmd.Parameters.AddWithValue("@HORARIO", dtpHorario.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@CLIENTE_ID", (int)cbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@SERVICO_ID", (int)cbProduto.SelectedValue);
                cmd.Parameters.AddWithValue("@FUNCIONARIO_ID", (int)cbFuncionario.SelectedValue);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Horário alterado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro de conexão com o banco de dados. " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            LimparCampos();
            CarregarHorarios();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int horarioId;
            if (int.TryParse(txtId.Text, out horarioId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "DELETE FROM horario WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", horarioId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Horário excluído com sucesso.");
                            LimparCampos();
                        }
                        else
                        {
                            MessageBox.Show("Horário não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir horário: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de horário válido.");
            }

            CarregarHorarios();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            int horarioId;
            if (int.TryParse(txtId.Text, out horarioId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "SELECT id, data_agendamento, horario, cliente_id, servico_id, funcionario_id FROM horario WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", horarioId);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtId.Text = reader["id"].ToString();
                            dtpDataAgendamento.Value = DateTime.Parse(reader["data_agendamento"].ToString());
                            dtpHorario.Value = DateTime.Parse(reader["horario"].ToString());
                            cbCliente.SelectedValue = reader["cliente_id"];
                            cbProduto.SelectedValue = reader["servico_id"];
                            cbFuncionario.SelectedValue = reader["funcionario_id"];
                        }
                        else
                        {
                            MessageBox.Show("Horário não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao consultar horário: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de horário válido.");
            }
            CarregarHorarios();
        }


        private void dgvHorarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHorarios.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvHorarios.SelectedRows[0];

                txtId.Text = selectedRow.Cells["id"].Value.ToString();
                dtpDataAgendamento.Value = DateTime.Parse(selectedRow.Cells["data_agendamento"].Value.ToString());
                dtpHorario.Value = DateTime.Parse(selectedRow.Cells["horario"].Value.ToString());
                cbCliente.SelectedValue = selectedRow.Cells["cliente_id"].Value;
                cbProduto.SelectedValue = selectedRow.Cells["servico_id"].Value;
                cbFuncionario.SelectedValue = selectedRow.Cells["funcionario_id"].Value;
            }
        }

        private void CadastroHorario_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salaoAppBanco.cliente' table. You can move, or remove it, as needed.
            this.clienteTableAdapter.Fill(this.salaoAppBanco.cliente);
            // TODO: This line of code loads data into the 'salaoAppBanco.cidade' table. You can move, or remove it, as needed.


        }

        // Classes auxiliares para carregar dados nas comboboxes
        public class Cliente
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        public class Servico
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        public class Funcionario
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
    }

 }

