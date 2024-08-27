using Bombones.Datos.Interfaces;
using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Dapper;
using System.Data.SqlClient;

namespace Bombones.Datos.Repositorios
{
    public class RepositorioBombones : IRepositorioBombones
    {
        public List<BombonListDto>? GetLista(SqlConnection conn, int? currentPage, int? pageSize, SqlTransaction? tran)
        {
            var selectQuery = @"SELECT b.BombonId, 
                           b.NombreBombon, 
                           tc.Descripcion AS TipoDeChocolate, 
                           tn.Descripcion AS TipoDeNuez, 
                           tr.Descripcion AS TipoDeRelleno, 
                           f.NombreFabrica AS Fabrica, 
                           b.Stock, 
                           b.PrecioVenta AS Precio
                    FROM Bombones b 
                    INNER JOIN TiposDeChocolates tc ON b.TipoDeChocolateId = tc.TipoDeChocolateId
                    INNER JOIN TiposDeNueces tn ON b.TipoDeNuezId = tn.TipoDeNuezId
                    INNER JOIN TiposDeRellenos tr ON b.TipoDeRellenoId = tr.TipoDeRellenoId
                    INNER JOIN Fabricas f ON b.FabricaId = f.FabricaId";

            // Añadir un ORDER BY para usar OFFSET-FETCH
            selectQuery += " ORDER BY b.NombreBombon";

            if (currentPage.HasValue && pageSize.HasValue)
            {
                var offset = (currentPage.Value - 1) * pageSize;
                selectQuery += $" OFFSET {offset} ROWS FETCH NEXT {pageSize.Value} ROWS ONLY";
            }

            return conn.Query<BombonListDto>(selectQuery, transaction: tran).ToList();
        }

        public int GetCantidad(SqlConnection conn, SqlTransaction? tran)
        {
            string selectQuery = @"SELECT COUNT(*) FROM Bombones";
            return conn.ExecuteScalar<int>(selectQuery);
        }


    }
}
