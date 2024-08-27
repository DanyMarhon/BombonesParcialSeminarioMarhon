using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Entidades.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombones.Datos.Interfaces
{
    public interface IRepositorioClientes
    {
        void Borrar(int clienteId, SqlConnection conn, SqlTransaction? tran = null);

        void Agregar(Cliente cliente, SqlConnection conn, SqlTransaction? tran = null);

        bool Existe(Cliente cliente, SqlConnection conn, SqlTransaction? tran = null);

        void Editar(Cliente cliente, SqlConnection conn, SqlTransaction? tran=null);
        Cliente? GetClientePorId(int clienteId, SqlConnection conn);
        List<ClienteListDto> GetLista(SqlConnection conn,int? pageNumber,int? pageSize,
            Orden? orden = Orden.Ninguno, Pais? pais = null, SqlTransaction? tran = null);
        int GetCantidad(SqlConnection conn, Pais? pais = null, SqlTransaction? tran = null);
    }
}
