using Bombones.Datos.Interfaces;
using Bombones.Entidades.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombones.Datos.Repositorios
{

        public class RepositorioFormaDeVenta : IRepositorioFormaDeVenta
        {
            public RepositorioFormaDeVenta()
            {
            }

            public void Agregar(FormaDeVenta forma, SqlConnection conn, SqlTransaction? tran = null)
            {
                string insertQuery = @"INSERT INTO FormasVentas (Descripcion) 
                    VALUES(@Descripcion); SELECT CAST(SCOPE_IDENTITY() as int)";

                var primaryKey = conn.QuerySingle<int>(insertQuery, forma, tran);
                if (primaryKey > 0)
                {

                    forma.FormaDeVentaId = primaryKey;
                    return;
                }

                throw new Exception("No se pudo agregar la forma de venta");
            }

        public void Borrar(int formaId, SqlConnection conn, SqlTransaction? tran = null)
        {
            string deleteQuery = @"DELETE FROM FormasVentas 
                    WHERE FormaDeVentaId=@formaId";
            int registrosAfectados = conn
                .Execute(deleteQuery, new { formaId }, tran);
            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo borrar la forma de venta");
            }
        }

        public void Editar(FormaDeVenta forma, SqlConnection conn, SqlTransaction? tran = null)
            {
                string updateQuery = @"UPDATE FormasVentas 
                    SET Descripcion=@Descripcion 
                    WHERE FormaDeVentaId=@FormaDeVentaId";

                int registrosAfectados = conn.Execute(updateQuery, forma, tran);
                if (registrosAfectados == 0)
                {
                    throw new Exception("No se pudo editar la forma de venta");
                }
            }

            public bool Existe(FormaDeVenta forma, SqlConnection conn, SqlTransaction? tran = null)
            {
                try
                {
                    string selectQuery = @"SELECT COUNT(*) FROM FormasVentas ";
                    string finalQuery = string.Empty;
                    string conditional = string.Empty;
                    if (forma.FormaDeVentaId == 0)
                    {
                        conditional = "WHERE Descripcion = @Descripcion";
                    }
                    else
                    {
                        conditional = @"WHERE Descripcion = @Descripcion
                                AND FormaDeVentaId<>@FormaDeVentaId";
                    }
                    finalQuery = string.Concat(selectQuery, conditional);
                    return conn.QuerySingle<int>(finalQuery, forma) > 0;


                }
                catch (Exception)
                {

                    throw new Exception("No se pudo comprobar si existe la forma de venta");
                }
            }

            public List<FormaDeVenta> GetLista(SqlConnection conn, SqlTransaction? tran = null)
            {
                var selectQuery = @"SELECT FormaDeVentaId, Descripcion FROM 
                        FormasVentas ORDER BY Descripcion";
                return conn.Query<FormaDeVenta>(selectQuery).ToList();
            }
        }
    }

