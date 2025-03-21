using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;

namespace Juego_Sin_Nombre.Services.interfacez
{
    public interface IDiamondOfertService
    {
        Task<DiamondOfert> CreateDiamondOfertAsync(DiamondOfertRequest request);
        Task<DiamondOfert> UpdateDiamondOfertAsync(int id, DiamondOfertRequest request);
        Task<List<DiamondOfert>> GetAllDiamondOfertsAsync();
        Task<bool> SoftDeleteDiamondOfertAsync(DeleteRequest request,int id);
    }


}
