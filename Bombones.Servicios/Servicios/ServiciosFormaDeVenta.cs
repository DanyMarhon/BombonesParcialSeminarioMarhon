using Bombones.Datos.Interfaces;
using Bombones.Entidades.Entidades;
using Bombones.Servicios.Intefaces;
using System.Data.SqlClient;

namespace Bombones.Servicios.Servicios
{
    public class ServiciosFormaDeVenta : IServiciosFormaDeVenta
    {
        private readonly IRepositorioFormaDeVenta? _repositorio;
        private readonly string? _cadena;
        public ServiciosFormaDeVenta(IRepositorioFormaDeVenta? repositorio, string? cadena)
        {
            _repositorio = repositorio ?? throw new ApplicationException("Dependencias no cargadas!!!"); ;
            _cadena = cadena;
        }

        public List<FormaDeVenta> GetLista()
        {
            using (var conn = new SqlConnection(_cadena))
            {
                return _repositorio!.GetLista(conn);

            }
        }
        public bool Existe(FormaDeVenta forma)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                return _repositorio!.Existe(forma, conn);

            }
        }
        public void Borrar(int formaDeVentaId)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    _repositorio!.Borrar(formaDeVentaId, conn, tran);

                }
            }
        }
        public void Guardar(FormaDeVenta formaDeVenta)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (formaDeVenta.FormaDeVentaId == 0)
                        {
                            _repositorio!.Agregar(formaDeVenta, conn, tran);
                        }
                        else
                        {
                            _repositorio!.Editar(formaDeVenta, conn, tran);
                        }
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
