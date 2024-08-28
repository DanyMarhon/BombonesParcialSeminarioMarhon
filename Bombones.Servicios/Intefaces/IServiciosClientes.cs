using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Entidades.Enumeraciones;
using System.Data.SqlClient;

namespace Bombones.Servicios.Intefaces
{
    public interface IServiciosClientes
    {
        void Borrar(int clienteId);
        bool Existe(Cliente cliente);
        List<ClienteListDto> GetLista(int? currentPage, int? pageSize, Orden? orden = Orden.Ninguno, Pais? paisSeleccionado = null);
        void Guardar(Cliente cliente);
        Cliente? GetClientePorId(int clienteId);
        int GetCantidad(Pais? paisSeleccionado = null);

        public List<ClienteListDto> BuscarClientesPorApellido(string apellido);
    }
}
