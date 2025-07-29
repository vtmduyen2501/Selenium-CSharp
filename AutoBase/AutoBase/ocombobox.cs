using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auto
{
    public class ocombobox : oelement
    {
        private SelectElement myCombobox = null;
        
        public ocombobox(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myCombobox = new SelectElement(this.MyEle);
        }
        public ocombobox(IWebDriver driver, IWebElement ele)
            : base(driver, ele)
        {
            this.myCombobox = new SelectElement(ele);
        }
        #region Properties
        //***********************************************************************************************************************************

        public SelectElement MyCombobox 
        {
            get { return this.myCombobox; }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string CurrentValue
        {
            get {
                if (this.myCombobox.AllSelectedOptions.Count != 0)
                    return this.myCombobox.SelectedOption.Text;
                else
                    return string.Empty;
            }
        }
        

        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Public methods

        public bool SelectItem(string item) 
        {
            bool flag = true;
            int index = GetItemIndex(item);
            if (index == -1)
            {
                flag = false;
            }
            else
            {
                this.myCombobox.SelectByIndex(index);
            }

            return flag;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool VerifyCurrentText(string text, [Optional] bool wait)
        {
            System.Console.WriteLine("[Call function]: VerifyCurrentText");
            bool flag = true;

            if (wait)
            {
                int count = 0;
                while (this.myCombobox.SelectedOption.Text.ToLower() != text.ToLower() && count < 5)
                {
                    Thread.Sleep(1000);
                    count++;
                }
            }

            System.Console.WriteLine("[Runtime value]:(" + this.myCombobox.SelectedOption.Text + ")");

            if (text.Substring(0, 2) == "@@")
            {
                string temp = text.Substring(2);
                flag = this.myCombobox.SelectedOption.Text.ToLower().Contains(temp.ToLower());
            }
            else
                flag = (this.myCombobox.SelectedOption.Text.ToLower() == text.ToLower());

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool VerifyItemList(string items)
        {
            System.Console.WriteLine("[Call function]: VerifyItemList");
            bool flag = true;
            string[] arr = null;
            if (items.Contains(";"))
            {
                arr = items.Split(';');
            }
            else { arr = new string[] { items }; }

            if (arr.Length != this.myCombobox.Options.Count) 
            {
                System.Console.WriteLine("[WARNING]: NOT match items count. Expected:(" + arr.Length + "). Runtime:(" + this.myCombobox.Options.Count + ")");
            }
            else System.Console.WriteLine("[Passed]: Match items count. Expected:(" + arr.Length + "). Runtime:(" + this.myCombobox.Options.Count + ")");
            
            bool flagF = true;
            foreach (string item in arr) 
            {
                flagF = VerifyItem(item);
                if (!flagF && flag) { flag = false; }
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public bool HaveItemInlist(string item)
        {
            bool flag = false;

            foreach (IWebElement ele in this.MyCombobox.Options)
            {
                
                if (ele.Text.ToLower().Equals(item.ToLower()))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public string GetItemlist()
        {
            string itemList = string.Empty;

            foreach (IWebElement ele in this.MyCombobox.Options)
            {
                itemList = itemList + ele.Text.Trim() + ";";
            }
            return itemList;
        }
        
        #endregion End - Public methods

        #region Private methods

        private int GetItemIndex(string item)
        {
            int index = -1;
            bool flagM = true;
            string temp = string.Empty;

            if (item.Contains("@@"))
            {
                temp = item.Substring(2);
                flagM = false;
            }
            else
            {
                temp = item;
            }

            foreach (IWebElement e in this.myCombobox.Options)
            {
                if (flagM)
                {
                    if (e.Text.ToLower() == temp.ToLower() && e.Enabled == true)
                    {
                        index = this.myCombobox.Options.IndexOf(e);
                        break;
                    }
                }
                else
                {
                    if (e.Text.ToLower().Contains(temp.ToLower()) && e.Enabled == true)
                    {
                        index = this.myCombobox.Options.IndexOf(e);
                        break;
                    }
                }
            }
            if (index == -1) { Console.WriteLine("-*-ERROR: Not found item (" + temp + ") in list. Input param:(" + item + ")"); }
            return index;
        }

        private bool VerifyItem(string item) 
        {
            bool flag = false;
            foreach(IWebElement ele in this.myCombobox.Options)
            {
                if (ele.Text.Trim().ToLower().Equals(item.ToLower())) 
                {
                    flag = true;
                    System.Console.WriteLine("-*-[Passed]: Found item (" + item + ") in combobox item list.");
                    break;
                }
            }
            if (!flag) System.Console.WriteLine("-*-[FAILED]: Not found (" + item + ") in combobox item list.");
            return flag;
        }
        #endregion End - Private methods
    }
}
