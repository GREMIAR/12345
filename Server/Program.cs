using Server;

TcpServer server = new("127.0.0.1", 8888);

await server.Start();
