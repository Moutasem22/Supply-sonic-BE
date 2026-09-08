using Helpers;
using IServiceContractor.ICommonService;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly EmailServiceSettings _emailServiceSettings;
    public RabbitMQProducer(IOptions<EmailServiceSettings> emailServiceSettings)
    {
        _emailServiceSettings = emailServiceSettings.Value;
    }
    public async void SendMessage<T>(T message)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _emailServiceSettings.HostName,
                UserName = _emailServiceSettings.UserName,
                Password = _emailServiceSettings.Password,
                VirtualHost = _emailServiceSettings.VirtualHost,
                Port = _emailServiceSettings.Port
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("sendemail", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "sendemail", body: body);
        }
        catch (Exception ex)
        {

        }
    }
}
