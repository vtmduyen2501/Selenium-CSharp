using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auto
{
    public class olookup : oelement
    {
        IWebDriver myDriver;
        public olookup(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myDriver = driver;
        }
        public olookup(IWebDriver driver, IWebElement ele)
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
        public bool SetText(string text, [Optional] bool enter)
        {
            bool flag = true;
            try
            {
                System.Console.WriteLine("[Call function]: SetText");
                System.Console.WriteLine("***[Set text value]:(" + text + ")");
                this.MyEle.Clear();
                this.MyEle.Click();
                Thread.Sleep(1000);
                this.MyEle.SendKeys(text);
                int count = 0;
                while (count < 5 && this.Text != text)
                {
                    Thread.Sleep(1000);
                    this.MyEle.Clear();
                    this.MyEle.Click();
                    this.MyEle.SendKeys(text);
                    count++;
                }
                System.Console.WriteLine("-*-[Loop]:(" + count + ")");
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
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool Select_Old(string text, [Optional] bool noFrame)
        {
            System.Console.WriteLine("[Call function]: Select");
            bool flag = true;
            try
            {
                System.Console.WriteLine("***[Select value]:(" + text + ")");
                Console.WriteLine("-*- Texbox Enable:" + this.MyEle.Enabled);
                this.myDriver.SwitchTo().Window(this.myDriver.WindowHandles[obase.CurrentWindowIndex]);
                this.myDriver.SwitchTo().DefaultContent();
                if (!noFrame)
                    this.myDriver.SwitchTo().Frame("gsft_main");
                this.MyEle.Click();
                Thread.Sleep(1000);

                flag = SetText(text);
                if (flag)
                {
                    Thread.Sleep(1000);
                    this.myDriver.SwitchTo().Window(this.myDriver.WindowHandles[obase.CurrentWindowIndex]);
                    this.myDriver.SwitchTo().DefaultContent();
                    if (!noFrame)
                        this.myDriver.SwitchTo().Frame("gsft_main");

                    if (PopupWaiting())
                    {
                        string define = "div[id^='AC'][id$='" + this.Key + "'] span, div[id^='AC'][id$='" + this.Key + "'] td[class='ac_cell']";
                        oelementlist list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);

                        int count = 0;
                        bool flagF = false;

                        flagF = list.HaveItemInlist(text);
                        while (count < 10 && !flagF)
                        {
                            Thread.Sleep(1000);
                            list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                            flagF = list.HaveItemInlist(text);
                            count++;
                        }

                        if (list.Count > 0)
                        {
                            if (flagF)
                            {
                                flag = false;
                                count = 0;
                                while (count < 5 && !flag)
                                {
                                    try
                                    {
                                        list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                                        Thread.Sleep(1000);
                                        flag = list.ClickOnItem(text);
                                        Thread.Sleep(1000);
                                    }
                                    catch { flag = false; list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false); }
                                    count++;
                                }
                                
                                if (!flag)
                                {
                                    System.Console.WriteLine("***[INFO]: Error when click on item.");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("***[INFO]: Not found item in list.");
                            }
                        }
                        else System.Console.WriteLine("***[INFO]: Not found any item.");
                    }
                    else
                    {
                        System.Console.WriteLine("***[INFO]: Popup not show.");
                        this.MyEle.SendKeys(Keys.Enter);
                    }
                }
                else
                {
                    this.MyEle.Clear();
                    this.MyEle.Click();
                    this.MyEle.SendKeys(text);
                    this.MyEle.SendKeys(Keys.Enter);
                }

                if (this.Text != text)
                {
                    flag = false;
                    System.Console.WriteLine("-*-[ERROR]:Cannot set text. Current: " + this.Text + "| Expected:" + text);
                }
                
                
                if (!WaitReferenceIcon())
                {
                    flag = false;
                    System.Console.WriteLine("***[ERROR]: Invalid refernce.");
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

        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool Select(string text, [Optional] bool noFrame)
        {
            System.Console.WriteLine("[Call function]: Select");
            bool flag = true;
            try
            {
                System.Console.WriteLine("***[Select value]:(" + text + ")");
                Console.WriteLine("-*- Texbox Enable:" + this.MyEle.Enabled);
                Console.WriteLine("Current window index: " + obase.CurrentWindowIndex);
                try
                {
                    this.myDriver.SwitchTo().Window(this.myDriver.WindowHandles[obase.CurrentWindowIndex]);
                }
                catch { Console.WriteLine("***Error when switch window"); }
                
                this.myDriver.SwitchTo().DefaultContent();
                if (!noFrame)
                    this.myDriver.SwitchTo().Frame("gsft_main");

                this.MyEle.Clear();
                this.MyEle.Click();
                Thread.Sleep(1000);

                try
                {
                    this.myDriver.SwitchTo().Window(this.myDriver.WindowHandles[obase.CurrentWindowIndex]);
                }
                catch { Console.WriteLine("***Error when switch window"); }

                this.myDriver.SwitchTo().DefaultContent();
                if (!noFrame)
                    this.myDriver.SwitchTo().Frame("gsft_main");
                
                if (PopupWaiting())
                {
                    string define = "div[id^='AC'][id$='" + this.Key + "'] span, div[id^='AC'][id$='" + this.Key + "'] td[class='ac_cell']";
                    oelementlist list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);

                    int count = 0;
                    bool flagF = false;

                    flagF = list.HaveItemInlist(text);
                    while (count < 5 && !flagF)
                    {
                        Thread.Sleep(1000);
                        list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                        flagF = list.HaveItemInlist(text);
                        count++;
                    }

                    if (flagF)
                    {
                        Thread.Sleep(1000);
                        list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);

                        if (obase.browser.ToLower() == "ff")
                            flag = list.ClickOnItem(text, false, true);
                        else
                            flag = list.ClickOnItem(text);
                        

                        if (!flag)
                        {
                            System.Console.WriteLine("***[INFO]: Error when click on item.");
                        }
                    }
                    else 
                    {
                        this.MyEle.SendKeys(text);

                        if (PopupWaiting())
                        {
                            count = 0;
                            flagF = false;

                            flagF = list.HaveItemInlist(text);
                            while (count < 5 && !flagF)
                            {
                                Thread.Sleep(1000);
                                list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                                flagF = list.HaveItemInlist(text);
                                count++;
                            }

                            if (flagF)
                            {
                                Thread.Sleep(1000);
                                list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                                flag = list.ClickOnItem(text);
                                if (!flag)
                                {
                                    System.Console.WriteLine("***[INFO]: Error when click on item.");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("***[INFO]: Not found item in list.");
                                this.MyEle.SendKeys(Keys.Enter);
                            }
                        }
                        else 
                        {
                            System.Console.WriteLine("***[INFO]: Popup not show.");
                            this.MyEle.SendKeys(Keys.Enter);
                        }
                    }
                }
                else 
                {
                    System.Console.WriteLine("***[INFO]: Popup not show.");
                    this.MyEle.SendKeys(text);

                    if (PopupWaiting())
                    {
                        string define = "div[id^='AC'][id$='" + this.Key + "'] span, div[id^='AC'][id$='" + this.Key + "'] td[class='ac_cell']";
                        oelementlist list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);

                        int count = 0;
                        bool flagF = false;

                        flagF = list.HaveItemInlist(text);
                        while (count < 5 && !flagF)
                        {
                            Thread.Sleep(1000);
                            list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                            flagF = list.HaveItemInlist(text);
                            count++;
                        }

                        if (flagF)
                        {
                            Thread.Sleep(1000);
                            list = new oelementlist("List", By.CssSelector(define), this.myDriver, null, false);
                            flag = list.ClickOnItem(text);
                            if (!flag)
                            {
                                System.Console.WriteLine("***[INFO]: Error when click on item.");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("***[INFO]: Not found item in list.");
                            this.MyEle.SendKeys(Keys.Enter);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("***[INFO]: Popup not show.");
                        this.MyEle.SendKeys(Keys.Enter);
                    }
                }
                
                //-- verify
                if (this.Text != text)
                {
                    flag = false;
                    System.Console.WriteLine("-*-[ERROR]:Cannot select value. Current: " + this.Text + "| Expected:" + text);
                }
                
                if (flag) 
                {
                    if (!WaitReferenceIcon())
                    {
                        flag = false;
                        System.Console.WriteLine("***[ERROR]: Invalid refernce.");
                    }

                    if (flag)
                    {
                        System.Console.WriteLine("***[OK]");
                    }
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

        #region Private methods

        private bool PopupWaiting() 
        {
            bool flag = false;
            ReadOnlyCollection<IWebElement> eles = null;
            string define = "div[id^='AC'][id$='" + this.Key + "']";
            eles = this.myDriver.FindElements(By.CssSelector(define));
            Console.WriteLine("-*- Poup count:(" + eles.Count + ")");
            if (eles.Count > 0) 
            {
                int count = 0;
                while (count < 5 && !eles[0].Displayed) 
                {
                    Thread.Sleep(1000);
                    count++;
                }
                flag = eles[0].Displayed;
            }
            if (flag) Console.WriteLine("-*- Found poup.");
            return flag;
        }
        
        private IWebElement GParent() 
        {
            IWebElement parent = this.MyEle.FindElement(By.XPath(".."));
            string sclass = parent.GetAttribute("class");
            while (!sclass.Contains("form-group")) 
            {
                parent = parent.FindElement(By.XPath(".."));
                sclass = parent.GetAttribute("class");
            }
            return parent;
        }

        private bool WaitReferenceIcon() 
        {
            bool flag = true;
            IWebElement parent = GParent();


            //Loc Truong && T Tran co-operate fix Sandbox2 London upgrade
            //Added css : button[id^='viewr'][data-type='reference_popup']
            //string define = "a[id^='view'], a[id^='IO'][id$='info'][data-original-title='View'], button[id^='viewr'][data-type='reference_popup']";
            string define = "[id^='view']:not([style*='display: none']), [id^='IO'][id$='info'][data-original-title='View']:not([style*='display: none'])";
            ReadOnlyCollection<IWebElement> eles = parent.FindElements(By.CssSelector(define));
            int count = 0;

            while (count < 5 && eles.Count <= 0) 
            {
                Thread.Sleep(1000);
                eles = parent.FindElements(By.CssSelector(define));
                count++;
            }


            if (eles.Count > 0)
            {
                count = 0;
                while (!eles[0].Displayed && count < 5) 
                {
                    Thread.Sleep(1000);
                    count++;
                }
                flag = eles[0].Displayed;
            }
            else 
            {
                flag = false;
            }

            return flag;
        }
        #endregion End - Private methods
    }
}
