using OpenQA.Selenium;

namespace Auto
{
    public class ocheckbox : oelement
    {
        IWebDriver myDriver;
        public ocheckbox(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myDriver = driver;
        }
        public ocheckbox(IWebDriver driver, IWebElement ele, string name)
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
                bool flag = true;
                oelement parent = new oelement("Parent", By.XPath(".."), this.myDriver, this, 0, true);
                flag = parent.Existed;
                if (flag) 
                {
                    oelement item = new oelement("Item", By.CssSelector("input"), this.myDriver, parent, 0, false);
                    flag = item.Existed;
                    if (flag)
                        flag = item.MyEle.Selected;
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
                    {
                        result = item.MyEle.GetAttribute("disabled");
                        if(result == null)
                        {
                            result = item.MyEle.GetAttribute("aria-readonly");
                        }
                    }                       
                }

                return result;
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Properties

    }
}
