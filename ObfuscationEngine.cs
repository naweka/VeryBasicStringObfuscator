using dnlib.DotNet;
using dnlib.DotNet.Writer;
using ExmapleObfuscator.Interfaces;
using ExmapleObfuscator.Protections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExmapleObfuscator
{
    public class ObfuscationEngine : IObfuscationEngine
    {
        bool isStringsEnabled;
        List<IProtection> protections = new List<IProtection>();

        ModuleDefMD mod;

        /// <summary>
        /// Настройка защит перед началом работы
        /// </summary>
        public void CreateConfig()
        {
            isStringsEnabled = GetAnswer("Шифруем строки?");


            if (isStringsEnabled)
                protections.Add(new StringsEncryption());
        }

        /// <summary>
        /// Выполнение всех защит в порядке их добавления
        /// </summary>
        /// <param name="path">Путь до исполняемого файла или DLL</param>
        public void Run(string path)
        {
            mod = ModuleDefMD.Load(path);

            foreach (IProtection protection in protections)
                protection.Run(mod);

            mod.Name = "owefoweufniweuf";
            mod.Assembly.Name = "oebnorbwornbown";
            var options = new ModuleWriterOptions(mod);
            options.MetadataOptions.Flags = MetadataFlags.PreserveRids;
            options.Logger = DummyLogger.NoThrowInstance;
            mod.Write("test." + path.Substring(path.Length - 3, 3), options);
        }

        private static bool GetAnswer(string text)
        {
            Console.WriteLine(text);
            Console.Write("Нажмите пробел, чтобы подтвердить, или любую клавишу для отмены: ");
            var res = ' ' == Console.ReadKey().KeyChar;
            Console.WriteLine();
            return res;
        }

        private static void ResetColor() => Console.ForegroundColor = ConsoleColor.White;
        private static void SetWarningColor() => Console.ForegroundColor = ConsoleColor.DarkYellow;
        private static void SetInfoColor() => Console.ForegroundColor = ConsoleColor.Cyan;
    }
}
