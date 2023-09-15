using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Models;
using Microsoft.EntityFrameworkCore;


namespace Juego_Sin_Nombre.Services
{
    public class CardService
    {
        private readonly Data.ApplicationContext _context;
        public List<int> cardsiD { get; set; }
        
        public CardService(Data.ApplicationContext context)
        {
            _context = context;
            
            this.cardsiD = new List<int>();
             
        }

        //public async Task CreateDeckAsync()
        //{
        //    List<Card> lista = await _context.Cards.Where(c=>c.IsPlayable==true).ToListAsync();
        //    foreach (Card card in lista)
        //    {
        //        cardsiD.Add(card.Id);
        //    }
        //}
        public async Task CreateDeckAsync(List<int> charactersId)
        {
            List<Card> lista = await _context.Cards.Where(c => c.IsPlayable == true && charactersId.Contains((int)c.CharacterId)).ToListAsync();
            foreach (Card card in lista)
            {
                cardsiD.Add(card.Id);
            }
        }

        public async Task<int> TakeCard(List<int> charactersId)
        {
            await this.CreateDeckAsync(charactersId);
            Random random = new Random();
            int i = random.Next(0, cardsiD.Count);
            int card = cardsiD[i];
            return card;
        }

        //public async Task<int> TakeCard(int index)
        //{
        //    await this.CreateDeckAsync();
        //    if (index < cardsiD.Count)
        //    {
        //        int card = cardsiD[index];
        //        return card;
        //    }
        //    else
        //    {
        //        Random random = new Random();
        //        int i = random.Next(0, cardsiD.Count);
        //        int card = cardsiD[i];
        //        return card;
        //    }

            
        //}


    }
}
