using Microsoft.AspNetCore.DataProtection;

namespace SaritasaT.Securities
{
    public class IDataProtection
    {
        private readonly IDataProtector protector;
        public IDataProtection(IDataProtectionProvider dataProtectionProvider)
        {
            protector = dataProtectionProvider.CreateProtector(Environment.GetEnvironmentVariable("ASPNETCORE_DATAKEY"));
        }
        public string Encode(string data)
        {
            return protector.Protect(data);
        }
        public string Decode(string data)
        {
            return protector.Unprotect(data);
        }
    }
}
