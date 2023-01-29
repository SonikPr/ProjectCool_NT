using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCool_NT.Class
{
    class Fan : LED
    {
        private int program_fan_speed;
        private int tacho_fan_speed;
        private int fan_mode;
        private int manual_fan_speed;
        private double maxCFM;
        private double currentCFM;
        private int maxRPM;
        private int currentRPM;
        private int intakeCount;
        private int exhaustCount;
        private int hysteresis;
        private string fan_description;

        public void CreateFans() { }

        public int TachoFanSpeed
        {
            get { return tacho_fan_speed; }
            set { tacho_fan_speed = value; }
        }

        public string FanDescription
        {
            get { return fan_description; }
            set { fan_description = value; }
        }

        public int ProgramFanSpeed
        {
            get { return program_fan_speed; }
            set { program_fan_speed = value; }
        }

        public int Hysteresis
        {
            get { return hysteresis; }
            set { hysteresis = value; }
        }

        public int CurrentFanMode
        {
            get { return fan_mode; }
            set { fan_mode = value; }
        }

        public int ManualFanSpeed
        {
            get { return manual_fan_speed; }
            set { manual_fan_speed = value; }
        }


        public double MaxCFM
        {
            get { return maxCFM; }
            set { maxCFM = value; }
        }

        public int MaxRPM
        {
            get { return maxRPM; }
            set { maxRPM = value; }
        }

        public int IntakeFanCount
        {
            get { return intakeCount; }
            set { intakeCount = value; }
        }

        public int ExhaustFanCount
        {
            get { return exhaustCount; }
            set { exhaustCount = value; }
        }

        public double GetRealCFM() { 

            currentCFM = (maxCFM * tacho_fan_speed / 100);
            return currentCFM;
        }

        public double GetRealRPM()
        {

            currentRPM = (maxRPM * tacho_fan_speed / 100);
            return currentRPM;
        }

        public double GetMinTemp()
        {
            switch (fan_mode){
                case 0:
                case 1:
                case 2:
                    return 30.0;
                default:
                    return 20.0;
            }

        }

        public double GetMaxTemp()
        {
            switch (fan_mode)
            {
                case 0:
                    return 40.0;
                case 1:
                    return 33.5;
                case 2:
                    return 45.0;
                default:
                    return 50.0;
            }

        }
    }
}
