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
    public partial class CadastroProduto : Form
    {
        public CadastroProduto()
        {
            InitializeComponent();
        }

        private void CadastroProduto_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salaoAppBanco.servico' table. You can move, or remove it, as needed.
            this.servicoTableAdapter.Fill(this.salaoAppBanco.servico);

        }
    }
}
