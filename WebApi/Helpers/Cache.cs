using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace WebApi.Helpers;

public class Cache : ICache
{
    private readonly IMemoryCache _cache;
    public Cache(IMemoryCache cache)
    {
        _cache = cache;
    }
    public void StoreRandomNumber(int value)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
        };

        _cache.Set("random_number", value, options);
    }
    public int? GetRandomNumber()
    {
        _cache.TryGetValue("random_number", out int randomNumber);
        return randomNumber;
    }
    public void StoreEmail(string email)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
        };

        _cache.Set("user_email", email, options);
    }
    public string? GetUserEmail()
    {
        _cache.TryGetValue("user_email", out string? email);
        return email;
    }
}