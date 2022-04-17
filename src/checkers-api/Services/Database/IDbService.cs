using checkers_api.Models.PrimitiveModels;

namespace checkers_api.Services;

public interface IDbService
{
    Task<Profile?> GetUserByEmailAsync(string userEmail);
    Task<Profile?> GetUserByIdAsync(string userId);
    Task AddUserAsync(Profile user);
    Task RemoveUserByIdAsync(int userId);
    Task UpdateUserAsync(Profile user);
    Task<IEnumerable<Review>> GetReviewsAsync();
    Task AddReviewAsync(Review comment);
}