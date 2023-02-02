using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace ProjectCool_NT.Class
{
    class LED
    {
        private int BRIGHTNESS;
        private int LIGHT_COLOR;
        private int LIGHT_SAT;
        private int BREATHE_SPEED;
        private int COLOR_CHANGE_SPEED;
        private int MODE;
        private int VARIABLE_BRIGHTNESS_MODE;
        private int VARIABLE_BRIGHTNESS_VALUE;
        private string LED_DESCRIPTION = "WS2812b";

        public void CreateLed(string description, int brightness)
        {
            LED_DESCRIPTION = description;
            BRIGHTNESS = brightness;

        }

        public void CreateLed() { }

        public void setBrightnessFromDevice(int brightness)
        {
            BRIGHTNESS = brightness;
        }

        public string leds_description
        {
            get { return LED_DESCRIPTION; }
            set { LED_DESCRIPTION = value; }
        }

        public int variable_brightness_mode
        {
            get { return VARIABLE_BRIGHTNESS_MODE; }
            set { VARIABLE_BRIGHTNESS_MODE = value; }
        }

        public int variable_brightness_value
        {
            get { return VARIABLE_BRIGHTNESS_VALUE; }
            set { VARIABLE_BRIGHTNESS_VALUE = value; }
        }

        public int Brightness
        {
            get { return map(BRIGHTNESS, 0, 255, 0, 100); }
            set { BRIGHTNESS = map(value, 0, 100, 0, 255); }
        }

        public int brightness255
        {
            get { return BRIGHTNESS; }
        }

        public int Hue
        {
            get { return LIGHT_COLOR; }
            set { LIGHT_COLOR = value; }
        }

        public int Sat
        {
            get { return LIGHT_SAT; }
            set { LIGHT_SAT = value; }
        }


        public int BreatheSpeed
        {
            get { return BREATHE_SPEED; }
            set { BREATHE_SPEED = value; }
        }

        public int ColorChangeSpeed
        {
            get { return COLOR_CHANGE_SPEED; }
            set { COLOR_CHANGE_SPEED = value; }
        }

        public int Mode
        {
            get { return MODE; }
            set { MODE = value; }
        }

        public string LedModeName()
        {
            switch (MODE){
                case 0:
                    return "Static color";
                case 1:
                    return "Spectre";
                case 2:
                    return "Rainbow";
                case 3:
                    return "Breathe";
                case 4:
                    return "Flame";
                case 5:
                    return "Fan speed graph";
                case 6:
                    return "Running line";
                case 7:
                    return "Variable brightness";
                default:
                    return "unknown";
            }
            
        }

        /*
        private int constrain(int x, int min, int max)
        {
            if (x < min) return min;
            else
                if (x > max) return max;
            else
                return x;
        }
        */
        public int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }
}
