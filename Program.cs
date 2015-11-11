using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ConsoleGame
{
    class Program
    {
        private static Screen _currentScreen;
        public const int LineWidth = 79;

        private static void Main(string[] args)
        {
            using (var screens = new Screens())
            {
                _currentScreen = screens[ScreenList.MainMenu];
                Console.WriteLine(_currentScreen.Background);
                _currentScreen.Menu.Draw();
                Console.ReadLine();
            }
        }
    }

    class Screens : IDisposable
    {
        private Dictionary<ScreenList, Screen> _screens;

        public Screens()
        {
            _screens = new Dictionary<ScreenList, Screen>
            {
                {ScreenList.MainMenu, new Screen(new Background(), new Menu("MainMenu"))}
            };
        }

        public void Dispose()
        {
            _screens = null;
        }

        public Screen this[ScreenList screenList]
        {
            get { return _screens[ScreenList.MainMenu]; }
        }
    }

    class Screen
    {
        public Screen(Background background, Menu menu)
        {
            Background = background;
            Menu = menu;
        }

        public Background Background { get; set; }
        public Menu Menu { get; set; }
    }

    class Background
    {
        public override string ToString()
        {
            return "TEST";
        }
    }

    class Menu
    {
        private const string MenuFolder = "Menus/";
        public MenuData MenuData { get; set; }

        public Menu(string menuName)
        {
            var menuLocation = string.Concat(MenuFolder, menuName, ".json");
            var menuJson = string.Concat(File.ReadAllLines(menuLocation));
            MenuData = JsonConvert.DeserializeObject<MenuData>(menuJson);
        }

        public override string ToString()
        {
            return string.Concat(MenuData.SelectMany(x => x));
        }
    }

    static class MenuExtensions
    {
        public static void Draw(this Menu menu)
        {
            foreach (var line in menu.MenuData.SelectMany(x => x))
            {
                Console.BackgroundColor = line.BackColor;
                Console.ForegroundColor = line.ForeColor;
                for (var i = 0; i < line.Count; i++)
                {
                    Console.Write(line.Symbol);
                }
            }
        }
    }

    class MenuData : List<List<Line>>
    {
        
    }

    class Line
    {
        public Line()
        {
            ForeColor = (ConsoleColor) 15;
            Count = 1;
        }
        public char Symbol { get; set; }
        public int Count { get; set; }
        public ConsoleColor BackColor { get; set; }
        public ConsoleColor ForeColor { get; set; }
    }

    class CharList : List<char>
    {
        public CharList()
        {
        }

        public CharList(string s)
        {
            AddRange(s.ToCharArray());
        }

        public CharList(char c)
        {
            Add(c);
        }

        public override string ToString()
        {
            return new string(ToArray());
        }
    }

    class MenuOption
    {

    }

    enum ScreenList
    {
        MainMenu
    }
}
