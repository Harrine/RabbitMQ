
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World! Sender is able to send the message....");

// ConnectionFactory: Creates a connection factory object to configure the connection to the RabbitMQ server. In this case, it connects to a RabbitMQ instance running on localhost
var factory = new ConnectionFactory { HostName = "localhost" };
// CreateConnectionAsync: Establishes an asynchronous connection to the RabbitMQ server.
using var connection = await factory.CreateConnectionAsync();
// CreateChannelAsync: Creates an asynchronous channel for communication with the RabbitMQ server. Channels are used to send and receive messages.
using var channel = await connection.CreateChannelAsync();


// await channel.QueueDeclareAsync(queue: "harrine", durable: false, exclusive: false, autoDelete: false,
//     arguments: null);

// const string message = "Hello Harrine!";
// var body = Encoding.UTF8.GetBytes(message);

// await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
// Console.WriteLine($" [x] Sent {message}");


// // At every two second the message will sent here we have simple excute the loop at each and every 2 second an the logic is as follow ...
// int messageCount = 0;
// while (messageCount < 10)
// {
//     var current_time = DateTime.Now.ToString("HH:mm:ss");
//     var mess = $"Hey Harrine the message send at : {current_time}";
//     var body = Encoding.UTF8.GetBytes(mess);

//     await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello1", body: body);
//     Console.WriteLine($"[x] Sent The {mess}.....");

//     await Task.Delay(2000);
//     messageCount++;
// }
// Console.WriteLine("Finished Sending message ...");

// To harrine receive massage from krish
await channel.QueueDeclareAsync(queue: "krish", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Krish : {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("krish", autoAck: true, consumer: consumer);


// For Sending the message from harrine to krish
await channel.QueueDeclareAsync(queue: "harrine", durable: false, exclusive: false, autoDelete: false,
    arguments: null);
Console.WriteLine("--------------------------------------------------");
Console.WriteLine("To Exit -->> type Exit");
while (true)
{
    Console.WriteLine("From Harrine :");
    var userInput = Console.ReadLine();

    if (userInput.ToLower() == "exit")
    {
        break;
    }

    if (!string.IsNullOrEmpty(userInput))
    {
        var body = Encoding.UTF8.GetBytes(userInput);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "harrine", body: body);
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