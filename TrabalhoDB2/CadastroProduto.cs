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

namespace TrabalhoDB2
{
    public partial class CadastroProduto : Form
    {
        public CadastroProduto()
        {
            InitializeComponent();
            CarregarProdutos();
        }

        private void LimparCampos()
        {
            txtId.Text = string.Empty;
            txtNome.Text = string.Empty;
            mtbPreco.Text = string.Empty;
        }

        private void CarregarProdutos()
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";
            string query = "SELECT id, nome, preco FROM servico";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvProdutos.AutoGenerateColumns = false;
                    dgvProdutos.Columns.Clear();

                    dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "id", HeaderText = "ID" });
                    dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "nome", HeaderText = "Nome" });
                    dgvProdutos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "preco", HeaderText = "Preço" });

                    dgvProdutos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
                }
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InserirProduto", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                Random random = new Random();
                int idRandom = random.Next(0, 99999);

                cmd.Parameters.AddWithValue("@ID", idRandom);     
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@PRECO", decimal.Parse(mtbPreco.Text));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Produto incluído com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao incluir produto: " + ex.Message);
                }
            }

            LimparCampos();
            CarregarProdutos();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AtualizarProduto", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@PRECO", decimal.Parse(mtbPreco.Text));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Produto alterado com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao alterar produto: " + ex.Message);
                }
            }

            LimparCampos();
            CarregarProdutos();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int produtoId;
            if (int.TryParse(txtId.Text, out produtoId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExcluirProduto", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", produtoId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produto excluído com sucesso.");
                            LimparCampos();
                        }
                        else
                        {
                            MessageBox.Show("Produto não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir produto: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de produto válido.");
            }

            CarregarProdutos();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            int produtoId;
            if (int.TryParse(txtId.Text, out produtoId))
            {
                string connectionString = "Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ConsultarProduto", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", produtoId);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtId.Text = reader["id"].ToString();
                            txtNome.Text = reader["nome"].ToString();
                            mtbPreco.Text = reader["preco"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Produto não encontrado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao consultar produto: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, insira um ID de produto válido.");
            }

            CarregarProdutos();
        }

        private void txtId_TextChanged(object sender, EventArgs e) { }

        private void txtNome_TextChanged(object sender, EventArgs e) { }

        private void mtbPreco_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }

        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void CadastroProduto_Load(object sender, EventArgs e)
        {
            CarregarProdutos();
        }
    }
}
