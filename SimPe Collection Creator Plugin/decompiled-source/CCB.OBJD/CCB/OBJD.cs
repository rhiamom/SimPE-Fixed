using System;
using System.IO;
using System.Text;

namespace CCB;

public class OBJD
{
	public static string[] FetchCatalogCategories(string DatPath)
	{
		if (!ValidPrimaryOBJD(DatPath))
		{
			return new string[1] { "Error" };
		}
		FileStream fileStream = new FileStream(DatPath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] array = new byte[2];
		fileStream.Position = 144L;
		array = binaryReader.ReadBytes(2);
		Array.Reverse((Array)array);
		string text = Translate("BuyCategories", Math.ToDec(getBytesAsHexString(array, bSpaced: false)));
		fileStream.Position = 252L;
		array = binaryReader.ReadBytes(2);
		Array.Reverse((Array)array);
		string[] array2 = TranslateArray(text, Math.ToDec(getBytesAsHexString(array, bSpaced: false)));
		string[] array3 = new string[array2.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array3[i] = text + "/" + array2[i];
		}
		binaryReader.Close();
		fileStream.Close();
		return array3;
	}

	public static bool ValidOBJD(string DatPath)
	{
		FileStream fileStream = new FileStream(DatPath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte b = 0;
		fileStream.Position = 64L;
		b = binaryReader.ReadByte();
		if (b != 139)
		{
			binaryReader.Close();
			fileStream.Close();
			return false;
		}
		binaryReader.Close();
		fileStream.Close();
		return true;
	}

	public static bool ValidPrimaryOBJD(string DatPath)
	{
		FileStream fileStream = new FileStream(DatPath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] array = new byte[2];
		fileStream.Position = 84L;
		array = binaryReader.ReadBytes(2);
		if (getBytesAsHexString(array, bSpaced: false) == "0000")
		{
			binaryReader.Close();
			fileStream.Close();
			return true;
		}
		array = binaryReader.ReadBytes(2);
		if (getBytesAsHexString(array, bSpaced: false) == "FFFF")
		{
			binaryReader.Close();
			fileStream.Close();
			return true;
		}
		binaryReader.Close();
		fileStream.Close();
		return false;
	}

	public static string Translate(string Type, int Value)
	{
		StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\OBJD.EN.txt", detectEncodingFromByteOrderMarks: true);
		string[] array = new string[12];
		int num = (int)(System.Math.Log(Value) / System.Math.Log(2.0));
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = streamReader.ReadLine();
			string[] array2 = array[i].Split(new char[1] { '=' });
			if (Type == array2[0])
			{
				string[] array3 = array2[1].Split(new char[1] { ',' });
				if (num < 0)
				{
					return "No Buy Mode Category";
				}
				return array3[num];
			}
		}
		return "Error";
	}

	public static string FetchGUID(string DatPath)
	{
		if (!ValidPrimaryOBJD(DatPath))
		{
			return "Error";
		}
		FileStream fileStream = new FileStream(DatPath, FileMode.Open, FileAccess.ReadWrite);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] array = new byte[4];
		fileStream.Position = 92L;
		array = binaryReader.ReadBytes(4);
		Array.Reverse((Array)array);
		binaryReader.Close();
		fileStream.Close();
		return getBytesAsHexString(array, bSpaced: false);
	}

	public static string[] TranslateArray(string Type, int Value)
	{
		StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\OBJD.EN.txt", detectEncodingFromByteOrderMarks: true);
		string[] array = new string[12];
		string[] array2 = new string[500];
		int num = 0;
		int num2 = (int)(System.Math.Log(Value) / System.Math.Log(2.0));
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = streamReader.ReadLine();
			string[] array3 = array[i].Split(new char[1] { '=' });
			if (!(Type == array3[0]))
			{
				continue;
			}
			string[] array4 = array3[1].Split(new char[1] { ',' });
			for (int j = 1; j < array4.Length; j++)
			{
				if ((double)(Value & (int)System.Math.Pow(2.0, j - 1)) == System.Math.Pow(2.0, j - 1))
				{
					array2[num] = array4[j - 1];
					num++;
				}
			}
			string[] array5 = new string[num];
			for (int k = 0; k < num; k++)
			{
				if (array2[k] != null)
				{
					array5[k] = array2[k];
				}
			}
			return array5;
		}
		array2[0] = "No Sub Category";
		return array2;
	}

	public static string getBytesAsHexString(byte[] byBytes, bool bSpaced)
	{
		char[] array = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
		int num = byBytes.Length * 2;
		if (bSpaced)
		{
			num = byBytes.Length * 3;
		}
		char[] array2 = new char[num];
		for (int i = 0; i < byBytes.Length; i++)
		{
			int num2 = byBytes[i];
			if (bSpaced)
			{
				array2[i * 3] = array[num2 >> 4];
				array2[i * 3 + 1] = array[num2 & 0xF];
				array2[i * 3 + 2] = ' ';
			}
			else
			{
				array2[i * 2] = array[num2 >> 4];
				array2[i * 2 + 1] = array[num2 & 0xF];
			}
		}
		return new string(array2);
	}

	public static string getBytesAsCharString(byte[] byBytes)
	{
		int num = byBytes.Length;
		char[] array = new char[num];
		for (int i = 0; i < byBytes.Length; i++)
		{
			int num2 = byBytes[i];
			array[i] = (char)num2;
		}
		return new string(array);
	}

	public static byte[] MakeByteArray(string ToConvert, int FillSize, bool Reverse, bool FromChars)
	{
		int length = ToConvert.Length;
		byte[] array = ((FillSize > 0) ? new byte[FillSize] : ((!FromChars) ? new byte[length / 2] : new byte[length]));
		for (int i = 0; i < array.Length; i++)
		{
			if (FromChars)
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				array = aSCIIEncoding.GetBytes(ToConvert);
				Array.Reverse((Array)array);
			}
			else if (FillSize > 0)
			{
				if (ToConvert.Length == i * 2 + 2)
				{
					array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 2), 2));
				}
				else if (ToConvert.Length == i * 2 + 1)
				{
					array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 1), 1));
				}
				else if (ToConvert.Length < i * 2 + 2)
				{
					array[i] = 0;
				}
			}
			else
			{
				array[i] = (byte)Math.ToDec(ToConvert.Substring(ToConvert.Length - (i * 2 + 2), 2));
			}
		}
		if (Reverse)
		{
			return array;
		}
		Array.Reverse((Array)array);
		return array;
	}
}
