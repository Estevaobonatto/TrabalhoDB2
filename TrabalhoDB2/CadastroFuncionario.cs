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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CarregarFuncionarios", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

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

        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            mtbCpf.Text = LimparCPF(mtbCpf.Text);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_InserirFuncionario", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

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

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            mtbCpf.Text = LimparCPF(mtbCpf.Text);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_AtualizarFuncionario", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExcluirFuncionario", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ID", funcionarioId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Funcionário excluído com sucesso.");
                            LimparCampos();
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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ConsultarFuncionario", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
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

        private void cbCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dtpDataNasc_ValueChanged(object sender, EventArgs e)
        {
        }

        private void mtbCpf_MaskInputRejected(object sender, EventArgs e)
        {
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
        }

        private void CadastroFuncionario_Load(object sender, EventArgs e)
        {
            CarregarFuncionarios();
            CarregarCidades();
        }

    }
}
