using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using ECS;

namespace ConsolePaint
{
	public class DataTransfer
	{
		private const int _port = 8888; // порт для прослушивания подключений
		private IPAddress _localAddr = IPAddress.Parse("127.0.0.1");

		private TcpListener _server = null;
		private bool _clientConnected = false;
		private NetworkStream _stream;
		private BinaryFormatter _binaryFormatter = new BinaryFormatter();

		public DataTransfer(World world)
		{
			world.UpdateDataDebugs += _world_UpdateDataDebugs;
			CreateTCP();
		}

		private void _world_UpdateDataDebugs(object sender, System.Collections.Generic.List<DataDebug> e)
		{
			if (_clientConnected)
			{
				try
				{
					_binaryFormatter.Serialize(_stream, e);
				}
				catch (Exception)
				{
					_clientConnected = false;
					if (_server != null)
					{
						_server.Stop();
					}
				}
			}
		}

		private void CreateTCP()
		{
			Task.Run(() =>
			{
				while (true)
				{
					while (!_clientConnected)
					{
						try
						{
							_server = new TcpListener(_localAddr, _port);
							_server.Start();
							TcpClient client = _server.AcceptTcpClient();
							_clientConnected = true;
							_stream = client.GetStream();
						}
						catch (Exception)
						{
							if (_server != null)
							{
								_server.Stop();
							}
						}
						finally
						{

						}
					} 
				}
			});
		}
	}
}
