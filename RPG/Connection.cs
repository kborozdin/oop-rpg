using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.IO;

namespace RPG
{
	public class Connection
	{
		const int Size = (int)1e5;
		private readonly JsonSerializer serializer = new JsonSerializer();

		private Socket socket;

		public Connection(Socket socket)
		{
			this.socket = socket;
		}

		public void Write<T>(T obj)
		{
			var buffer = new byte[Size];
			var ms = new MemoryStream(buffer);
			using (var writer = new BsonWriter(ms))
				serializer.Serialize(writer, obj);
			socket.Send(buffer);
		}

		public T Read<T>(int timeout = -1)
		{
			socket.ReceiveTimeout = timeout;
			var buffer = new byte[Size];
			socket.Receive(buffer);
			var ms = new MemoryStream(buffer);
			using (var reader = new BsonReader(ms))
				return serializer.Deserialize<T>(reader);
		}
	}
}
