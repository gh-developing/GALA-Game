using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bystronic
{
    public interface IProcessService
    {

        public Task StartProcessAsync(string path, params string[] args);
        public Task StartProcessAsync(ProcessStartInfo processStartInfo);

    }
}
