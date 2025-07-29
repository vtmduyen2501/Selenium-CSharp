using OpenQA.Selenium;

namespace Auto
{
    public class obutton : oelement
    {
        IWebDriver myDriver;
        public obutton(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {}

        public obutton(IWebDriver driver, IWebElement ele)
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
