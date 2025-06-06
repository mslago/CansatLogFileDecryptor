﻿using System;
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
                Packet? packet = Decode.GetPacketInformation(extractedPacket);
                if (packet == null)
                {
                    Console.WriteLine("Invalid packet");
                    continue;
                }
                if (
                    packet.Payload == null
                    || packet.Payload.Value == null
                    || packet.Payload.Value.Length == 0
                )
                {
                    Console.WriteLine("Empty payload");
                    continue;
                }

                string data = packet.DeviceId switch
                {
                    DeviceId.PressureSensor => BitConverter
                        .ToSingle(packet.Payload.Value, 0)
                        .ToString(),
                    DeviceId.TemperatureSensor => BitConverter
                        .ToSingle(packet.Payload.Value, 0)
                        .ToString(),
                    DeviceId.HumiditySensor => BitConverter
                        .ToSingle(packet.Payload.Value, 0)
                        .ToString(),
                    DeviceId.System => System.Text.Encoding.ASCII.GetString(packet.Payload.Value),
                    DeviceId.GPS =>
                        $"{BitConverter.ToDouble(packet.Payload.Value, 0)}, {BitConverter.ToDouble(packet.Payload.Value, 8)}, {BitConverter.ToSingle(packet.Payload.Value, 16)}",
                    _ => "Unknown device",
                };
                Console.WriteLine($"Device: {packet.DeviceId}, Data: {data}");
                string outputLine = $"{packet.DeviceId}, @{packet.Timestamp}, {data}\n";
                File.AppendAllText(outputFile, outputLine);
            }
        }
    }
}
