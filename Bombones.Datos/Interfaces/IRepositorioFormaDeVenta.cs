using Bombones.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombones.Datos.Interfaces
{
    public interface IRepositorioFormaDeVenta
    {
        
            void Agregar(FormaDeVenta formaDeVenta, SqlConnection conn, SqlTransaction? tran = null);
            void Borrar(int formaDeVentaId, SqlConnection conn, SqlTransaction? tran = null);
            void Editar(FormaDeVenta formaDeVenta, SqlConnection conn, SqlTransaction? tran = null);
            bool Existe(FormaDeVenta formaDeVenta, SqlConnection conn, SqlTransaction? tran = null);
            List<FormaDeVenta> GetLista(SqlConnection conn, SqlTransaction? tran = null);

        
    }
}
