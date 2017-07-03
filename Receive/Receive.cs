using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receive
{
    class Receive
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Receive += (model, ea) =>
                      {
                          var body = ea.body;
                          var message = Encoding.UTF8.GetString(body);
                          System.Console.WriteLine(" [x] Receive {0}", message);

                      };

                    channel.BasicConsume(queue: "hello", noAck: true, consumer: consumer);
                    System.Console.WriteLine(" press [enter] to exit.");
                    Console.ReadLine();

                }
            }

        }
    }
}
