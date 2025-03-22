using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Services
{
    public class TiendaService
    {
        private readonly Data.ApplicationContext _context;
        public TiendaService(Data.ApplicationContext context) {
            _context = context;
        }
        public async Task<bool> createCardOfert(CreateCardOfertRequest request)
        {
            Usuario user =await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == request.Userid);
            if (user == null) {
                throw new Exception("El usuario no existe");
            }
            else if (user.Clave!=request.clave)
            {
                throw new Exception("Clave incorrecta");
            }
            else if (user.Rol!="Admin")
            {
                throw new Exception("El Usuario con id: " + request.Userid + " no tiene permisos de Administrador");

            }
            Character character =await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.CharacterId);
            CardOfert cardOfert = new CardOfert();
            cardOfert.CharacterId = request.CharacterId;
            cardOfert.GoldPrice = request.GoldPrice;
            cardOfert.DiamondPrice = request.DiamondPrice;
            cardOfert.CharacterName = character.Name;

            _context.CardOferts.Add(cardOfert);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<List<CardOfert>> GetCardOfertsAsync(int UserId)
        {
            Usuario user = await _context.Usuarios
                .Include(u => u.Characters)
                .FirstOrDefaultAsync(u => u.Id == UserId);

            List<int> charactersId = user.Characters.Select(c => c.Id).ToList();

            // Filtra las CardOferts excluyendo las que tienen un CharacterId en charactersId
            return await _context.CardOferts
                .Where(co => !charactersId.Contains(co.CharacterId)) // Excluir los CharacterId
                .ToListAsync();
        }

        public async Task<string> UnlockCharacterAsync(int userId, string clave, int idCardOfert)
        {
            
            Usuario user = await _context.Usuarios
                .Include(u => u.Characters)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }
            else if (user.Clave != clave)
            {
                throw new Exception("Clave incorrecta");
            }
            CardOfert cardOfert = await _context.CardOferts.FirstOrDefaultAsync(c => c.Id == idCardOfert);
            if (cardOfert==null)
            {
                throw new Exception("No existe la oferta");
            }
            List<int> charactersId = user.Characters.Select(c => c.Id).ToList();
            if (charactersId.Contains(cardOfert.CharacterId))
            {
                throw new Exception("Ya tienes desbloqueado a "+cardOfert.CharacterName);
            }

            if (user.Diamonds>=cardOfert.DiamondPrice && user.Gold>=cardOfert.GoldPrice)
            {
            Character unlockableCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == cardOfert.CharacterId);
            user.Characters.Add(unlockableCharacter);
                user.Diamonds = user.Diamonds - cardOfert.DiamondPrice;
                user.Gold = user.Gold - cardOfert.GoldPrice;
                await _context.SaveChangesAsync();
                return unlockableCharacter.Name;
            }
            throw new Exception("No cuenta con suficiente oro o gemas para comprar esta Carta");
            
        }

        public async Task<List<CardOfert>> GetAllCardOfertsAsync()
        {
            return await _context.CardOferts.ToListAsync();
        }

        public async Task<bool> UpdateCardOfert(UpdateCardOfertRequest request)
        {
            Usuario user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == request.Userid);

            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (user.Clave != request.clave)
            {
                throw new Exception("Clave incorrecta");
            }

            if (user.Rol != "Admin")
            {
                throw new Exception($"El usuario con id: {request.Userid} no tiene permisos de Administrador");
            }

            // Validar si la oferta existe
            CardOfert cardOfert = await _context.CardOferts.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (cardOfert == null)
            {
                throw new Exception($"No se encontró la oferta con ID {request.Id}");
            }

            // Validar si el personaje existe
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.CharacterId);
            if (character == null)
            {
                throw new Exception($"No se encontró el personaje con ID {request.CharacterId}");
            }

            // Actualizar la oferta
            cardOfert.CharacterId = request.CharacterId;
            cardOfert.GoldPrice = request.GoldPrice;
            cardOfert.DiamondPrice = request.DiamondPrice;
            cardOfert.CharacterName = character.Name;

            // Guardar cambios
            _context.CardOferts.Update(cardOfert); 
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCardOfertAsync(int id, UserCredentials request)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                throw new Exception("El usuario no existe");

            if (user.Clave != request.Clave)
                throw new Exception("Clave incorrecta");

            if (user.Rol != "Admin")
                throw new Exception($"El usuario con ID {request.UserId} no tiene permisos de administrador");

            var cardOfert = await _context.CardOferts.FirstOrDefaultAsync(c => c.Id == id);
            if (cardOfert == null)
                throw new Exception("La oferta no existe");

            _context.CardOferts.Remove(cardOfert);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
