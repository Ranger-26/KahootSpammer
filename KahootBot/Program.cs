using System;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace KahootBot
{
    public class Program
    {
        private static IWebDriver _driver;
        private static int _count = 0;

        private static string _baseName;
        private static int _numBots;
        private static string _code;

        static void Main(string[] args)
        {
            ParseArgs(args);
            InitDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            for (int i = 0; i < _numBots; i++)
            {
                _driver.Navigate().GoToUrl("https://kahoot.it/");
                EnterIn(_code, _baseName+i);
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.open();");
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            }
        }

        private static void EnterIn(string code, string username)
        {
            _driver.FindElement(By.XPath("//*[@id='game-input']")).SendKeys(code);
            _driver.FindElement(By.XPath("//*[@id='root']/div[1]/div/div[3]/div[2]/main/div/form/button")).Click();
            _driver.FindElement(By.XPath("//*[@id='nickname']")).SendKeys(username);
            _driver.FindElement(By.XPath("//*[@id='root']/div[1]/div/div[3]/div[2]/main/div/form/button")).Click();
        }

        private static void ParseArgs(string[] args)
        {
            _code = args[0];
            _numBots = Int32.Parse(args[1]);
            _baseName = args[2];

            Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Got {_baseName} as the base name.");
            Console.WriteLine($"Got {_numBots} as the number of bots.");
            Console.WriteLine($"Got {_code} as for the kahoot game code.");
        }

        private static void InitDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            _driver = new ChromeDriver(chromeOptions);
        }
    }
}