using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Utils;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace LoginServer.Cryptography.Encoder
{
    public class NostaleLoginEncoder : MessageToMessageEncoder<string>, IEncoder
    {
        private static readonly Logger Log = Logger.GetLogger<NostaleLoginEncoder>();

        protected override void Encode(IChannelHandlerContext context, string message, List<object> output)
        {
            try
            {
                byte[] tmp = Encoding.Default.GetBytes($"{message} ");
                if (tmp.Length == 0)
                {
                    return;
                }

                for (int i = 0; i < message.Length; i++)
                {
                    tmp[i] = Convert.ToByte(tmp[i] + 15);
                }

                tmp[tmp.Length - 1] = 25;

                output.Add(Unpooled.WrappedBuffer(tmp));
            }
            catch (Exception e)
            {
                Log.Error("[ENCODE]", e);
            }
        }
    }
}