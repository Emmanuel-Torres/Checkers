using System;
using System.Collections.Concurrent;
using System.Text;
using Azure.Messaging.ServiceBus;
using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;
using Newtonsoft.Json;

namespace checkers_api.Services;

public class MatchmakingService : IMatchmakingService
{
    private readonly ILogger<MatchmakingService> logger;
    private readonly IConfiguration configuration;
    private readonly ServiceBusClient serviceBusClient;
    private readonly ConcurrentQueue<Player> matchMakingQueue;
    private readonly string serviceBusConnectionString;
    private readonly string queueName;
    private Action<Player, Player>? startGame;

    public MatchmakingService(ILogger<MatchmakingService> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        serviceBusConnectionString = configuration["QUEUE_CONNECTION_STRING"];
        queueName = configuration["QUEUE_NAME"];
        serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
        matchMakingQueue = new ConcurrentQueue<Player>();
        startGame = null;
    }

    public bool CancelMatchMaking(Id playerId)
    {
        throw new NotImplementedException();
    }

    public async Task MatchMakeAsync(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);

        var p = JsonConvert.SerializeObject(player);

        try
        {
            var sender = serviceBusClient.CreateSender(queueName);
            var message = new ServiceBusMessage(p);

            await sender.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not send player {playerId} to service bus. Exception: {ex}", 
                nameof(MatchmakingService), player.PlayerId.Value, ex);
            throw;
        }
    }

    public async Task ConfigureQueue(Action<Player, Player> startGame)
    {
        this.startGame = startGame;
        var processor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        await processor.StartProcessingAsync();
        logger.LogInformation("[{location}]: Service bus queue was set up correctly", nameof(MatchmakingService));
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        logger.LogDebug("[{location}]: Received a message from service bus", nameof(MatchmakingService));

        var body = Encoding.ASCII.GetString(args.Message.Body);
        var player = JsonConvert.DeserializeObject<Player>(body);

        await args.CompleteMessageAsync(args.Message);

        if (player is null)
        {
            throw new NullReferenceException(nameof(player));
        }

        matchMakingQueue.Enqueue(player);

        TryStartGame();
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }

    private void TryStartGame()
    {
        while (matchMakingQueue.Count > 1)
        {
            if (!matchMakingQueue.TryDequeue(out var p1))
            {
                break;
            }
            if (!matchMakingQueue.TryDequeue(out var p2))
            {
                matchMakingQueue.Enqueue(p1);
                break;
            }

            //Logic to start one game
            startGame!(p1, p2);
        }
    }
}