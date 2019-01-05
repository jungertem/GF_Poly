using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polinomial_Basis_Galua_Field
{
    public static class Program
    {
        //VARIANT #1 
        //m = 173;

        public static byte[] Bin_String_To_Byte_Array(string word)
        {

            byte[] two_base = new byte[word.Length];
            for (int i = 0; i < word.Length; i++)
                two_base[i] = Convert.ToByte(word.Substring(i, 1), 2);
            Array.Reverse(two_base);
            return two_base;
        }

        public static string Byte_To_Binary_String(byte[] number)
        {
            string start = "";
            for (int i = 0; i < number.Length; i++)
                start = start + Convert.ToString(number[i]);
            return start;
        }


        public static string Binary_String_To_Hex_String(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }


        public static byte[] Hex_to_Byte(string hexvalue)
        {
            var a = String.Join(String.Empty, hexvalue.Select(c => Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            var b = "";
            for (int i = 3; i < a.Length; i++)
                b += a[i];
            return Bin_String_To_Byte_Array(b);
        }


        public static string SetHexKeyBoard()
        {
            return Console.ReadLine();
        }
        public static string SetHex(string hex)
        {
            return hex;
        }

        public static byte[] Set_Binary_Line_KeyBoard()
        {
            Console.Write("\nEnter string: ");
            string a = Console.ReadLine();
            var number = Program.Bin_String_To_Byte_Array(a);
            return number;
        }
        public static byte[] Set_Binary_Line(string word)
        {
            var number = Program.Bin_String_To_Byte_Array(word);
            return number;
        }


        public static void ShowNumber(byte[] number)
        {
            Console.Write("\nNumber = ");
            for (int i = 0; i < number.Length; i++)
                Console.Write("{0}", number[i]);
        }
        public static void ShowNumber(string number)
        {
            Console.Write("\nNumber = {0}", number);
        }


        public static string Generator(int dimention, int deg0, int deg1, int deg2, int deg3, int deg4)
        {
            string start = "";

            for (int w = 0; w <= dimention; w++)
                start += "0";

            StringBuilder strBuilder = new StringBuilder(start);

            strBuilder[dimention - deg0] = '1';
            strBuilder[dimention - deg1] = '1';
            strBuilder[dimention - deg2] = '1';
            strBuilder[dimention - deg3] = '1';
            strBuilder[dimention - deg4] = '1';
            string str = strBuilder.ToString();

            return str;
        }


        public static byte[] CutHighZerous(byte[] massive, int dimention)
        {
            int count = 0;
            var i = dimention - 1;

            while (massive[i] == 0)
            {
                count++;
                i--;
                if (i == -1) return massive;
            }

            byte[] result = new byte[massive.Length - count];

            for (int w = 0; w < massive.Length - count; w++)
                result[w] = massive[w];

            return result;

        }
        public static byte[] ShiftBits(byte[] number, int position)
        {
            byte[] ShiftedNumber = new byte[number.Length + position];
            for (int i = 0; i < number.Length; i++)
            {
                ShiftedNumber[i + position] = number[i];
            }
            return ShiftedNumber;
        }
        public static byte[] Addition(byte[] a, byte[] b)
        {
            int greatest_length = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, greatest_length);
            Array.Resize(ref b, greatest_length);

            byte[] result = new byte[greatest_length];
            for (int i = 0; i < greatest_length; i++)
            {
                result[i] = Convert.ToByte(a[i] ^ b[i]);
            }

            return CutHighZerous(result, greatest_length);
        }
        public static string Addition_to_string(byte[] a, byte[] b)
        {
            var temp = Program.Addition(a, b);
            Array.Reverse(temp);
            var str = Program.Byte_To_Binary_String(temp);

            return str;
        }


        public static byte[] Module_FX(byte[] a)
        {
            var Gen = Generator(173, 173, 10, 2, 1, 0);
            byte[] Gen_into_byte = Bin_String_To_Byte_Array(Gen);
            byte[] result = a;

            var MaxLength = Math.Max(a.Length, Gen_into_byte.Length);

            if (a.Length < Gen_into_byte.Length)
            {
                return a;
            }
            else
            {
                byte[] temp = new byte[1];

                while (result.Length >= Gen_into_byte.Length)
                {
                    temp = ShiftBits(Gen_into_byte, result.Length - Gen_into_byte.Length);
                    result = Addition(result, temp);
                }
            }

            return LengthCorrecting(result);
        }

        public static byte[] LengthCorrecting(byte[] a)
        {
            if (a.Length < 172)
            {
                Array.Resize(ref a, 173);
            }
            return a;
        }

        public static byte[] Mulltiplication(byte[] a, byte[] b)
        {
            var Max_length = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, Max_length);
            Array.Resize(ref b, Max_length);

            byte[] temp = new byte[1];
            byte[] result = new byte[1];

            for (int w = 0; w < a.Length; w++)
            {
                if (b[w] == 1)
                {
                    temp = ShiftBits(a, w);
                    result = Addition(result, temp);
                }
            }
            result = Module_FX(result);

            Array.Reverse(result);
            return result;
        }

        public static byte[] Square(byte[] a)
        {
            byte[] A = new byte[(a.Length << 1)];
            byte[] result = new byte[1];

            for (int w = 0; w < a.Length; w++)
                A[w << 1] = a[w];
            result = Module_FX(A);
            Array.Reverse(result);

            return result;
        }

        public static byte[] Trace(byte[] a)
        {
            byte[] result = a;
            byte[] temp = a;

            for(int w = 1; w < 173; w++)
            {
                temp = Square(temp);
                Array.Reverse(temp);
                result = Addition(result, temp);
            }

            result = Module_FX(result);
            Array.Reverse(result);
            return result;
        }

        public static string Trace_into_String(byte[] a)
        {
            var answer_temp = Trace(a);
            string result = Binary_String_To_Hex_String(Byte_To_Binary_String(answer_temp));
            result = result.TrimStart('0');

            return string.IsNullOrEmpty(result) ? "0" : result;
        }


        public static string Get_One(int dimention)
        {
            var temp = "";
            for (int w = 0; w < dimention-1; w++)
                temp += 0;
            temp = temp + '1';

            return temp;
        }

        public static string Get_Ones(int dimention)
        {
            var temp = "";
            for (int w = 0; w < dimention - 1; w++)
                temp += '1';
            temp = temp + '0';

            return temp;
        }


        public static byte[] Inversed_Element(byte[]a)
        {
            var power = Get_Ones(173);
            var power1 = Bin_String_To_Byte_Array(power);
            var res = Power(a, power1);

            return res;
        }


        public static byte[] Power(byte[] a, byte[] n)
        {
            var One = Get_One(173);
            byte[] result = new byte[a.Length];
            result = Bin_String_To_Byte_Array(One);
            
            for(int w = 0; w < a.Length; w++)
            {
                if(n[w] == 1)
                {
                    result = Mulltiplication(result, a);
                    Array.Reverse(result);
                }
                a = Square(a);
                Array.Reverse(a);
            }

            Array.Reverse(result);
            return result;
        }

        static void Main(string[] args)
        {

            //var a = Generator(173, 173, 10, 2, 1, 0);
            //ShowNumber(a);
            //Console.Write("\nLength = {0}", a.Length);


            //var a1 = Set_Binary_Line("01100110111101111100000101100011100101011111100010010000101001111010011100111101011000110010100100001110110001010010101011011010100010010011000110011001001001100111010100111");
            //var b1 = Set_Binary_Line("10100110011001110001110111110110110000010110111110111110110001011000000000011111011010010010001010010100001111010100100101110101011111010011010011011110100000100011101100100");

            //var hex1 = "166059213BAA966CC85D318BD5FC541612515C012BA7";
            ////var hex2 = "11340DF148A86CB702C747B32395F62295106AD94A18";

            //var hex11 = Hex_to_Byte(hex1);
            ////var hex22 = Hex_to_Byte(hex2);

            var hex1 = Hex_to_Byte("12B0870C6ED04DC97F5175A45636D081315839BD7C3D");
            //var hex2 = Hex_to_Byte("1CE16283C7B41EB0206704FAFA6FA3EEF7F9CA93A210");

            var result = Inversed_Element(hex1);
            ShowNumber(Binary_String_To_Hex_String(Byte_To_Binary_String(result)));
            ////Array.Reverse(result);

            //ShowNumber(Trace_into_String(hex11));

            ////var answer = Addition_to_string(hex11, hex22);
            //ShowNumber(Binary_String_To_Hex_String(answer));
            ////var result_temp = Mulltiplication(Hex_to_Byte(hex1), Hex_to_Byte(hex2));
            //var result = Byte_To_Binary(Addition(a1,b1));

            //ShowNumber(BinaryStringToHexString(result));

            //var bin1 = "00011011000100110011001101111000001010010000100010010101100001010010101011101010001100110111011011011100100001110101100100001100101000110011011010111110011001011111101110111";
            //var bin2 = "01101100111001110011111101110010010011110101010001011001100000100111100111011100100001101000001001101010111100110110001001111010101001000101011101100001010000000001100110011";
            //var bin11 = Bin_String_To_Byte_Array(bin1);
            //var bin22 = Bin_String_To_Byte_Array(bin2);

            //var answer = Addition_to_string(bin11, bin22);
            //var result = Mulltiplication(bin11, bin22);

            //ShowNumber(Binary_String_To_Hex_String(answer));
            ////ShowNumber(Binary_String_To_Hex_String(Byte_To_Binary_String(answer)));

            ////ShowNumber(Byte_To_Binary_String(result));
            //ShowNumber(Binary_String_To_Hex_String(Byte_To_Binary_String(result)));

            //var str = Generator(173, 173, 10, 2, 1, 0);
            //Console.Write("jj = {0}", str.Length);



            Console.Write("\nPut any key to continue..");
            Console.ReadKey();
        }


    }
}


