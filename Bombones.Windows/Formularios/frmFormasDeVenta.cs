using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Servicios.Intefaces;
using Bombones.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Bombones.Windows.Formularios
{
    public partial class frmFormasDeVenta : Form
    {
        private readonly IServiceProvider? _serviceProvider;
        private readonly IServiciosFormaDeVenta? _servicio;
        private List<FormaDeVenta>? lista;
        public frmFormasDeVenta(IServiceProvider? serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _servicio = serviceProvider?.GetService<IServiciosFormaDeVenta>()
                ?? throw new ApplicationException("Dependencias no cargadas!!!"); ;
        }

        private void frmFormaDeVenta_Load(object sender, EventArgs e)
        {
            try
            {
                lista = _servicio!.GetLista();
                MostrarDatosEnGrilla();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MostrarDatosEnGrilla()
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            if (lista is not null)
            {
                foreach (var item in lista)
                {
                    var r = GridHelper.ConstruirFila(dgvDatos);
                    GridHelper.SetearFila(r, item);
                    GridHelper.AgregarFila(r, dgvDatos);
                }

            }
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmFormaDeVentaAE frm = new frmFormaDeVentaAE() { Text = "Agregar Forma de venta" };
            DialogResult dr = frm.ShowDialog(this);
            try
            {
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                FormaDeVenta? forma = frm.GetFormaDeVenta();
                if (forma is not null)
                {
                    if (!_servicio!.Existe(forma))
                    {
                        _servicio!.Guardar(forma);
                        DataGridViewRow r = GridHelper.ConstruirFila(dgvDatos);
                        GridHelper.SetearFila(r, forma);
                        GridHelper.AgregarFila(r, dgvDatos);
                        MessageBox.Show("Registro agregado",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Forma de venta duplicada!!!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            if (r.Tag is null) return;

            //FormaDeVenta forma = (r.Tag as FormaDeVenta);
            var forma = (FormaDeVenta)r.Tag;

            try
            {
                DialogResult dr = MessageBox.Show($@"¿Desea dar de baja la forma {forma.Descripcion}?",
                        "Confirmar Baja",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.No)
                {
                    return;
                }
                _servicio!.Borrar(forma.FormaDeVentaId);
                GridHelper.QuitarFila(r, dgvDatos);
                MessageBox.Show("Registro eliminado",
                    "Mensaje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            }


        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            FormaDeVenta? forma = (r.Tag as FormaDeVenta) ?? new FormaDeVenta();
            frmFormaDeVentaAE frm = new frmFormaDeVentaAE() { Text = "Editar Forma" };
            frm.SetFormaDeVenta(forma);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                forma = frm.GetFormaDeVenta();
                if (forma is null) return;
                if (!_servicio!.Existe(forma))
                {
                    _servicio!.Guardar(forma);

                    GridHelper.SetearFila(r, forma);
                    MessageBox.Show("Registro modificado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Forma duplicada!!!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
