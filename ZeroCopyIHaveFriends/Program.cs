using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V108.Page;

namespace ZeroCopyIHaveFriends // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static private IWebDriver tempmail;
        static private IWebDriver zerocopy;
        static private Random random = new Random();
        static private char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
        static void Main(string[] args)
        {
           
            CHAOS();




        }

        static void CHAOS()
        {
            
            tempmail = new ChromeDriver();
            tempmail.Url = "https://tempmailo.com/";
            zerocopy = new ChromeDriver();
            zerocopy.Url = "https://app.zerocopy.be/ref/145037";

            zerocopy.FindElement(By.LinkText("Create an account")).Click();

            zerocopy.FindElement(By.Id("txtCreateAccountEmail")).SendKeys(getMail());
            String password = getRandomPassword();
            zerocopy.FindElement(By.Id("txtCreateAccountPassword")).SendKeys(password);
            zerocopy.FindElement(By.Id("txtCreateAccountPasswordRepeat")).SendKeys(password);
            zerocopy.FindElement(By.Id("chkCreateAccountPolicy")).Click();
            zerocopy.FindElement(By.Id("btnCreateAccount")).Click();

            bool found = false;
            while (!found)
            {
                try
                {
                    if (tempmail.FindElement(By.ClassName("title")).Text.ToLower().Contains("zerocopy"))
                    {
                        found = true;
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(100);
                }
            }
            tempmail.FindElement(By.ClassName("title")).Click();
            tempmail.SwitchTo().Frame("fullmessage");
            tempmail.FindElement(By.PartialLinkText("Confirm email")).Click();
            Thread.Sleep(5000);
            zerocopy.Close();
            tempmail.Close();
            zerocopy.Quit();
            tempmail.Quit();
            
            
            
        }

        static String getMail()
        {
            return tempmail.FindElement(By.ClassName("vs-input")).GetAttribute("value");
        }

        static String getRandomPassword()
        {
            String returnpass = "";
            int passlenght = random.Next(8, 13);

            for (int i = 0; i <= passlenght; i++)
            {

                returnpass = $"{returnpass}{chars[random.Next(chars.Length)]}";
            }
            return returnpass;
        }
        
        
        
    }
}