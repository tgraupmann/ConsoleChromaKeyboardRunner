using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleChromaKeyboardRunner
{
    class KeyboardRunner
    {
        /// <summary>
        /// Wait to exit
        /// </summary>
        private static bool _sWaitForExit = true;

        private static DateTime _sTimer = DateTime.Now;

        private class KeyData
        {
            public Key _mKey;
            public Color _mColor;
            public static implicit operator KeyData(Key key)
            {
                KeyData keyData = new KeyData();
                keyData._mKey = key;
                keyData._mColor = Color.Black;
                return keyData;
            }
        }

        #region Key layout

        private static KeyData[,] _sKeys =
        {
            {
                Key.Tab,
                Key.Q,
                Key.W,
                Key.E,
                Key.R,
                Key.T,
                Key.Y,
                Key.U,
                Key.I,
                Key.O,
                Key.P,
                Key.Oem4, //[
                Key.Oem5, //]
                Key.Oem6, //\
                Key.Delete,
                Key.End,
                Key.PageDown,
                Key.Num7,
                Key.Num8,
                Key.Num9,
            },
            {
                Key.CapsLock,
                Key.A,
                Key.S,
                Key.D,
                Key.F,
                Key.G,
                Key.H,
                Key.J,
                Key.K,
                Key.L,
                Key.Oem7, //;
                Key.Oem8, //'
                Key.Enter,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Num4,
                Key.Num5,
                Key.Num6,
            },
            {
                Key.LeftShift,
                Key.Z,
                Key.X,
                Key.C,
                Key.V,
                Key.B,
                Key.N,
                Key.M,
                Key.Oem9, //,
                Key.Oem10, //.
                Key.Oem11, //?
                Key.RightShift,
                Key.Invalid,
                Key.Invalid,
                Key.Invalid,
                Key.Up,
                Key.Invalid,
                Key.Num1,
                Key.Num2,
                Key.Num3,
            },
        };

        #endregion

        static int GetMapWidth()
        {
            return _sKeys.GetLength(0);
        }

        static int GetMapHeight()
        {
            return _sKeys.GetLength(1);
        }

        private static void SetColor(Key key, Color color)
        {
            if (key != Key.Invalid)
            {
                Keyboard.Instance.SetKey(key, color);
            }
        }

        public static void Start()
        {
            for (int i = 0; i < GetMapWidth(); ++i)
            {
                for (int j = 0; j < GetMapHeight(); ++j)
                {
                    KeyData keyData = _sKeys[i, j];
                    keyData._mColor = Color.Green;
                    SetColor(keyData._mKey, keyData._mColor);
                }
            }
        }

        public static void Update()
        {
            Random random = new Random(123);
            while (_sWaitForExit)
            {
                while (_sTimer < DateTime.Now)
                {
                    _sTimer = DateTime.Now + TimeSpan.FromMilliseconds(100);
                    for (int i = 0; i < GetMapWidth(); ++i)
                    {
                        for (int j = 0; j < (GetMapHeight() - 1); ++j)
                        {
                            KeyData keyData = _sKeys[i, j];
                            KeyData nextKeyData = _sKeys[i, j + 1];
                            if (keyData._mColor != nextKeyData._mColor)
                            {
                                keyData._mColor = nextKeyData._mColor;
                                SetColor(keyData._mKey, keyData._mColor);
                            }
                        }
                    }

                    for (int i = 0; i < GetMapWidth(); ++i)
                    {
                        for (int j = GetMapHeight() - 1; j < GetMapHeight(); ++j)
                        {
                            KeyData keyData = _sKeys[i, j];
                            Color color;
                            switch (random.Next() % 8)
                            {
                                case 0:
                                    color = new Color(random.NextDouble(), random.NextDouble(), random.NextDouble(), 1.0);
                                    break;
                                default:
                                    color = Color.Black;
                                    break;
                            }
                            if (keyData._mColor != color)
                            {
                                keyData._mColor = color;
                                SetColor(keyData._mKey, keyData._mColor);
                            }
                        }
                    }
                }

                Thread.Sleep(0);
            }
        }

        public static void Stop()
        {
            _sWaitForExit = false;

            for (int i = 0; i < GetMapWidth(); ++i)
            {
                for (int j = 0; j < GetMapHeight(); ++j)
                {
                    KeyData keyData = _sKeys[i, j];
                    keyData._mColor = Color.Black;
                    SetColor(keyData._mKey, keyData._mColor);
                }
            }
        }
    }
}
