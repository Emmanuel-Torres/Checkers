using checkers_api.Models.PersistentModels;
using Npgsql;

namespace checkers_api.Services;

public class UserDbService : IUserDbService, IAsyncDisposable
{
    private readonly ILogger<UserDbService> logger;
    private readonly NpgsqlConnection connection;

    public UserDbService(ILogger<UserDbService> logger, IConfiguration configuration)
    {
        this.logger = logger;
        connection = new(configuration["CONNECTION_STRING"]);
        connection.Open();
    }

    public async Task AddUserAsync(DbProfile user)
    {
        try
        {
            logger.LogDebug("[{location}]: Adding user {email} to database", nameof(UserDbService), user.Email);

            var query = "INSERT INTO checkers.player (player_id, email, given_name, family_name, picture_url) VALUES (@player_id, @email, @given_name, @family_name, @picture_url)";
            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("player_id", user.Id);
                cmd.Parameters.AddWithValue("email", user.Email);
                cmd.Parameters.AddWithValue("given_name", user.GivenName);
                cmd.Parameters.AddWithValue("family_name", user.FamilyName);
                cmd.Parameters.AddWithValue("picture_url", user.Picture);
                await cmd.ExecuteNonQueryAsync();
            }

            logger.LogDebug("[{location}]: User {email} successfully added to the database", nameof(UserDbService), user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not add user {email} to database. Ex: {ex}", nameof(UserDbService), user.Email, ex);
            throw;
        }
    }


    public async Task<DbProfile?> GetUserByEmailAsync(string email)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for {email}", nameof(UserDbService), email);

            var query = "SELECT * FROM checkers.player WHERE email=@email";
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("email", email);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var profile = new DbProfile(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                logger.LogDebug("[{location}]: User profile found for {email}", nameof(UserDbService), email);
                return profile;
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for {email}. Ex: {ex}", nameof(UserDbService), email, ex);
        }
        return null;
    }

    public async Task<DbProfile?> GetUserByIdAsync(string playerId)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for id {id}", nameof(UserDbService), playerId);

            var query = "SELECT * FROM checkers.player WHERE player_id=@player_id";
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("player_id", playerId);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var profile = new DbProfile(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                logger.LogDebug("[{location}]: User profile found for id {id}", nameof(UserDbService), playerId);
                return profile;
            }
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: User profile for id {id} does not exist. Ex: {ex}", nameof(UserDbService), playerId, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for id {id}. Ex: {ex}", nameof(UserDbService), playerId, ex);
        }
        return null;
    }

    public async Task RemoveUserByIdAsync(int userId)
    {
        throw new NotImplementedException();
        // try
        // {
        //     throw new NotImplementedException();
        //     logger.LogDebug("[{location}]: Removing user with id {id} from database", nameof(UserDbService), userId);
        //     logger.LogInformation("[{location}]: User {id} was successfully removed from the database", nameof(UserDbService), userId);
        // }
        // catch (ArgumentNullException ex)
        // {
        //     logger.LogWarning("[{location}]: Could not remove user with id {id} because it does not exist. Ex: {ex}", nameof(UserDbService), userId, ex);
        //     throw;
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError("[{location}]: Could not remove user with id {id}. Ex: {ex}", nameof(UserDbService), userId, ex);
        //     throw;
        // }
    }

    public async Task UpdateUserAsync(DbProfile user)
    {
        throw new NotImplementedException();
        // var userId = user.Id;

        // try
        // {
        //     logger.LogDebug("[{location}]: Updating user with id {id}.", nameof(UserDbService), userId);
        //     logger.LogInformation("[{location}]: User {id} was successfully updated", nameof(UserDbService), userId);
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError("[{location}]: Could not update user with id {id}. Ex: {ex}", nameof(UserDbService), userId, ex);
        //     throw;
        // }
    }

    public async ValueTask DisposeAsync()
    {
        await connection.DisposeAsync();
    }
}