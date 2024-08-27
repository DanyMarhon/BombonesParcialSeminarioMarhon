using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using System.Data.SqlClient;

namespace Bombones.Datos.Interfaces
{
    public interface IRepositorioBombones
    {
        int GetCantidad(SqlConnection conn, SqlTransaction? tran = null);
        List<BombonListDto>? GetLista(SqlConnection conn, int? currentPage, int? pageSize, SqlTransaction? tran = null);

    }
}
