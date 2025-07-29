using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Threading;

namespace Auto
{
    public class otable : oelement
    {
        private string myName;
        private IWebDriver myDriver;
        private oelementlist currentCelllist;
        private oelement currentRow;
        private string colDefine = "thead>tr[id^='hdr'] i[class*='col-menu'], thead>tr[role='row']>th";
        private string rowDefine = "tbody>tr[id^='row'], tbody>tr[role='row']";
        //private string cellDefine = "td[class='vt']";
        private string cellDefine = "td:not([class^='list_decoration_cell'])";
        private string checkboxDefine = "label[class='checkbox-label']";
        public int findRowIndex = -1;


        public otable(string name, By by, IWebDriver driver, oelement parent, int index, bool noWait) 
            : base(name, by, driver, parent, index, noWait)
        {
            this.myDriver = driver;
            this.myName = name;
        }
        public otable(IWebDriver driver, IWebElement ele, string name)
            : base(driver, ele)
        {
            this.myDriver = driver;
            this.myName = name;
        }

        #region Properties
        //***********************************************************************************************************************************

        public string Name 
        {
            get { return this.myName; }
        }

        public oelementlist CurrentCellList
        {
            get { return this.currentCelllist; }
        }

        public oelement CurrentRow
        {
            get { return this.currentRow; }
        }
        
        //***********************************************************************************************************************************
        #endregion End - Properties

        #region Public methods

        public int ColumnCount()
        {
            return GColumns().Count;
        }

        public int RowCount([Optional] bool noWait, [Optional] string rowD)
        {
            bool flag = false;
            int count = 0;
            ReadOnlyCollection<IWebElement> rows = null;
            if (rowD != null && rowD != string.Empty)
                rowDefine = rowD;
            try
            {
                while (!flag && count < 5) 
                {
                    rows = this.MyEle.FindElements(By.CssSelector(rowDefine));
                    flag = true;
                    count = 5;
                }
            }
            catch { 
                flag = false; 
                count++;
                Thread.Sleep(1000);
            }
            
            return rows.Count;
        }

        public string AllColumnString(string split, [Optional] string colD, [Optional] string rowD )
        {
            //string define = "thead>tr[id^='hdr'] a[class^='column_head']";
            string define = colDefine;
            if (colD != null && colD != string.Empty)
                define = colD;
            return GColumns(define).GetAllItems(split);
        }

        public bool FindRow(string conditions, [Optional] bool expectedNotfound, [Optional] string colD, [Optional] string rowD, [Optional] string cellD, [Optional] bool getNoDisplayed) 
        {
            bool flag = true;

            findRowIndex = FindRowIndex(conditions, colD, rowD, cellD, getNoDisplayed);

            if (findRowIndex < 0) 
            {
                if (!expectedNotfound) 
                {
                    flag = false;
                    System.Console.WriteLine("***[FindRow]: Not found with conditions:(" + conditions + ")");
                }
            }
            else 
            {
                if (!expectedNotfound)
                    System.Console.WriteLine("***[OK]: Found row with conditions:(" + conditions + ")");
                else 
                {
                    flag = false;
                    System.Console.WriteLine("***[FindRow]: Found row with conditions:(" + conditions + ")");
                }
            }
            return flag;
        }

        public bool FindRowAndClickCheckbox(string conditions)
        {
            bool flag = true;
            int index = FindRowIndex(conditions);
            if (index >= 0)
            {
                System.Console.WriteLine("***[OK]: Found row with conditions:(" + conditions + ")");
                oelementlist list = GCheckbox();
                if (list.Count - 1 >= index)
                {
                    ocheckbox checkbox = new ocheckbox(this.myDriver, list.MyList[index].MyEle, "Checkbox");
                    flag = checkbox.Existed;
                    if (flag)
                    {
                        if (!checkbox.Checked)
                        {
                            flag = checkbox.Click(true);
                            if (!flag) System.Console.WriteLine("***[ERROR]: Error when click on checkbox.");
                        }
                    }
                    else System.Console.WriteLine("***[ERROR]: Cannot get checkbox.");
                }
                else 
                {
                    flag = false;
                }
            }
            else 
            {
                flag = false;
                System.Console.WriteLine("***[ERROR]: Not found with conditions:(" + conditions + ")");
            }
            return flag;
        }

