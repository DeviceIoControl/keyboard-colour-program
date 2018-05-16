using System;
using System.Media;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using DataCenter;
using System.Windows.Forms;

//Keyboard Colour System (created by Josh Stephenson)

namespace _KeyboardColour_
{
    [StructLayout(LayoutKind.Explicit)]
    struct Union
    {
        [FieldOffset(0)]
        public int i0;

        [FieldOffset(0)]
        public byte b0;

        [FieldOffset(1)]
        public byte b1;

        [FieldOffset(2)]
        public byte b2;

        [FieldOffset(3)]
        public byte b3;

    }

    public enum MSG_SEVERITY : byte
    {
        NOTIFICATION = 0x00000001,
        WARNING = 0x00000002,
        ERROR = 0x00000004
    }

    public enum AnimationState : byte
    {
        PLAYING = 0x00000001,
        STOPPED = 0x00000002
    }

    public enum KeyboardPower : byte
    {
        ON = 0x00000001,
        OFF = 0x00000002
    }

    public enum KeyBoardPart : byte
    {
        LEFT = 0x00000001,
        MIDDLE = 0x00000002,
        RIGHT = 0x00000003,
        ALL = 0x00000006
    }

    public enum AppMode : byte
    {
        STATIC_COLOUR_PROFILE = 0x00000001,
        ANIMATED_COLOUR_PROFILE = 0x00000002,
        USER_DEFINED_COLOUR = 0x00000004
    }

    public partial class Form1 : Form
    {
        public static WmiHandle wmi = new WmiHandle();
        //public static Device myDevice = new Device();
        public Form1() { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            wmi.StartWmi();
            thisKeyboard.Init();
        }
    }

    public static class DefaultLightThemes
    {
        public static FrameObjectAnimation BasicColourWave()
        {
            ColourObject cObj1 = new ColourObject(0, 0, 255, KeyBoardPart.LEFT);
            ColourObject cObj2 = new ColourObject(0, 255, 0, KeyBoardPart.MIDDLE);
            ColourObject cObj3 = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            ColourObjectFrame Frame1 = new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });

            cObj1 = new ColourObject(255, 0, 0, KeyBoardPart.LEFT);
            cObj2 = new ColourObject(0, 0, 255, KeyBoardPart.MIDDLE);
            cObj3 = new ColourObject(0, 255, 0, KeyBoardPart.RIGHT);

