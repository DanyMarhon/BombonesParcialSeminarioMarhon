﻿using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Entidades.Enumeraciones;
using Bombones.Servicios.Intefaces;
using Bombones.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Bombones.Windows.Formularios
{
    public partial class frmClientes : Form
    {
        private readonly IServiceProvider? _serviceProvider;
        private readonly IServiciosClientes? _servicio;
        private List<ClienteListDto>? lista;
        private Orden orden = Orden.ClienteAZ;

        private int currentPage = 1;//pagina actual
        private int totalPages = 0;//total de paginas
        private int pageSize = 10;//registros por página
        private int totalRecords = 0;//cantidad de registros

        private FiltroContexto filtroContexto = FiltroContexto.Pais;
        private Pais? paisFiltro = null;
        private bool filterOn = false;
        public frmClientes(IServiceProvider? serviceProvider)
        {
            InitializeComponent();
            if (serviceProvider == null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            _serviceProvider = serviceProvider;
            _servicio = serviceProvider?.GetService<IServiciosClientes>()
                ?? throw new ApplicationException("Dependencias no cargadas!!!"); ;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmClientesAE frm = new frmClientesAE(_serviceProvider) { Text = "Agregar Cliente" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            Cliente? cliente = frm.GetCliente();
            if (cliente is null) return;
            try
            {
                if (_servicio is null)
                {
                    throw new ApplicationException("Dependencias no cargadas");
                }
                _servicio.Guardar(cliente);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            try
            {
                totalRecords = _servicio!.GetCantidad();
                totalPages = (int)Math.Ceiling((decimal)totalRecords / pageSize);
                LoadData();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void LoadData()
        {
            try
            {
                lista = _servicio?.GetLista(currentPage, pageSize, orden, paisFiltro);
                MostrarDatosEnGrilla(lista);

                if (cboPaginas.Items.Count != totalPages)
                {
                    CombosHelper.CargarComboPaginas(ref cboPaginas, totalPages);
                }

                txtCantidadPaginas.Text = totalPages.ToString();

                cboPaginas.SelectedIndexChanged -= cboPaginas_SelectedIndexChanged;

                if (totalPages > 0)
                {
                    cboPaginas.SelectedIndex = currentPage == 1 ? 0 : currentPage - 1;
                    cboPaginas.Enabled = true; // Habilita el combo box si hay páginas
                }
                else
                {
                    cboPaginas.SelectedIndex = -1; // Ningún elemento seleccionado
                    cboPaginas.Enabled = false; // Deshabilita el combo box si no hay páginas
                }

                cboPaginas.SelectedIndexChanged += cboPaginas_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnPrimero_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadData();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void cboPaginas_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = int.Parse(cboPaginas.Text);
            LoadData();
        }

        private void MostrarDatosEnGrilla(List<ClienteListDto> lista)
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            if (lista is not null)
            {
                foreach (var c in lista)
                {
                    var r = GridHelper.ConstruirFila(dgvDatos);
                    GridHelper.SetearFila(r, c);
                    GridHelper.AgregarFila(r, dgvDatos);
                }

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

            var clienteDto = (ClienteListDto)r.Tag;

            try
            {
                DialogResult dr = MessageBox.Show($@"¿Desea dar de baja al cliente {clienteDto.NombreCompleto}?",
                        "Confirmar Baja",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.No)
                {
                    return;
                }

                if (dr == DialogResult.Yes)
                {
                    _servicio!.Borrar(clienteDto.ClienteId);
                    GridHelper.QuitarFila(r, dgvDatos);
                    MessageBox.Show("Registro eliminado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }



                else
                {
                    MessageBox.Show("Baja Denegada",
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

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            if (r.Tag == null) return;
            ClienteListDto clienteDto = (ClienteListDto)r.Tag;
            Cliente? cliente = _servicio!?.GetClientePorId(clienteDto.ClienteId);
            if (cliente is null) return;
            frmClientesAE frm = new frmClientesAE(_serviceProvider) { Text = "Editar Cliente" };
            frm.SetCliente(cliente);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            cliente = frm.GetCliente();

            if (cliente == null) return;
            try
            {
                if (!_servicio!.Existe(cliente))
                {
                    _servicio!.Guardar(cliente);

                    //TODO:Hacer manejo de la edición
                    MessageBox.Show("Registro editado",
                                    "Mensaje",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro existente\nEdición denegada",
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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                lista = _servicio!?.GetLista(currentPage, pageSize);
                if (lista is not null)
                {
                    currentPage = 1;
                    orden = 0;
                    paisFiltro = null;
                    totalRecords = _servicio!?.GetCantidad() ?? 0;
                    totalPages = (int)Math.Ceiling((decimal)totalRecords / pageSize);
                    filterOn = false;
                    tsbFiltrar.Enabled = true;
                    LoadData();

                    //Activo los botones de paginación nuevamente
                    cboPaginas.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    txtCantidadPaginas.Visible = true;
                    btnPrimero.Visible = true;
                    btnAnterior.Visible = true;
                    btnSiguiente.Visible = true;
                    btnUltimo.Visible = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            if (r.Tag == null) return;
            ClienteListDto clienteDto = (ClienteListDto)r.Tag;
            Cliente? cliente = _servicio!?.GetClientePorId(clienteDto.ClienteId);
            if (cliente is null) return;
            frmDetalleCliente frm = new frmDetalleCliente(_serviceProvider) { Text = "Editar Cliente" };
            frm.SetCliente(cliente);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            cliente = frm.GetCliente();

            if (cliente == null) return;
        }

        private void aZPorClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            orden = Orden.ClienteAZ;
            LoadData();
        }

        private void zAPorClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            orden = Orden.ClienteZA;
            LoadData();
        }


        private void tsbFiltrar_Click(object sender, EventArgs e)
        {
            frmFormularioFiltro frm = new frmFormularioFiltro(_serviceProvider, filtroContexto) { Text = "Seleccionar Pais para Filtrar" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            paisFiltro = frm.GetPais();
            if (paisFiltro is null) return;
            totalRecords = _servicio!?.GetCantidad(paisFiltro) ?? 0;
            totalPages = (int)Math.Ceiling((decimal)totalRecords / pageSize);

            LoadData();
            filterOn = true;
            tsbFiltrar.Enabled = false;
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            frmFormularioBusqueda frm = new frmFormularioBusqueda(_serviceProvider) { Text = "Ingresar apellido para buscar" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            string apellidoBuscado = frm.ApellidoBuscado;
            if (string.IsNullOrEmpty(apellidoBuscado)) return;

            // Llamar al servicio para buscar clientes por apellido
            var clientesFiltrados = _servicio!.BuscarClientesPorApellido(apellidoBuscado);
            MostrarDatosEnGrilla(clientesFiltrados);

            // Ocultar botones y vistas de paginado porque cuando busco
            // se me rompe la paginación, consultar a Carlos!!!!
            cboPaginas.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            txtCantidadPaginas.Visible = false;
            btnPrimero.Visible = false;
            btnAnterior.Visible = false;
            btnSiguiente.Visible = false;
            btnUltimo.Visible = false;
            // Luego los activo al refrescar
            
        }
    }
}
