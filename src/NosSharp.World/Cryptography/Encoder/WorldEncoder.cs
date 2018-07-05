using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace WingsEmu.World.Cryptography.Encoder
{
    public class WorldEncoder : MessageToMessageEncoder<string>, IEncoder
    {
        private static readonly Encoding encoding = Encoding.GetEncoding(1252);

        protected override void Encode(IChannelHandlerContext context, string message, List<object> output)
        {
            Span<byte> strBytes = Encoding.Convert(Encoding.UTF8, encoding, Encoding.UTF8.GetBytes(message));
            int bytesLength = strBytes.Length;
            Span<byte> encryptedData = new byte[bytesLength + (int)Math.Ceiling((decimal)bytesLength / 0x7E) + 1];

            int j = 0;
            for (int i = 0; i < bytesLength; i++)
            {
                if ((i % 126) == 0)
                {
                    encryptedData[i + j] = (byte)(bytesLength - i > 126 ? 126 : bytesLength - i);
                    j++;
                }

                encryptedData[i + j] = (byte)~strBytes[i];
            }

            encryptedData[encryptedData.Length - 1] = 0xFF;

            output.Add(Unpooled.WrappedBuffer(encryptedData.ToArray()));
        }
    }
}