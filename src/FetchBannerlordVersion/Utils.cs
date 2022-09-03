using System;
using System.Globalization;
using System.Text;

namespace FetchBannerlordVersion
{
    public static class Utils
    {
        public static string UnescapeNonASCII(ReadOnlySpan<char> unescapedStr)
        {
            var sb = unescapedStr.Length <= 256 ? new ValueStringBuilder(stackalloc char[256]) : new ValueStringBuilder(unescapedStr.Length);
            var idx = -1;
            Span<char> hexb = stackalloc char[4];
            for (var i = 0; i < unescapedStr.Length; i++)
            {
                var chr = unescapedStr[i];
                if (idx == -1)
                {
                    if (chr == '\\')
                    {
                        idx = i;
                        continue;
                    }
                    sb.Append(chr);
                }
                else
                {
                    switch (chr)
                    {
                        case 'u':
                        case 'U':
                            if (unescapedStr.Length - i < 4)
                            {
                                sb.Append(unescapedStr.Slice(idx, i - idx + 1));
                                idx = -1;
                                continue;
                                //throw new Exception($"Not enough length for the unicode escape symbol");
                            }
                            unescapedStr.Slice(i + 1, 4).CopyTo(hexb);
                            chr = (char) ushort.Parse(hexb, NumberStyles.HexNumber);
                            sb.Append(chr);
                            i += 4;
                            break;
                        default:
                            sb.Append(unescapedStr.Slice(idx, i - idx + 1));
                            idx = -1;
                            continue;
                            //throw new Exception($"Invalid backslash escape (\\ + charcode {(int) chr})");
                    }
                    idx = -1;
                }
            }
            return sb.ToString();
        }

        public static string EscapeNonASCII(string value)
        {
            var sb = value.Length <= 256 ? new ValueStringBuilder(stackalloc char[256]) : new ValueStringBuilder(value.Length);
            foreach (var c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    sb.Append($"\\u{((int) c):x4}");
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}