        public bool ColumnClick(oelementlist celllist, string colname, [Optional] bool javaClick, [Optional] bool doubleClick, [Optional] bool rightClick, [Optional] bool getNoDisplayed) 
        {
            bool flag = true;
            oelementlist collist = GColumns(null, getNoDisplayed);
            int colindex = GColumnIndex(collist, colname, getNoDisplayed);
            if (colindex >= 0)
            {
                oelement cell = celllist.MyList[colindex];
                if (cell.Existed)
                {
                    if (doubleClick) 
                    {
                        if (javaClick)
                        {
                            string doubleClickJS = "if(document.createEvent){var evObj = document.createEvent('MouseEvents');evObj.initEvent('dblclick',";
                            doubleClickJS = doubleClickJS + "true, false); arguments[0].dispatchEvent(evObj);} else if(document.createEventObject)";
                            doubleClickJS = doubleClickJS + "{ arguments[0].fireEvent('ondblclick');}";
                            IJavaScriptExecutor js = this.myDriver as IJavaScriptExecutor;
                            js.ExecuteScript(doubleClickJS, cell.MyEle);
                        } 
                        else 
                        {
                            Actions ac = new Actions(this.myDriver);
                            ac.MoveToElement(cell.MyEle, 1, 1);
                            ac.DoubleClick();
                            ac.Build().Perform();
                        }
                        
                    }
                    else if (rightClick) 
                    {
                        Actions ac = new Actions(this.myDriver);
                        ac.MoveToElement(cell.MyEle, 1, 1);
                        ac.ContextClick(cell.MyEle);
                        ac.Build().Perform();
                    }
                    else 
                    {
                        oelement link = new oelement("Link", By.CssSelector("a"), this.myDriver, cell, 0, false);
                        if (link.Existed)
                        {
                            flag = link.Click(javaClick);
                        }
                        else { flag = false; }
                    }
                }
                else 
                {
                    flag = false;
                }
            }
            else 
            { 
                flag = false;
                System.Console.WriteLine("***[ERROR]: Not found column:(" + colname + ")");
            }
            return flag;
        }

        public string ColumnGetValue(oelementlist celllist, string colname) 
        {
            string result = string.Empty;
            oelementlist collist = GColumns();
            int colindex = GColumnIndex(collist, colname);
            if (colindex >= 0)
            {
                oelement cell = celllist.MyList[colindex];
                if (cell.Existed) 
                {
                    result = cell.Text;
                }
            }
            return result;
        }

        public string ColumnGetColorValue(oelementlist celllist, string colname)
        {
            string result = string.Empty;
            oelementlist collist = GColumns();
            int colindex = GColumnIndex(collist, colname);
            if (colindex >= 0)
            {
                oelement cell = celllist.MyList[colindex];
                if (cell.Existed)
                {
                    result = cell.MyEle.FindElement(By.CssSelector("div")).GetAttribute("style");
                }
            }
            return result;
        }
        #endregion End - Public methods

        #region Private methods

        private int FindRowIndex(string conditions, [Optional] string colD, [Optional] string rowD, [Optional] string cellD, [Optional] bool getNoDisplayed)
        {
            int index = -1;
            oelementlist rowlist = GRows(false, rowD);
            oelementlist collist = GColumns(colD, getNoDisplayed);
            if (collist.MyList != null && collist.Count > 0)
            {
                if (rowlist.MyList != null && rowlist.Count > 0) 
                {
                    foreach (oelement row in rowlist.MyList)
                    {
                        oelementlist cellist = GCells(row, cellD);
                        bool flag = true;
                        if (cellist.Count > 0)
                        {
                            flag = VerifyRow(cellist, conditions, collist, getNoDisplayed);
                            if (flag)
                            {
                                index = rowlist.MyList.IndexOf(row);
                                this.currentCelllist = cellist;
                                this.currentRow = row;
                                break;
                            }
                        }
                    }
                } 
            }
            
            return index;
        }

        private oelementlist GColumns([Optional] string colD, [Optional] bool getNoDisplayed) 
        {
            if (colD != null && colD != string.Empty)
                colDefine = colD;
            return new oelementlist("Columh list", By.CssSelector(colDefine), this.myDriver, this, false, getNoDisplayed);
        }

