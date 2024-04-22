using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "user", Password = "mypass" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();


        string queueName = "hello";

        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var response = channel.QueueDeclarePassive(queueName);
        Console.WriteLine($"Messages ready state in the queue: {response.MessageCount}");
        

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {            
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received {message}");
        };
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();

    }
}
