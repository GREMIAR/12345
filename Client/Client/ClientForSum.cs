using Client.Views;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Client;

public static class ClientForSum
{
    const string ipAddress = "127.0.0.1";

    const int port = 8888;

    public async static Task Connecting()
    {
        try
        {
            using TcpClient tcpClient = new();
            await tcpClient.ConnectAsync(ipAddress, port);
            var stream = tcpClient.GetStream();

            byte[] buffer = new byte[256];
            int bytesRead;

            while (true)
            {
                await stream.WriteAsync(MainWindow.ViewModelInstance.PackageSent.FullPackage.AsMemory(0, MainWindow.ViewModelInstance.PackageSent.FullPackage.Length));

                bytesRead = await stream.ReadAsync(buffer);

                string responseData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Application.Current.Dispatcher.Invoke(() => MainWindow.ViewModelInstance.Sum = responseData);

                Array.Clear(buffer, 0, buffer.Length);
                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}
