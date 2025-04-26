namespace Juego_Sin_Nombre.Services
{
    using Juego_Sin_Nombre.config;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using MimeKit;

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendPurchaseConfirmationAsync(string toEmail, string ofertaNombre, int cantidadDiamantes);
        Task SendGiftCouponEmailAsync(string toEmail, string userName, string couponCode);
        Task SendGiftCouponToAllUsersAsync(string couponCode);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly Data.ApplicationContext _context;


        public EmailService(IOptions<EmailSettings> settings, Data.ApplicationContext context)
        {
            _settings = settings.Value;
            _context = context;
        }
        public async Task SendGiftCouponToAllUsersAsync(string couponCode)
        {
            // Obtenemos todos los usuarios que NO sean admins
            var users = await _context.Usuarios
                .Where(u => u.Rol != "Admin") 
                .ToListAsync();

            foreach (var user in users)
            {
                try
                {
                    await SendGiftCouponEmailAsync(
                        toEmail: user.Email,
                        userName: user.Username, 
                        couponCode: couponCode
                    );
                }
                catch (Exception ex)
                {
                  
                    Console.WriteLine($"Error enviando email a {user.Email}: {ex.Message}");
                }
            }
        }

        public async Task SendGiftCouponEmailAsync(string toEmail, string userName, string couponCode)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "¡Tenés un cupón de regalo esperándote!";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; padding: 25px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                        <h2 style='color: #FF5722;'>¡Hola {userName}!</h2>
                        <p>Queremos premiarte con un regalo especial 🎁</p>
                        <p>Usá el siguiente <strong>cupón</strong> y obtené premios exclusivos en <strong>Baraja Real</strong>:</p>
                        <div style='margin: 20px 0; padding: 15px; background-color: #f2f2f2; border-left: 5px solid #FF5722; font-size: 20px; text-align: center; letter-spacing: 1px;'>
                            <strong>{couponCode}</strong>
                        </div>
                        <p>¡Canjealo ahora mismo y descubrí tu recompensa!</p>
                        <a href='https://barajareal.online/Front/pages/pagesIndex/Tienda/tienda.html' 
                           style='display: inline-block; margin-top: 15px; padding: 10px 20px; background-color: #FF5722; color: white; text-decoration: none; border-radius: 5px;'>
                           Canjear Cupón
                        </a>
                        <hr style='margin-top: 30px;' />
                        <p style='font-size: 0.9em; color: gray;'>Este mensaje fue enviado automáticamente. No respondas a este correo.</p>
                    </div>
                </body>
            </html>"
            };

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPurchaseConfirmationAsync(string toEmail, string ofertaNombre, int cantidadDiamantes)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "¡Tu compra fue exitosa!";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                        <h2 style='color: #4CAF50;'>¡Compra realizada con éxito!</h2>
                        <p>Gracias por adquirir la oferta <strong>{ofertaNombre}</strong>.</p>
                        <p>Ya podés ver <strong>{cantidadDiamantes} diamantes</strong> acreditados en tu cuenta.</p>
                        <p>¡Disfrutá de todos los beneficios en <strong>Baraja Real</strong>!</p>
                        <a href='https://barajareal.online/Front/pages/pagesIndex/index.html' 
                           style='display: inline-block; margin-top: 15px; padding: 10px 15px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>
                           Ver mis diamantes
                        </a>
                        <hr style='margin-top: 30px;' />
                        <p style='font-size: 0.9em; color: gray;'>Este es un mensaje automático, no respondas este correo.</p>
                        <p style='font-size: 0.9em; color: gray;'>Si tenes una consulta podes comunicarte con soporte@barajareal.online</p>
                    </div>
                </body>
            </html>"
            };

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
