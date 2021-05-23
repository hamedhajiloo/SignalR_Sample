using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var text = Console.ReadLine();
            await ConnectToHubAsync(text);
        }

        static async Task ConnectToHubAsync(string text)
        {
            bool started = false;
            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:5001/myhub")
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await StartAsync(connection, started, text);
            };

            await StartAsync(connection, started, text);

            static async Task StartAsync(HubConnection connection, bool started, string text)
            {
                if (started)
                    return;

                while (!started)
                {
                    try
                    {
                        await connection.StartAsync();
                        started = true;
                        await connection.InvokeAsync("SendMessage", text);

                    }
                    catch
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1));
                        await StartAsync(connection, started, text);
                    }
                }
            }
        }
    }
}
