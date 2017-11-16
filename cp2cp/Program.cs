
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace cp2cp
{
	class Program
	{
		static string rowLine = new string('-', 77);
		const string tplEncLine = "|{0,5}|{1,24}|{2,44}|";
		const string tplU1 = " {0}: {1} <{2}> [<{3}>]";
		const string tplU2 = " // {0}, {1}.";
		const string PAC = "Press any key to continue . . . ";
		const string msgSIn = "Программа читает стандартный ввод";
		const string msgSOut = "и пишет в стандартный вывод";
		const string msgUsage = "Использование";
		const string msgSrcEnc = "Исходная кодировка";
		const string msgDstEnc = "Целевая кодировка";
		const string msgCode = "Код";
		const string msgName = "Имя";
		const string msgDName = "Отображаемое имя";
		const string msgInvSrcEnc = "Неверная входящая кодировка {0}!";
		const string msgInvDstEnc = "Неверная исходящая кодировка {0}!";
		const string msgEncFmt1 = " // Кодировку можно задать ";
		const string msgEncFmt2 = "в виде кода или имени.";
		const string msgEncInfo1 = " // Если стандартный вывод не перенаправлен -";
		const string msgEncInfo2 = " // указывайе только исходную кодировку.";

		static void Usage()
		{
			string exeName = Path.GetFileName(
				                 Assembly.GetAssembly(typeof(Program)).CodeBase
			                 );
			Console.WriteLine(tplU1, msgUsage, exeName, msgSrcEnc, msgDstEnc);
			Console.WriteLine(tplU2, msgSIn, msgSOut);
			Console.WriteLine(msgEncFmt1 + msgEncFmt2);
			Console.WriteLine(msgEncInfo1);
			Console.WriteLine(msgEncInfo2);
			Console.WriteLine(rowLine);
			Console.WriteLine(tplEncLine, msgCode, msgName, msgDName);
			Console.WriteLine(rowLine);
			Array.ForEach(
				Encoding.GetEncodings(),
				enc => Console.WriteLine(
					tplEncLine,
					enc.CodePage,
					enc.Name,
					enc.DisplayName));
			Console.WriteLine(rowLine);
			Console.WriteLine();
		}
		
		static void WaitForKey()
		{
			Console.Write(PAC);
			Console.ReadKey(true);
		}
		
		static Encoding GuessEncoding(string encarg)
		{
			Encoding result;
			int cpNum;
			try {
				result = int.TryParse(encarg, out cpNum)
					? Encoding.GetEncoding(cpNum)
					: Encoding.GetEncoding(encarg);
			} catch {
				result = null;
			}
			return result;
		}

		public static int Main(string[] args)
		{
			int result = 0;
			bool outRedir = Console.IsOutputRedirected;
			Encoding srcEnc, dstEnc;
			if (args.Length < 1 + (outRedir ? 1 : 0)) {
				Usage();
				WaitForKey();
				return 0;
			}
			srcEnc = GuessEncoding(args[0]);
			if (srcEnc == null) {
				Console.WriteLine(msgInvSrcEnc, args[0]);
				result |= 1;
			}
			
			if (outRedir) {
				dstEnc = GuessEncoding(args[1]);
				if (dstEnc == null) {
					Console.WriteLine(msgInvDstEnc, args[1]);
					result |= 2;
				} else {
					Console.OutputEncoding = dstEnc;
				}
			}
			if (result != 0)
				return result;
			
			var stdin = new StreamReader(Console.OpenStandardInput(), srcEnc);
			while (!stdin.EndOfStream)
				Console.WriteLine(stdin.ReadLine());
			
			// WaitForKey();
			return result;
		}
	}
}