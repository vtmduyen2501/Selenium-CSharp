using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using NUnit.Framework;
using System.Threading;
using ExcelReader;
using Excel;
using System.Data;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;


namespace Auto
{
    public class obase
    {
        public static int CurrentWindowIndex = 0;
        public static string browser = string.Empty;
        
        #region Private variables
        //***********************************************************************************************************************************

        const string PAGETIMINGDEFINE = "#page_timing_div>i,#page_timing_div";
        private IWebDriver driver = null;
        private Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
        private Stopwatch swStep, swCase;
        private string debug, profileFolderPath;
        public string tempFolderPath, testFolderPath;
        public string isUseGlobalPass = string.Empty;
        int elementTimeOut = 5;
        int pageTimeOut = 120;

        //--------------------------------------------
        Configuration appConfig = null;
        string readType = string.Empty;
        string row_config = string.Empty;
        string evr_config = string.Empty;
        //--------------------------------------------
        //***********************************************************************************************************************************
        #endregion End - Private variables

        #region NUnit SetUp functions
        //***********************************************************************************************************************************

        public void BeforeRunTestCase(string caseName, ref obase Base, ref bool flagExit, ref bool flagW, ref bool flag, ref bool flagC, [Optional] string sRow)
        {
            string startUpPath, dataFilePath,browserType, error;

            flagExit = true;
            flagW = true;
            flag = true;
            flagC = true;

            startUpPath = string.Empty;
            dataFilePath = string.Empty;
            error = string.Empty;
            browserType = string.Empty;
            debug = string.Empty;

            try
            {
                swCase = Stopwatch.StartNew();

                Console.WriteLine("[Run test case - " + caseName + "]");
                Console.WriteLine("*****************************************************************************************************************");
                Console.WriteLine("[Start - Init]");
                //--------
                appConfig = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                readType = appConfig.AppSettings.Settings["read_data"].Value;
                row_config = appConfig.AppSettings.Settings["row_config"].Value;
                evr_config = appConfig.AppSettings.Settings["evr_config"].Value;
                //--------
                startUpPath = Application.StartupPath;
                testFolderPath = startUpPath.Replace(@"\NUnit 2.6.3\bin", "");
                if(readType.ToLower() == "xml")
                    dataFilePath = testFolderPath + @"\Data\" + caseName + ".xml";
                else
                    dataFilePath = testFolderPath + @"\Data\" + caseName + ".xlsx";
                profileFolderPath = testFolderPath + @"\Profile\";
                tempFolderPath = testFolderPath + @"\Temp\";


                if (File.Exists(dataFilePath) == true)
                {
                    Base.LoadDictionary(dataFilePath, sRow);
                    browserType = Base.GData("Type");
                    browser = browserType;
                    debug = Base.GData("Debug");
                    isUseGlobalPass = Base.GData("UseGlobalPass");

                    flag = Base.GDriver(browserType, debug);

                    if (flag == false)
                    {
                        error = "Cannot get driver.";
                    }
                }
                else
                {
                    flag = false;
                    error = "Data file not found.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }

            //------------------------------------------------------------
            
            if (flag == false)
            {
                Console.WriteLine("*-- Data File Path: " + dataFilePath);
                Console.WriteLine("*-- Browser Type: " + browserType);
                Console.WriteLine("[End - Init] - ERROR: " + error);
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("[End - Init] - OK");
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public void BeforeRunTestStep(ref bool flag, ref bool flagExit, ref string error)
        {
            swStep = Stopwatch.StartNew();

            if (flag == false && flagExit == true)
            {
                Assert.Ignore("Previous step had critical error, Ignore this step.");
            }
            else
            {
                flag = true;
                flagExit = true;
                error = string.Empty;
            }
        }
        
        //***********************************************************************************************************************************
        #endregion End - NUnit SetUP functions

        #region NUnit TearDown functions
        //***********************************************************************************************************************************

        public void AfterRunTestStep(bool flag, ref bool flagExit, ref bool flagW, ref bool flagC, string error, [Optional] string mess)
        {
            swStep.Stop();
            TimeSpan seconds = TimeSpan.FromSeconds(swStep.Elapsed.TotalSeconds);
            string time = seconds.ToString(@"hh\:mm\:ss");

            if (error == "WARNING")
            {
                Assert.Inconclusive();
            }
            else
            {
                if (flag == false && flagC == true)
                {
                    flagC = false;
                }

                if (flag == true)
                {
                    Console.WriteLine("RESULT: PASSED | ELAPSED TIME: " + time);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                    if (mess != null)
                        Assert.Pass();
                    else
                        Assert.Pass(mess);
                }
                else
                {
                    if (flagExit == false)
                    {
                        Console.WriteLine("RESULT: FAILED | ELAPSED TIME: " + time + " | ERRORS: " + error);
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");

                        Assert.Fail(error);
                    }
                    else
                    {
                        if (flagW == true)
                        {
                            Console.WriteLine("RESULT: FAILED | ELAPSED TIME: " + time + " | ERRORS: " + error);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");

                            flagW = false;

                            Assert.Fail(error);
                        }
                        else
                        {
                            Console.WriteLine("RESULT: IGNORE");
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                            Assert.Ignore();
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public void AfterRunTestCase(bool flagC, string caseName)
        {
            swCase.Stop();

            TimeSpan seconds = TimeSpan.FromSeconds(swCase.Elapsed.TotalSeconds);
            string time = seconds.ToString(@"hh\:mm\:ss");

            Console.WriteLine("[End test case - " + caseName + "]");

            if (flagC == true)
            {
                Console.WriteLine("RESULT: PASSED | ELAPSED TIME: " + time);
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("RESULT: FAILED | ELAPSED TIME: " + time);
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            }

            Console.WriteLine("*****************************************************************************************************************");
        }

        //***********************************************************************************************************************************
        #endregion End - NUnit TearDown functions

        #region Get object methods
        //***********************************************************************************************************************************

        public object GObject(string type, string name, By by, [Optional] oelement parent, [Optional] int index, [Optional] bool noWait) 
        {
            object obj = null;

            if (noWait) { this.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));}
            
            switch (type.ToLower()) 
            {
                case "textbox":
                    obj = new otextbox("Textbox|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "textarea":
                    obj = new otextarea("Textarea|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "lookup":
                    obj = new olookup("Lookup|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "button":
                    obj = new obutton("Button|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "buttonlist":
                    obj = new obuttonlist("Button list|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "checkbox":
                    obj = new ocheckbox("Checkbox|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "label":
                    obj = new olabel("Label|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "datetime":
                    obj = new odatetime("Datetime|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "combo":
                case "combobox":
                    obj = new ocombobox("Combobox|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "table":
                    obj = new otable("Table|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "list":
                    obj = new olist("List|" + name, by, this.driver, parent, index, noWait);
                    break;
                case "radio":
                    obj = new oradio("Radio|" + name, by, this.driver, parent, index, noWait);
                    break;
                default:
                    obj = new oelement("Element|" + name, by, this.driver, parent, index, noWait);
                    break;
            }

            if (noWait) { this.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut)); }

            return obj;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public oelementlist GObjects(string name, By by, [Optional] oelement parent, [Optional] bool noWait, [Optional] bool getNoDisplayed)
        {
            oelementlist list = null;
            list = new oelementlist(name, by, this.driver, parent, noWait, getNoDisplayed);
            return list;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private oelement GParent(oelement ele, string mainFrame)
        {
            oelement parent = null;
            this.driver.SwitchTo().DefaultContent();
            if (mainFrame != null)
            {
                this.driver.SwitchTo().Frame(mainFrame);
            }
            parent = (oelement)GObject("", "Parent", By.XPath(".."), ele, 0, true);
            bool flag = parent.MyEle.GetAttribute("class").Contains("form-group");
            while (!flag)
            {
                Thread.Sleep(100);
                parent = (oelement)GObject("", "Parent", By.XPath(".."), parent, 0, true);
                flag = parent.MyEle.GetAttribute("class").Contains("form-group");
            }
            return parent;
        }
       
        //-----------------------------------------------------------------------------------------------------------------------------------
        private oelementlist GAllSection(string mainFrame)
        {
            oelementlist list = null;
            this.driver.SwitchTo().DefaultContent();
            if (mainFrame != null)
            {
                this.driver.SwitchTo().Frame(mainFrame);
            }
            string define = "span[id^='section'][class^='section'][data-header-only='false']:not([style='display: none;']), div[class^='form_action']";
            

            list = GObjects("Section", By.CssSelector(define), null, false, true);
            if (list.Count > 0) 
            {
                foreach (oelement e in list.MyList) 
                {
                    IWebElement parent = e.MyEle.FindElement(By.XPath(".."));
                    if (parent != null)
                    {
                        string sectionName = parent.GetAttribute("tab_caption");
                        if (sectionName == null)
                            sectionName = "Button section";
                        if (sectionName != string.Empty)
                        {
                            e.Section = sectionName;
                        }
                    }
                }
            }
            return list;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private oelementlist GItemOfSection(oelement section, string type, [Optional] string mainFrame)
        {
            oelementlist list = new oelementlist();
            string define = string.Empty;
            switch (type.ToLower())
            {
                case "textbox":
                    //textbox is including 'type' = string; decimal; integer; ph_number; currency
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='string']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='integer']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='decimal']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='ph_number']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='currency']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='float']:not([oncontextmenu])";
                    break;
                case "textarea":
                    //Loc Truong update new define
                    //define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='journal_input'], div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='string'][oncontextmenu]";
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='journal_input'], div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='string'][oncontextmenu], div[class^='form-group'][style*='none'] div[id^='label'][type='journal_input']";
                    break;
                case "lookup":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='reference']";
                    break;
                case "datetime":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='date_time'], div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='date']";
                    break;
                case "checkbox":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='boolean']";
                    break;
                case "radio":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='radio']";
                    break;
                case "combobox":
                    //define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='choice'],div[class^='form-group']>div[id^='label'][type='sys_class_name'],div[class^='form-group']>div[id^='label'][type='table_name']";
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='choice'],div[class^='form-group']>div[id^='label'][type='sys_class_name'],div[class^='form-group']>div[id^='label'][type='table_name'],div[class^='form-group']>div[id^='label'][type='currency']";
                    break;
                case "mandatory":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'] span[id][mandatory='true']";
                    break;
                case "mandatorynovalue":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']):not([class*='filled']) div[id^='label'] span[id][mandatory='true']:not([title*='has changed']):not([aria-label*='Read only']):not([class*='changed'])";
                    break;
                case "button":
                    define = "div>button[class^='form_action']";
                    break;
                case "buttonlist":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='glide_list']";
                    break;
            }
            this.driver.SwitchTo().DefaultContent();
            if (mainFrame != null) 
            {
                this.driver.SwitchTo().Frame(mainFrame);
            }
            oelementlist templist = GObjects("Temp list", By.CssSelector(define), section, true, true);
            foreach (oelement e in templist.MyList)
            {
                oelement p = null;
                if(type.ToLower() != "button")
                    p = GParent(e, mainFrame);
                string itemDefine = string.Empty;
                switch (type.ToLower())
                {
                    case "checkbox":
                        itemDefine = "label[class*='checkbox']:not([type='hidden'])";
                        break;
                    case "radio":
                        itemDefine = "label[class*='radio']:not([type='hidden'])";
                        break;
                    case "textarea":
                        itemDefine = "textarea[class*='form-control']:not([type='hidden'])";
                        break;
                    case "combobox":
                        itemDefine = "select[class*='form-control']:not([type='hidden']), input[id='sys_readonly.alm_consumable.sys_class_name'][class*='form-control']:not([type='hidden'])";
                        break;
                    case "textbox":
                    case "lookup":
                    case "datetime":
                        itemDefine = "input[class*='form-control']:not([type='hidden'])";
                        break;
                    case "mandatory":
                    case "mandatorynovalue":
                        itemDefine = "textarea[class*='form-control']:not([type='hidden']), input[class*='form-control']:not([type='hidden']), label[class*='checkbox']:not([type='hidden']), label[class*='radio']:not([type='hidden']), select[class*='form-control']:not([type='hidden'])";
                        break;
                    case "buttonlist":
                        itemDefine = "button[id$='_unlock']";
                        break;
                }
                this.driver.SwitchTo().DefaultContent();
                if (mainFrame != null)
                {
                    this.driver.SwitchTo().Frame(mainFrame);
                }
                oelement ele = null;
                if (type.ToLower() != "button")
                    ele = (oelement)GObject("", "Item", By.CssSelector(itemDefine), p, 0, true);
                else
                    ele = e;
                if(ele != null)
                {
                    // type
                    ele.Type = type;
                    // Get label for element
                    string label = string.Empty;
                    switch (type.ToLower()) 
                    {
                        case "mandatory":
                        case "mandatorynovalue":
                            label = e.MyEle.FindElement(By.XPath("..")).FindElement(By.CssSelector("span[class='label-text']")).GetAttribute("textContent");
                            break;
                        case "button":
                            label = e.MyEle.GetAttribute("textContent");
                            break;
                        default:
                            label = e.MyEle.FindElement(By.CssSelector("span[class='label-text']")).GetAttribute("textContent");
                            break;
                    }
                    ele.Label = label;
                    // Get the name of parent section if possible
                    ele.Section = section.Section;
                    list.MyList.Add(ele);
                }
            }
            return list;

        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public oelementlist GControlOnForm(string type, [Optional] string mainFrame)
        {
            oelementlist list = new oelementlist();
            oelementlist sections = GAllSection(mainFrame);
            foreach (oelement section in sections.MyList)
            {
                string[] arr = null;
                if (type.Contains(";"))
                {
                    arr = type.Split(';');
                }
                else { arr = new string[] { type }; }
                foreach (string t in arr) 
                {
                    Console.WriteLine("-*-Get (" + t + ") on section (" + section.Section + ")");
                    oelementlist l = GItemOfSection(section, t, mainFrame);
                    Console.WriteLine("-/*/-[FOUND]: (" + l.Count + ") " + t + " item(s) on section (" + section.Section + ")");
                    list.MyList.AddRange(l.MyList);
                }
            }
            Console.WriteLine("-*-[FOUND TOTAL]: (" + list.Count + ") " + type + " item(s)");
            foreach (oelement e in list.MyList) 
            {
                Console.WriteLine("-***-" + e.Label.Trim());
            }
            return list;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public oelementlist Portal_GAllItem(string type, [Optional] string mainFrame)
        {
            oelementlist list = new oelementlist();
            string define = string.Empty;
            switch (type.ToLower())
            {
                case "textbox":
                    //textbox is including 'type' = string; decimal; integer; ph_number; currency
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='string']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='integer']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='decimal']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='ph_number']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='currency']:not([oncontextmenu]), div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='float']:not([oncontextmenu])";
                    break;
                case "textarea":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='journal_input'], div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'][type='string'][oncontextmenu]";
                    break;
                case "lookup":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='reference']";
                    break;
                case "datetime":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='date_time'], div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='date']";
                    break;
                case "checkbox":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='boolean']";
                    break;
                case "radio":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='radio']";
                    break;
                case "combobox":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true'])>div[id^='label'][type='choice'],div[class^='form-group']>div[id^='label'][type='sys_class_name'],div[class^='form-group']>div[id^='label'][type='table_name']";
                    break;
                case "mandatory":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']) div[id^='label'] span[id][mandatory='true']";
                    break;
                case "mandatorynovalue":
                    define = "div[class^='form-group']:not([style*='none']):not([aria-hidden='true']):not([class*='filled']) div[id^='label'] span[id][mandatory='true']:not([title*='has changed']):not([aria-label*='Read only']):not([class*='changed'])";
                    break;
            }
            this.driver.SwitchTo().DefaultContent();
            if (mainFrame != null)
            {
                this.driver.SwitchTo().Frame(mainFrame);
            }
            oelementlist templist = GObjects("Temp list", By.CssSelector(define), null, true, true);
            foreach (oelement e in templist.MyList)
            {
                oelement p = GParent(e, mainFrame);
                string itemDefine = string.Empty;
                switch (type.ToLower())
                {
                    case "checkbox":
                        itemDefine = "label[class*='checkbox']:not([type='hidden'])";
                        break;
                    case "radio":
                        itemDefine = "label[class*='radio']:not([type='hidden'])";
                        break;
                    case "textarea":
                        itemDefine = "textarea[class*='form-control']:not([type='hidden'])";
                        break;
                    case "combobox":
                        itemDefine = "select[class*='form-control']:not([type='hidden'])";
                        break;
                    case "textbox":
                    case "lookup":
                    case "datetime":
                        itemDefine = "input[class*='form-control']:not([type='hidden'])";
                        break;
                    case "mandatory":
                    case "mandatorynovalue":
                        itemDefine = "[class*='form-control']:not([class*='filled']):not([type='hidden']), label[class*='checkbox']:not([type='hidden']), label[class*='radio']:not([type='hidden'])";
                        break;
                }
                this.driver.SwitchTo().DefaultContent();
                if (mainFrame != null)
                {
                    this.driver.SwitchTo().Frame(mainFrame);
                }
                oelement ele = (oelement)GObject("", "Item", By.CssSelector(itemDefine), p, 0, true);
                if (ele != null)
                {
                    // type
                    ele.Type = type;

                    // Get label for element
                    string label = string.Empty;
                    switch (type.ToLower())
                    {
                        case "mandatory":
                        case "mandatorynovalue":
                            label = e.MyEle.FindElement(By.XPath("..")).FindElement(By.CssSelector("span[class='label-text'],.sn-tooltip-basic")).GetAttribute("textContent");
                            break;
                        default:
                            label = e.MyEle.FindElement(By.CssSelector("span[class='label-text']")).GetAttribute("textContent");
                            break;
                    }
                    ele.Label = label;
                    Console.WriteLine(ele.Label + ": " + ele.Type);
                    // Get the name of parent section if possible
                    list.MyList.Add(ele);
                }
            }
            return list;

        }
        //***********************************************************************************************************************************
        #endregion End - Get object methods

        #region Get data method
        //***********************************************************************************************************************************

        public string GData(string columnName)
        {
            string temp = string.Empty;
            Console.WriteLine("[GData] - <" + columnName + ">");
            try
            {
                temp = dict[columnName.ToLower()];
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GData] - ERRORS: <" + ex.Message + ">");
            }
            
            Console.WriteLine("[GData] - RESULT: <" + temp + ">");
            Console.WriteLine(".................................................................................................................");
            
            return temp;
        }

        //***********************************************************************************************************************************
        #endregion End - Get data method

        #region Get driver method
        //***********************************************************************************************************************************

        public bool GDriver(string browserType, string debug)
        {
            string error = string.Empty;
            bool flag = true;
            browser = browserType;
            switch (browserType.ToLower())
            {
                case "ff":
                    flag = G_FF_Driver(debug);
                    break;
                case "chr":
                    flag = G_CHR_Driver(testFolderPath + @"\", debug);
                    break;
                case "ie":
                    flag = G_IE_Driver(testFolderPath + @"\", debug);
                    break;
            }

            //--------------------------------------------------------------------

            if (flag == true)
            {
                Console.WriteLine("[GDriver] - OK");
            }
            else
            {
                Console.WriteLine("[GDriver] - ERROR: Cannot get driver.");
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private bool G_FF_Driver(string debug) 
        {
            bool flag = true;
            System.Uri uri;
            try 
            {

                if (debug != null && debug.ToLower() == "yes")
                {
                    uri = new System.Uri("http://localhost:7055/hub");
                    driver = new RemoteWebDriver(uri, DesiredCapabilities.Firefox());

                    if (driver != null)
                    {
                        Console.WriteLine("[GDriver] - <FireFox> - Existed");
                        //-- Maximize window
                        driver.Manage().Window.Maximize();
                        //-- Set time out for page load
                        driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(pageTimeOut));
                        //-- Set time out for element
                        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut));
                    }
                    else { flag = false; }
                }
                else 
                {
                    var str = Path.GetTempPath();
                    Console.WriteLine("///*** " + str);
                    string[] arr = Directory.GetDirectories(str);

                    foreach (string s in arr)
                    {
                        if ((s.Contains("anonymous") && s.Contains("webdriver-profile")) || (s.Contains("rust_mozprofile")))
                        {
                            Console.WriteLine("Delete folder *** [" + s + "]");
                            try
                            {
                                Directory.Delete(s, true);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }

                    FirefoxProfile profile = new FirefoxProfile();

                    profile.AddExtension(profileFolderPath + "firebug@software.joehewitt.com.xpi");
                    profile.AddExtension(profileFolderPath + "FireXPath@pierre.tholence.com.xpi");
                    profile.SetPreference("extensions.firebug.showFirstRunPage", false);

                    driver = new FirefoxDriver(new FirefoxBinary() { Timeout = TimeSpan.FromSeconds(120) }, profile);

                    if (driver != null)
                    {
                        Console.WriteLine("[GDriver] - <FireFox> - New");
                        //-- Clear cache
                        ClearCache();
                        //-- Maximize window
                        driver.Manage().Window.Maximize();
                        //-- Set time out for page load
                        driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(pageTimeOut));
                        //-- Set time out for element
                        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut));
                    }
                    else { flag = false; }
                }

                return flag;
            }
            catch
            {
                if (debug != null && debug.ToLower() == "yes")
                {
                    var str = Path.GetTempPath();
                    Console.WriteLine("///*** " + str);
                    string[] arr = Directory.GetDirectories(str);

                    foreach (string s in arr)
                    {
                        if ((s.Contains("anonymous") && s.Contains("webdriver-profile")) || (s.Contains("rust_mozprofile")))
                        {
                            Console.WriteLine("Delete folder *** [" + s + "]");
                            try
                            {
                                Directory.Delete(s, true);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }

                    FirefoxProfile profile = new FirefoxProfile();

                    profile.AddExtension(profileFolderPath + "firebug@software.joehewitt.com.xpi");
                    profile.AddExtension(profileFolderPath + "FireXPath@pierre.tholence.com.xpi");
                    profile.SetPreference("extensions.firebug.showFirstRunPage", false);

                    driver = new FirefoxDriver(new FirefoxBinary() { Timeout = TimeSpan.FromSeconds(120) }, profile);

                    if (driver != null)
                    {
                        Console.WriteLine("[GDriver] - <FireFox> - New");
                        //-- Clear cache
                        ClearCache();
                        //-- Maximize window
                        driver.Manage().Window.Maximize();
                        //-- Set time out for page load
                        driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(pageTimeOut));
                        //-- Set time out for element
                        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut));
                    }
                    else { flag = false; }
                }
                
                return flag;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private bool G_IE_Driver(string path, string debug)
        {
            bool flag = true;
            try
            {
                if (Environment.Is64BitOperatingSystem == true)
                {
                    path = path + @"W64";
                }
                else
                {
                    path = path + @"W32";
                }

                Console.WriteLine("[G_IE_Driver] - <" + path + ">");

                var pr = Process.GetProcessesByName("IEDriverServer");

                if (pr.Length > 0 && debug != null && debug.ToLower() == "yes")
                {
                    MyRemoteWebDriver.newSession = false;

                    var configuration = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                    var url = configuration.AppSettings.Settings["ie_url"].Value;

                    driver = new MyRemoteWebDriver(new Uri(url), DesiredCapabilities.InternetExplorer());

                    Console.WriteLine("***-- Existed Driver.");
                    Console.WriteLine(((RemoteWebDriver)Driver).SessionId);
                }
                else
                {
                    var options = new InternetExplorerOptions();

                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;

                    driver = new InternetExplorerDriver(path.Trim(), options, TimeSpan.FromSeconds(120));

                    var url = driver.Url.ToString();
                    var session = ((RemoteWebDriver)driver).SessionId;

                    var configuration = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                    configuration.AppSettings.Settings["ie_url"].Value = url;
                    configuration.AppSettings.Settings["ie_session"].Value = session.ToString();
                    configuration.Save();

                    ConfigurationManager.RefreshSection("appSettings");

                    Console.WriteLine("***-- New Driver.");
                    Console.WriteLine("Url:" + url);
                    Console.WriteLine("Session Id:" + session.ToString());
                    //-- Clear cache
                    ClearCache();
                }

                //-------------------------------------------------------------------------------------

                if (driver != null)
                {
                    driver.Manage().Window.Maximize();
                    //-- Set time out for page load
                    driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(pageTimeOut));
                    //-- Set time out for element
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut));
                }
                else 
                {
                    flag = false;
                }
                return flag;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private bool G_CHR_Driver(string path, string debug)
        {
            bool flag = true;
            try
            {
                if (Environment.Is64BitOperatingSystem == true)
                {
                    path = path + @"W64";
                }
                else
                {
                    path = path + @"W32";
                }

                Console.WriteLine("[G_CHR_Driver] - <" + path + ">");

                var pr = Process.GetProcessesByName("ChromeDriver");

                if (pr.Length > 0 && debug != null && debug.ToLower() == "yes")
                {
                    MyRemoteWebDriver.newSession = false;
                    var configuration = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                    var url = configuration.AppSettings.Settings["chr_url"].Value;

                    driver = new MyRemoteWebDriver(new Uri(url), DesiredCapabilities.Chrome());
                    
                    Console.WriteLine("***-- Existed Driver.");
                    Console.WriteLine(((RemoteWebDriver)Driver).SessionId);
                }
                else
                {
                    var str = Path.GetTempPath();
                    Console.WriteLine("///*** " + str);
                    string[] arr = Directory.GetDirectories(str);

                    foreach (string s in arr)
                    {
                        if (s.Contains("chrome_url_fetcher") || s.Contains("scoped"))
                        {
                            Console.WriteLine("Delete folder *** [" + s + "]");
                            try
                            {
                                Directory.Delete(s, true);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    
                    
                    var options = new ChromeOptions();
                    options.AddArgument("disable-extensions");
                    options.AddArgument("--start-maximized");
                    options.AddArgument("disable-infobars");
                    options.AddArgument("--ignore-certificate-errors");
                    options.AddArgument("--ignore-ssl-errors");
                    options.AddUserProfilePreference("credentials_enable_service", false);
                    options.AddUserProfilePreference("profile.password_manager_enabled", false);
                    

                    ChromeDriverService crService = ChromeDriverService.CreateDefaultService(path.Trim());
                    crService.Port = 55560;
                    driver = new ChromeDriver(crService, options, TimeSpan.FromSeconds(120));
                    

                    var url = "http://localhost:55560/";
                    var session = ((RemoteWebDriver)driver).SessionId;

                    var configuration = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                    configuration.AppSettings.Settings["chr_url"].Value = url;
                    configuration.AppSettings.Settings["chr_session"].Value = session.ToString();
                    configuration.Save();

                    ConfigurationManager.RefreshSection("appSettings");

                    Console.WriteLine("***-- New Driver.");
                    Console.WriteLine("Url:" + url);
                    Console.WriteLine("Session Id:" + session.ToString());
                    //-- Clear cache
                    ClearCache();
                }

                //-------------------------------------------------------------------------------------

                if (driver != null)
                {
                    //driver.Manage().Window.Maximize();
                    //-- Set time out for page load
                    driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(pageTimeOut));
                    //-- Set time out for element
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(elementTimeOut));
                }
                else
                {
                    flag = false;
                }
                return flag;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Get driver method

        #region Clear cache method
        //***********************************************************************************************************************************

        public void ClearCache()
        {
            if (Driver == null) return;

            Driver.Manage().Cookies.DeleteAllCookies();

            if (Driver.GetType() == typeof(RemoteWebDriver) || Driver.GetType() == typeof(InternetExplorerDriver) || Driver.GetType() == typeof(FirefoxDriver) || Driver.GetType() == typeof(ChromeDriver))
            {
                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.FileName = Path.Combine(Environment.SystemDirectory, "RunDll32.exe");
                psInfo.Arguments = "InetCpl.cpl, ClearMyTracksByProcess 2";
                psInfo.CreateNoWindow = true;
                psInfo.UseShellExecute = false;
                Process p = new Process { StartInfo = psInfo };
                p.Start();
                p.WaitForExit(10000);
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Clear cache method

        #region Dictionary method
        //***********************************************************************************************************************************
        private void LoadD_Dll(string path, string srow) 
        {
            string domain;
            int iRow, i;
            i = 0;

            domain = xlReader.ReadAllDataInColumn("Domain", "Config", path)[0].Trim();

            if (srow == null || srow == string.Empty)
            {
                if (row_config.Trim() == string.Empty)
                {
                    iRow = Convert.ToInt32(xlReader.ReadAllDataInColumn("Row", "Config", path)[0].ToString().Trim());
                }
                else 
                {
                    iRow = Convert.ToInt32(row_config);
                }  
            }
            else
            {
                iRow = Convert.ToInt32(srow);
            }
            
            string[] array = xlReader.ReadAllDataBetweenRows(0, iRow, domain, path);
            string[] ColumnHeaders = array[0].Split('~');
            string[] ColumnValues = array[iRow].Split('~');

            foreach (string colH in ColumnHeaders)
            {
                string value = string.Empty;

                if (colH.Trim().ToLower() == "url" && evr_config != string.Empty)
                    value = evr_config.Trim();
                else
                    value = ColumnValues[i].Trim();

                dict.Add(colH.Trim().ToLower(), value);
                i++;
            }
        }

        private void LoadD_NoDll(string path, string srow)
        {
            //************************ DEV OPT *******************************************
            string domain;
            int iRow, index;

            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;

            DataSet result = excelReader.AsDataSet();
            domain = result.Tables[0].Rows[0][0].ToString();
            index = result.Tables.IndexOf(domain);
            if (srow == null || srow == string.Empty)
            {
                iRow = Convert.ToInt32(result.Tables[0].Rows[0][1].ToString().Trim());
            }
            else
            {
                iRow = Convert.ToInt32(srow);
            }


            var ColumnHeaders = result.Tables[index].Columns;
            int i = 0;

            foreach (var colH in ColumnHeaders)
            {
                string col = colH.ToString().Trim();
                if (!col.StartsWith("Column"))
                {
                    dict.Add(col.ToLower(), result.Tables[index].Rows[iRow - 1][i].ToString().Trim());
                    i++;
                }
            }
        }

        private void LoadD_Xml(string path, string srow) 
        {
            int i = 0;
            path = path.Replace(".xlsx", ".xml");
            if (File.Exists(path) == true)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                int iRow;

                if (srow == null || srow == string.Empty)
                {
                    if (row_config.Trim() == string.Empty)
                    {
                        XmlNode n = doc.GetElementsByTagName("RunRow")[0];
                        iRow = Convert.ToInt32(n.InnerText.Trim());
                    }
                    else 
                    {
                        iRow = Convert.ToInt32(row_config);
                    }
                }
                else
                {
                    iRow = Convert.ToInt32(srow);
                }
                
                string str = "Row" + iRow;
                XmlNode root = doc.GetElementsByTagName(str)[0];
                foreach (XmlNode node in root.ChildNodes) 
                {
                    string value = string.Empty;
                    
                    if (node.Name.ToLower() == "url" && evr_config != string.Empty)
                        value = evr_config.Trim();
                    else
                        value = node.InnerText.Trim();
                    
                    //if(node.Name.ToLower() == "debug" && readType.ToLower() == "xml")
                    //    value = "no";
                    //else
                    //    value = node.InnerText.Trim();

                    dict.Add(node.Name.Trim().ToLower(), value);
                    i++;
                }
            }
            else 
            {
                Console.WriteLine("Not found xml file.");
            }
        }

        private void LoadDictionary(string path, string srow) 
        {
            try
            {
                //var appConfig = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                //string readType = appConfig.AppSettings.Settings["read_data"].Value;
                
                switch (readType.Trim().ToLower()) 
                {
                    case "nodll":
                        LoadD_NoDll(path, srow);
                        break;
                    case "xml":
                        LoadD_Xml(path, srow);
                        break;
                    default:
                        LoadD_Dll(path, srow);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Dictionary method

        #region Switch page method
        //***********************************************************************************************************************************
        
        /// <summary>
        /// Switch to page with index
        /// </summary>
        /// <param name="windowIndex">Start with 0</param>
        /// <returns>none</returns>
        public bool SwitchToPage(int windowIndex, [Optional] bool maximize)
        {
            int windowCount, count;
            bool flag = true;

            if (driver != null)
            {
                try
                {
                    windowCount = driver.WindowHandles.Count;

                    count = 0;
                    
                    //-- wait if need
                    while ((windowCount < (windowIndex + 1)) && count < 5)
                    {
                        System.Threading.Thread.Sleep(2000);
                        windowCount = driver.WindowHandles.Count;
                        count = count + 1;

                    }
                    
                    System.Console.WriteLine("Window count: " + windowCount);
                    
                    if (windowCount >= windowIndex + 1)
                    {
                        driver.SwitchTo().Window(driver.WindowHandles[windowIndex]);
                        if(maximize)
                            driver.Manage().Window.Maximize();
                        CurrentWindowIndex = windowIndex;
                    }
                    else
                    {
                        System.Console.WriteLine("***WARN:Index out of range.");
                        driver.SwitchTo().Window(driver.WindowHandles[0]);
                        CurrentWindowIndex = 0;
                    }

                }
                catch { flag = false; }
            }
            else
            {
                flag = false;
            }
            
            return flag;
        }

        //***********************************************************************************************************************************
        #endregion End - Switch window page method

        #region Switch frame method
        //***********************************************************************************************************************************

        public bool SwitchToDefaultContent()
        {
            bool flag = true;

            if (driver != null)
            {
                try
                {
                    driver.SwitchTo().DefaultContent();
                }
                catch { flag = false; }
            }
            else
            {
                flag = false;
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool SwitchToFrame(string frameDefine)
        {
            bool flag = true;
            string temp = string.Empty;

            if (driver != null)
            {
                temp = frameDefine.Trim();

                if (temp != "")
                {
                    try
                    {
                        driver.SwitchTo().Frame(frameDefine);
                    }
                    catch { flag = false; }
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool SwitchToFrame(int index)
        {
            bool flag = true;
            string temp = string.Empty;

            if (driver != null)
            {
                try
                {
                    driver.SwitchTo().Frame(index);
                }
                catch { flag = false; }
            }
            else
            {
                flag = false;
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool SwitchToFrame(IWebElement frame)
        {
            bool flag = true;
            string temp = string.Empty;

            if (driver != null)
            {
                try
                {
                    driver.SwitchTo().Frame(frame);
                }
                catch { flag = false; }
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        public void ActiveWindow() 
        {
            //-- Active window
            IJavaScriptExecutor jscript = driver as IJavaScriptExecutor;
            jscript.ExecuteScript("alert('Test')");
            driver.SwitchTo().Alert().Accept();
            //----
        }
        //***********************************************************************************************************************************
        #endregion End - Switch frame methods

        #region Page waitLoading method
        //***********************************************************************************************************************************

        public void PageWaitLoading([Optional] bool noCheckGui, [Optional] bool noMainFrame)
        {
            int iWait = 0;
            //-- Wait script load completed
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                bool jsFlag = ((IJavaScriptExecutor)js).ExecuteScript("return document.readyState").Equals("complete");
                iWait = 0;

                while (jsFlag == false && iWait < 10)
                {
                    Thread.Sleep(2000);
                    jsFlag = ((IJavaScriptExecutor)js).ExecuteScript("return document.readyState").Equals("complete");
                    iWait = iWait + 1;
                }
            }
            catch { }

            System.Console.WriteLine("-*-Java loop: <" + iWait + ">");
            Thread.Sleep(2000);
            //-- Wait timing image displayed
            if (noCheckGui == false)
            {
                Thread.Sleep(3000);
                oelement ele = null;
                
                driver.SwitchTo().DefaultContent();

                if (noMainFrame == false)
                {
                    driver.SwitchTo().Frame("gsft_main");
                }

                try
                {
                    ele = (oelement)GObject("element", "Page timing image", By.CssSelector(PAGETIMINGDEFINE), null, 0, true);
                }
                catch { ele = null; }

                int count = 0;
                while (!ele.Existed && count < 10)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        ele = (oelement)GObject("element", "Page timing image", By.CssSelector(PAGETIMINGDEFINE), null, 0, true);
                    }
                    catch { ele = null; }

                    count = count + 1;
                }

                System.Console.WriteLine("-*-Gui loop: <" + count + ">");
            }
        }

        //***********************************************************************************************************************************
        #endregion End - WaitLoading method

        #region Window Form methods
        public bool WFileUploadSelect(string filename)
        {
            bool flag = true;

            try
            {
                string fullPath = tempFolderPath + filename;
                if (File.Exists(fullPath))
                {
                    AutoItX3Lib.AutoItX3 ait = new AutoItX3Lib.AutoItX3();
                    string title = string.Empty;
                    string browser = GData("Type");
                    if(browser.ToLower() == "ff")
                        title = "File Upload";
                    else if(browser.ToLower() == "chr")
                        title = "Open";
                    Thread.Sleep(2000);
                    ait.WinActive(title, "");
                    ait.WinActivate(title, "");
                    ait.WinWaitActive(title, "", 15);
                    ait.WinActive(title, "");
                    ait.WinActivate(title, "");
                    Thread.Sleep(2000);
                    ait.ControlSetText(title, "", "[CLASS:Edit; INSTANCE:1]", fullPath);
                    Thread.Sleep(2000);
                    if (browser.ToLower() == "chr")
                        SendKeys.SendWait("%{O}");
                    ait.ControlClick(title, "", "[CLASS:Button; INSTANCE:1]");
                    if (browser.ToLower() == "chr")
                        SendKeys.SendWait("%{O}");
                    Thread.Sleep(2000);
                    ait = null;
                }
                else 
                {
                    flag = false;
                    Console.WriteLine("***[ERROR]: Not found file (" + fullPath + ")");
                }
            }
            catch (Exception e)
            {
                flag = false;
                Console.WriteLine(e.Message);
            }

            //-----------------------------------------------------------------------------------------------------------

            if (flag == true)
            {
                Console.WriteLine("***[OK]: File upload selected.");
            }
            
            Console.WriteLine(".......................................................................................");

            return flag;
        }

        public bool WFileDownloadSelect()
        {
            bool flag = true;

            try
            {
                AutoItX3Lib.AutoItX3 ait = new AutoItX3Lib.AutoItX3();
                string title = "Opening attachments.zip";
                ait.WinWaitActive(title, "", 20);
                Thread.Sleep(2000);
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(1000);
                SendKeys.SendWait("%{s}");
                Thread.Sleep(2000);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(2000);
                ait = null;
            }
            catch (Exception e)
            {
                flag = false;
                Console.WriteLine(e.Message);
            }

            //-----------------------------------------------------------------------------------------------------------

            if (flag == true)
            {
                Console.WriteLine("***[OK]: File download selected.");
            }

            Console.WriteLine(".......................................................................................");

            return flag;
        }

        public bool IsDllRegistered()
        {
            bool flag = false;
            try
            {
                AutoItX3Lib.AutoItX3 ait = new AutoItX3Lib.AutoItX3();
                flag = true;
            }
            catch
            {
                flag = false;
            }

            return flag;
        }

        public void OpenNewBrowser(IWebDriver driver, string url)
        {
            IJavaScriptExecutor jscript = driver as IJavaScriptExecutor;
            jscript.ExecuteScript("window.open()");
            //-- Switch to last window
            driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]);
            //-- Navigate to url
            driver.Navigate().GoToUrl(url);
        }
        #endregion

        #region Properties
        //***********************************************************************************************************************************

        public IWebDriver Driver 
        {
            get { return this.driver; }
        }
        
        //***********************************************************************************************************************************
        #endregion End - Properties

        public string DecryptString(string cipherText)
        {

            string initVector = "hoanhth2uhoanh44";
            // This constant is used to determine the keysize of the encryption algorithm.
            int keysize = 256;
            string passPhrase = "tho25";

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
    #region MyRemoteWebDriver class

    public class MyRemoteWebDriver : RemoteWebDriver
    {
        public static bool newSession = false;
        
        //-------------------------------------------------------------------------------------------------------
        public MyRemoteWebDriver(Uri remoteAddress, DesiredCapabilities dd)
            : base(remoteAddress, dd)
        { }
        //-------------------------------------------------------------------------------------------------------
        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            if (driverCommandToExecute == DriverCommand.NewSession)
            {
                if (!newSession)
                {
                    string sidText = string.Empty;
                    var configuration = ConfigurationManager.OpenExeConfiguration("AutoBase.dll");
                    switch (obase.browser.ToLower()) 
                    {
                        case "ie":
                            sidText = configuration.AppSettings.Settings["ie_session"].Value;
                            break;
                        case "chr":
                            sidText = configuration.AppSettings.Settings["chr_session"].Value;
                            break;
                    }
                    return new Response
                    {
                        SessionId = sidText,
                    };
                }
                else
                {
                    var response = base.Execute(driverCommandToExecute, parameters);
                    return response;
                }
            }
            else
            {
                var response = base.Execute(driverCommandToExecute, parameters);
                return response;
            }
        }
    }

    #endregion End - MyRemoteWebDriver class
}
