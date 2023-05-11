using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

public interface IString
{
    byte GetLength();
    char this[int index] { get; set; }
}

class String :IString, IComparable, ICloneable
{
    protected char[] data;
    protected byte length;

    // without parameters
    public String()
    {
        data = new char[0];
        length = 0;
    }

    // string as parameter
    public String(string str)
    {
        data = str.ToCharArray();
        length = (byte)data.Length;
    }

    // char as parameter
    public String(char c)
    {
        data = new char[] { c };
        length = 1;
    }

    public byte GetLength()
    {
        return length;
    }
    public char this[int index]
    {
        get
        {
            if (index < 0 || index >= length)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            return data[index];
        }

        set
        {
            if (index < 0 || index >= length)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }

            data[index] = value;
        }
    }
    object ICloneable.Clone()
    {
        return this.MemberwiseClone();
    }

    int IComparable.CompareTo(object obj)
    {
        if (obj is String other)
        {
            return this.length.CompareTo(other.length);
        }
        else
        {
            throw new ArgumentException("Object is not a String");
        }
    }
    

    // clear string
    public void Clear()
    {
        data = new char[0];
        length = 0;
    }
}
class BitString : String, IComparable, ICloneable
{
    public BitString(string str)
    {
        bool isValid = true;

        foreach (char symbol in str)
        {
            if (symbol != '0' && symbol != '1')
            {
                isValid = false;
                break;
            }
        }

        if (isValid)
        {

            data = str.ToCharArray();
            length = (byte)data.Length;
        }
        else
        {
            data = new char[1];
            length = 0;
        }
    }

    // Implement ICloneable interface
    object ICloneable.Clone()
    {
        return this.MemberwiseClone();
    }

    // Implement IComparable interface
    int IComparable.CompareTo(object obj)
    {
        if (obj is BitString other)
        {
            return this.length.CompareTo(other.length);
        }
        else
        {
            throw new ArgumentException("Object is not a BitString");
        }
    }
    public void Invert()
    {
        // Determine the sign of the number
        bool isNegative = (data[0] == '1');

        // Invert all bits if the number is negative
        if (isNegative)
        {
            for (int i = 0; i < length; i++)
            {
                if (data[i] == '0')
                {
                    data[i] = '1';
                }
                else
                {
                    data[i] = '0';
                }
            }
            bool carry = true;
            for (int i = length - 1; i >= 0; i--)
            {
                if (data[i] == '0' && carry)
                {
                    data[i] = '1';
                    carry = false;
                }
                else if (data[i] == '1' && carry)
                {
                    data[i] = '0';
                }
            }
            data[0] = '1';
        }

    }

    public BitString Sum(BitString other)
    {
        string result = "";
        int carry = 0;

        int i = this.length - 1;
        int j = other.length - 1;

        while (i >= 0 || j >= 0 || carry > 0)
        {
            int sum = carry;
            if (i >= 0) sum += this.data[i--] - '0';
            if (j >= 0) sum += other.data[j--] - '0';
            result = (sum % 2) + result;
            carry = sum / 2;
        }

        return new BitString(result);
    }

    public bool IsEqual(BitString other)
    {
        if (length != other.length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            if (data[i] != other.data[i])
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        return new string(data);
    }
}

internal class Program
{
    static void Main(string[] args)
    {

        BitString[] array = new BitString[6];

        
        //for (int i = 0; i < array.Length / 2; i++)
        //{
        //    array[i] = new BitString("101010");
        //}
        array[0] = new BitString("101");
        array[1] = new BitString("1010");
        array[2] = new BitString("101010");


        //Clone first part to the second
        for (int i = array.Length / 2; i < array.Length; i++)
        {
            array[i] = ((ICloneable)array[i - array.Length / 2]).Clone() as BitString;
        }

        Console.WriteLine("Before sorting: ");
        foreach (var item in array)
        {
            Console.WriteLine(item.ToString());
        }


        Array.Sort(array);

        Console.WriteLine("After sorting: ");
        foreach (var item in array)
        {
            Console.WriteLine(item.ToString());
        }
    }
}