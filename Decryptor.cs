using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Orbipacket;

namespace CanSatLogsDecryptor
{
    public class Decrpytor
    {

        private readonly PacketBuffer packetBuffer = new();

        public void Decryptor(string filepath, string outputFile)
        {
            byte[] rawbytes = File.ReadAllBytes(filepath);
            packetBuffer.Add(rawbytes);

            byte[]? extractedPacket;

            while ((extractedPacket = packetBuffer.ExtractFirstValidPacket()) != null)
            {
                Console.WriteLine("Extracted packet: " + BitConverter.ToString(extractedPacket));
                Packet? packet = Decode.GetPacketInformation(extractedPacket);

                string data;
                if (packet == null)
                {
                    Console.WriteLine("Invalid packet");
                    continue;
                }
                if (packet.Payload == null || packet.Payload.Value == null || packet.Payload.Value.Length == 0)
                {
                    Console.WriteLine("Empty payload");
                    continue;
                }
                switch (packet.DeviceId)
                {
                    case DeviceId.PressureSensor:
                        data = BitConverter.ToSingle(packet.Payload.Value, 0).ToString();
                        break;
                    case DeviceId.TemperatureSensor:
                        data = BitConverter.ToSingle(packet.Payload.Value, 0).ToString();
                        break;
                    case DeviceId.HumiditySensor:
                        data = BitConverter.ToSingle(packet.Payload.Value, 0).ToString();
                        break;
                    case DeviceId.System:
                        data = System.Text.Encoding.ASCII.GetString(packet.Payload.Value);
                        break;
                    case DeviceId.GPS:
                        data = $"{BitConverter.ToDouble(packet.Payload.Value, 0)}, {BitConverter.ToDouble(packet.Payload.Value, 8)}, {BitConverter.ToSingle(packet.Payload.Value, 16)}";
                        break;
                    default:
                        data = "Unknown device";
                        break;
                }
                Console.WriteLine($"Device: {packet.DeviceId}, Data: {data}");
                string outputLine = $"{packet.DeviceId}, @{packet.Timestamp}, {data}\n";
                File.AppendAllText(outputFile, outputLine);
            }
        }
    }
}
