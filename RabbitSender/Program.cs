using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        string queueName = "testQueue";

        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "user", Password = "mypass" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var response = channel.QueueDeclarePassive(queueName);

        Console.WriteLine($"Number of Consumers the queue has & Connected Right Now: {response.ConsumerCount}");

        Console.Write("Enter the Message Id: ");
        string? messageNumber = Console.ReadLine();

        string message = $"Message #{messageNumber}";
        Console.WriteLine(message);
        var body = Encoding.UTF8.GetBytes(message);

        // channel.
        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
        Console.WriteLine($"Sent {message}");
    }
}
