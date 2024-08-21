using Bombones.Entidades.Entidades;
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
    public partial class frmFormaDeVentaAE : Form
    {
        private FormaDeVenta? forma;
        public frmFormaDeVentaAE()
        {
            InitializeComponent();
        }

        public FormaDeVenta? GetFormaDeVenta()
        {
            return forma;
        }

        public void SetFormaDeVenta(FormaDeVenta? forma)
        {
            this.forma = forma;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (forma != null)
            {
                txtForma.Text = forma.Descripcion;
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (forma == null)
                {
                    forma = new FormaDeVenta();
                }
                forma.Descripcion = txtForma.Text;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();


            if (string.IsNullOrEmpty(txtForma.Text))
            {
                valido = false;
                errorProvider1.SetError(txtForma, "El chocolate es requerido");

            }
            return valido;
        }
    }
}