            ColourObjectFrame Frame2 = new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });

            cObj1 = new ColourObject(0, 255, 0, KeyBoardPart.LEFT);
            cObj2 = new ColourObject(255, 0, 0, KeyBoardPart.MIDDLE);
            cObj3 = new ColourObject(0, 0, 255, KeyBoardPart.RIGHT);

            ColourObjectFrame Frame3 = new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });

            return new FrameObjectAnimation(new ColourObjectFrame[] { Frame1, Frame2, Frame3 }, 500);
        }
        public static FrameObjectAnimation BasicSlowBreathing()
        {
            ColourObject cObj1;
            ColourObject cObj2;
            ColourObject cObj3;

            ColourObjectFrame[] Frames = new ColourObjectFrame[510];

            int idx = 0;
            for (byte i = 255; i > 0; i--)
            {
                cObj1 = new ColourObject(0, 0, i, KeyBoardPart.LEFT);
                cObj2 = new ColourObject(0, i, 0, KeyBoardPart.MIDDLE);
                cObj3 = new ColourObject(i, 0, 0, KeyBoardPart.RIGHT);

                Frames[idx] = new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });
                idx++;
            }

            for (byte y = 0; y < 255; y++)
            {
                cObj1 = new ColourObject(0, 0, y, KeyBoardPart.LEFT);
                cObj2 = new ColourObject(0, y, 0, KeyBoardPart.MIDDLE);
                cObj3 = new ColourObject(y, 0, 0, KeyBoardPart.RIGHT);

                Frames[idx] = new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });
                idx++;
            }

            return new FrameObjectAnimation(Frames, 0);
        }
        public static FrameObjectAnimation FlashingLights()
        {
            ColourObject[] cObjArray = new ColourObject[3];

            for (byte i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(255, 0, 0, (KeyBoardPart)(i + 1)); }

            ColourObjectFrame Frame1 = new ColourObjectFrame(cObjArray);
            for (byte i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(0, 255, 0, (KeyBoardPart)(i + 1)); }

            ColourObjectFrame Frame3 = new ColourObjectFrame(cObjArray);
            for (byte i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(0, 0, 255, (KeyBoardPart)(i + 1)); }

            ColourObjectFrame Frame5 = new ColourObjectFrame(cObjArray);
            cObjArray[0] = new ColourObject(0, 0, 255, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 255, 0, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            ColourObjectFrame Frame7 = new ColourObjectFrame(cObjArray);

            for (byte i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(0, 0, 0, (KeyBoardPart)(i + 1)); }

            ColourObjectFrame BlankFrame = new ColourObjectFrame(cObjArray);

            return new FrameObjectAnimation(new ColourObjectFrame[] { Frame1, BlankFrame, Frame3, BlankFrame, Frame5, BlankFrame, Frame7, BlankFrame }, 500);
        }

        public static FrameObjectAnimation TransformingLights()
        {
            ColourObject[] cObjArray = new ColourObject[3];
            ColourObjectFrame[] AnimationFrames = new ColourObjectFrame[15];

            cObjArray[0] = new ColourObject(0, 0, 255, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[0] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 0, 255, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[1] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(0, 0, 255, KeyBoardPart.RIGHT);

            AnimationFrames[2] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(0, 255, 0, KeyBoardPart.RIGHT);

            AnimationFrames[3] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 255, 0, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[4] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(0, 255, 0, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[5] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(255, 0, 0, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[6] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(255, 0, 0, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(KeyBoardPart.RIGHT);

            AnimationFrames[7] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            AnimationFrames[8] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(0, 0, 255, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 0, 0, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            AnimationFrames[9] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(0, 0, 0, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 0, 255, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            AnimationFrames[10] = new ColourObjectFrame(cObjArray);

            cObjArray[0] = new ColourObject(0, 255, 0, KeyBoardPart.LEFT);
            cObjArray[1] = new ColourObject(0, 0, 255, KeyBoardPart.MIDDLE);
            cObjArray[2] = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            AnimationFrames[11] = new ColourObjectFrame(cObjArray);

            for (int i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(255, 0, 0, (KeyBoardPart)(i + 1)); }

            AnimationFrames[12] = new ColourObjectFrame(cObjArray);

            for (int i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(0, 255, 0, (KeyBoardPart)(i + 1)); }

            AnimationFrames[13] = new ColourObjectFrame(cObjArray);

            for (int i = 0; i < 3; ++i) { cObjArray[i] = new ColourObject(0, 0, 255, (KeyBoardPart)(i + 1)); }

            AnimationFrames[14] = new ColourObjectFrame(cObjArray);

            return new FrameObjectAnimation(AnimationFrames, 500);
        }

        public static FrameObjectAnimation ChristmasLights()
        {
            ColourObject[] colourObjArray = new ColourObject[3];
            ColourObjectFrame[] Frames = new ColourObjectFrame[3];

            for (int i = 0; i < 3; ++i) { colourObjArray[i] = new ColourObject(255, 215, 0, (KeyBoardPart)(i + 1)); }
            Frames[0] = new ColourObjectFrame(colourObjArray);


            for (int y = 0; y < 3; ++y) { colourObjArray[y] = new ColourObject(0, 0, 0, (KeyBoardPart)(y + 1)); }
            Frames[1] = new ColourObjectFrame(colourObjArray);


            for (int z = 0; z < 3; ++z) { colourObjArray[z] = new ColourObject(0, 255, 0, (KeyBoardPart)(z + 1)); }
            Frames[2] = new ColourObjectFrame(colourObjArray);

            return new FrameObjectAnimation(Frames);
        }

        public static ColourObjectFrame WhiteKeyboard()
        {
            ColourObject c1 = new ColourObject(255, 255, 255, KeyBoardPart.LEFT);
            return new ColourObjectFrame(new ColourObject[3] { c1, c1, c1 });
        }

        public static ColourObjectFrame BGRKeyboard()
        {
            ColourObject cObj1 = new ColourObject(0, 0, 255, KeyBoardPart.LEFT);
            ColourObject cObj2 = new ColourObject(0, 255, 0, KeyBoardPart.MIDDLE);
            ColourObject cObj3 = new ColourObject(255, 0, 0, KeyBoardPart.RIGHT);

            return new ColourObjectFrame(new ColourObject[] { cObj1, cObj2, cObj3 });
        }
    }

  
    public static unsafe class thisKeyboard
    {
        static Set2KbColor _keyboard = new Set2KbColor();

        private static AppMode applicationMode = AppMode.ANIMATED_COLOUR_PROFILE;
        private static KeyboardPower powerState = KeyboardPower.ON;
        private static AnimationState animState = AnimationState.PLAYING;

        public static AppMode getAppMode() { return applicationMode; }
        private static void ReInit(object sender, PowerModeChangedEventArgs e) { Init(); }

        public static void Init()
        { 
            SystemEvents.PowerModeChanged += ReInit;
            if (powerState == KeyboardPower.OFF) { setKeyBoardPower(KeyboardPower.OFF); }

            if (applicationMode == AppMode.STATIC_COLOUR_PROFILE) { SetKeyboardColour(DefaultLightThemes.BGRKeyboard()); }
            if (applicationMode == AppMode.ANIMATED_COLOUR_PROFILE) { PlayKeyboardAnimation(DefaultLightThemes.TransformingLights()); }
            if (applicationMode == AppMode.USER_DEFINED_COLOUR) { }
        }

        public static void setKeyBoardPower(KeyboardPower POWER, bool returntoFunc = false)
        {
            if (POWER == KeyboardPower.OFF) { _keyboard.SetColorAll(0, 0, 0, KeyGlobal.KbColorPart.allpart); }

            while (true) {
                if (powerState == KeyboardPower.ON && !returntoFunc) { Init(); }
                if (powerState == KeyboardPower.ON && returntoFunc) { return; }
                Thread.Sleep(1000);
            }
        }

        public static void ManageKeyboardMode(AppMode App_Mode) { applicationMode = App_Mode; }

        public static void PlayKeyboardAnimation(FrameObjectAnimation animColour)
        {
            if (applicationMode != AppMode.ANIMATED_COLOUR_PROFILE) { return; }
            //Time to wait after each frame (Colour Object Set).
            int timeWait = animColour.getTimeWait();
            int AnimationFrames = animColour.getSize();

            ColourObjectFrame[] colourObjectSets = new ColourObjectFrame[AnimationFrames];
            for (int i = 0; i < AnimationFrames; ++i) { colourObjectSets[i] = animColour.getColoursObjectSet(i); }

            //FrameObjectAnimation size is equal to the amount of frames in the animation.
            // 1 Colour Object Frame per "Frame of Animation".
            // 3 Colour Objects per Colour Object Frame. 

            while (applicationMode == AppMode.ANIMATED_COLOUR_PROFILE && powerState == KeyboardPower.ON)
            {
                if (timeWait == 0)
                {
                    for (int i = 0; i < AnimationFrames; ++i) { SetKeyboardColour(colourObjectSets[i]); }
                }
                else
                {
                    for (int i = 0; i < AnimationFrames; ++i)
                    {
                        if (powerState != KeyboardPower.ON) { setKeyBoardPower(KeyboardPower.ON, true); }
                        SetKeyboardColour(colourObjectSets[i]);
                        Thread.Sleep(timeWait);
                    }
                }
                if (animState != AnimationState.PLAYING) { while (animState != AnimationState.PLAYING) { Thread.Sleep(500); } }
            }
            return;
        }

        private static void SetKeyboardColour(ColourObjectFrame colour)
        {
            ColourObject[] objArray = new ColourObject[] { colour.getColourObject(0), colour.getColourObject(1), colour.getColourObject(2) };
            byte[] ColourArray = new byte[3];

            for (int i = 0; i < 3; ++i)
            {
                ColourArray = objArray[i].getColours();
                _keyboard.SetColor(ColourArray[0], ColourArray[1], ColourArray[2], objArray[i].getPart());
            }
        }

    }

    public class ColourObject
    {
        private byte m_Red, m_Green, m_Blue = 0;
        private KeyBoardPart KBPART = 0;

        public ColourObject()
        {
            m_Red = 0;
            m_Green = 0;
            m_Blue = 0;
            KBPART = KeyBoardPart.LEFT;
        }

        public ColourObject(KeyBoardPart part) { KBPART = part; }

        public ColourObject(byte Red, byte Green, byte Blue, KeyBoardPart part)
        {
            m_Red = Red;
            m_Green = Green;
            m_Blue = Blue;
            KBPART = part;
        }

        public byte[] getColours() { return new byte[] { m_Red, m_Green, m_Blue }; }

        public KeyGlobal.KbColorPart getPart()
        {
            switch (KBPART)
            {
                case KeyBoardPart.LEFT:
                    return KeyGlobal.KbColorPart.left;

                case KeyBoardPart.MIDDLE:
                    return KeyGlobal.KbColorPart.mid;

                case KeyBoardPart.RIGHT:
                    return KeyGlobal.KbColorPart.right;
            }
            return KeyGlobal.KbColorPart.allpart;
        }
    }

    public class ColourObjectFrame
    {
        private ColourObject[] m_Array = new ColourObject[0];

        public ColourObjectFrame(ColourObject[] objArray)
        {
            int arraySize = objArray.Length;
            if (arraySize > 3)
            {
                Environment.Exit(0xFF);
            }
            m_Array = new ColourObject[arraySize];
            for (int i = 0; i < arraySize; ++i) { m_Array[i] = objArray[i]; }
        }

        public ColourObject getColourObject(int idx) { return m_Array[idx]; }
    }

    public class FrameObjectAnimation
    {
        private ColourObjectFrame[] m_FrameArray = new ColourObjectFrame[0];
        private int m_timeWait = 0;

        public FrameObjectAnimation(ColourObjectFrame[] colourObjectFrames, int frameTimeSpan = 1000)
        {
            m_timeWait = frameTimeSpan;
            int colourObjectFramesSize = colourObjectFrames.Length;
            m_FrameArray = new ColourObjectFrame[colourObjectFramesSize];
            for (int i = 0; i < colourObjectFramesSize; ++i) { m_FrameArray[i] = colourObjectFrames[i]; }
        }

        public int getSize() { return m_FrameArray.Length; }

        public int getTimeWait() { return m_timeWait; }

        public ColourObjectFrame getColoursObjectSet(int idx) { return m_FrameArray[idx]; }
    }
   
    public class WmiHandle
    {
        private EventLog inlog = new EventLog();
        private EventLog outlog = new EventLog();

        public WmiHandle()
        {
            outlog.Source = "PowerBIOSServer_Out";
            outlog.Log = "OutLog";
            inlog.Source = "PowerBiosServerSource";
            inlog.Log = "PowerBiosServerLog";
        }

        public bool StartWmi()
        {
            if (new ServiceController { ServiceName = "PowerBiosServer" }.Status != ServiceControllerStatus.Running) { return false; }
            else
            {
                inlog.WriteEntry("w_arg0");
                return true;
            }            
        }
    }
}
