using Juego_Sin_Nombre.Controllers;
using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Juego_Sin_Nombre.Services
{
    public class GameService
    {
        private readonly Data.ApplicationContext _context;
        private CardService cardService;

        public GameService(Data.ApplicationContext context, CardService cardService)
        {
            _context = context;
            this.cardService = cardService;
        }

        public GameResponse MapperGameToGameResponse(Game game,Boolean characterUnlocked,String unlockableCharacterName)
        {
            GameResponse gameResponse = new GameResponse();
            gameResponse.characterUnlocked = characterUnlocked;
            gameResponse.unlockableCharacterName= unlockableCharacterName;
            gameResponse.Id = game.Id;
            gameResponse.Armystate = (int)game.Armystate;
            gameResponse.Populationstate = (int)game.Populationstate;
            gameResponse.Economystate = (int)game.Economystate;
            gameResponse.Magicstate = (int)game.Magicstate;
            gameResponse.Gamestate = game.Gamestate;
            gameResponse.Day = (int)game.Day;
            Usuario user = _context.Usuarios.FirstOrDefault(u => u.Id == game.Userid);
            gameResponse.UserName = user.Email;
            gameResponse.Gold = user.Gold;
            gameResponse.Diamonds = user.Diamonds;
            gameResponse.Lives = user.Lives;

            Card card = _context.Cards.Include(u => u.Character).FirstOrDefault(c => c.Id == game.Lastcardid);
            CardResponse cardResponse = new CardResponse();
            cardResponse.Typeid = (int)card.Typeid;
            cardResponse.Character = card.Character.Name;
            cardResponse.Description = card.Description;

            Decision decision = _context.Decisions.FirstOrDefault(d => d.Id == card.Decision1);
            PostDecisionDto decisionResponse1 = new PostDecisionDto();
            decisionResponse1.Convert(decision);
            cardResponse.Decision1 = decisionResponse1;

            Decision decision2 = _context.Decisions.FirstOrDefault(d => d.Id == card.Decision2);
            PostDecisionDto decisionResponse2 = new PostDecisionDto();
            decisionResponse2.Convert(decision2);
            cardResponse.Decision2 = decisionResponse2;

            gameResponse.LastCard = cardResponse;
            return gameResponse;
        }
        public GameResponse MapperGameToGameResponse(Game game, Boolean characterUnlocked)
        {
            



            GameResponse gameResponse = new GameResponse();
            gameResponse.characterUnlocked = characterUnlocked;
            
            gameResponse.Id = game.Id;
            gameResponse.Armystate = (int)game.Armystate;
            gameResponse.Populationstate = (int)game.Populationstate;
            gameResponse.Economystate = (int)game.Economystate;
            gameResponse.Magicstate = (int)game.Magicstate;
            gameResponse.Gamestate = game.Gamestate;
            gameResponse.Day = (int)game.Day;

            Usuario user = _context.Usuarios.FirstOrDefault(u => u.Id == game.Userid);
            gameResponse.UserName = user.Email;
            gameResponse.Gold = user.Gold;
            gameResponse.Diamonds = user.Diamonds;
            gameResponse.Lives = user.Lives;

            UpdateLives(user);
            _context.SaveChanges();

            if (user.Lives < 1)
            {
                throw new ArgumentException("No tiene suficientes vidas para jugar");
            }

            Card card = _context.Cards.Include(u => u.Character).FirstOrDefault(c => c.Id == game.Lastcardid);
            CardResponse cardResponse = new CardResponse();
            cardResponse.Typeid = (int)card.Typeid;
            cardResponse.Character = card.Character.Name;
            cardResponse.Description = card.Description;

            Decision decision = _context.Decisions.FirstOrDefault(d => d.Id == card.Decision1);
            PostDecisionDto decisionResponse1 = new PostDecisionDto();
            decisionResponse1.Convert(decision);
            cardResponse.Decision1 = decisionResponse1;

            Decision decision2 = _context.Decisions.FirstOrDefault(d => d.Id == card.Decision2);
            PostDecisionDto decisionResponse2 = new PostDecisionDto();
            decisionResponse2.Convert(decision2);
            cardResponse.Decision2 = decisionResponse2;

            gameResponse.LastCard = cardResponse;
            return gameResponse;
        }
        public async Task<GameResponse> Play(Game game, int decision)
        {
            Boolean characterUnlocked = false;
            string unlockableCharacterName = "";

            if (game.Gamestate=="FINISHED")
            {
                throw new Exception("El Juego esta acabado");
            }
            Usuario user = await _context.Usuarios.Include(u=>u.Characters).FirstOrDefaultAsync(u => u.Id == game.Userid);
            UpdateLives(user);
            _context.SaveChanges();

            if (user.Lives < 1)
            {
                throw new ArgumentException("No tiene suficientes vidas para jugar");
            }

            List<int> charactersId = user.Characters.Select(c => c.Id).ToList();

            Card card = await _context.Cards.FirstOrDefaultAsync(c=>c.Id==game.Lastcardid);
            Decision decision1 = new Decision();
            if (decision == 1)
            {
                 decision1 = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == card.Decision1);
            }
            else
            {
                 decision1 = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == card.Decision2);
            }

            //Comprobar si desbloquea Personaje
            if (decision1.UnlockableCharacter!=null)
            {
                if (!charactersId.Contains((int)decision1.UnlockableCharacter))
                {
                    Character unlockableCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == decision1.UnlockableCharacter);
                    user.Characters.Add(unlockableCharacter);
                    characterUnlocked = true;
                    unlockableCharacterName = unlockableCharacter.Name;
                }
            }

            game.Populationstate += decision1.Population;
            game.Economystate += decision1.Economy;
            game.Armystate += decision1.Army;
            game.Magicstate += decision1.Magic;
            //COMPRUEBA QUE PERDIO

            if (game.Populationstate < 1)
            {
                game.Gamestate = "FINISHED";
                
            }
            else if (game.Economystate < 1)
            {
                game.Gamestate = "FINISHED";
               
            }
            else if (game.Armystate < 1)
            {
                game.Gamestate = "FINISHED";
               
            }
            else if (game.Magicstate < 1)
            {
                game.Gamestate = "FINISHED";
                
            }
            else
            {
                game.Day += 1;
                //TODO
                user.Gold += 1;
                if (game.Day > user.Maxdays ||user.Maxdays==null)
                {
                    user.Maxdays = game.Day;
                    await _context.SaveChangesAsync();
                }
                game.Lastcardid = await cardService.TakeCard(charactersId);
            }
            
            //COMPRUEBA QUE NO SE PASE DE 100

            if (game.Populationstate > 100)
            {
                game.Populationstate = 100;
            }
            if (game.Economystate > 100)
            {
                game.Economystate = 100;
            }
            if (game.Armystate > 100)
            {
                game.Armystate = 100;
            }
            if (game.Magicstate > 100)
            {
                game.Magicstate = 100;
            }
            await _context.SaveChangesAsync();
            // TOODO PROBAR
            if (game.Gamestate == "FINISHED")
            {
                //  Card cardGameOver= await _context.Cards.Where(c => c.Id== 666).FirstOrDefaultAsync();

                user.Lives--;
                game.Lastcardid = 6;//carta de Game over
                 _context.Games.Remove(game);
               await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            return MapperGameToGameResponse(game,characterUnlocked, unlockableCharacterName);
        }

        public async Task<Game> CreateGame(int playerId)
        {
            Usuario user = await _context.Usuarios.Include(u => u.Characters).FirstOrDefaultAsync(u => u.Id == playerId);

            UpdateLives(user);
            _context.SaveChanges();
            if (user.Lives < 1)
            {
                throw new InvalidOperationException("No tiene vidas suficientes");
            }

            Game game = new Game();
            game.Armystate = 50;
            game.Gamestate = "Playing";
            game.Magicstate = 50;
            game.Day = 0;
            game.Economystate = 50;
            game.Populationstate = 50;

            List<int> charactersId = user.Characters.Select(c => c.Id).ToList();

            game.Lastcardid = await cardService.TakeCard(charactersId);


            //game.Lastcardid = await cardService.TakeCard((int)game.Day);

            bool userExist = _context.Usuarios.Where(u=>u.Id==playerId).Any();
            if (userExist)
            {
                game.Userid = playerId;
                return game;
            }
            else
            {
                throw new Exception("el usuario con el id "+playerId+" no existe!");
            }

        }


        private const int LifeRechargeTimeMinutes = 30;

        public void UpdateLives(Usuario player)
        {
            if (player.Lives < 0)
            {
                player.Lives = 0;  
            }

            if (player.Lives >= player.MaxLives) {

                player.LastLifeRecharge = DateTime.UtcNow;
                return; // No necesita más vidas
            } 
            

            if (player.LastLifeRecharge == null)
            {
                player.LastLifeRecharge = DateTime.UtcNow;
                return;
            }

            TimeSpan timeElapsed = DateTime.UtcNow - player.LastLifeRecharge.Value;
            int livesToAdd = (int)(timeElapsed.TotalMinutes / LifeRechargeTimeMinutes);

            if (livesToAdd > 0)
            {
                player.Lives += livesToAdd;

                // Asegurar que no supere el máximo de 5 vidas
                if (player.Lives > player.MaxLives)
                {
                    player.Lives = player.MaxLives;
                }

                player.LastLifeRecharge = DateTime.UtcNow; // Reiniciar el tiempo de regeneración
            }
        }

        internal async Task RechargeAsync(RechargeLivesRequest request)
        {
            //validar usuario 
            Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.Userid).FirstOrDefaultAsync();
            if (request.clave != usuario.Clave)
            {
                throw new InvalidOperationException("La contraseña es incorrecta");
            }
            if (usuario.MaxLives==usuario.Lives)
            {
                throw new InvalidOperationException("Ya tienes la vida al maximo");
            }
            if (usuario.Gold<1000)
            {
                throw new InvalidOperationException("No tienes suficiente oro para realizar la compra");
            }
            usuario.Gold = usuario.Gold - 1000;
            usuario.Lives = usuario.MaxLives;
            _context.SaveChanges();
            return;
        }
    }
}
