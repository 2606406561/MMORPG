using System;

public class CRC32
{
    // 单例实例
    private static CRC32 _instance;
    public static CRC32 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CRC32();
            }
            return _instance;
        }
    }

    // CRC32表（256项）
    private int[] _crc32Table = new int[256];

    // 私有构造函数（初始化CRC表）
    private CRC32()
    {
        InitCRC32Table();
    }

    /// <summary>
    /// 位反转（与C++ Reflect函数完全一致）
    /// </summary>
    /// <param name="value">要反转的数值</param>
    /// <param name="bits">要反转的位数</param>
    /// <returns>反转后的数值</returns>
    private int Reflect(int value, int bits)
    {
        int result = 0;
        // 逐位反转
        for (int i = 1; i <= bits; i++)
        {
            if ((value & 1) != 0)
            {
                // 若当前最低位为1，在目标位置置1
                result |= 1 << (bits - i);
            }
            // 右移一位（逻辑右移，忽略符号位）
            value >>= 1;
        }
        return result;
    }

    /// <summary>
    /// 初始化CRC32表（与C++ Init_CRC32_Table完全一致）
    /// </summary>
    private void InitCRC32Table()
    {
        int polynomial = 0x04c11db7; // 多项式（与C++一致）

        for (int i = 0; i <= 0xFF; i++)
        {
            // 步骤1：将i进行8位反转后左移24位
            _crc32Table[i] = Reflect(i, 8) << 24;

            // 步骤2：循环8次处理每位
            for (int j = 0; j < 8; j++)
            {
                // 左移1位，若溢出则异或多项式
                if ((_crc32Table[i] & (1 << 31)) != 0)
                {
                    // 最高位为1，左移后异或多项式
                    _crc32Table[i] = (_crc32Table[i] << 1) ^ polynomial;
                }
                else
                {
                    // 最高位为0，直接左移
                    _crc32Table[i] <<= 1;
                }
            }

            // 步骤3：将结果进行32位反转
            _crc32Table[i] = Reflect(_crc32Table[i], 32);
        }
    }

    /// <summary>
    /// 计算CRC32值（与C++ Get_CRC完全一致）
    /// </summary>
    /// <param name="buffer">待计算的字节数组</param>
    /// <param name="length">有效长度</param>
    /// <returns>CRC32结果（int类型，二进制与C++ unsigned int一致）</returns>
    public int GetCrc(byte[] buffer, int length)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        if (length < 0 || length > buffer.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        int crc = unchecked((int)0xffffffff); // 初始值（与C++一致）

        for (int i = 0; i < length; i++)
        {
            // 关键：逻辑右移8位（用& 0x00FFFFFF清除符号位影响，模拟C++无符号右移）
            int rightShifted = (crc >> 8) & 0x00FFFFFF;
            // 取低8位与当前字节异或，作为表索引
            int tableIndex = (crc & 0xFF) ^ buffer[i];
            // 计算新的CRC值
            crc = rightShifted ^ _crc32Table[tableIndex];
        }

        // 最终异或0xffffffff（与C++一致）
        return crc ^ unchecked((int)0xffffffff);
    }
}