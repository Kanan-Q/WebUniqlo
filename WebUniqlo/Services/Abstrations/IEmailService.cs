namespace WebUniqlo.Services.Abstrations
{
    public interface IEmailService
    {
       public void SendEmailConfirmation(string receiver, string name, string token);
    }
}
