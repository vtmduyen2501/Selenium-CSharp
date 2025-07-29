using OpenQA.Selenium;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auto
{
    public class otextbox : oelement
    {
        IWebDriver myDriver;
        public otextbox(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myDriver = driver;
        }
        public otextbox(IWebDriver driver, IWebElement ele)
            : base(driver, ele)
        {
            this.myDriver = driver;
        }
        
        #region Properties
        //***********************************************************************************************************************************
        
        
        
        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Methods
        //***********************************************************************************************************************************

        public bool VerifyCurrentText(string text, [Optional] bool wait)
        {
            System.Console.WriteLine("[Call function]: VerifyCurrentText");
            bool flag = true;

            if (text.Substring(0, 2) == "@@")
            {
                string temp = text.Substring(2);

                if (wait)
                {
                    int count = 0;
                    while (!this.Text.ToLower().Contains(temp) && count < 10)
                    {
                        Thread.Sleep(1000);
                        count++;
                    }
                }

                flag = this.Text.Contains(temp);
            }
            else
            {
                if (wait)
                {
                    int count = 0;
                    while (this.Text.Trim().ToLower() != text.ToLower() && count < 10)
                    {
                        Thread.Sleep(1000);
                        count++;
                    }
                }

                flag = (this.Text.Trim().ToLower() == text.ToLower());
            }
            System.Console.WriteLine("[Runtime value]:(" + this.Text + ")");
            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool SetText(string text, [Optional] bool enter, [Optional] bool javaClick)
        {
            bool flag = true;
            try
            {
                System.Console.WriteLine("[Call function]: SetText");
                System.Console.WriteLine("***[Set text value]:(" + text + ")");
                IJavaScriptExecutor js = null;
                this.MyEle.Clear();
                if (javaClick)
                {
                    js = this.myDriver as IJavaScriptExecutor;
                    js.ExecuteScript("arguments[0].click();", this.MyEle);
                }
                else { this.MyEle.Click(); }
                this.MyEle.SendKeys(text);
                
                int count = 0;
                while (count < 5 && this.Text != text) 
                {
                    Thread.Sleep(1000);
                    this.MyEle.Clear();
                    if (javaClick) { js.ExecuteScript("arguments[0].click();", this.MyEle); }
                    else { this.MyEle.Click(); }
                    this.MyEle.SendKeys(text);
                    count++;
                }
                System.Console.WriteLine("-*-[Loop]:(" + count + ")");
                if(this.Text != text && !enter)
                {
                    flag = false;
                    System.Console.WriteLine("-*-[ERROR]:Cannot set text. Current: " + this.Text + "| Expected:" + text);
                }

                if (enter)
                {
                    Thread.Sleep(1000);
                    this.MyEle.SendKeys(Keys.Enter);
                }

                if (flag)
                {
                    System.Console.WriteLine("***[OK]");
                }
                Console.WriteLine(".................................................................................................................");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("***[ERROR]:" + ex.Message);
                Console.WriteLine(".................................................................................................................");
                flag = false;
            }
            return flag;
        }

        //***********************************************************************************************************************************
        #endregion End - Methods
    }
}
