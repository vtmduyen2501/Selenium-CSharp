using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
namespace Auto
{
    public class oelementlist
    {
        private List<oelement> myList = null;
        private IWebDriver mydriver = null;
        public oelementlist(string name, By by, IWebDriver driver, oelement parent, bool noWait, [Optional] bool getNoDisplayed) 
        {
            mydriver = driver;
            this.myList = GElements(name, by, driver, parent, noWait, getNoDisplayed);
        }
        
        public oelementlist([Optional] IWebDriver driver)
        {
            if(driver != null && mydriver == null)
                mydriver = driver;
            this.myList = new List<oelement>();
        }

        #region Properties
        //***********************************************************************************************************************************

        public List<oelement> MyList 
        {
            get { return this.myList; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public int Count 
        {
            get { return this.myList.Count; }
        }

        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Public methods

        public bool ClickOnItem(string item, [Optional] bool javaClick, [Optional] bool rightClick, int expIndex = -1) 
        {
            bool flag = true;
            int index = -1;
            if (expIndex >= 0)
                index = expIndex;
            else
                index = GetItemIndex(item);

            if (index == -1)
            {
                flag = false;
            }
            else 
            {
                IWebElement ele = this.myList[index].MyEle;
                if (rightClick)
                {
                    Actions ac = new Actions(mydriver);
                    ac.MoveToElement(ele);
                    ac.ContextClick(ele);
                    ac.Build().Perform();
                }
                else
                {
                    Actions ac = new Actions(mydriver);
                    ac.MoveToElement(ele);
                    ac.Build().Perform();
                    this.myList[index].Click(javaClick);
                }
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string GetAllItems(string split) 
        {
			string result = string.Empty;
			int count = 0;
			foreach (oelement e in this.myList)
			{
				//string temp = e.Text;
				//temp = temp.Replace("Sort in ascending order", "");
				//temp = temp.Replace("Sort in descending order", "");
                string temp = null;
                try
                {
                    temp = e.MyEle.GetAttribute("aria-label").ToLower();
                }
                catch { temp = null; }
                

				if (temp == null || temp == string.Empty)
				{
					temp = e.Text.Trim().ToLower();
				}
				temp = temp.Replace(": activate to sort column ascending", "");
				temp = temp.Replace(": activate to sort column descending", "");
				temp = temp.Replace("column menu", "");
				temp = temp.Replace("column options", "");

				if (count == 0)
					result = temp.Trim();
				else
					result = result + split + temp.Trim();
				count++;
			}
			return result;
		}
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool HaveItemInlist(string item)
        {
            bool flag = false;
            try
            {
                flag = HaveItem(item);
            }
            catch { flag = false; }
            
            return flag;
        }
        #endregion End - Public methods

        #region Private methods
        //***********************************************************************************************************************************
        private bool HaveItem(string item) 
        {
            bool flag = false;

            foreach (oelement e in this.myList)
            {
                if (e.Text.Trim().ToLower().Normalize(System.Text.NormalizationForm.FormKD) == item.ToLower().Normalize(System.Text.NormalizationForm.FormKD))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private List<oelement> GElements(string name, By by, IWebDriver driver, oelement parent, bool noWait, bool getNoDisplayed)
        {
            List<IWebElement> elist = new List<IWebElement>();
            List<oelement> eles = new List<oelement>();
            ReadOnlyCollection<IWebElement> reles = null;
            int count = 5, i = 0;

            try
            {
                Console.WriteLine("***[Find items]:" + name + "|" + by.ToString());
                if (noWait) { driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1)); }
                bool flag = false;
                int countF = 0;
                try
                {
                    while (!flag && countF < 2) 
                    {
                        if (parent == null)
                        {
                            reles = driver.FindElements(by);
                            if (reles.Count > 0)
                                elist = reles.ToList();
                        }
                        else
                        {
                            reles = parent.MyEle.FindElements(by);
                            if (reles.Count > 0)
                                elist = reles.ToList();
                        }
                        flag = true;
                        countF = 2;
                    }
                }
                catch { flag = false; countF++; Thread.Sleep(1000); }

                if (noWait) count = 1;

                while (elist.Count <= 0 && count > 0)
                {
                    flag = false;
                    try
                    {
                        while (!flag && countF < 2)
                        {
                            if (parent == null)
                            {
                                reles = driver.FindElements(by);
                                if (reles.Count > 0)
                                    elist = reles.ToList();
                            }
                            else
                            {
                                reles = parent.MyEle.FindElements(by);
                                if (reles.Count > 0)
                                    elist = reles.ToList();
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
                if(noWait) driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

                Console.WriteLine("-*-[Loop]:(" + i + ") times.");
                Console.WriteLine("-*-[Found]:(" + elist.Count + ") element(s).");

                if (elist.Count > 0)
                {
                    foreach (IWebElement e in elist) 
                    {
                        oelement ele;
                        if (!getNoDisplayed)
                        {
                            if (e.Displayed)
                            {
                                ele = new oelement(driver, e);
                                eles.Add(ele);
                            }
                        }
                        else 
                        {
                            ele = new oelement(driver, e);
                            eles.Add(ele);
                        }
                    }
                }
                else 
                {
                    if(!noWait)
                        Console.WriteLine("***[ERROR]: Not found any item.");
                }
                Console.WriteLine(".................................................................................................................");
                return eles;
            }
            catch (Exception ex)
            {
                //if(!noWait)
                Console.WriteLine("***[Ex:oelementlist]:" + ex.Message);
                Console.WriteLine(".................................................................................................................");
                return null;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private int GetItemIndex(string item)
        {
            int index = -1;
            bool flagM = true;
            string temp = string.Empty;

            if (item.Substring(0, 2) == "@@")
            {
                temp = item.Substring(2);
                flagM = false;
            }
            else
            {
                temp = item;
            }

            foreach (oelement e in this.myList)
            {
                if (flagM)
                {
                    if (e.Text.ToLower() == temp.ToLower())
                    {
                        index = this.myList.IndexOf(e);
                        break;
                    }
                }
                else
                {
                    if (e.Text.ToLower().Contains(temp.ToLower()))
                    {
                        index = this.myList.IndexOf(e);
                        break;
                    }
                }
            }
            if (index == -1) { Console.WriteLine("-*-ERROR: Not found item (" + temp + ") in list. Input param:(" + item + ")"); }
            Console.WriteLine("-*-Return index:" + index);
            return index;
        }
        
        //***********************************************************************************************************************************
        #endregion End - Private methods
    }
}
