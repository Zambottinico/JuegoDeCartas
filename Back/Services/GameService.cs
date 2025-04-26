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
        public async Task<GameResponse> MapperGameToGameResponseAsync(Game game, Boolean characterUnlocked)
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

            Usuario user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == game.Userid);
            gameResponse.UserName = user.Email;
            gameResponse.Gold = user.Gold;
            gameResponse.Diamonds = user.Diamonds;
            gameResponse.Lives = user.Lives;

            await UpdateLivesAsync(user);
            await _context.SaveChangesAsync();

            if (user.Lives < 1)
            {
                throw new ArgumentException("No tiene suficientes vidas para jugar");
            }

            Card card = await _context.Cards.Include(u => u.Character).FirstOrDefaultAsync(c => c.Id == game.Lastcardid);
            CardResponse cardResponse = new CardResponse();
            cardResponse.Typeid = (int)card.Typeid;
            cardResponse.Character = card.Character.Name;
            cardResponse.Description = card.Description;

            Decision decision = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == card.Decision1);
            PostDecisionDto decisionResponse1 = new PostDecisionDto();
            decisionResponse1.Convert(decision);
            cardResponse.Decision1 = decisionResponse1;

            Decision decision2 = await _context.Decisions.FirstOrDefaultAsync(d => d.Id == card.Decision2);
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
            await UpdateLivesAsync(user);
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
                //Comprobar si desbloquea Personaje
                if (decision1.UnlockableCharacter != null)
                {
                    if (!charactersId.Contains((int)decision1.UnlockableCharacter))
                    {
                        Character unlockableCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == decision1.UnlockableCharacter);
                        user.Characters.Add(unlockableCharacter);
                        characterUnlocked = true;
                        unlockableCharacterName = unlockableCharacter.Name;
                    }
                }


                var config = await this.GetGameConfigAsync();
                game.Day += 1;
                //TODO
                user.Gold += 1;
                //El dia es múltiplo de 50 gana 1 diamante
                if (game.Day % config.DaysToEarnDiamond == 0)
                {
                    user.Diamonds++;
                    await _context.SaveChangesAsync();
                }
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

            await UpdateLivesAsync(user);
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


       

        public async Task UpdateLivesAsync(Usuario player)
        {
            var config = await this.GetGameConfigAsync();
            if (player.Lives < 0)
            {
                player.Lives = 0;  
            }

            if (player.Lives >= player.MaxLives) {

                player.LastLifeRecharge = DateTime.Now;
                return; // No necesita más vidas
            } 
            

            if (player.LastLifeRecharge == null)
            {
                player.LastLifeRecharge = DateTime.Now;
                return;
            }

            TimeSpan timeElapsed = DateTime.Now - player.LastLifeRecharge.Value;
            int livesToAdd = (int)(timeElapsed.TotalMinutes / config.MinutesToEarnLife);

            if (livesToAdd > 0)
            {
                player.Lives += livesToAdd;

                // Asegurar que no supere el máximo de 5 vidas
                if (player.Lives > player.MaxLives)
                {
                    player.Lives = player.MaxLives;
                }

                player.LastLifeRecharge = DateTime.Now; // Reiniciar el tiempo de regeneración
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
            var existingConfig = await _context.GameConfig.FirstOrDefaultAsync();
            if (usuario.Gold<existingConfig.LifeRechargePrice)
            {
                throw new InvalidOperationException("No tienes suficiente oro para realizar la compra");
            }
            usuario.Gold = usuario.Gold - existingConfig.LifeRechargePrice;
            usuario.Lives = usuario.MaxLives;
            _context.SaveChanges();
            return;
        }


        public async Task<GameConfig> UpdateOrCreateGameConfigAsync(GameConfig gameConfig)
        {
            // Verifica si existe un GameConfig
            var existingConfig = await _context.GameConfig.FirstOrDefaultAsync();
            if (existingConfig != null)
            {
                // Si existe, actualiza el registro
                existingConfig.LifeRechargePrice = gameConfig.LifeRechargePrice;
                existingConfig.DaysToEarnDiamond = gameConfig.DaysToEarnDiamond;
                existingConfig.MinutesToEarnLife = gameConfig.MinutesToEarnLife;

                _context.GameConfig.Update(existingConfig);
            }
            else
            {
                // Si no existe, crea uno nuevo
                _context.GameConfig.Add(gameConfig);
            }

            // Guarda los cambios
            await _context.SaveChangesAsync();

            return gameConfig;
        }

        public async Task<GameConfig> GetGameConfigAsync()
        {
            var config = await _context.GameConfig.FirstOrDefaultAsync();
            if (config == null)
            {
                throw new InvalidOperationException("Game configuration not found.");
            }
            return config;
        }
        public async Task<Cupon> CrearCupon(CrearCuponRequest request)
        {
            var cupon = new Cupon
            {
                NumeroDiamantes = request.NumeroDiamantes,
                NumeroOro = request.NumeroOro,
                Codigo = request.Codigo,
                IsDeleted = false
                
            };

            _context.Cupones.Add(cupon); 
            await _context.SaveChangesAsync(); 

            return cupon; 
        }

        internal async Task<CanjearCuponResponse> CanjearCupon(string codigo, Usuario usuario)
        {
            Cupon cupon =await _context.Cupones.Include(c => c.Players).Where(c=>c.IsDeleted==false).FirstOrDefaultAsync(c=>c.Codigo==codigo);
            if (cupon==null)
            {
                return new CanjearCuponResponse
                {
                    Codigo = codigo,
                    message = "el cupón no existe o ya no es valido",
                    Ok = false,
                    NumeroDiamantes = 0,
                    NumeroOro = 0
                };
            }
            if (cupon.Players.Contains(usuario))
            {
                return new CanjearCuponResponse
                {
                    Codigo = codigo,
                    message = "No se puede canjear el mismo cupón 2 veces",
                    Ok = false,
                    NumeroDiamantes = 0,
                    NumeroOro = 0
                }; 
            }
            usuario.Diamonds = usuario.Diamonds + cupon.NumeroDiamantes;
            usuario.Gold = usuario.Gold + cupon.NumeroOro;
            cupon.Players.Add(usuario);
            await _context.SaveChangesAsync();
            return new CanjearCuponResponse
            {
                Codigo=codigo,
                message="Cupón canjeado con éxito",
                Ok=true,
                NumeroDiamantes=cupon.NumeroDiamantes,
                NumeroOro=cupon.NumeroOro


            };
        }

        internal async Task updateCupon(UpdateCuponRequest request)
        {
            
            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Id == request.Id);

            if (cupon == null)
            {
                throw new Exception("Cupón no encontrado");
            }
            cupon.NumeroDiamantes = request.NumeroDiamantes;
            cupon.NumeroOro = request.NumeroOro;
            cupon.Codigo = request.Codigo;
            cupon.IsDeleted = request.IsDeleted;

            // Guardar cambios
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cupon>> GetAllCupons()
        {
            return await _context.Cupones.ToListAsync();
        }

    }
}
