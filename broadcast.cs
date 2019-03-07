/*
 * DEVICE DISCOVERY using UDP
 * This code broadcasts data on network.
 * In our case, this has to run on the raspberry pi.
 */

using System;
using System.Net;
using System.Net.Sockets;
					
public class Program
{
	static UdpClient udp;
	static System.Timers.Timer broadcastTimer;
	
	const string MULTICAST_ADDR = "224.0.0.116";
	const int MULTICAST_PORT = 3333;
	static IPAddress multicastAddress = IPAddress.Parse(MULTICAST_ADDR);
	static IPEndPoint multicastGroup = new IPEndPoint(multicastAddress, MULTICAST_PORT);
	
	public static void Main()
	{
		udp = new UdpClient();
		udp.JoinMulticastGroup(multicastAddress);
		
		broadcastTimer = new System.Timers.Timer();
        broadcastTimer.Elapsed += (s, e) => { Broadcast(); };
        broadcastTimer.Interval = 1000;
	}
	
	static void Broadcast() {
		byte[] buffer = System.Text.Encoding.ASCII.GetBytes("data to send, can be json string or whatever.");
		udp.Send(buffer, buffer.Length, multicastGroup);
	}
}