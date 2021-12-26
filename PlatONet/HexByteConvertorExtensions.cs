using System;
using System.Linq;

namespace PlatONet
{
    /// <summary>
    /// ������������16�����ַ���֮���ת��<br/>
    /// ������Ҫ��Դ�����£�http://blogs.msdn.com/b/heikkiri/archive/2012/07/17/hex-string-to-corresponding-byte-array.aspx
    /// </summary>
    public static class HexByteConvertorExtensions
    {        
        private static readonly byte[] Empty = new byte[0];
        /// <summary>
        /// ����������ת��Ϊ16�����ַ���
        /// </summary>
        /// <param name="value">��ת���Ķ���������</param>
        /// <param name="prefix">����ַ����Ƿ���"0x"ǰ׺</param>
        /// <returns>ת�����16�����ַ���</returns>
        public static string ToHex(this byte[] value, bool prefix = false)
        {
            var strPrex = prefix ? "0x" : "";
            return strPrex + string.Concat(value.Select(b => b.ToString("x2")).ToArray());
        }
        /// <summary>
        /// 16�����ַ����Ƿ���"0x"ǰ׺
        /// </summary>
        /// <param name="value">16�����ַ���</param>
        /// <returns>��"0x"ǰ׺����true�����򷵻�false</returns>
        public static bool HasHexPrefix(this string value)
        {
            return value.StartsWith("0x");
        }
        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ16�����ַ���
        /// </summary>
        /// <param name="value">���ж����ַ���</param>
        /// <returns>���򷵻�true�����򷵻�false</returns>
        public static bool IsHex(this string value)
        {
            bool isHex;
            foreach (var c in value.RemoveHexPrefix())
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// ȥ���ַ�����"0x"ǰ׺
        /// </summary>
        /// <param name="value">�������ַ���</param>
        /// <returns>ȥ��"0x"ǰ׺���ַ���</returns>
        public static string RemoveHexPrefix(this string value)
        {
            return value.Substring(value.StartsWith("0x") ? 2 : 0);
        }
        /// <summary>
        /// �ж������ַ����Ƿ�Ϊͬһ��16��������
        /// </summary>
        /// <param name="first">��һ���ַ���</param>
        /// <param name="second">�ڶ����ַ���</param>
        /// <returns>���򷵻�true�����򷵻�false</returns>
        public static bool IsTheSameHex(this string first, string second)
        {
            return string.Equals(EnsureHexPrefix(first).ToLower(), EnsureHexPrefix(second).ToLower(),
                StringComparison.Ordinal);
        }
        /// <summary>
        /// ȷ���ַ�������"0x"ǰ׺
        /// </summary>
        /// <param name="value">�������ַ���</param>
        /// <returns>����"0x"ǰ׺���ַ���</returns>
        public static string EnsureHexPrefix(this string value)
        {
            if (value == null) return null;
            if (!value.HasHexPrefix())
                return "0x" + value;
            return value;
        }
        /// <summary>
        /// ȷ���ַ�������"0x"ǰ׺
        /// </summary>
        /// <param name="values">�������ַ����б�</param>
        /// <returns>����"0x"ǰ׺���ַ����б�</returns>
        public static string[] EnsureHexPrefix(this string[] values)
        {
            if (values != null)
                foreach (var value in values)
                    value.EnsureHexPrefix();
            return values;
        }
        /// <summary>
        /// ����������ת��Ϊ16�����ַ�������ȥ��ͷ����0
        /// </summary>
        /// <param name="value">��ת���Ķ���������</param>
        /// <returns>16�����ַ���</returns>
        public static string ToHexCompact(this byte[] value)
        {
            return ToHex(value).TrimStart('0');
        }

        private static byte[] HexToByteArrayInternal(string value)
        {
            byte[] bytes = null;
            if (string.IsNullOrEmpty(value))
            {
                bytes = Empty;
            }
            else
            {
                var string_length = value.Length;
                var character_index = value.StartsWith("0x", StringComparison.Ordinal) ? 2 : 0;
                // Does the string define leading HEX indicator '0x'. Adjust starting index accordingly.               
                var number_of_characters = string_length - character_index;

                var add_leading_zero = false;
                if (0 != number_of_characters % 2)
                {
                    add_leading_zero = true;

                    number_of_characters += 1; // Leading '0' has been striped from the string presentation.
                }

                bytes = new byte[number_of_characters / 2]; // Initialize our byte array to hold the converted string.

                var write_index = 0;
                if (add_leading_zero)
                {
                    bytes[write_index++] = FromCharacterToByte(value[character_index], character_index);
                    character_index += 1;
                }

                for (var read_index = character_index; read_index < value.Length; read_index += 2)
                {
                    var upper = FromCharacterToByte(value[read_index], read_index, 4);
                    var lower = FromCharacterToByte(value[read_index + 1], read_index + 1);

                    bytes[write_index++] = (byte)(upper | lower);
                }
            }

            return bytes;
        }
        /// <summary>
        /// 16�����ַ���ת��Ϊ����������
        /// </summary>
        /// <param name="value">��ת����16�����ַ���</param>
        /// <returns>ת����Ķ���������</returns>
        /// <exception cref="FormatException">�ַ�����ʽ����</exception>
        public static byte[] HexToByteArray(this string value)
        {
            try
            {
                return HexToByteArrayInternal(value);
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format(
                    "String '{0}' could not be converted to byte array (not hex?).", value), ex);
            }
        }
        private static byte FromCharacterToByte(char character, int index, int shift = 0)
        {
            var value = (byte)character;
            if (0x40 < value && 0x47 > value || 0x60 < value && 0x67 > value)
            {
                if (0x40 == (0x40 & value))
                    if (0x20 == (0x20 & value))
                        value = (byte)((value + 0xA - 0x61) << shift);
                    else
                        value = (byte)((value + 0xA - 0x41) << shift);
            }
            else if (0x29 < value && 0x40 > value)
            {
                value = (byte)((value - 0x30) << shift);
            }
            else
            {
                throw new FormatException(string.Format(
                    "Character '{0}' at index '{1}' is not valid alphanumeric character.", character, index));
            }

            return value;
        }
    }
}
