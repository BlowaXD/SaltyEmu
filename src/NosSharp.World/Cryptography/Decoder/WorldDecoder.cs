using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using NosSharp.World.Session;

namespace NosSharp.World.Cryptography.Decoder
{
    public class WorldDecoder : MessageToMessageDecoder<IByteBuffer>, IDecoder
    {
        private static string DecryptPrivate(string str)
        {
            List<byte> receiveData = new List<byte>();
            char[] table = { ' ', '-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'n' };
            int count;
            for (count = 0; count < str.Length; count++)
            {
                if (str[count] <= 0x7A)
                {
                    int len = str[count];

                    for (int i = 0; i < len; i++)
                    {
                        count++;

                        try
                        {
                            receiveData.Add(unchecked((byte)(str[count] ^ 0xFF)));
                        }
                        catch
                        {
                            receiveData.Add(255);
                        }
                    }
                }
                else
                {
                    int len = str[count];
                    len &= 0x7F;

                    for (int i = 0; i < len; i++)
                    {
                        count++;
                        int highbyte;
                        try
                        {
                            highbyte = str[count];
                        }
                        catch
                        {
                            highbyte = 0;
                        }

                        highbyte &= 0xF0;
                        highbyte >>= 0x4;

                        int lowbyte;
                        try
                        {
                            lowbyte = str[count];
                        }
                        catch
                        {
                            lowbyte = 0;
                        }

                        lowbyte &= 0x0F;

                        if (highbyte != 0x0 && highbyte != 0xF)
                        {
                            receiveData.Add(unchecked((byte)table[highbyte - 1]));
                            i++;
                        }

                        if (lowbyte != 0x0 && lowbyte != 0xF)
                        {
                            receiveData.Add(unchecked((byte)table[lowbyte - 1]));
                        }
                    }
                }
            }

            return Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, receiveData.ToArray()));
        }

        public string DecryptCustomParameter(Span<byte> str)
        {
            try
            {
                var encryptedStringBuilder = new StringBuilder();
                for (int i = 1; i < str.Length; i++)
                {
                    if (Convert.ToChar(str[i]) == 0xE)
                    {
                        return encryptedStringBuilder.ToString();
                    }

                    int firstbyte = Convert.ToInt32(str[i] - 0xF);
                    int secondbyte = firstbyte;
                    secondbyte &= 0xF0;
                    firstbyte = Convert.ToInt32(firstbyte - secondbyte);
                    secondbyte >>= 0x4;

                    switch (secondbyte)
                    {
                        case 0:
                        case 1:
                            encryptedStringBuilder.Append(' ');
                            break;

                        case 2:
                            encryptedStringBuilder.Append('-');
                            break;

                        case 3:
                            encryptedStringBuilder.Append('.');
                            break;

                        default:
                            secondbyte += 0x2C;
                            encryptedStringBuilder.Append(Convert.ToChar(secondbyte));
                            break;
                    }

                    switch (firstbyte)
                    {
                        case 0:
                            encryptedStringBuilder.Append(' ');
                            break;

                        case 1:
                            encryptedStringBuilder.Append(' ');
                            break;

                        case 2:
                            encryptedStringBuilder.Append('-');
                            break;

                        case 3:
                            encryptedStringBuilder.Append('.');
                            break;

                        default:
                            firstbyte += 0x2C;
                            encryptedStringBuilder.Append(Convert.ToChar(firstbyte));
                            break;
                    }
                }

                return encryptedStringBuilder.ToString();
            }
            catch (OverflowException)
            {
                return string.Empty;
            }
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            Span<byte> str = message.Array;
            str = str.Slice(message.ArrayOffset, message.ReadableBytes);

            if (!SessionManager.Instance.GetSession(context.Channel.Id.AsLongText(), out int sessionId))
            {
                output.Add(DecryptCustomParameter(str));
                return;
            }

            var encryptedString = new StringBuilder();

            int sessionKey = sessionId & 0xFF;
            byte sessionNumber = unchecked((byte)(sessionId >> 6));
            sessionNumber &= 0xFF;
            sessionNumber &= 3;

            switch (sessionNumber)
            {
                case 0:
                    foreach (byte character in str)
                    {
                        byte firstbyte = unchecked((byte)(sessionKey + 0x40));
                        byte b = unchecked((byte)(character - firstbyte));
                        encryptedString.Append((char)b);
                    }

                    break;

                case 1:
                    foreach (byte character in str)
                    {
                        byte firstbyte = unchecked((byte)(sessionKey + 0x40));
                        byte b = unchecked((byte)(character + firstbyte));
                        encryptedString.Append((char)b);
                    }

                    break;

                case 2:
                    foreach (byte character in str)
                    {
                        byte firstbyte = unchecked((byte)(sessionKey + 0x40));
                        byte b = unchecked((byte)(character - firstbyte ^ 0xC3));
                        encryptedString.Append((char)b);
                    }

                    break;

                case 3:
                    foreach (byte character in str)
                    {
                        byte firstbyte = unchecked((byte)(sessionKey + 0x40));
                        byte b = unchecked((byte)(character + firstbyte ^ 0xC3));
                        encryptedString.Append((char)b);
                    }

                    break;

                default:
                    encryptedString.Append((char)0xF);
                    break;
            }

            Span<string> temp = encryptedString.ToString().Split((char)0xFF);
            var save = new StringBuilder();

            for (int i = 0; i < temp.Length; i++)
            {
                save.Append(DecryptPrivate(temp[i]));
                if (i < temp.Length - 2)
                {
                    save.Append((char)0xFF);
                }
            }

            output.Add(save.ToString());
        }
    }
}