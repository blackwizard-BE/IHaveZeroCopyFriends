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

        static private FileStream accounts = File.Open("ZeroCopyAccounts.txt", FileMode.Append);
        static private StreamWriter writeaccounts = new StreamWriter(accounts);
        static void Main(string[] args)
        {
            while (true)
            {
                
                try
                {
                    CHAOS();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }

        }

        static void CHAOS()
        {
            
            tempmail = new ChromeDriver();
            tempmail.Url = "https://tempmailo.com/";
            if (tempmail.PageSource.Contains("Rate limit exceeded! Try again later."))
            {
                Console.WriteLine("Rate limit.");
                tempmail.Close();
                tempmail.Quit();
                Thread.Sleep(300000);
                return;
            }
            zerocopy = new ChromeDriver();
            zerocopy.Url = "https://app.zerocopy.be/ref/145037";

            zerocopy.FindElement(By.LinkText("Create an account")).Click();

            zerocopy.FindElement(By.Id("txtCreateAccountEmail")).SendKeys(getMail());
            String password = getRandomPassword();
            SaveAccount(password,getMail());
            zerocopy.FindElement(By.Id("txtCreateAccountPassword")).SendKeys(password);
            zerocopy.FindElement(By.Id("txtCreateAccountPasswordRepeat")).SendKeys(password);
            zerocopy.FindElement(By.Id("chkCreateAccountPolicy")).Click();
            zerocopy.FindElement(By.Id("btnCreateAccount")).Click();
            
            zerocopy.Close();
            zerocopy.Quit();
            bool found = false;
            int timeout = 0;
            while (!found)
            {
                if (timeout>=500)
                {
                    break;
                }
                try
                {
                    if (tempmail.FindElement(By.ClassName("title")).Text.ToLower().Contains("zerocopy"))
                    {
                        found = true;
                    }
                    else
                    {
                        timeout++;
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
            tempmail.Close();
            tempmail.Quit();
            
            
            
        }

        static String getMail()
        {
            return tempmail.FindElement(By.ClassName("vs-input")).GetAttribute("value");
        }

        static void SaveAccount(String password, String email)
        {
            writeaccounts.WriteLine($"password: {password} , e-mail: {email}");
            writeaccounts.Flush();
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