using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.IO;



public class UDPListener
{



    private const int listenPort = 80;

    public static Image byteArrayToImage(byte[] byteArrayIn)
    {
        ImageConverter converter = new ImageConverter();
        Image img =  (Image)converter.ConvertFrom(byteArrayIn);
        
        return img;
    }

    private static void StartListener()
    {
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(localAddr, listenPort);
        
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] data = listener.Receive(ref groupEP);

                Console.WriteLine($"Received broadcast from {groupEP} :");


                byte[] filename = new byte[18];

                Buffer.BlockCopy(data,0,filename,0,filename.Length);

                byte[] image = new byte[data.Length-18];

                Buffer.BlockCopy(data, 18, image, 0, image.Length);

                string path = Encoding.UTF8.GetString(filename);

                Image Image = byteArrayToImage(image);

                var i = new Bitmap(Image);

                i.Save("C:\\img\\" + path, System.Drawing.Imaging.ImageFormat.Png);  // Or Png

            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public static void Main()
    {
        StartListener();
    }
}