using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bombones.Windows.Formularios
{
    public partial class frmFormularioBusqueda : Form
    {
        public string ApellidoBuscado { get; private set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApellidoBuscado = txtApellido.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public frmFormularioBusqueda(IServiceProvider? _serviceProvider)
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
