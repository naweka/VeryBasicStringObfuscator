using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExmapleObfuscator.Interfaces
{
    public interface IObfuscationEngine
    {
        /// <summary>
        /// Запуск процесса защиты сборки
        /// </summary>
        /// <param name="path">Путь (желательно полный) до файла</param>
        void Run(string path);

        /// <summary>
        /// Создание конфига для обфускатора (например, на основе данных, введенных юзером
        /// или загрузка из файла)
        /// </summary>
        void CreateConfig();
    }
}
