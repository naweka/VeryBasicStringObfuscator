using System;

namespace ExmapleObfuscator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Drag-n-Drop!");

            var engine = new ObfuscationEngine();

            SetWarningColor();
            Console.WriteLine("Очень простой пример паблик обфускатора");
            Console.WriteLine("Здесь нет виртуализации");
            Console.WriteLine("Здесь нет JIT");
            Console.WriteLine("Здесь нет антиэмуляции");
            Console.WriteLine("Здесь нет антидебага");
            Console.WriteLine("Может работать нестабильно на крупных проектах");
            Console.WriteLine("Пожалуйста, не используйте это для защиты своих приложений");

            SetInfoColor();
            Console.WriteLine("Настройка защит:");
            ResetColor();
            engine.CreateConfig();


            SetInfoColor();
            Console.WriteLine("Пожалуйста, подождите");
            ResetColor();
            engine.Run(args[0]);

            SetInfoColor();
            Console.WriteLine("Готово! Нажмите Enter, чтобы закрыть это окно");
            Console.ReadLine();
        }

        private static void ResetColor() => Console.ForegroundColor = ConsoleColor.White;
        private static void SetWarningColor() => Console.ForegroundColor = ConsoleColor.DarkYellow;
        private static void SetInfoColor() => Console.ForegroundColor = ConsoleColor.Cyan;
    }
}