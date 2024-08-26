using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Entidades.Enumeraciones;

namespace Bombones.Servicios.Intefaces
{
    public interface IServiciosClientes
    {
        void Borrar(int clienteId);
        //bool EstaRelacionado(int clienteId);
        bool Existe(Cliente cliente);
        List<ClienteListDto> GetLista(int? currentPage, int? pageSize, Orden? orden = Orden.Ninguno);
        void Guardar(Cliente cliente);
        Cliente? GetClientePorId(int clienteId);
        int GetCantidad();
    }
}
