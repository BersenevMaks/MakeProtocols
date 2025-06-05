using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MakeProtocols
{
    public class Logger
    {
        string pathLog = "";

        /// <summary>
        /// Класс для записи логов в указанную папку
        /// </summary>
        /// <param name="pathLog">Путь до папки, без указания файла</param>
        public Logger(string pathLog)
        {
            this.pathLog = pathLog+"\\log";
        }

        public void Message(string message)
        {
            try
            {
                MessageBox.Show(message, "Обратите внимание", MessageBoxButton.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool Log(string message)
        {
            bool result = false;
            try
            {
                using (FileStream fileStream = new FileStream(pathLog, mode: FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(message);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
                result = true;
            }
            catch(Exception ex)
            { MessageBox.Show(ex.ToString()); }
            return result;
        }
    }
}
