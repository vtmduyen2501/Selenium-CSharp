using OpenQA.Selenium;

namespace Auto
{
    public class obuttonlist : oelement
    {
        IWebDriver myDriver;
        public obuttonlist(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {}

        public obuttonlist(IWebDriver driver, IWebElement ele)
            : base(driver, ele)
        {
            this.myDriver = driver;
        }
        #region Properties
        //***********************************************************************************************************************************

        

        //***********************************************************************************************************************************
        #endregion End - Properties

    }
}
