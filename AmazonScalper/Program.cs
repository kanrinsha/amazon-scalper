using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Threading;

namespace AmazonScalper
{
    class Program
    {
        IWebDriver driver;

        private string Username { get => ConfigurationManager.AppSettings.Get("username"); }
        private string Password { get => ConfigurationManager.AppSettings.Get("password"); }
        private string BuyPage { get => ConfigurationManager.AppSettings.Get("url"); }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Initialize();
        }

        public void Initialize()
        {
            driver = new ChromeDriver(ConfigurationManager.AppSettings.Get("chromedir"))
            {
                Url = "https://www.amazon.ca/gp/sign-in.html"
            };
            
            driver.FindElement(By.CssSelector("#ap_email")).SendKeys(Username);
            driver.FindElement(By.CssSelector(".a-button-input")).Click();
            driver.FindElement(By.CssSelector("#ap_password")).SendKeys(Password);
            driver.FindElement(By.CssSelector("#signInSubmit")).Click();

            Console.WriteLine("Starting to check for items...");
            AttemptBuy();
        }

        private void AttemptBuy()
        {
            driver.Url = BuyPage;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            while (true)
            {
                //main item checking/refresh loop
                try
                {
                    driver.FindElement(By.CssSelector("button.style__button__1MgdH")).Click();

                    driver.Url = "https://www.amazon.ca/gp/cart/view.html?ref_=nav_cart";
                    //check to see if the item is in our cart... sometimes the bot can go too fast and/or the item can get snagged faster
                    try
                    {
                        driver.FindElement(By.CssSelector("#sc-buy-box-ptc-button > span:nth-child(1) > input:nth-child(1)")).Click();
                    }
                    catch
                    {
                        Console.WriteLine("Nothing in cart... Retrying...");
                        AttemptBuy();
                        break;
                    }

                    //check to see if amazon logged us out
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                    try
                    {
                        if (driver.FindElement(By.CssSelector("#ap_password")).Displayed)
                        {
                            driver.FindElement(By.CssSelector("#ap_password")).SendKeys(Password);
                            driver.FindElement(By.CssSelector("#signInSubmit")).Click();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No login needed...");
                    }

                    //finalize order
                    driver.FindElement(By.CssSelector(".place-your-order-button")).Click();

                    Console.WriteLine("Snatched!!!");

                    Environment.Exit(0);
                }
                catch(NoSuchElementException)
                {
                    Console.WriteLine("Refreshing...");
                    driver.Url = BuyPage;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
