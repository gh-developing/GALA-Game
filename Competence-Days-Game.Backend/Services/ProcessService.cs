using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bystronic
{
    public class ProcessService : IProcessService
    {

        public int Timeout { get; set; } = 30000;

        public async Task StartProcessAsync(string path, params string[] args)
        {
            await StartProcessAsync(new ProcessStartInfo(path, string.Join(" ", args)));
        }

        public async Task StartProcessAsync(ProcessStartInfo processStartInfo)
        {
            using (var process = new Process() { 
                EnableRaisingEvents = true
            })
            {
                process.StartInfo = processStartInfo;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;

                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);

                process.Start();

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                var task = process.WaitForExitAsync();
                if(await Task.WhenAny(task, Task.Delay(Timeout)) != task)
                {   
                    process.Kill();
                }

                if (process.ExitCode == 0) return;

                throw new Exception($"Failed to run process <{process.StartInfo.FileName}> exited with code {process.ExitCode}");
            }
        }
    }
}