        private oelementlist GRows([Optional] bool noWait, [Optional] string rowD)
        {
            if (rowD != null && rowD != string.Empty)
                rowDefine = rowD;
            return new oelementlist("Row list", By.CssSelector(rowDefine), this.myDriver, this, noWait);
        }

        private oelementlist GCheckbox() 
        {
            return new oelementlist("Checkbox list", By.CssSelector(checkboxDefine), this.myDriver, this, false);
        }

        private oelementlist GCells(oelement row, [Optional] string cellD) 
        {
            if (cellD != null && cellD != string.Empty)
                cellDefine = cellD;
            return new oelementlist("Cell list", By.CssSelector(cellDefine), this.myDriver, row, false); 
        }

        private int GColumnIndex(oelementlist cols, string colname, [Optional] bool getNoDisplayed)
        {
            int index = -1;
            foreach (oelement e in cols.MyList)
            {
                string temp = string.Empty;

                //if (getNoDisplayed)
                //{
                //    temp = e.MyEle.GetAttribute("textContent").ToLower();
                //}
                //else
                //    temp = e.Text.Trim().ToLower();

                //if (temp.Equals("column control menu") || temp == "")
                //{
                //    oelement parent = new oelement(this.myDriver, e.MyEle.FindElement(By.XPath("..")));
                //    temp = parent.MyEle.Text.Trim().ToLower();
                //    temp = temp.Replace("\r\n", "");
                //    temp = temp.Replace("column control menu", "");
                //    temp = temp.Replace("sort", "");
                //}
                //else 
                //{
                //    if (getNoDisplayed)
                //    {
                //        temp = e.MyEle.GetAttribute("textContent").ToLower();
                //    }
                //    else
                //        temp = e.Text.Trim().ToLower();
                //}

                try
                {
                    temp = e.MyEle.GetAttribute("aria-label").ToLower();
                }
                catch { temp = null; }
                
                if (temp == null || temp == string.Empty) 
                {
                    temp = e.Text.Trim().ToLower();
                }
                temp = temp.Replace("\r\n", "");
                temp = temp.Replace("sort in descending order", "");
                temp = temp.Replace("sort in ascending order", "");
                temp = temp.Replace(": activate to sort column ascending", "");
                temp = temp.Replace(": activate to sort column descending", "");
                temp = temp.Replace("column menu", "");
                temp = temp.Replace("column options", "");

                if (temp.Trim().Equals(colname.Trim().ToLower()))
                {
                    index = cols.MyList.IndexOf(e);
                    break;
                }
            }
            return index;
        }

        private bool VerifyRow(oelementlist cellist, string conditions, oelementlist collist, [Optional] bool getNoDisplayed)
        {
            bool flag = true;

            string[] arrCondition;

            if (conditions.Contains("|"))
            {
                arrCondition = conditions.Split('|');

                foreach (string condition in arrCondition)
                {
                    flag = Verify(cellist, condition, collist, getNoDisplayed);

                    if (!flag)
                    {
                        break;
                    }
                }
            }
            else
            {
                flag = Verify(cellist, conditions, collist, getNoDisplayed);
            }

            return flag;
        }

        private bool Verify(oelementlist cellist, string condition, oelementlist collist, [Optional] bool getNoDisplayed)
        {
            bool flag = false;
            string[] arr = condition.Split('=');
            string value = arr[1].Trim();
            int index = GColumnIndex(collist, arr[0].Trim(), getNoDisplayed);
            if (index >= 0) 
            {
                if (value.Contains("@@"))
                {
                    value = value.Replace("@@", "");
                    if (cellist.MyList[index].Text.ToLower().Trim().Contains(value.ToLower().Trim()))
                    {
                        cellist.MyList[index].Highlight();
                        flag = true;
                    }
                }
                else
                {
                    if (cellist.MyList[index].Text.ToLower().Trim().Equals(value.ToLower().Trim()))
                    {
                        cellist.MyList[index].Highlight();
                        flag = true;
                    }
                }               
            }

            return flag;
        }

        #endregion End - Private methods
    }
}
