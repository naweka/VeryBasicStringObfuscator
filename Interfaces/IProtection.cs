using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExmapleObfuscator.Interfaces
{
    public interface IProtection
    {
        /// <summary>
        /// Запуск обфускации
        /// </summary>
        void Run(ModuleDefMD module);
    }
}
