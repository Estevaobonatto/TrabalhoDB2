using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabalhoDB2
{
    public partial class CadastroFuncionario : Form
    {
        public CadastroFuncionario()
        {
            InitializeComponent();
        }

        private void CadastroFuncionario_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salaoAppBanco.funcionario' table. You can move, or remove it, as needed.
            this.funcionarioTableAdapter.Fill(this.salaoAppBanco.funcionario);
            // TODO: This line of code loads data into the 'salaoAppBanco.cidade' table. You can move, or remove it, as needed.
            this.cidadeTableAdapter.Fill(this.salaoAppBanco.cidade);

        }
    }
}
