using checkers_api.Models.PrimitiveModels;
using Npgsql;

namespace checkers_api.Services;

public class DbService : IDbService, IAsyncDisposable
{
    private readonly ILogger<DbService> logger;
    private readonly NpgsqlConnection connection;

    public DbService(ILogger<DbService> logger, IConfiguration configuration)
    {
        this.logger = logger;
        connection = new(configuration["CONNECTION_STRING"]);
        connection.Open();
    }

    public async Task AddUserAsync(Profile user)
    {
        try
        {
            logger.LogDebug("[{location}]: Adding user {email} to database", nameof(DbService), user.Email);

            var query = "INSERT INTO checkers.player (player_id, email, given_name, family_name, picture_url) VALUES (@player_id, @email, @given_name, @family_name, @picture_url)";

            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("player_id", user.Id);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("given_name", user.GivenName);
            cmd.Parameters.AddWithValue("family_name", user.FamilyName);
            cmd.Parameters.AddWithValue("picture_url", user.Picture);
            await cmd.ExecuteNonQueryAsync();

            logger.LogDebug("[{location}]: User {email} successfully added to the database", nameof(DbService), user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not add user {email} to database. Ex: {ex}", nameof(DbService), user.Email, ex);
            throw;
        }
    }


    public async Task<Profile?> GetUserByEmailAsync(string email)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for {email}", nameof(DbService), email);

            var query = "SELECT * FROM checkers.player WHERE email=@email";
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("email", email);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var profile = new Profile(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                logger.LogDebug("[{location}]: User profile found for {email}", nameof(DbService), email);
                return profile;
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for {email}. Ex: {ex}", nameof(DbService), email, ex);
        }
        return null;
    }

    public async Task<Profile?> GetUserByIdAsync(string playerId)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for id {id}", nameof(DbService), playerId);

            var query = "SELECT * FROM checkers.player WHERE player_id=@player_id";
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("player_id", playerId);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var profile = new Profile(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                logger.LogDebug("[{location}]: User profile found for id {id}", nameof(DbService), playerId);
                return profile;
            }
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: User profile for id {id} does not exist. Ex: {ex}", nameof(DbService), playerId, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for id {id}. Ex: {ex}", nameof(DbService), playerId, ex);
        }
        return null;
    }

    public Task RemoveUserByIdAsync(int userId)
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

    public Task UpdateUserAsync(Profile user)
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

    //TODO: add logging
    public async Task<IEnumerable<Review>> GetReviewsAsync()
    {
        try
        {
            var reviews = new List<Review>();
            var query = "SELECT r.review_id, COALESCE(p.given_name, 'Anonymous'), r.content, r.posted_on FROM checkers.review r LEFT JOIN checkers.player p ON (r.player_id = p.player_id)";

            await using var cmd = new NpgsqlCommand(query, connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var review = new Review(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3));
                reviews.Add(review);
            }

            return reviews;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get reviews. Ex {ex}", nameof(DbService), ex);
            throw;
        }
    }

    public async Task AddReviewAsync(string content, string? playerId)
    {
        try
        {
            var query = "INSERT INTO checkers.review (review_id, player_id, content, posted_on) VALUES (@review_id, @player_id, @content, @posted_on)";
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.Add(new NpgsqlParameter<string>("review_id", IdGenerator.GetId()));
            cmd.Parameters.Add(new NpgsqlParameter<string?>("player_id", playerId));
            cmd.Parameters.Add(new NpgsqlParameter<string>("content", content));
            cmd.Parameters.Add(new NpgsqlParameter<DateTime>("posted_on", DateTime.Now));
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not add review. Ex: {ex}", nameof(DbService), ex);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await connection.DisposeAsync();
    }
}