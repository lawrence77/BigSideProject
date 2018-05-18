using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.Components
{
    public abstract class AbstractHammingObject 
    {
        public abstract HammingReport Report { get; }
        
        public abstract void StimulateError();
    }
}
