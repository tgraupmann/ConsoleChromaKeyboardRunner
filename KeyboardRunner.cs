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

        private static Dictionary<Key, Color> _sColorMap = new Dictionary<Key, Color>();

        private static Key[,] _sKeys =
        {
            {
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
            },
            {
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
            },
            {
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
            },
        };

        public static void Start()
        {
            foreach (Key key in _sKeys)
            {
                SetColor(key, Color.Red);
            }
        }

        static int GetMapWidth()
        {
            return _sKeys.GetLength(0);
        }

        static int GetMapHeight()
        {
            return _sKeys.GetLength(1);
        }

        static void SetColor(Key key, Color color)
        {
            _sColorMap[key] = color;
            Keyboard.Instance.SetKey(key, color);
        }

        static Color GetColor(Key key)
        {
            if (_sColorMap.ContainsKey(key))
            {
                return _sColorMap[key];
            }
            return Color.Black;
        }

        public static void Update()
        {
            Random random = new Random(123);
            while (_sWaitForExit)
            {
                while (_sTimer < DateTime.Now)
                {
                    _sTimer = DateTime.Now + TimeSpan.FromMilliseconds(250);
                    for (int i = 0; i < GetMapWidth(); ++i)
                    {
                        for (int j = 0; j < (GetMapHeight() - 1); ++j)
                        {
                            Key key = _sKeys[i, j];
                            Key nextKey = _sKeys[i, j+1];

                            Color color = GetColor(nextKey);
                            SetColor(key, color);
                            Keyboard.Instance.SetKey(key, color);
                        }
                    }

                    for (int i = 0; i < GetMapWidth(); ++i)
                    {
                        for (int j = GetMapHeight() - 1; j < GetMapHeight(); ++j)
                        {
                            Key key = _sKeys[i, j];
                            Color color;
                            switch (random.Next() % 4)
                            {
                                case 0:
                                    color = new Color(1f, 1f, 0f);
                                    break;
                                case 1:
                                    color = new Color(1f, 0.5f, 0f);
                                    break;
                                case 2:
                                    color = new Color(1f, 0f, 0f);
                                    break;
                                default:
                                    color = Color.Black;
                                    break;
                            }
                            SetColor(key, color);
                            Keyboard.Instance.SetKey(key, color);
                        }
                    }
                }

                Thread.Sleep(0);
            }
        }

        public static void Stop()
        {
            _sWaitForExit = false;

            foreach (Key key in _sKeys)
            {
                SetColor(key, Color.Black);
            }
        }
    }
}
