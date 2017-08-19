using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DogShow.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            tcpConnectonCreator();
            Console.ReadKey();
        }
        static async void tcpConnectonCreator()
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Parse("35.198.154.185"), 9858);
            TcpClient clientSocket = default(TcpClient);
            Console.WriteLine("Server Started");
            serverSocket.Start();
            try
            {
                while (true)
                {
                    clientSocket = await serverSocket.AcceptTcpClientAsync();

                    Console.WriteLine("\n" + @"\*** Accept connection ***/");
                    byte[] bytesFrom = new byte[4096];
                    NetworkStream networkStream = clientSocket.GetStream();
                    await networkStream.ReadAsync(bytesFrom, 0, bytesFrom.Length);
                    var dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    Console.WriteLine(@"\*** Get data from connection ***/");
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.LastIndexOf("$", StringComparison.Ordinal));
                    try
                    {
                        var addUser = new Incapsulation.ConnectionUser
                        {
                            UserName = dataFromClient.Split('$')[1].Split('=')[1],
                            UserTcpClient = clientSocket
                        };
                        if (!Incapsulation.connectedUsers.Exists(i => i.UserName == addUser.UserName))
                        {
                            Incapsulation.connectedUsers.Add(addUser);
                            new ConnectHandle().startClient(clientSocket, Incapsulation.connectedUsers);
                            Console.WriteLine($"User {dataFromClient.Split('$')[0].Split('=')[1]} [{clientSocket.Client.RemoteEndPoint}] connected");

                            Console.WriteLine(Incapsulation.connectedUsers.Count);
                        }
                        else
                        {
                            var text = "User already login";
                            Console.WriteLine(text);
                            ConnectHandle.sendSystemMessage($"type=systemcommand$text={text}$", clientSocket);
                            clientSocket.Client.Dispose();
                            clientSocket = null;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                clientSocket?.Dispose();
                serverSocket.Stop();
                Console.WriteLine("exit");
                Console.ReadLine();
            }
        }
    }
    public static class Incapsulation
    {
        public static List<ConnectionUser> connectedUsers { get; set; } = new List<ConnectionUser>();

        public class ConnectionUser
        {
            public string UserName { get; set; }
            public TcpClient UserTcpClient { get; set; }
        }
    }
    public class ConnectHandle
    {
        private TcpClient _clientSocket;
        public List<Task> TaskList { get; } = new List<Task>();
        public List<Incapsulation.ConnectionUser> ClientsList { get; private set; }

        public void startClient(TcpClient inClientSocket, List<Incapsulation.ConnectionUser> cList)
        {
            this._clientSocket = inClientSocket;
            this.ClientsList = cList;
            TaskList.Add(new Task(doDataExchange));
            TaskList[TaskList.Count - 1].Start();
        }

        private async void doDataExchange()
        {
            while (true)
            {
                try
                {
                    NetworkStream networkStream = _clientSocket.GetStream();
                    var bytesFrom = new byte[4096];
                    await networkStream.ReadAsync(bytesFrom, 0, bytesFrom.Length);
                    var dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.LastIndexOf("$", StringComparison.Ordinal));
                    dataLineParser(dataFromClient);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        public static async void sendSystemMessage(string msg, TcpClient client)
        {
            NetworkStream broadcastStream = client.GetStream();
            var broadcastBytes = Encoding.UTF8.GetBytes(msg);
            broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            await broadcastStream.FlushAsync();
        }

        private async void dataLineParser(string data)
        {
            string command = String.Empty,
                userLogin = String.Empty;
            var dataFromClientArray = data?.Split('$');
            if (dataFromClientArray == null) return;
            command = dataFromClientArray[0]?.Split('=')[1];
            userLogin = dataFromClientArray[1]?.Split('=')[1];
            if (command != "disconnect") return;
            var removeIndex = Incapsulation.connectedUsers.FindIndex(i => i.UserName == userLogin);
            Incapsulation.connectedUsers.RemoveAt(removeIndex);
            await TaskList[removeIndex];
            Console.WriteLine($"\nUser {userLogin} disconnected!\n");
        }
    }
}
