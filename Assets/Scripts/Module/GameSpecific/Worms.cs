using Nofun.Driver.Audio;
using Nofun.Module.VMStream;
using Nofun.Util.Logging;
using Nofun.VM;
using System;
using System.IO;

namespace Nofun.Module.GameSpecific
{


    [Module]
    public partial class Worms
    {
        private VMSystem system;

        public Worms(VMSystem system)
        {
            this.system = system;

        }

        [ModuleCall]
        private void wormsSetFocusFunc()
        {

        }
    }
}
