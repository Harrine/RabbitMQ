using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using RabbitMQ.Client;

namespace Chat_RabbitMQ.Services
{
    public interface IRabbitMQService
    {
        // void SendMessage(string queueName, string message);
        void SendMessage(string queueName, string sender, string message);

        // string ReceiveMessage(string queueName);
        (string Sender, string Message) ReceiveMessage(string queueName);
    }
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _hostName;

        public RabbitMQService(IConfiguration configuration)
        {
            _hostName = configuration["RabbitMQ:HostName"];
            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        // public string ReceiveMessage(string queueName)
        // {
        //     // Making channel
        //     _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        //     var result = _channel.BasicGet(queueName, autoAck: true);
        //     return result != null ? Encoding.UTF8.GetString(result.Body.ToArray()) : null;
        // }

        public (string Sender, string Message) ReceiveMessage(string queueName)
        {
            // Declare the queue
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Retrieve the message
            var result = _channel.BasicGet(queueName, autoAck: true);
            if (result == null)
            {
                return (null, null); // No message available
            }

            // Extract the message body
            var messageBody = Encoding.UTF8.GetString(result.Body.ToArray());

            // Extract the sender from the message (assuming the sender is included in the message)
            // For example, the message format could be "sender:message"
            var messageParts = messageBody.Split(new[] { ':' }, 2);
            if (messageParts.Length != 2)
            {
                return (null, null); // Invalid message format
            }

            var sender = messageParts[0];
            var message = messageParts[1];

            return (sender, message);
        }


        // public void SendMessage(string queueName, string message)
        // {
        //     // channel is created
        //     _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        //     // message to send
        //     var body = Encoding.UTF8.GetBytes(message);
        //     _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        // }
        public void SendMessage(string queueName, string sender, string message)
        {
            // Declare the queue
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Combine the sender and message into a single string
            var fullMessage = $"{sender}:{message}";

            // Convert the message to bytes
            var body = Encoding.UTF8.GetBytes(fullMessage);

            // Publish the message
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}