using System.Net.Sockets;
using System.Text;

namespace Server;

internal class ClientHandler
{
    readonly int number;

    public ClientHandler(int number)
    {
        this.number = number;
    }

    string From => "Клиент №" + number + ": ";

    const byte A = 0xA;
    const byte B = 0xB;

    static int CalculateSum(byte[] array)
    {
        int indexA = Array.IndexOf(array, A) + 1;
        int indexB = Array.LastIndexOf(array, B);

        int sum = array.Skip(indexA).Take(indexB - indexA).Select(i => (int)i).Sum();

        return sum;
    }

    static string Formatting(int sum)
    {
        return "0x" + sum.ToString("X");
    }

    public async Task ProcessClientAsync(TcpClient tcpClient)
    {
        try
        {
            var stream = tcpClient.GetStream();
            byte[] buffer = new byte[256];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    Console.WriteLine(From + "отключён");
                    break;
                }

                int sum = CalculateSum(buffer);
                string answer = Formatting(sum);
                Console.WriteLine(From + answer);

                await stream.WriteAsync(Encoding.UTF8.GetBytes(answer + "\n"));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            tcpClient.Close();
        }
    }
}
