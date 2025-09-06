using System;
using UnityEngine;
using System.Diagnostics;

namespace Editor.Build
{
    public class PythonServerLauncher
    {
        public static Process StartPythonHttpServer(string folderPath, int port)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"-m http.server {port}",
                    WorkingDirectory = folderPath,
                    UseShellExecute = true,      
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false    
                };

                Process process = new Process { StartInfo = psi };
                process.Start();

                UnityEngine.Debug.Log($"Python HTTP server запущен в папке {folderPath} на порту {port}");

                return process;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Ошибка запуска Python сервера: " + ex.Message);
                return null;
            }
        }
    }
}