using Bombones.Entidades.Dtos;

namespace Bombones.Servicios.Intefaces
{
    public interface IServiciosBombones
    {
        int GetCantidad();
        List<BombonListDto>? GetLista(int? currentPage, int? pageSize);

    }
}
