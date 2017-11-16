
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
		const string tplU1 = " {0}: {1} <{2}> <{3}>";
		const string tplU2 = " // {0}, {1}.";
		const string PAC = "Press any key to continue . . . ";
		const string msgSIn = "Читаем стандартный ввод";
		const string msgSOut = "пишем в стандартный вывод";
		const string msgUsage = "Использование";
		const string msgSrcEnc = "Исходная кодировка";
		const string msgDstEnc = "Целевая кодировка";
		const string msgCode = "Код";
		const string msgName = "Имя";
		const string msgDName = "Отображаемое имя";
		const string msgInvSrcEnc = "Неверная входящая кодировка {0}!";
		const string msgInvDstEnc = "Неверная исходящая кодировка {0}!";

		static void Usage()
		{
			string exeName = Path.GetFileName(
				                 Assembly.GetAssembly(typeof(Program)).CodeBase
			                 );
			Console.WriteLine(tplU1, msgUsage, exeName, msgSrcEnc, msgDstEnc);
			Console.WriteLine(tplU2, msgSIn, msgSOut);
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
			if (args.Length < 2) {
				Usage();
				WaitForKey();
				return 0;
			}
			var srcEnc = GuessEncoding(args[0]);
			var dstEnc = GuessEncoding(args[1]);
			if (srcEnc == null) {
				Console.WriteLine(msgInvSrcEnc, args[0]);
				result |= 1;
			}
			if (dstEnc == null) {
				Console.WriteLine(msgInvDstEnc, args[1]);
				result |= 2;
			}
			WaitForKey();
			return result;
		}
	}
}