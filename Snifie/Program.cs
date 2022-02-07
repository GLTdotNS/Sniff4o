using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
Console.WriteLine($"Your local IP is {host.AddressList[1]}");

Console.WriteLine("Choose an option");
Console.WriteLine();
Console.WriteLine("To scan everyone in your own network , please press 1");
Console.WriteLine("To scan devices in range (given by you) , please press 2");
Console.WriteLine();
string hostThirdOctet = host.AddressList[1].ToString().Split(".").ElementAt(2);

while (true)
{

  
    try
    {
        int command = int.Parse(Console.ReadLine());
        if (command == 1)
        {
            Scanning($"192.168.{hostThirdOctet}.1", $"192.168.{hostThirdOctet}.255");
        }
        else
        {
            Console.WriteLine("Please enter the range");
            Console.WriteLine("Example: 192.168.0.1 - 192.168.0.35");

            string[] input = Console.ReadLine().Split(" - ");


            string startRange = input[0];
            string stopRange = input[1];

            Scanning(startRange, stopRange);
        }
    }
    catch (Exception e)
    {
        Console.Clear();
        Console.WriteLine(e.Message);
        continue;
    }
}








static void Scanning(string start, string end)
{
    string[] starting = start.Split(".");
    string[] ending = end.Split(".");
    int startIP = int.Parse(starting[3]);
    int endIP = int.Parse(ending[3]);
    Dictionary<IPAddress, string> hosts = new Dictionary<IPAddress, string>();
    Ping ping = new Ping();
    PingReply reply;
    IPAddress adress;
    IPHostEntry host;

    for (int i = startIP; i <= endIP; i++)
    {

        string ipAddress = start;
        if (ping != null)
        {
            reply = ping.Send(ipAddress, 5);

        }
        else
        {
            break;
        }


        Console.WriteLine($"{ipAddress}");

        if (reply.Status == IPStatus.Success)
        {
            adress = IPAddress.Parse(ipAddress);
            host = Dns.GetHostEntry(adress);
            if (!hosts.ContainsKey(adress))
            {
                hosts.Add(adress, host.HostName);
            }


        }
        else
        {
            hosts.Add(IPAddress.Parse(ipAddress), "n/a");
        }

        string oldIP = startIP.ToString();
        startIP++;
        int del = start.Length - oldIP.Length;
        start = start[0..del];
        start += startIP.ToString();
    }
    Console.Clear();
    foreach (var h in hosts)
    {
        Console.WriteLine($"{h.Key} - {h.Value}");
    }
}
