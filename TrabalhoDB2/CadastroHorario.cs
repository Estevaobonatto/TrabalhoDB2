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
            string query = @"
            SELECT h.id, h.data_agendamento, h.horario, h.cliente_id, c.nome AS cliente_nome, 
                   h.servico_id, s.nome AS servico_nome, h.funcionario_id, f.nome AS funcionario_nome
            FROM horario h
            JOIN cliente c ON h.cliente_id = c.id
            JOIN servico s ON h.servico_id = s.id
            JOIN funcionario f ON h.funcionario_id = f.id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvHorarios.AutoGenerateColumns = false;
                    dgvHorarios.Columns.Clear();

                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "id", HeaderText = "ID" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "data_agendamento", HeaderText = "Data Agendamento" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "horario", HeaderText = "Horário" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cliente_id", HeaderText = "ID Cliente" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cliente_nome", HeaderText = "Nome Cliente" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "servico_id", HeaderText = "ID Serviço" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "servico_nome", HeaderText = "Nome Serviço" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "funcionario_id", HeaderText = "ID Funcionário" });
                    dgvHorarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "funcionario_nome", HeaderText = "Nome Funcionário" });

                    dgvHorarios.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar horários: " + ex.Message);
                }
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InserirHorario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DATA_AGENDAMENTO", dtpDataAgendamento.Value);
                cmd.Parameters.AddWithValue("@HORARIO", dtpHorario.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@CLIENTE_ID", (int)cbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@SERVICO_ID", (int)cbProduto.SelectedValue);
                cmd.Parameters.AddWithValue("@FUNCIONARIO_ID", (int)cbFuncionario.SelectedValue);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Horário incluído com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao incluir horário: " + ex.Message);
                }
            }

            LimparCampos();
            CarregarHorarios();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AtualizarHorario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@DATA_AGENDAMENTO", dtpDataAgendamento.Value);
                cmd.Parameters.AddWithValue("@HORARIO", dtpHorario.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@CLIENTE_ID", (int)cbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@SERVICO_ID", (int)cbProduto.SelectedValue);
                cmd.Parameters.AddWithValue("@FUNCIONARIO_ID", (int)cbFuncionario.SelectedValue);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Horário alterado com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao alterar horário: " + ex.Message);
                }
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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExcluirHorario", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ConsultarHorario", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
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

