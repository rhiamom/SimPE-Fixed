using System.IO;

namespace CCB;

public class Common
{
	public static string Translate(string SearchValue, string FilePath, long ArrayPosToMatch, long ArrayPosToUse, char Seperator)
	{
		StreamReader streamReader = new StreamReader(FilePath, detectEncodingFromByteOrderMarks: true);
		string[] array = new string[GetLineCount(FilePath)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = streamReader.ReadLine();
			string[] array2 = array[i].Split(new char[1] { Seperator });
			string text;
			try
			{
				text = array2[ArrayPosToMatch];
			}
			catch
			{
				text = "ErrorKludgeMush";
			}
			if (SearchValue == text)
			{
				streamReader.Close();
				try
				{
					return array2[ArrayPosToUse];
				}
				catch
				{
					return "Error In Pulling Data From Array";
				}
			}
		}
		streamReader.Close();
		return "No Match Found";
	}

	public static long GetLineCount(string FilePath)
	{
		long num = 0L;
		StreamReader streamReader = File.OpenText(FilePath);
		while (streamReader.ReadLine() != null)
		{
			num++;
		}
		streamReader.Close();
		return num;
	}

	public static void AppendToFile(string StringToAppend, string FilePath)
	{
		StreamWriter streamWriter = new StreamWriter(FilePath, append: true);
		streamWriter.WriteLine(StringToAppend);
		streamWriter.Close();
	}
}
