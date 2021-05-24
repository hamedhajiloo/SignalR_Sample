using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = await ConnectToHubAsync();
            while (true)
            {
                Console.Write("Insert message: ");
                var text = Console.ReadLine();
                await connection.InvokeAsync("SendMessage", text);

            }
        }

        static async Task<HubConnection> ConnectToHubAsync()
        {
            bool started = false;
            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://test-hamedhajiloo.fandogh.cloud/myhub")
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await StartAsync(connection, started);
            };

            await StartAsync(connection, started);
            return connection;
            static async Task StartAsync(HubConnection connection, bool started)
            {
                if (started)
                    return;

                while (!started)
                {
                    try
                    {
                        await connection.StartAsync();
                        started = true;

                    }
                    catch
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1));
                        await StartAsync(connection, started);
                    }
                }
            }
        }
    }
}
