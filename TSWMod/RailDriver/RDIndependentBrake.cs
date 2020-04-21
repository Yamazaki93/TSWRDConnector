using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RDIndependentBrake : RDLever
    {
        public RDIndependentBrake(int min, int max, int bailOffEngagedMin, int bailOffDisengagedMin) : base(min, max)
        {
            _bailOffEngagedMin = bailOffEngagedMin;
            _bailOffDisengagedMin = bailOffDisengagedMin;
        }

        private readonly int _bailOffEngagedMin;
        private readonly int _bailOffDisengagedMin;


        public int BailOffValue { get; set; }

        public bool BailOffEngaged => BailOffValue >= _bailOffEngagedMin - 5;
    }
}
