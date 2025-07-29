using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
namespace Auto
{
    public class oelement
    {
        private IWebElement myEle = null;
        private IWebDriver myDriver = null;
        private string key = string.Empty;
        private string mySection = string.Empty;
        private string myLabel = string.Empty;
        private string myType = string.Empty;
        private string myName = string.Empty;
        public By myBy = null;
        #region Contructor
        //***********************************************************************************************************************************

        public oelement(IWebDriver driver, IWebElement e, [Optional] By by) 
        {
            this.myEle = e;
            this.myDriver = driver;
            this.myBy = by;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public oelement(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
        {
            this.myBy = by;
            this.myEle = GElement(name, driver,parent, by, index, noWait);
            this.myDriver = driver;
            this.myName = name;
        }

        //***********************************************************************************************************************************
        #endregion End - Contructor

        #region Properties
        //***********************************************************************************************************************************
        
        public IWebElement MyEle 
        {
            get { return this.myEle; }
            set { this.myEle = value; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool Existed
        {
            get 
            {
                if (this.myEle != null)
                {
                    if(!this.myName.Trim().ToLower().Contains("page timing image"))
                        MoveToElement();
                    return this.myEle.Displayed;
                }
                else 
                {
                    return false;
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool ReadOnly 
        {
            get
            {
                if (this.myEle != null)
                {
                    string temp = string.Empty;
                    if (this.myEle.TagName == "label" && this.myEle.GetAttribute("class").ToLower().Contains("checkbox"))
                    {
                        ocheckbox cb = new ocheckbox(this.myDriver, this.myEle, "Checkbox");
                        temp = cb.IsReadOnly;
                    }
                    else 
                    {
                        temp = this.myEle.GetAttribute("readonly"); 
                    }

                    string disable = this.myEle.GetAttribute("disabled");
                    if ((temp != null && (temp.ToString().ToLower().Trim() == "true" || temp.ToString().ToLower().Trim() == "readonly")) || (disable != null && disable.ToString().ToLower().Trim() == "true"))
                        return true;
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string Text
        {
            get 
            { 
                string tagname = this.myEle.TagName.ToLower();
                string text = string.Empty;
                switch (tagname) 
                {
                    case "textarea":
                    case "input":
                        text = this.MyEle.GetAttribute("value").Trim(); 
                        break;
                    case "select":
                        {
                            ocombobox combo = new ocombobox(this.myDriver, this.myEle);
                            text = combo.CurrentValue;
                        }
                        break;
                    case "label":
                        {
                            if (this.myEle.GetAttribute("class").ToLower().Contains("checkbox")) 
                            {
                                ocheckbox checkbox = new ocheckbox(this.myDriver, this.myEle, "Checkbox");
                                text = checkbox.Checked.ToString();
                            }
                        }
                        break;
                    default:
                        text = this.MyEle.Text.Trim(); 
                        break;
                }
                return text;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string Key 
        {
            get { return this.key; }
            set { this.key = value; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool Mandatory
        {
            get
            {
                bool flag = false;
                string define = "label>span[id]";
                oelement parent = GParent();
                if (parent != null)
                {
                    oelement ele = new oelement("Mandatory", By.CssSelector(define), this.myDriver, parent, 0, true);
                    if (ele != null && ele.MyEle.GetAttribute("mandatory") == "true")
                    {
                        flag = true;
                    }
                }
                return flag;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string Label
        {
            get
            {
                if (this.myLabel != string.Empty)
                    return this.myLabel.Replace("  ", " ");
                else 
                {
                    string define = "label>span[class='label-text'], label>[ng-bind='f.label'], label>span[class*='sn-tooltip-basic']";
                    oelement parent = GParent();
                    if (parent != null)
                    {
                        oelement ele = new oelement("Label", By.CssSelector(define), this.myDriver, parent, 0, true);
                        if (ele != null)
                        {
                            string str = ele.MyEle.GetAttribute("textContent");
                            this.myLabel = str.Replace("  ", " ");
                        }
                    }
                    return this.myLabel;
                }
            }
            set { this.myLabel = value; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string Section 
        {
            get { return this.mySection; }
            set { this.mySection = value; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string Type
        {
            get
            {
                string type = string.Empty;
                if (this.myType != string.Empty && this.myType != "mandatory" && this.myType != "mandatorynovalue")
                    return this.myType;
                else
                {
                    string tagname = this.MyEle.TagName.ToLower();
                    switch (tagname)
                    {
                        case "textarea":
                            type = "textarea";
                            break;
                        case "select":
                            type = "combobox";
                            break;
                        case "label":
                            string temp = this.MyEle.GetAttribute("class");
                            if (temp.Contains("radio"))
                            { type = "radio"; }
                            else { type = "checkbox"; }
                            break;
                        case "input":
                            oelement parent = GParent();
                            IWebElement e = parent.MyEle.FindElement(By.CssSelector("div[id^='label']"));
                            string str = e.GetAttribute("type");
                            switch (str.ToLower()) 
                            {
                                case "date":
                                case "9":
                                    type = "date";
                                    break;
                                case "date_time":
                                case "10":
                                    type = "datetime";
                                    break;
                                case "reference":
                                case "8":
                                    type = "lookup";
                                    break;
                                default:
                                    type = "textbox";
                                    break;
                            }
                            
                            break;
                    }
                    
                    this.myType = type;
                    
                    return this.myType;
                }
            }
            set { this.myType = value; }
        }
        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Public methods

        public bool Click([Optional] bool javaClick, [Optional] bool notHighlight)
        {
            if(!notHighlight)
                Highlight();
            
            if (javaClick)
            {
                return ElementClick(this.myEle, this.myDriver);
            }
            else
            {
                return ElementClick(this.myEle);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public void Highlight()
        {
            if (this.myEle != null && this.myEle.GetAttribute("class") != "context_menu" && this.myEle.TagName != "label")
            {
                IJavaScriptExecutor js = this.myDriver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].setAttribute('style', 'border-width: 1px; border-style: solid; border-color: lightblue')", this.myEle);
            }
        }

        public string GSection()
        {
            string section = null;
            oelement parent = new oelement("Parent", By.XPath(".."), this.myDriver, this, 0, true);
            bool flag = parent.MyEle.GetAttribute("id").Contains("section_tab");
            while (!flag)
            {
                Thread.Sleep(100);
                parent = new oelement("Parent", By.XPath(".."), this.myDriver, parent, 0, true);
                flag = parent.MyEle.GetAttribute("id").Contains("section_tab");
            }
            section = parent.myEle.GetAttribute("tab_caption").Trim();
            return section;
        }

        public void MoveToElement()
        {
            try
            {
                Actions ac = new Actions(myDriver);
                ac.MoveToElement(myEle);
                ac.Perform();
            }
            catch { }
        }
        #endregion End - Public methods

        #region Private methods
        //***********************************************************************************************************************************

        private IWebElement GElement(string name, IWebDriver driver,oelement parent, By by, [Optional] int index, [Optional] bool noWait)
        {
            List<IWebElement> elist = new List<IWebElement>();
            IWebElement e = null;
            ReadOnlyCollection<IWebElement> eles = null;
            int count = 5, i = 0;
            
            try
            {
                if(!name.ToLower().Contains("parent"))
                    Console.WriteLine("***[Find]:" + name + "|" + by.ToString());
                if (noWait) { driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1)); }
                bool flag = false;
                int countF = 0;
                try
                {
                    while (!flag && countF < 2)
                    {
                        if (parent == null)
                        {
                            eles = driver.FindElements(by);
                            if (eles.Count > 0)
                                elist = eles.ToList();
                        }
                        else
                        {
                            eles = parent.MyEle.FindElements(by);
                            if (eles.Count > 0)
                                elist = eles.ToList();
                        }
                        flag = true;
                        countF = 2;
                    }
                }
                catch { flag = false; countF++; Thread.Sleep(1000); }
                
                if (noWait) count = 1;

                while (elist.Count <= 0 && count > 0)
                {
                    try
                    {
                        while (!flag && countF < 2)
                        {
                            if (parent == null)
                            {
                                eles = driver.FindElements(by);
                                if (eles.Count > 0)
                                    elist = eles.ToList();
                            }
                            else
                            {
                                eles = parent.MyEle.FindElements(by);
                                if (eles.Count > 0)
                                    elist = eles.ToList();
                            }
                            flag = true;
                            countF = 2;
                        }
                    }
                    catch { flag = false; countF++; Thread.Sleep(1000); }
                    i++;
                    count--;
                    Thread.Sleep(1000);
                }
                if (noWait) { driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5)); }

                if (!name.ToLower().Contains("parent")) 
                {
                    Console.WriteLine("-*-[Loop]:(" + i + ") times.");
                    Console.WriteLine("-*-[Found]:(" + elist.Count + ") element(s).");
                }
                
                if (elist.Count > 0)
                {
                    e = elist[index];
                    
                    if (!noWait) 
                    {
                        count = 0;
                        while (count < 10 && !e.Displayed)
                        {
                            Thread.Sleep(1000);
                            count++;
                        }
                        if (!name.ToLower().Contains("parent"))
                            Console.WriteLine("-*-[Wait display]:(" + count + ") times.");
                    }
                }

                if (e != null && e.Displayed)
                {
                    if (!noWait && e.GetAttribute("class") != "context_menu" && !name.ToLower().Contains("parent") && e.TagName != "label" && !name.ToLower().Contains("icon")) 
                    {
                        //string bgr = e.GetCssValue("background-color");
                        IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                        //js.ExecuteScript("arguments[0].setAttribute('style', 'border-width: 1px; border-style: solid; border-color: lightblue; background-color: " + bgr + "')", e);
                        js.ExecuteScript("arguments[0].setAttribute('style', 'border-width: 1px; border-style: solid; border-color: lightblue" + "')", e);
                    }
                    if (!name.ToLower().Contains("parent"))
                        Console.WriteLine("Displayed:" + e.Displayed);
                }
                else 
                {
                    if(!noWait)
                        Console.WriteLine("***[ERROR]:NOT found element.");
                }
                if (!name.ToLower().Contains("parent"))    
                    Console.WriteLine(".................................................................................................................");
                return e;
            }
            catch (Exception ex)
            {
                //if(!noWait)
                Console.WriteLine("***[Ex:oelement]:" + ex.Message);
                Console.WriteLine(".................................................................................................................");
                return null;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private bool ElementClick(IWebElement e, [Optional] IWebDriver driver)
        {
            bool flag = true;
            try
            {
                Console.WriteLine("***[Click]");

                if (driver != null)
                {
                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                    js.ExecuteScript("arguments[0].click();", e);
                }
                else
                {
                    e.Click();
                }

                Console.WriteLine("***[OK]");
                Console.WriteLine(".................................................................................................................");
            }
            catch (Exception ex)
            {
                Console.WriteLine("***[ERROR]:" + ex.Message);
                Console.WriteLine(".................................................................................................................");
                flag = false;
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private oelement GParent()
        {
            oelement parent = null;
            parent = new oelement("Parent", By.XPath(".."), this.myDriver, this, 0, true);
            bool flag = parent.MyEle.GetAttribute("class").Contains("form-group");
            while (!flag)
            {
                Thread.Sleep(100);
                parent = new oelement("Parent", By.XPath(".."), this.myDriver, parent, 0, true);
                flag = parent.MyEle.GetAttribute("class").Contains("form-group");
            }
            return parent;
        }

        //***********************************************************************************************************************************
        #endregion End - Private methods
    }
}
