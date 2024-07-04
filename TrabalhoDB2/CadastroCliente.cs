using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace TrabalhoDB2
{
    public partial class CadastroCliente : Form
    {
        public CadastroCliente()
        {
            InitializeComponent();
            this.dtvClientes.SelectionChanged += new System.EventHandler(this.dtvClientes_SelectionChanged_1);
            CarregarCidades();
            CarregarClientes();
        }

        private void LimparCampos()
        {
            txtId.Text = string.Empty;
            txtNome.Text = string.Empty;
            mtbCpf.Text = string.Empty;
            dtpDataNasc.Value = DateTime.Now;
            cbCidade.SelectedIndex = -1;
            txtId.Text = string.Empty;
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

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (@ID, @NOME, @CPF, @DATA_NASCIMENTO, @CIDADE_ID)";

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            LimparCPF(mtbCpf.Text);

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

                MessageBox.Show("Registro Incluido com sucesso!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocorreu um erro de conexão com o banco de dados. " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
        }

        private void CarregarClientes()
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome, cpf, data_nascimento, cidade_id FROM cliente";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dtvClientes.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar clientes: " + ex.Message);
                }
            }
        }
        private void dtvClientes_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dtvClientes.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtvClientes.SelectedRows[0];

                txtId.Text = selectedRow.Cells["id"].Value.ToString();
                txtNome.Text = selectedRow.Cells["nome"].Value.ToString();
                mtbCpf.Text = selectedRow.Cells["cpf"].Value.ToString();
                dtpDataNasc.Value = DateTime.Parse(selectedRow.Cells["data_nascimento"].Value.ToString());
                cbCidade.SelectedValue = selectedRow.Cells["cidade_id"].Value;
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "UPDATE cliente SET nome = @NOME, cpf = @CPF, data_nascimento = @DATA_NASCIMENTO, cidade_id = @CIDADE_ID WHERE id = @ID";

            string LimparCPF(string cpf)
            {
                return new string(cpf.Where(char.IsDigit).ToArray());
            }

            LimparCPF(mtbCpf.Text);

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));  // Aqui assumimos que o ID do cliente será inserido em um campo de texto chamado txtId
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCpf.Text);
                cmd.Parameters.AddWithValue("@DATA_NASCIMENTO", dtpDataNasc.Value);
                cmd.Parameters.AddWithValue("@CIDADE_ID", (int)cbCidade.SelectedValue);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Registro Alterado com sucesso!");
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
            CarregarClientes();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int clienteId;
            if (int.TryParse(txtId.Text, out clienteId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "DELETE FROM cliente WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", clienteId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente excluído com sucesso.");
                            LimparCampos();
                        }
                        else
                        {
                            MessageBox.Show("Cliente não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir cliente: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de cliente válido.");
            }

            CarregarClientes();
        }

    private void btnConsultar_Click(object sender, EventArgs e)
        {
            int clienteId;
            if (int.TryParse(txtId.Text, out clienteId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
                string query = "SELECT id, nome, cpf, data_nascimento, cidade_id FROM cliente WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", clienteId);

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
                            MessageBox.Show("Cliente não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao consultar cliente: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de cliente válido.");
            }
            CarregarClientes();
        }

        private void dtpDataNasc_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbCidade_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void mtbCpf_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void CadastroCliente_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salaoAppBanco.cidade' table. You can move, or remove it, as needed.
            this.cidadeTableAdapter.Fill(this.salaoAppBanco.cidade);
            // TODO: This line of code loads data into the 'salaoAppBanco.cliente' table. You can move, or remove it, as needed.
            this.clienteTableAdapter.Fill(this.salaoAppBanco.cliente);

        }

    }

    internal class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
