using MercadoPago.Resource.Preference;

namespace Juego_Sin_Nombre.Dtos
{
    public class CreatePreferenceResponse
    {
        public Preference Preference { get; set; }
        public int InvoiceId { get; set; }
    }
}
