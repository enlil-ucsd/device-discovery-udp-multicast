/*
 * DEVICE DISCOVERY using UDP
 * This is the code that finds the device ip and port.
 * In our case, this has to run on the client to find the raspberry pi ip and port.
 */

using System;
using System.Net;
using System.Net.Sockets;
					
public class Program
{
	static UdpClient udp;
	
	// Multicast is a way to send data to a group of computers on a network rather than just one computer.
	// in our case the group is the raspberry pi and the client.
	// Each of these groups of computers has an ip address and port.
	const string MULTICAST_ADDR = "224.0.0.116"; // we pick whatever ip we want (it has to follow certain rules tho)
	const int MULTICAST_PORT = 3333;
	
	public static void Main()
	{
		udp = new UdpClient();
		udp.Client.Bind(new IPEndPoint(IPAddress.Any, MULTICAST_PORT)); // Listen on multicast port and data coming from any ip address.
		IPAddress multicastAddress = IPAddress.Parse(MULTICAST_ADDR);
		udp.JoinMulticastGroup(multicastAddress); // Join our multicast group -- only receive data from those part of multicast group.
		udp.BeginReceive(new AsyncCallback(ReceiveCallback), null);
	}
	
	static void ReceiveCallback(IAsyncResult asyncResult) {
		// called when we receive data from a device
		
		IPEndPoint ip = new IPEndPoint(IPAddress.Any, MULTICAST_PORT);
		byte[] receiveBytes = udp.EndReceive(asyncResult, ref ip); // receive the data as a byte array and updates the ip variable to the device's ip addr
		
		string deviceIP = ip.Address.ToString(); // This is the device's ip
		
		udp.BeginReceive(new AsyncCallback(ReceiveCallback), null); // If we want to start listening for data again.
	}
}