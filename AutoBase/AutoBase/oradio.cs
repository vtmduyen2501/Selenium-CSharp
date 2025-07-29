using OpenQA.Selenium;

namespace Auto
{
    public class oradio : oelement
    {
        IWebDriver myDriver;
        public oradio(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myDriver = driver;
        }
        public oradio(IWebDriver driver, IWebElement ele, string name)
            : base(driver, ele)
        {
            this.myDriver = driver;
        }
        #region Properties
        //***********************************************************************************************************************************

        public bool Checked 
        {
            get 
            {
                string check = string.Empty;
                bool flag = true;
                oelement parent = new oelement("Parent", By.XPath(".."), this.myDriver, this, 0, true);
                flag = parent.Existed;
                if (flag) 
                {
                    oelement item = new oelement("Item", By.CssSelector("input"), this.myDriver, parent, 0, false);
                    flag = item.Existed;
                    if (flag)
                    {
                       check = item.MyEle.GetAttribute("checked");
                       if (check != null && check == "checked")
                       {flag = true;}
                       else { flag = false; }
                    }
                }

                return flag;
            }
        }

        public string IsReadOnly
        {
            get
            {
                string result = string.Empty;
                oelement parent = new oelement("Parent", By.XPath(".."), this.myDriver, this, 0, true);
                bool flag = parent.Existed;
                if (flag)
                {
                    oelement item = new oelement("Item", By.CssSelector("input"), this.myDriver, parent, 0, false);
                    flag = item.Existed;
                    if (flag)
                        result = item.MyEle.GetAttribute("readonly");
                }

                return result;
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Properties

    }
}
