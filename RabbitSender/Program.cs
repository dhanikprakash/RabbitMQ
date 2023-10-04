using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://username:password@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";


IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "DemoRoutingKey";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

foreach(int i in Enumerable.Range(1, 1000))
{
    byte[] messageBodyByte = Encoding.UTF8.GetBytes($"Hello World-{i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyByte);
    Thread.Sleep(10);
}


channel.Close();
cnn.Close();