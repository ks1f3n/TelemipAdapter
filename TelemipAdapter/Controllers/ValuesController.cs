using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using TelemipAdapter.Dtos;
using TelemipAdapter.Models.Gaps;
using TelemipAdapter.Models.Incls;
using TelemipAdapter.Models.Values;

namespace TelemipAdapter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _clientOptions;
        private readonly SensorDbContext _sensorDbContext;

        public ValuesController(IMqttClient mqttClient, IMqttClientOptions clientOptions, SensorDbContext sensorDbContext, ILogger<ValuesController> logger)
        {
            _mqttClient = mqttClient;
            _clientOptions = clientOptions;
            _logger = logger;
            _sensorDbContext = sensorDbContext;
        }

        [HttpPost("Gap/{id}")]
        public async Task<IActionResult> CreateGapAsync([FromForm] CreateGapSensorDto createGapSensorDto)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                    await _mqttClient.ConnectAsync(_clientOptions);

                var dateNow = DateTime.Now;
                var dateTimeOffset = new DateTimeOffset(dateNow);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();               

                if (createGapSensorDto.v != null)
                {
                    _logger.LogInformation("D: {0}", createGapSensorDto.v[0]);

                    var currGap = await _sensorDbContext.Gap.FindAsync(createGapSensorDto.ID);
                    if (currGap == null)
                    {
                        await _sensorDbContext.Gap.AddAsync(new Gap()
                        {
                            Id = createGapSensorDto.ID,
                            Value = createGapSensorDto.v[0],
                            InitValue = - createGapSensorDto.v[0],                            
                            Period = 120
                        });
                        await _sensorDbContext.SaveChangesAsync(true);
                        currGap = await _sensorDbContext.Gap.FindAsync(createGapSensorDto.ID);
                    }

                    currGap.Value = createGapSensorDto.v[0];
                    _sensorDbContext.SaveChanges(true);

                 
                    var info = new SensorInfo(createGapSensorDto.PER, createGapSensorDto.VOLT, createGapSensorDto.CSQ);
                    var meas = createGapSensorDto.v.Select((d, i) => new GapSensorMeas(d, createGapSensorDto.t[i], currGap.InitValue));
                    var msg = new Message(info, meas);

                    var str = JsonConvert.SerializeObject(msg);

                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic("legacy/gap/" + createGapSensorDto.ID)
                        .WithPayload(str)
                        .WithAtMostOnceQoS()
                        .Build();

                    await _mqttClient.PublishAsync(message);

                    var sper = currGap.Period > 0 ? currGap.Period : 120;
                    var mper = sper / 20;
                    return Ok(String.Format("PER={0},TSP={1},ENC={2},", sper, unixDateTime, mper));
                }   
                return Ok(String.Format("PER={0},TSP={1},ENC={2},", 120, unixDateTime, 6));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            await _mqttClient.DisconnectAsync();
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("Incl/{id}")]
        public async Task<IActionResult> CreateInclAsync([FromForm] CreateInclSensorDto createInclSensorDto)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                    await _mqttClient.ConnectAsync(_clientOptions);

                var dateNow = DateTime.Now;
                var dateTimeOffset = new DateTimeOffset(dateNow);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

                if (createInclSensorDto.X != null)
                {
                    _logger.LogInformation("X: {0}; Y: {1}", createInclSensorDto.X[0], createInclSensorDto.Y[0]);

                    var currIncl = await _sensorDbContext.Incl.FindAsync(createInclSensorDto.UID);
                    if (currIncl == null)
                    {
                        await _sensorDbContext.Incl.AddAsync(new Incl()
                        {
                            Id = createInclSensorDto.UID,
                            X = createInclSensorDto.X[0],
                            Y = createInclSensorDto.Y[0],
                            InitX = createInclSensorDto.X[0] - 1024,
                            InitY = createInclSensorDto.Y[0] - 1024,
                            Period = 120
                        });
                        await _sensorDbContext.SaveChangesAsync(true);
                        currIncl = await _sensorDbContext.Incl.FindAsync(createInclSensorDto.UID);
                    }

                    var info = new SensorInfo(createInclSensorDto.PER, createInclSensorDto.VOLT, createInclSensorDto.CSQ);
                    var meas = createInclSensorDto.X.Select((x, i) => new InclSensorMeas(x, createInclSensorDto.Y[i], createInclSensorDto.T[i], createInclSensorDto.TS[i], currIncl.InitX, currIncl.InitY));
                    var msg = new Message(info, meas);

                    var str = JsonConvert.SerializeObject(msg);

                    var message = new MqttApplicationMessageBuilder()
                       .WithTopic("legacy/incl/" + createInclSensorDto.UID)
                       .WithPayload(str)
                       .WithAtMostOnceQoS()
                       .Build();

                    await _mqttClient.PublishAsync(message);

                    var sper = currIncl.Period > 0 ? currIncl.Period : 120;
                    var mper = sper / 20;
                    return Ok(String.Format("UID={0}, ST={1}, TS={2}, SPER={3}, MPER={4}, IGVAL={5},CRVAL={6},", createInclSensorDto.UID, createInclSensorDto.ST, unixDateTime, sper, mper, 2, 150));
                }
                return Ok(String.Format("UID={0}, ST={1}, TS={2}, SPER={3}, MPER={4}, IGVAL={5},CRVAL={6},", createInclSensorDto.UID, createInclSensorDto.ST, unixDateTime, 120, 6, 2, 150));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            await _mqttClient.DisconnectAsync();
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}