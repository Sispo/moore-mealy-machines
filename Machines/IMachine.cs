using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machines
{
    public interface IMachine
    {
        string Run(string[] inputString);
    }
}
