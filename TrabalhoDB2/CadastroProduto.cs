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
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "INSERT INTO servico (id, nome, preco) VALUES (@ID, @NOME, @PRECO)";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                Random random = new Random();
                int idRandom = random.Next(0, 99999);

                cmd.Parameters.AddWithValue("@ID", idRandom);
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@PRECO", decimal.Parse(mtbPreco.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Produto incluído com sucesso!");
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
            CarregarProdutos();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;");
            string sql = "UPDATE servico SET nome = @NOME, preco = @PRECO WHERE id = @ID";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@PRECO", decimal.Parse(mtbPreco.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Produto alterado com sucesso!");
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
                    conn.Open();

                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string deleteHorarioQuery = "DELETE FROM horario WHERE servico_id = @ID";
                        SqlCommand deleteHorarioCmd = new SqlCommand(deleteHorarioQuery, conn, transaction);
                        deleteHorarioCmd.Parameters.AddWithValue("@ID", produtoId);
                        deleteHorarioCmd.ExecuteNonQuery();

                        string deleteServicoQuery = "DELETE FROM servico WHERE id = @ID";
                        SqlCommand deleteServicoCmd = new SqlCommand(deleteServicoQuery, conn, transaction);
                        deleteServicoCmd.Parameters.AddWithValue("@ID", produtoId);
                        int rowsAffected = deleteServicoCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produto excluído com sucesso.");
                            LimparCampos();
                        }
                        else
                        {
                            MessageBox.Show("Produto não encontrado.");
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
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
                string query = "SELECT id, nome, preco FROM servico WHERE id = @ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
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
