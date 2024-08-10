using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using  RaceElement.Util;
using System.Collections.Generic;
using RaceElement.Broadcast.Structs;
using RaceElement.Broadcast;

namespace RaceElement.Broadcast
{
    public class ACCUdpRemoteClient : IDisposable
    {

        private UdpClient _client;
        private Task _listenerTask;
        public BroadcastingNetworkProtocol MessageHandler { get; }
        public string IpPort { get; }
        public string DisplayName { get; }
        public string ConnectionPassword { get; }
        public string CommandPassword { get; }
        public int MsRealtimeUpdateInterval { get; }

        // Event for driver names received
        public event Action<string[]> OnDriverNamesReceived;

        public ACCUdpRemoteClient(string ip, int port, string displayName, string connectionPassword, string commandPassword, int msRealtimeUpdateInterval)
        {
            IpPort = $"{ip}:{port}";
            MessageHandler = new BroadcastingNetworkProtocol(IpPort, Send);
            MessageHandler.DriverNamesReceived += (drivers) => OnDriverNamesReceived?.Invoke(drivers);

            _client = new UdpClient(new IPEndPoint(IPAddress.Loopback, 0));
            _client.Connect(ip, port);

            DisplayName = displayName;
            ConnectionPassword = connectionPassword;
            CommandPassword = commandPassword;
            MsRealtimeUpdateInterval = msRealtimeUpdateInterval;

            _listenerTask = ConnectAndRun();
        }

        public void RequestData()
        {
            Console.WriteLine("Requesting data from MessageHandler...");
            MessageHandler.RequestData();
        }

        private void Send(byte[] payload)
        {
            if (_client != null)
                _client.Send(payload, payload.Length);
        }

        public void Shutdown()
        {
            ShutdownAsnyc().ContinueWith(t =>
            {
                if (t.Exception?.InnerExceptions?.Any() == true)
                    System.Diagnostics.Debug.WriteLine($"Broadcast Client shut down with {t.Exception.InnerExceptions.Count} errors");
            });
        }

        public async Task ShutdownAsnyc()
        {
            if (_client == null)
                return;

            if (_listenerTask != null && !_listenerTask.IsCompleted)
            {
                MessageHandler.Disconnect();
                _client.Close();
                _client = null;
                await _listenerTask;
            }
        }

        private async Task ConnectAndRun()
        {
            MessageHandler.RequestConnection(DisplayName, ConnectionPassword, MsRealtimeUpdateInterval, CommandPassword);
            while (_client != null)
            {
                try
                {
                    var udpPacket = await _client.ReceiveAsync();
                    Console.WriteLine("Received UDP packet...");
                    using (var ms = new System.IO.MemoryStream(udpPacket.Buffer))
                    using (var reader = new System.IO.BinaryReader(ms))
                    {
                        MessageHandler.ProcessMessage(reader);
                    }
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Client disposed.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving UDP packet: {ex.Message}");
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        if (_client != null)
                        {
                            _client.Close();
                            _client.Dispose();
                            _client = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }

                disposedValue = true;
            }
        }

        public List<CarInfo> GetPilots()
        {
            // foreach (var car in MessageHandler.GetEntryListCars())
            // {
            //     Console.WriteLine(car.DriverName);
            // }
            return MessageHandler.GetEntryListCars();
        }

        #region IDisposable Support
        

       
        public void Dispose()
        {
            Dispose2(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose2(bool disposing)
        {
            if (disposing)
        {
            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
                _client = null;
            }
            
        }
        }
    }
        #endregion
}
    #endregion


