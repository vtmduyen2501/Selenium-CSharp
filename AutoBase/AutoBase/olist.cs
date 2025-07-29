using OpenQA.Selenium;

namespace Auto
{
    public class olist : ocombobox
    {
        public olist(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {}
        #region Properties
        //***********************************************************************************************************************************

        //***********************************************************************************************************************************
        #endregion End - Properties

        
    }
}
