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

namespace TrabalhoDB2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnCliente_Click_1(object sender, EventArgs e)
        {
            CadastroCliente Form2 = new CadastroCliente();

            
            Form2.Show();
        }

        private void btnHorario_Click_1(object sender, EventArgs e)
        {
            CadastroHorario formHorario = new CadastroHorario();
            formHorario.Show(); 
        }

        private void btnCadastroFuncionario_Click(object sender, EventArgs e)
        {
            CadastroFuncionario formFuncionario = new CadastroFuncionario();
            formFuncionario.Show();
        }

        private void btnCadastroProduto_Click(object sender, EventArgs e)
        {
            CadastroProduto formProduto = new CadastroProduto();
            formProduto.Show();
        }
    }



    //Data Source=localhost\SQLDATABASE;Initial Catalog=SalaoAppBanco;Integrated Security=True;Encrypt=False;Trust Server Certificate=True
}
