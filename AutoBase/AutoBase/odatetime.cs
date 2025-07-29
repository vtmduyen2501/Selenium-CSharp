using OpenQA.Selenium;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auto
{
    public class odatetime : oelement
    {
        private IWebDriver myDriver;
        public odatetime(IWebDriver driver, IWebElement ele)
            : base(driver, ele)
        {
            this.myDriver = driver;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public odatetime(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        { this.myDriver = driver; }

        #region Properties
        //***********************************************************************************************************************************
        
        
        
        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Methods
        //***********************************************************************************************************************************

        public bool VerifyCurrentText(string text)
        {
            System.Console.WriteLine("[Call function]: VerifyCurrentText");
            bool flag = true;
            System.Console.WriteLine("[Runtime value]:(" + this.Text + ")");
            if (text.Substring(0, 2) == "@@")
            {
                string temp = text.Substring(2);
                flag = this.Text.Contains(temp);
            }
            else
                flag = (this.Text.Trim().ToLower() == text.ToLower());

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool SetText(string text, [Optional] bool enter)
        {
            bool flag = true;
            try
            {
                System.Console.WriteLine("[Call function]: SetText");
                System.Console.WriteLine("***[Set text value]:(" + text + ")");
                this.MyEle.Clear();
                this.MyEle.Click();
                this.MyEle.SendKeys(text);

                int count = 0;
                while (count < 2 && this.Text != text)
                {
                    Thread.Sleep(1000);
                    this.MyEle.Clear();
                    IJavaScriptExecutor js = this.myDriver as IJavaScriptExecutor;
                    js.ExecuteScript("arguments[0].click();", this.MyEle);
                    this.MyEle.SendKeys(text);
                    count++;
                }

                if (this.Text != text && !enter)
                {
                    flag = false;
                    System.Console.WriteLine("-*-[ERROR]:Cannot set text. Current: " + this.Text + "| Expected:" + text);
                }

                if (enter == true)
                {
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
