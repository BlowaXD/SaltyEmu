using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Core.Logging;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace LoginServer.Cryptography.Decoder
{
    public class NostaleLoginDecoder : MessageToMessageDecoder<IByteBuffer>, IDecoder
    {
        private static readonly Logger Log = Logger.GetLogger<NostaleLoginDecoder>();

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            try
            {
                var decryptedPacket = new StringBuilder();

                foreach (byte character in ((Span<byte>)message.Array).Slice(message.ArrayOffset, message.ReadableBytes))
                {
                    decryptedPacket.Append(character > 14
                        ? Convert.ToChar(character - 15 ^ 195)
                        : Convert.ToChar(256 - (15 - character) ^ 195));
                }

                output.Add(decryptedPacket.ToString());
            }
            catch (Exception e)
            {
                Log.Error("[DECODE]", e);
            }
        }
    }
}