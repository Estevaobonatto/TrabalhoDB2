using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TrabalhoDB2
{
    public partial class CadastroFuncionario : Form
    {
        public CadastroFuncionario()
        {
            InitializeComponent();
            this.dtvFuncionarios.SelectionChanged += new System.EventHandler(this.dtvFuncionarios_SelectionChanged);
            CarregarCidades();
            CarregarFuncionarios();
        }

        private void LimparCampos()
        {
            txtId.Text = string.Empty;
            txtNome.Text = string.Empty;
            mtbCpf.Text = string.Empty;
            dtpDataNasc.Value = DateTime.Now;
            cbCidade.SelectedIndex = -1;
        }

        private void CarregarCidades()
        {
            List<Cidade> cidades = new List<Cidade>();

            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome FROM cidade";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cidades.Add(new Cidade
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar cidades: " + ex.Message);
                }
            }

            cbCidade.DataSource = cidades;
            cbCidade.DisplayMember = "Nome";
            cbCidade.ValueMember = "Id";
        }

        private void CarregarFuncionarios()
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = @"
            SELECT f.id, f.nome, f.cpf, f.data_nascimento, f.cidade_id, ci.nome AS cidade_nome
            FROM funcionario f
            JOIN cidade ci ON f.cidade_id = ci.id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dtvFuncionarios.AutoGenerateColumns = false;
                    dtvFuncionarios.Columns.Clear();

                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "id", HeaderText = "ID" });
                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "nome", HeaderText = "Nome" });
                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cpf", HeaderText = "CPF" });
                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "data_nascimento", HeaderText = "Data de Nascimento" });
                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cidade_id", HeaderText = "ID Cidade" });
                    dtvFuncionarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cidade_nome", HeaderText = "Cidade" });

                    dtvFuncionarios.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar funcionários: " + ex.Message);
                }
            }
        }

        private void dtvFuncionarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dtvFuncionarios.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtvFuncionarios.SelectedRows[0];

                txtId.Text = selectedRow.Cells["id"].Value.ToString();
                txtNome.Text = selectedRow.Cells["nome"].Value.ToString();
                mtbCpf.Text = selectedRow.Cells["cpf"].Value.ToString();
                dtpDataNasc.Value = DateTime.Parse(selectedRow.Cells["data_nascimento"].Value.ToString());
                cbCidade.SelectedValue = selectedRow.Cells["cidade_id"].Value;
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (@ID, @NOME, @CPF, @DATA_NASCIMENTO, @CIDADE_ID)";

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            mtbCpf.Text = LimparCPF(mtbCpf.Text);

            try
            {


                SqlCommand cmd = new SqlCommand(sql, conn);

                Random random = new Random();
                int idRandom = random.Next(0, 99999);

                cmd.Parameters.AddWithValue("@ID", idRandom);
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCpf.Text);
                cmd.Parameters.AddWithValue("@DATA_NASCIMENTO", dtpDataNasc.Value);
                cmd.Parameters.AddWithValue("@CIDADE_ID", (int)cbCidade.SelectedValue);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Registro Incluído com sucesso!");
                LimparCampos();
                CarregarFuncionarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro de conexão com o banco de dados. " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "UPDATE funcionario SET nome = @NOME, cpf = @CPF, data_nascimento = @DATA_NASCIMENTO, cidade_id = @CIDADE_ID WHERE id = @ID";

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            mtbCpf.Text = LimparCPF(mtbCpf.Text);

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCpf.Text);
                cmd.Parameters.AddWithValue("@DATA_NASCIMENTO", dtpDataNasc.Value);
                cmd.Parameters.AddWithValue("@CIDADE_ID", (int)cbCidade.SelectedValue);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Registro Alterado com sucesso!");
                LimparCampos();
                CarregarFuncionarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro de conexão com o banco de dados. " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int funcionarioId;
            if (int.TryParse(txtId.Text, out funcionarioId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "DELETE FROM funcionario WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", funcionarioId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Funcionário excluído com sucesso.");
                            LimparCampos();
                            CarregarFuncionarios();
                        }
                        else
                        {
                            MessageBox.Show("Funcionário não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir funcionário: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de funcionário válido.");
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            int funcionarioId;
            if (int.TryParse(txtId.Text, out funcionarioId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "SELECT id, nome, cpf, data_nascimento, cidade_id FROM funcionario WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", funcionarioId);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtId.Text = reader["id"].ToString();
                            txtNome.Text = reader["nome"].ToString();
                            mtbCpf.Text = reader["cpf"].ToString();
                            dtpDataNasc.Value = DateTime.Parse(reader["data_nascimento"].ToString());
                            cbCidade.SelectedValue = reader["cidade_id"];
                        }
                        else
                        {
                            MessageBox.Show("Funcionário não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao consultar funcionário: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de funcionário válido.");
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void cbCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seu código aqui
        }

        private void dtpDataNasc_ValueChanged(object sender, EventArgs e)
        {
            // Seu código aqui
        }

        private void mtbCpf_MaskInputRejected(object sender, EventArgs e)
        {
            // Seu código aqui
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            // Seu código aqui
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            // Seu código aqui
        }

        private void CadastroFuncionario_Load(object sender, EventArgs e)
        {
            CarregarFuncionarios();
            CarregarCidades();
        }

    }
}
