using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public HomeController()
        {
            _factory = new ConnectionFactory()
            {
                Uri = new Uri("https://customer.cloudamqp.com/login")
            };
            _connection = _factory.CreateConnection();

        }




        [HttpPost]
        public IActionResult Post()
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare("inventiv-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);



                var message = "Hello Inventiv: " + DateTime.Now + "!..";
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                //Exchange parametresi boş ayarlanırsa, default olarak direct exchange olarak davranır.
                channel.BasicPublish("", "inventiv-queue", null, body);

            return Ok("Mesaj Gönderldi" + message);
        }
    }
}
