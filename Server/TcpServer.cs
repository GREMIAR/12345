using System.Net;
using System.Net.Sockets;

namespace Server;

public class TcpServer
{
    readonly TcpListener tcpListener;

    public TcpServer(string ipAddress, int port)
    {
        IPAddress localAddress = IPAddress.Parse(ipAddress);
        tcpListener = new TcpListener(localAddress, port);
    }

    public async Task Start()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений... ");
            int i = 0;
            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                i++;
                Console.WriteLine("Клиент №" + i + ": подключен");

                _ = Task.Run(async () =>
                {
                    var clientHandler = new ClientHandler(i);
                    await clientHandler.ProcessClientAsync(tcpClient);
                });
            }
        }
        finally
        {
            tcpListener.Stop();
        }
    }
}
