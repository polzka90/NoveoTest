using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Queue
{
    public static class MessageHandler
    {
        public static void Send(string queue, string data)
        {
            using (IConnection connection = new ConnectionFactory().CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
                }
            }
        }

        public static string Receive(string queue)
        {
            string data = String.Empty;
            using (IConnection connection = new ConnectionFactory().CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    //var consumer = new EventingBasicConsumer(channel);
                    BasicGetResult result = channel.BasicGet(queue, true);
                    if (result != null)
                    {
                        data =
                        Encoding.UTF8.GetString(result.Body);
                        Console.WriteLine(data);
                    }

                    //var consumer = new EventingBasicConsumer(channel);
                    //consumer.Received += (model, ea) =>
                    //{
                    //    var body = ea.Body;
                    //    var message = Encoding.UTF8.GetString(body);
                    //    Console.WriteLine(" [x] Received {0}", message);

                    //    int dots = message.Split('.').Length - 1;
                    //    Thread.Sleep(dots * 1000);

                    //    Console.WriteLine(" [x] Done");
                    //};
                    //channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
                }
            }
            return data;
        }
    }
}
