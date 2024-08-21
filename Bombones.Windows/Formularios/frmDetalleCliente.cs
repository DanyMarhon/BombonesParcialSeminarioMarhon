using Bombones.Entidades.Entidades;
using Bombones.Windows.Helpers;


namespace Bombones.Windows.Formularios
{
    public partial class frmDetalleCliente : Form
    {
        public frmDetalleCliente()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private readonly IServiceProvider? _serviceProvider;

        private Cliente? cliente;
        public frmDetalleCliente(IServiceProvider? serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        internal void SetCliente(Cliente cliente)
        {
            this.cliente = cliente;
        }
        public Cliente? GetCliente()
        {
            return cliente;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (cliente is not null)
            {
                txtDocumento.Text = cliente.Documento.ToString();
                txtApellido.Text = cliente.Apellido;
                txtNombre.Text = cliente.Nombres;

                foreach (var item in cliente.ClienteDirecciones)
                {
                    var r = GridHelper.ConstruirFila(dgvDirecciones);
                    GridHelper.SetearFila(r, item.Direccion, "frmDetalleCliente");
                    GridHelper.AgregarFila(r, dgvDirecciones);
                }

                foreach (var item in cliente.ClienteTelefonos)
                {
                    var r = GridHelper.ConstruirFila(dgvTelefonos);
                    GridHelper.SetearFila(r, item.Telefono, "frmDetalleCliente");
                    GridHelper.AgregarFila(r, dgvTelefonos);
                }
            }
        }
    }
}
