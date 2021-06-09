﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public enum TemperatureReg { 
        Dry,
        PretermBag,
        TermBag
    }
    public enum CordClamping { 
        Immediate,
        Now,
        Delayed
    }
    internal class InitialAssessment : Event
    {
        private ApgarScore apgar;
        private TemperatureReg tempreg;
        private CordClamping clamping;
        private Timing time;

        public ApgarScore Apgar { get => apgar; set => apgar = value; }
        public TemperatureReg TempReg { get => tempreg; set => tempreg = value; }
        public CordClamping Clamping { get => clamping; set => clamping = value; }
        public Timing Time { get => time; set => time = value; }

        public InitialAssessment(Timing t)
        {
            this.Time = t;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Initial Assessment at " + Time.ToString());
            sb.Append('\t' + Apgar.ToString() + '\n');
            sb.Append('\t' + TempToString(TempReg) + '\n');
            sb.Append('\t' + CordClampToString(Clamping));
            if (Clamping == CordClamping.Now)
            {
                sb.Append(" " + Time.ToString() + '\n');
            }
            else {
                sb.Append('\n');
            }

            return sb.ToString();
        }
        private String TempToString(TemperatureReg temp) {
            switch (temp)
            {
                case TemperatureReg.Dry:
                    return "Dry and Wrap";
                case TemperatureReg.PretermBag:
                    return "Bag (Preterm)";
                case TemperatureReg.TermBag:
                    return "Bag (Term)";
                default: 
                    return "";

            }
        }

        private String CordClampToString(CordClamping clamping) {
            switch (clamping)
            {
                case CordClamping.Immediate:
                    return "Immediate";
                case CordClamping.Delayed:
                    return "Delayed";
                case CordClamping.Now:
                    return "Delayed";
                default:
                    return "";
            }
        }
    }
}
