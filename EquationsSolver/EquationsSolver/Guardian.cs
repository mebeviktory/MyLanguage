using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    class Guardian
    {
        public Keeper currKeeper;
        public Guardian()
        {
            currKeeper = new Keeper("","","","");
        }

        public void ChangeKeeper(Keeper keeper)
        {
            currKeeper = keeper;
        }
    }
}
