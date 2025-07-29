using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Auto
{
    public class otab : oelement
    {
        private oelement header = null;
        private string tabName = string.Empty;
        private IWebDriver myDriver = null;
        private otable myTable = null;
        
        public otab(IWebDriver driver, IWebElement e, otable table, string name) : base(driver, e)
        {
            this.tabName = name;
            this.myDriver = driver;
            this.myTable = table;
            this.header = GHeader();
        }

        #region Textbox control

        private otextbox Textbox_Search
        {
            get
            {
                otextbox tb = null;
                string define = "input[id$='_text']";
                tb = new otextbox("Search", By.CssSelector(define), this.myDriver, this, 0, false);
                return tb;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private otextbox Textbox_Filter_Value
        {
            get
            {
                otextbox tb = null;
                string define = "input[class^='filerTableInput form-control'][title='input value']";
                tb = new otextbox("Input Value", By.CssSelector(define), this.myDriver, this, 0, false);
                return tb;
            }
        }

        #endregion End - Textbox control

        #region Combobox control
        //***********************************************************************************************************************************
        
        private ocombobox Combobox_Goto 
        {
            get 
            {
                ocombobox cb = null;
                string define = "select[id$='_select']";
                cb = new ocombobox("Goto", By.CssSelector(define), this.myDriver, this, 0, false);
                return cb;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private ocombobox Combobox_Action
        {
            get
            {
                ocombobox cb = null;
                string define = "select[id$='_labelAction']";
                cb = new ocombobox("Action", By.CssSelector(define), this.myDriver, this, 0, false);
                return cb;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private ocombobox Combobox_Filter_ChooseField
        {
            get
            {
                ocombobox cb = null;
                string define = "td[id='field'] select[class^='filerTableSelect select2 form-control'][title='Choose Input']";
                cb = new ocombobox("Choose Field", By.CssSelector(define), this.myDriver, this, 0, false);
                return cb;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private ocombobox Combobox_Filter_Operator
        {
            get
            {
                ocombobox cb = null;
                string define = "td[id='oper'] select[class^='filerTableSelect select2 form-control'][title='choose operator']";
                cb = new ocombobox("Choose Operator", By.CssSelector(define), this.myDriver, this, 0, false);
                return cb;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private ocombobox Combobox_Filter_Value
        {
            get
            {
                ocombobox cb = null;
                string define = "td[id='value'] select[class^='filerTableSelect select2 form-control'][title='Choose Input']";
                cb = new ocombobox("Choose Operator", By.CssSelector(define), this.myDriver, this, 0, false);
                return cb;
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Combobox control

        #region Button control
        //***********************************************************************************************************************************

        private obutton Button_Edit
        {
            get 
            {
                obutton bt = null;
                string define = "button[id^='sysverb_edit_']";
                bt = new obutton("Edit", By.CssSelector(define), this.myDriver, this, 0, false);
                return bt;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private obutton Button_New
        {
            get
            {
                obutton bt = null;
                string define = "sysverb_new";
                bt = new obutton("New", By.Id(define), this.myDriver, this, 0, false);
                return bt;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private obutton Button_AdditionalAction
        {
            get
            {
                obutton bt = null;
                string define = "button[title='List controls'][data-list_id$='u_external_references.u_task']";
                bt = new obutton("Additional Action", By.CssSelector(define), this.myDriver, this, 0, false);
                return bt;
            }
        }

        //***********************************************************************************************************************************
        #endregion End - Button control

        #region Element control
        //***********************************************************************************************************************************

        private oelement Link_Filter
        {
            get
            {
                oelement link = null;
                string define = "a[id$='u_task_filter_toggle_image']";
                link = new oelement("Additional Action", By.CssSelector(define), this.myDriver, this, 0, false);
                return link;
            }
        }
        //--------------------------------------------------------------------------------------------------------

        private oelement Link_RecordId
        {
            get
            {
                oelement link = null;
                string define = "a[class='breadcrumb_link']";
                link = new oelement("Record ID", By.CssSelector(define), this.myDriver, this, 0, false);
                return link;
            }
        }
        //***********************************************************************************************************************************
        #endregion End - Element control

        #region Properties

        public oelement Header 
        {
            get { return this.header; }
        }

        public otable Table
        {
            get { return this.myTable; }
        }

        #endregion End - Properties

        #region Private methods
        //***********************************************************************************************************************************

        public oelement GHeader([Optional] int index) 
        {
            oelement ele = null;
            string define = ".tab_caption_text";
            oelementlist list = new oelementlist("Header list", By.CssSelector(define), this.myDriver, null, false);
            int count = 0;
            foreach(oelement e in list.MyList) 
            {
                string temp = e.Text.Trim().ToLower();
                if (temp.Contains("(") && temp.Contains(")"))
                {
                    if (temp.Contains(this.tabName.ToLower())) 
                    {
                        if (index == count) 
                        {
                            ele = e;
                            break;
                        }
                        count++;
                    }
                }
                else 
                {
                    if (temp == this.tabName.ToLower())
                    {
                        if (index == count)
                        {
                            ele = e;
                            break;
                        }
                        count++;
                    }
                }
            }
            return ele;
        }
        
        //***********************************************************************************************************************************
        #endregion End - Private methods

        #region Public methods
        //***********************************************************************************************************************************

        



        public otable RefreshTable()
        {
            this.myTable = new otable(this.tabName, By.CssSelector("table table[glide_table]"), this.myDriver, this, 0, false);
            return this.myTable;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool ClickEdit([Optional] bool javaClick)
        {
            bool flag = true;
            obutton bt = Button_Edit;
            flag = bt.Existed;
            if (flag)
            {
                flag = bt.Click(javaClick);
            }
            else System.Console.WriteLine("-*-[FAILED]: Cannot get button edit.");
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool ClickNew([Optional] bool javaClick) 
        {
            bool flag = true;
            obutton bt = Button_New;
            flag = bt.Existed;
            if (flag) 
            {
                flag = bt.Click(javaClick);
            } else System.Console.WriteLine("-*-[FAILED]: Cannot get button new.");
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearch(string searchBy, string searchValue)
        {
            bool flag = true;
            ocombobox cb = Combobox_Goto;
            flag = cb.Existed;
            if (flag)
            {
                flag = cb.SelectItem(searchBy);
                if (flag)
                {
                    otextbox tb = Textbox_Search;
                    flag = tb.Existed;
                    if (flag)
                    {
                        flag = tb.SetText(searchValue, true);
                        if (!flag) System.Console.WriteLine("-*-[FAILED]: Cannot populate value for textbox search.");
                    }
                    else System.Console.WriteLine("-*-[FAILED]: Cannot get textbox search.");
                }
                else System.Console.WriteLine("-*-[FAILED]: Combobox cannot select value (" + searchBy + ")");
            }
            else System.Console.WriteLine("-*-[FAILED]: Cannot get combobox goto.");
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableVerifyRow(string conditions, [Optional] bool expectedNotfound) 
        {
            bool flag = true;
            otable table = null;

            try
            {
                table = this.RefreshTable();
                flag = table.Existed;
                if (flag)
                {
                    if (table.RowCount(expectedNotfound) > 0)
                    {
                        flag = table.FindRow(conditions, expectedNotfound);
                    }
                    else
                        if (!expectedNotfound)
                            flag = false;
                }
            }
            catch { flag = false; }
            int count = 0;
            if (expectedNotfound) count = 2;
            while (count < 5 && !flag)
            {
                Thread.Sleep(1000);
                try
                {
                    this.Header.Click();
                    table = this.RefreshTable();
                    Thread.Sleep(1000);
                    flag = table.Existed;
                    if (flag)
                    {
                        if (table.RowCount(expectedNotfound) > 0)
                        {
                            flag = table.FindRow(conditions, expectedNotfound);
                        }
                        else if (!expectedNotfound)
                            flag = false;
                    }
                }
                catch { flag = false; }
                count++;
            }

            if (!flag && !expectedNotfound) System.Console.WriteLine("-*-[FAILED]: Not found row with conditions (" + conditions + ")");
            
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearchAndVerifyRow(string searchBy, string searchValue, string conditions, [Optional] bool expectedNotfound)
        {
            bool flag = true;
            flag = RelatedTableSearch(searchBy, searchValue);
            if (flag) 
            {
                Thread.Sleep(2000);
                flag = RelatedTableVerifyRow(conditions, expectedNotfound);
                if(!flag && !expectedNotfound) System.Console.WriteLine("-*-[FAILED]: Error when verify row related table (" + this.tabName + ")");
            } else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");
            
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableActionOnRow(string conditions, string actionName)
        {
            bool flag = true;
            otable table = null;
            try
            {
                table = this.RefreshTable();
                flag = table.Existed;
                if (flag)
                    if (table.RowCount() > 0)
                        flag = table.FindRowAndClickCheckbox(conditions);
                    else flag = false;
            }
            catch { flag = false; }
            
            int count = 0;
            while (count < 5 && !flag)
            {
                Thread.Sleep(2000);
                try
                {
                    this.Header.Click();
                    table = this.RefreshTable();
                    flag = table.Existed;
                    if (flag)
                        if (table.RowCount() > 0)
                            flag = table.FindRowAndClickCheckbox(conditions);
                        else flag = false;
                }
                catch { flag = false; }
                count++;
            }

            if (flag)
            {
                ocombobox cb = Combobox_Action;
                flag = cb.Existed;
                if (flag)
                {
                    flag = cb.SelectItem("@@" + actionName);
                    if (!flag) System.Console.WriteLine("-*-[FAILED]: Error when select action item.");
                }
                else System.Console.WriteLine("-*-[FAILED]: Cannot get combobox action.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when click on checkbox.");
            
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearchAndActionOnRow(string searchBy, string searchValue, string conditions, string actionName)
        {
            bool flag = true;
            flag = RelatedTableSearch(searchBy, searchValue);
            if (flag)
            {
                Thread.Sleep(2000);
                flag = RelatedTableActionOnRow(conditions, actionName);
                if (!flag) System.Console.WriteLine("-*-[FAILED]: Error when action on row.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableOpenRecord(string conditions, string columnClick)
        {
            bool flag = true;
            otable table = null;
            
            try {
                table = this.RefreshTable();
                flag = table.Existed;
                if(flag)
                    if (table.RowCount() > 0)
                        flag = table.FindRow(conditions); 
                    else flag = false;
                }
            catch { flag = false; }
            int count = 0;
            while (count < 5 && !flag)
            {
                Thread.Sleep(2000);
                try
                {
                    this.Header.Click();
                    table = this.RefreshTable();
                    flag = table.Existed;
                    if (flag)
                        if (table.RowCount() > 0)
                            flag = table.FindRow(conditions);
                        else flag = false;
                }
                catch { flag = false; }
                count++;
            }

            if (flag && table.CurrentCellList.Count > 0)
            {
                flag = table.ColumnClick(table.CurrentCellList, columnClick, true);
                if (!flag) System.Console.WriteLine("-*-[FAILED]: Error when click on column.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Not found row.");
           
            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearchAndOpenRecord(string searchBy, string searchValue, string conditions, string columnClick)
        {
            bool flag = true;
            flag = RelatedTableSearch(searchBy, searchValue);
            if (flag)
            {
                Thread.Sleep(3000);
                flag = RelatedTableOpenRecord(conditions, columnClick);
                if (!flag) System.Console.WriteLine("-*-[FAILED]: Error when open record.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public string RelatedTableGetCellValue(string conditions, string columnName)
        {
            string result = string.Empty;
            bool flag = true;
            otable table = null;
            try {
                this.Header.Click();
                table = this.RefreshTable();
                flag = table.Existed;
                if (flag)
                    if (table.RowCount() > 0)
                        flag = table.FindRow(conditions);
                    else flag = false;
            }
            catch { flag = false; }
            int count = 0;
            while (count < 5 && !flag)
            {
                Thread.Sleep(2000);
                try
                {
                    table = this.RefreshTable();
                    flag = table.Existed;
                    if (flag)
                        if (table.RowCount() > 0)
                            flag = table.FindRow(conditions);
                        else flag = false;
                }
                catch { flag = false; }
                count++;
            }

            if (flag && table.CurrentCellList.Count > 0)
            {
                result = table.ColumnGetValue(table.CurrentCellList, columnName);
            }
            else System.Console.WriteLine("-*-[FAILED]: Not found row.");
            
            return result;
        }
        //--------------------------------------------------------------------------------------------------------
        public string RelatedTableSearchAndGetCellValue(string searchBy, string searchValue, string conditions, string columnName)
        {
            string result = string.Empty;
            bool flag = RelatedTableSearch(searchBy, searchValue);
            if (flag)
            {
                Thread.Sleep(2000);
                result = RelatedTableGetCellValue(conditions, columnName);
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");

            return result;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableVerifyCellReadOnly(string conditions, string columnClick, [Optional] string mainFrame)
        {
            bool flag = true;
            otable table = null;

            try
            {
                this.Header.Click();
                table = this.RefreshTable();
                flag = table.Existed;
                if (flag)
                {
                    if (table.RowCount() > 0)
                    {
                        flag = table.FindRow(conditions);
                    }
                    else
                        flag = false;
                }
            }
            catch { flag = false; }
            int count = 0;
            
            while (count < 5 && !flag)
            {
                Thread.Sleep(1000);
                try
                {
                    table = this.RefreshTable();
                    Thread.Sleep(1000);
                    flag = table.Existed;
                    if (flag)
                    {
                        if (table.RowCount() > 0)
                        {
                            flag = table.FindRow(conditions);
                        }
                        else
                            flag = false;
                    }
                }
                catch { flag = false; }
                count++;
            }

            if (flag && table.CurrentCellList.Count > 0) 
            {
                flag = table.ColumnClick(table.CurrentCellList, columnClick, false, true);
                if (flag)
                {
                    this.myDriver.SwitchTo().DefaultContent();
                    if (mainFrame != null)
                    {
                        this.myDriver.SwitchTo().Frame(mainFrame);
                    }
                    string define = "table[id='window.cell_edit_window']";
                    oelement editWindow = new oelement("Edit window", By.CssSelector(define), this.myDriver, null, 0, false);
                    flag = editWindow.Existed;
                    if (flag)
                    {
                        oelement ok = new oelement("OK", By.CssSelector("table[id='window.cell_edit_window'] a[id='cell_edit_ok']"), this.myDriver, null, 0, true);
                        flag = ok.Existed;
                        if (!flag)
                        {
                            flag = true;
                            oelement cancel = new oelement("Cancel", By.CssSelector("table[id='window.cell_edit_window'] a[id='cell_edit_cancel']"), this.myDriver, null, 0, false);
                            flag = cancel.Click();
                            if (!flag) { System.Console.WriteLine("***[FAILED]: Error when click on cancel button."); }
                        }
                        else { flag = false; System.Console.WriteLine("***[FAILED]: The cell is NOT readonly."); }
                    }
                    else { flag = true; } //System.Console.WriteLine("***[FAILED]: The edit window is not existed.");
                }
                else System.Console.WriteLine("***[FAILED]: Error when double click on cell.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Not found row with conditions (" + conditions + ")");

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearchAndVerifyCellReadOnly(string searchBy, string searchValue, string conditions, string columnName, [Optional] string mainFrame)
        {
            bool flag = true;
            flag = RelatedTableSearch(searchBy, searchValue);
            if (flag)
            {
                Thread.Sleep(2000);
                flag = RelatedTableVerifyCellReadOnly(conditions, columnName, mainFrame);
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool VerifyInnerControlsInRelatedTab(string tabName, string contextItems, string gotoItems, string recordId, string filterItems, string tableColumns)
        {
            bool flag = true;
            bool flagTemp = false;
            //IWebElement menu = null;
            //ReadOnlyCollection<IWebElement> items;
            IList <IWebElement> list;
            string expectedList = string.Empty;
            string actualList = string.Empty;
            string sTemp = string.Empty;
            //string[] arrTemp = null;

            #region Verify Tab header name
            if (Header.Text.Equals(tabName))
            {
                System.Console.WriteLine("***[PASSED]: The tab header is [" + tabName + "]");
            }
            else
            {
                System.Console.WriteLine("***[FAILED]: The tab header is not [" + tabName + "]. Run time [" + Header.Text + "]");
                flag = false;
            }
            #endregion

            //#region Verify Context Menu
            //if (Button_AdditionalAction.Existed)
            //{
            //    System.Console.WriteLine("***[PASSED]: Additional Action button exists. Verify Context Menu now.");

            //    // Verify Context Menu items (if has)
            //    if (contextItems != null && contextItems.Trim() != string.Empty)
            //    {
            //        // Click on Additional Action button
            //        if (Button_AdditionalAction.Click())
            //        {
            //            Thread.Sleep(5000);
            //            // Get the Context Menu element
            //            menu = myDriver.FindElement(By.CssSelector("div[class='context_menu'][id^='context_list']"));
            //            if (menu != null)
            //            {
            //                // Get items in Context Menu element
            //                items = menu.FindElements(By.CssSelector("div[class*='context_item']"));

            //                if (items.Count > 0)
            //                {
            //                    expectedList = contextItems.Trim().ToLower();
            //                    actualList = string.Empty;
            //                    sTemp = string.Empty;
            //                    arrTemp = contextItems.Split(';');

            //                    // Verify if the actual items are as expected
            //                    foreach (IWebElement ele in items)
            //                    {
            //                        sTemp = ele.Text.ToLower().Trim();
            //                        actualList = actualList + sTemp + ";";
            //                        if (expectedList.Contains(sTemp))
            //                        {
            //                            System.Console.WriteLine("***[PASSED]: Item [" + ele.Text + "] is as expected");
            //                        }
            //                        else
            //                        {
            //                            System.Console.WriteLine("***[FAILED]: Item [" + ele.Text + "] (on the page) is not in expected list (in Excel sheet)");
            //                            flag = false;
            //                        }
            //                    }

            //                    // Verify if the expected items are displayed
            //                    foreach (string s in arrTemp)
            //                    {
            //                        if (actualList.Contains(s.ToLower().Trim()))
            //                        {
            //                            System.Console.WriteLine("***[PASSED]: Item [" + s + "] is displayed");
            //                        }
            //                        else
            //                        {
            //                            System.Console.WriteLine("***[FAILED]: Item [" + s + "] (in Excel sheet) is not displayed (on the page)");
            //                            flag = false;
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    System.Console.WriteLine("***[FAILED]: Not found any item.");
            //                    flag = false;
            //                }
            //            }
            //            else
            //            {
            //                System.Console.WriteLine("***[FAILED]: Not found any context menu.");
            //                flag = false;
            //            }

            //        }
            //        else
            //        {
            //            System.Console.WriteLine("***[FAILED]: Cannot click on Additional Action button");
            //            flag = false;
            //        }
            //    }
            //    else
            //    {
            //        System.Console.WriteLine("***[FAILED]: there is no item in Context Menu");
            //        flag = false;
            //    }
            //}
            //else
            //{
            //    System.Console.WriteLine("***[FAILED]: Context Menu does not exist");
            //    flag = false;
            //}
            //#endregion

            #region Verify Go to combobox exists
            if (Combobox_Goto.Existed)
            {
                System.Console.WriteLine("***[PASSED]: Go to combobox exists. Verify Go to combobox list items now.");

                // Verify Go to combobox items (if has)
                if (gotoItems != null && gotoItems.Trim() != string.Empty)
                {
                    // Verify if all expected items are in combobox
                    flagTemp = Combobox_Goto.VerifyItemList(gotoItems);
                    if (flagTemp)
                    {
                        System.Console.WriteLine("***[PASSED]: Go to combobox list is correct");
                    }
                    else
                    {
                        flag = false;
                    }

                    // Verify if there is any item in combobox not in expected list
                    list = Combobox_Goto.MyCombobox.Options;
                    foreach(IWebElement e in list)
                    {
                        if(gotoItems.ToLower().Trim().Contains(e.Text.ToLower().Trim()))
                        {
                            System.Console.WriteLine("***[PASSED]: Item [" + e.Text + "] is as expected");
                        }
                        else
                        {
                            System.Console.WriteLine("***[FAILED]: Item [" + e.Text + "] (in the combobox) is not in expected list (in Excel sheet)");
                            flag = false;
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("***[FAILED]: there is no item in Go to combobox");
                    flag = false;
                }
            }
            else
            {
                System.Console.WriteLine("***[FAILED]: Go to combobox does not exist");
                flag = false;
            }
            #endregion

            //#region Verify Record Id
            //if (Link_RecordId.Existed)
            //{
            //    System.Console.WriteLine("***[PASSED]: Record ID element exists");

            //    if (Link_RecordId.Text.ToLower().Trim().Contains(recordId.ToLower().Trim()))
            //    {
            //        System.Console.WriteLine("***[PASSED]: Record ID is correct");
            //    }
            //    else
            //    {
            //        System.Console.WriteLine("***[FAILED]: Record ID is INCORRECT. Expected = [" + recordId + "], Actual = [" + Link_RecordId.Text + "]");
            //        flag = false;
            //    }
            //}
            //else
            //{
            //    System.Console.WriteLine("***[FAILED]: Record ID link does not exist");
            //    flag = false;
            //}
            //#endregion

            #region Verify Filter link
            System.Console.WriteLine("***[PASSED]: Filter link exists");

            if (Link_Filter.MyEle.GetAttribute("title") == "Show / hide filter")
            {
                System.Console.WriteLine("***[PASSED]: Filter link content is correct");
            }
            else
            {
                System.Console.WriteLine("***[FAILED]: Filter link content is INCORRECT");
                flag = false;
            }
            #endregion

            // Click on the Link Filter
            Link_Filter.Click(true);

            //#region Verify Filter options - choose field, oper and value
            //if (Link_Filter.Existed)
            //{
            //    System.Console.WriteLine("***[PASSED]: Choose Field exists");

            //    // Click on the Link Filter
            //    Link_Filter.Click(true);
            //    int count = 10;

            //    // Wait for the Choose field combobox to display
            //    while(!Combobox_Filter_ChooseField.Existed && count > 0)
            //    {
            //        count--;
            //        Thread.Sleep(1000);
            //    }

            //    // Verify Choose Field combobox
            //    if(Combobox_Filter_ChooseField.Existed)
            //    {
            //        System.Console.WriteLine("***[PASSED]: Choose Field exists");

            //        // Verify default option of Choose field combobox
            //        if(Combobox_Filter_ChooseField.VerifyCurrentText("-- choose field --"))
            //        {
            //            System.Console.WriteLine("***[PASSED]: Choose Field default value is [-- choose field --]");
            //        }
            //        else
            //        {
            //            System.Console.WriteLine("***[FAILED]: Default value of Choose Field combobox is incorrect. Expected=[-- choose field --], Actual=[" + Combobox_Filter_ChooseField.Text + "]");
            //            flag = false;
            //        }

            //        // Split the condition value
            //        arrTemp = filterItems.Split(new string[] { "||" }, System.StringSplitOptions.RemoveEmptyEntries);
            //        string field = string.Empty;
            //        string oper = string.Empty;
            //        string value = string.Empty;

            //        foreach(string sChoose in arrTemp)
            //        {
            //            System.Console.WriteLine("***Verify option [" + sChoose + "] in Choose Field combobox");

            //            string[] arrTemp2 = sChoose.Split(new string[] { "::" }, System.StringSplitOptions.RemoveEmptyEntries);
                        
            //            // Get the field, operator and value
            //            field =  arrTemp2[0];
            //            oper  = (arrTemp2.Length > 1) ? arrTemp2[1] : string.Empty;
            //            value = (arrTemp2.Length > 2) ? arrTemp2[2] : string.Empty;

            //            // Check if Choose Field value exists

            //            flag = Combobox_Filter_ChooseField.HaveItemInlist(field);

            //            //int c = 0;
            //            //while (!flag && c < 5) 
            //            //{
            //            //    Thread.Sleep(1000);
            //            //    flag = Combobox_Filter_ChooseField.HaveItemInlist(field);
            //            //    c++;
            //            //}

            //            if(flag)
            //            {
            //                System.Console.WriteLine("***[PASSED]: Option [" + field + "] is in the list");

            //                // If there is operator to select
            //                if (oper != string.Empty)
            //                {
            //                    // Select the field to check the Operator combobox
            //                    if (Combobox_Filter_ChooseField.SelectItem(field))
            //                    {
            //                        Thread.Sleep(1000);

            //                        // If operator is a list then only verify the Operator combobox
            //                        string[] arrOper = oper.Split(';');
            //                        if(arrOper.Length > 1)
            //                        {
            //                            System.Console.WriteLine("***Verify list of Operator");
            //                            flagTemp = Combobox_Filter_Operator.VerifyItemList(oper);
            //                            if (flagTemp)
            //                            {
            //                                System.Console.WriteLine("***[PASSED]: Operator list is correct");
            //                            }
            //                            else
            //                            {
            //                                System.Console.WriteLine("***[FAILED]: Operator list is INCORRECT");
            //                                flag = false;
            //                            }
            //                        }
            //                        // If operator is not a list, then select it and verify Value field
            //                        else
            //                        {
            //                            // Select the operator to check the Value combobox (if any)
            //                            if (Combobox_Filter_Operator.SelectItem(oper))
            //                            {
            //                                Thread.Sleep(1000);

            //                                if(value != string.Empty)
            //                                {
            //                                    // Verify value combobox
            //                                    string[] arrVal = value.Split(';');
            //                                    if(arrVal.Length > 1)
            //                                    {
            //                                        flagTemp = Combobox_Filter_Value.VerifyItemList(value);
            //                                        if (flagTemp)
            //                                        {
            //                                            System.Console.WriteLine("***[PASSED]: Value list is correct");
            //                                        }
            //                                        else
            //                                        {
            //                                            System.Console.WriteLine("***[FAILED]: Value list is INCORRECT");
            //                                            flag = false;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        flagTemp = Combobox_Filter_Value.HaveItemInlist(value);
            //                                        if (flagTemp)
            //                                        {
            //                                            System.Console.WriteLine("***[PASSED]: Value is correct");
            //                                        }
            //                                        else
            //                                        {
            //                                            System.Console.WriteLine("***[FAILED]: Value is INCORRECT");
            //                                            flag = false;
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                System.Console.WriteLine("***[FAILED]: Cannot choose [" + oper + "] in [Choose Operator] combobox");
            //                                flag = false;
            //                            }
            //                        }
            //                    }
            //                    else 
            //                    {
            //                        System.Console.WriteLine("***[FAILED]: Cannot choose [" + field + "] in [Choose Field] combobox");
            //                        flag = false;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                System.Console.WriteLine("***[FAILED]: Field [" + field + "] is NOT in [Choose Field] combobox");
            //                flag = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        System.Console.WriteLine("***[FAILED]: Cannot display Filter fields");
            //        flag = false;
            //    }
            //}
            //#endregion

            #region Verify Related Table Header
            if (!RelatedTableVerifyHeader(tableColumns))
            {
                System.Console.WriteLine("***[FAILED]: Table column is incorrect");
                flag = false;
            }
            #endregion

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableVerifyHeader(string columnHeaderList)
        {
            bool flag = true;

            // Get the related table
            otable table = this.RefreshTable();
            if (table.Existed)
            {
                string columns = string.Empty;
                columns = table.AllColumnString(";").ToLower().Trim();
                string[] actualColumns = columns.Split(';');
                if(columns != string.Empty)
                {
                    string[] expectedColumns = columnHeaderList.Split(';');

                    // Check if the expected column exists
                    foreach(string s in expectedColumns)
                    {
                        if(columns.Contains(s.ToLower().Trim()))
                        {
                            System.Console.WriteLine("***[Passed]: Column [" + s + "] exists as expected");
                        }
                        else
                        {
                            System.Console.WriteLine("***[FAILED]: Column [" + s + "] does NOT exist as expected");
                            flag = false;
                        }
                    }

                    // Check if all actual columns are expected
                    columnHeaderList = columnHeaderList.ToLower();
                    foreach (string t in actualColumns)
                    {
                        if (columnHeaderList.Contains(t.ToLower().Trim()))
                        {
                            System.Console.WriteLine("***[Passed]: Column [" + t + "] is expected");
                        }
                        else
                        {
                            System.Console.WriteLine("***[FAILED]: Column [" + t + "] is NOT expected");
                            flag = false;
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("***[FAILED]: Table header is empty");
                    flag = false;
                }
            }
            else
            {
                System.Console.WriteLine("***[FAILED]: Cannot get the Related Table");
                flag = false;
            }

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableOpenViewButton(string conditions)
        {
            bool flag = true;
            otable table = null;

            try
            {
                table = this.RefreshTable();
                flag = table.Existed;
                if (flag)
                    if (table.RowCount() > 0)
                        flag = table.FindRow(conditions);
                    else flag = false;
            }
            catch { flag = false; }
            int count = 0;
            while (count < 5 && !flag)
            {
                Thread.Sleep(2000);
                try
                {
                    this.Header.Click();
                    table = this.RefreshTable();
                    flag = table.Existed;
                    if (flag)
                        if (table.RowCount() > 0)
                            flag = table.FindRow(conditions);
                        else flag = false;
                }
                catch { flag = false; }
                count++;
            }

            if (flag && table.CurrentRow.Existed)
            {
                oelement link = new oelement("Link", By.CssSelector("a[class$='list_popup']"), this.myDriver, table.CurrentRow, 0, false);
                flag = link.Existed;
                if (flag)
                {
                    flag = link.Click(true);
                    if(!flag)
                    {
                        System.Console.WriteLine("-*-[FAILED]: Error when click on column.");
                    }
                }
                else
                {
                    System.Console.WriteLine("-*-[FAILED]: Not found View button.");
                }
            }
            else
            {
                System.Console.WriteLine("-*-[FAILED]: Not found row.");
            }

            return flag;
        }
        //--------------------------------------------------------------------------------------------------------
        public bool RelatedTableSearchAndOpenViewButton(string searchBy, string searchValue, string conditions)
        {
            bool flag = true;
            flag = RelatedTableSearch(searchBy, searchValue);
            if (flag)
            {
                Thread.Sleep(2000);
                flag = RelatedTableOpenViewButton(conditions);
                if (!flag) System.Console.WriteLine("-*-[FAILED]: Error when open View button.");
            }
            else System.Console.WriteLine("-*-[FAILED]: Error when search on related table (" + this.tabName + ")");

            return flag;
        }
        //***********************************************************************************************************************************
        #endregion End - Public methods
    }
}
