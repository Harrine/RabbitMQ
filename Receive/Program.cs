// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Receiver Started");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// For receive message from harrine to krish
await channel.QueueDeclareAsync(queue: "harrine", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Harrine: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("harrine", autoAck: true, consumer: consumer);

// For Sending the message from krish to harrine
await channel.QueueDeclareAsync(queue: "krish", durable: false, exclusive: false, autoDelete: false,
    arguments: null);
Console.WriteLine("--------------------------------------------------");
Console.WriteLine("To Exit -->> type Exit");
while (true)
{
    Console.WriteLine("From Krish :");
    var userInput = Console.ReadLine();

    if (userInput.ToLower() == "exit")
    {
        break;
    }

    if (!string.IsNullOrEmpty(userInput))
    {
        var body = Encoding.UTF8.GetBytes(userInput);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "krish", body: body);
        // Console.WriteLine("Message Sent.....");
        Console.WriteLine("--------------------------------------------------");
    }
    else
    {
        Console.WriteLine("Try Again message cannot be null");
        Console.WriteLine("--------------------------------------------------");
    }
}
Console.WriteLine("Exiting the programm...");


// Console.WriteLine(" Press [enter] to exit.");
// Console.ReadLine();