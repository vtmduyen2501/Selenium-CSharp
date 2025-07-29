using NUnit.Framework;
using System;
using System.Reflection;

namespace Incident
{
    [TestFixture("001")]
    [TestFixture("002")]
    [TestFixture("003")]
    [TestFixture("004")]
    [TestFixture("005")]
    [TestFixture("006")]
    [TestFixture("007")]
    [TestFixture("008")]
    [TestFixture("009")]
    [TestFixture("010")]
    [TestFixture("011")]
    [TestFixture("012")]
    [TestFixture("013")]
    [TestFixture("014")]
    [TestFixture("015")]
    [TestFixture("016")]
    [TestFixture("017")]
    [TestFixture("018")]
    [TestFixture("019")]
    [TestFixture("020")]
    [TestFixture("021")]
    [TestFixture("022")]
    [TestFixture("023")]
    [TestFixture("024")]
    [TestFixture("025")]
    [TestFixture("026")]
    [TestFixture("027")]
    [TestFixture("028")]
    [TestFixture("029")]
    [TestFixture("030")]
    [TestFixture("031")]
    [TestFixture("032")]
    [TestFixture("033")]
    [TestFixture("034")]
    [TestFixture("035")]
    [TestFixture("036")]
    [TestFixture("037")]
    [TestFixture("038")]
    [TestFixture("039")]
    [TestFixture("040")]
    [TestFixture("041")]
    [TestFixture("042")]
    [TestFixture("043")]
    [TestFixture("044")]
    [TestFixture("045")]
    [TestFixture("046")]
    [TestFixture("047")]
    [TestFixture("048")]
    [TestFixture("049")]
    [TestFixture("050")]
    [TestFixture("051")]
    [TestFixture("052")]
    [TestFixture("053")]
    [TestFixture("054")]
    [TestFixture("055")]
    [TestFixture("056")]
    [TestFixture("057")]
    [TestFixture("058")]
    [TestFixture("059")]
    [TestFixture("060")]
    [TestFixture("061")]
    [TestFixture("062")]
    [TestFixture("063")]
    [TestFixture("064")]
    [TestFixture("065")]
    [TestFixture("066")]
    [TestFixture("067")]
    [TestFixture("068")]
    [TestFixture("069")]
    [TestFixture("070")]
    [TestFixture("071")]
    [TestFixture("072")]
    [TestFixture("073")]
    [TestFixture("074")]
    [TestFixture("075")]
    [TestFixture("076")]
    [TestFixture("077")]
    [TestFixture("078")]
    [TestFixture("079")]
    [TestFixture("080")]
    [TestFixture("081")]
    [TestFixture("082")]
    [TestFixture("083")]
    [TestFixture("084")]
    [TestFixture("085")]
    [TestFixture("086")]
    [TestFixture("087")]
    [TestFixture("088")]
    [TestFixture("089")]
    [TestFixture("090")]
    [TestFixture("091")]
    [TestFixture("092")]
    [TestFixture("093")]
    [TestFixture("094")]
    [TestFixture("095")]
    [TestFixture("096")]
    [TestFixture("097")]
    [TestFixture("098")]
    [TestFixture("099")]
    [TestFixture("100")]
    [TestFixture("101")]
    [TestFixture("102")]
    [TestFixture("103")]
    [TestFixture("104")]
    [TestFixture("105")]
    [TestFixture("106")]
    [TestFixture("107")]
    [TestFixture("108")]
    [TestFixture("109")]
    [TestFixture("110")]
    [TestFixture("111")]
    [TestFixture("112")]
    [TestFixture("113")]
    [TestFixture("114")]
    [TestFixture("115")]
    [TestFixture("116")]
    [TestFixture("117")]
    [TestFixture("118")]
    [TestFixture("119")]
    [TestFixture("120")]
    [TestFixture("121")]
    [TestFixture("122")]
    [TestFixture("123")]
    [TestFixture("124")]
    [TestFixture("125")]
    [TestFixture("126")]
    [TestFixture("127")]
    [TestFixture("128")]
    [TestFixture("129")]
    [TestFixture("130")]
    [TestFixture("131")]
    [TestFixture("132")]
    [TestFixture("133")]
    [TestFixture("134")]
    [TestFixture("135")]
    [TestFixture("136")]
    [TestFixture("137")]
    [TestFixture("138")]
    [TestFixture("139")]
    [TestFixture("140")]
    [TestFixture("141")]
    [TestFixture("142")]
    [TestFixture("143")]
    [TestFixture("144")]
    [TestFixture("145")]
    [TestFixture("146")]
    [TestFixture("147")]
    [TestFixture("148")]
    [TestFixture("149")]
    [TestFixture("150")]
    [TestFixture("151")]
    [TestFixture("152")]
    [TestFixture("153")]
    [TestFixture("154")]
    

    public class INC_Category_b_main
    {
        #region Define default variables for test case (No need to update)
        //***********************************************************************************************************************************
        public bool flagC;
        public bool flag, flagExit, flagW;
        string caseName, error;
        Auto.obase Base;

        //***********************************************************************************************************************************
        #endregion End - Define default variables for test case (No need to update)

        #region Setup test case, set up and tear down test steps (No need to update)
        //***********************************************************************************************************************************
        
        string sRow = string.Empty;
        public INC_Category_b_main(string _Row)
        {
            if (_Row != string.Empty)
                this.sRow = (int.Parse(_Row)).ToString();
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            caseName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            Base = new Auto.obase();
            Base.BeforeRunTestCase(caseName, ref Base, ref flagExit, ref flagW, ref flag, ref flagC, sRow);
        }
        //-------------------------------------------------------------------------------------------------
        [SetUp]
        public void RunBeforeAnyTests()
        {
            System.Console.WriteLine("*|||*[Run step:" + TestContext.CurrentContext.Test.Name + "]");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Base.BeforeRunTestStep(ref flag, ref flagExit, ref error);
        }
        //-------------------------------------------------------------------------------------------------
        [TearDown]
        public void RunAfterAnyTests()
        {
            Base.AfterRunTestStep(flag, ref flagExit, ref flagW, ref flagC, error);
        }
        //***********************************************************************************************************************************
        #endregion End - Setup test case, set up and tear down test steps (No need to update)

        #region Tear down test case (NEED TO UPDATE: write result)
        //***********************************************************************************************************************************
        [TestFixtureTearDown()]
        public void TearDown()
        {
            Base.AfterRunTestCase(flagC, caseName);

            System.Console.WriteLine("Finished - Incident Id: " + incidentId);
            
            //----------------------------------------------------------------

            string temp = Base.GData("Debug").ToLower();

            if (Base.Driver != null && temp != "yes")
            {
                Base.Driver.Close();
                Base.Driver.Quit();
            }
        }
        //***********************************************************************************************************************************
        #endregion End - Tear down test case (NEED TO UPDATE: write result)

        #region Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)
        //***********************************************************************************************************************************

        Auto.otextbox textbox;
        Auto.olookup lookup;
        Auto.ocombobox combobox;
        Auto.otextarea textarea;
        //------------------------------------------------------------------
        Auto.Login login = null;
        Auto.Home home = null;
        Auto.Incident inc = null;
        //------------------------------------------------------------------
        string incidentId;
        
        //***********************************************************************************************************************************
        #endregion End - Define variables and objects (class) are used in test cases (NEED TO UPDATE: This case variables)

        #region Scenario of test case (NEED TO UPDATE)
        //***********************************************************************************************************************************

        [Test]
        public void ClassInit()
        {
            try
            {
                //------------------------------------------------------------------
                login = new Auto.Login(Base);
                home = new Auto.Home(Base);
                inc = new Auto.Incident(Base, "Incident");
                //------------------------------------------------------------------
                incidentId = string.Empty;

            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_001_OpenNewIncident()
        {
            try
            {
                flag = home.LeftMenuItemSelect("Incident", "Create New");
                if (flag)
                    inc.WaitLoading();
                else
                    error = "Error when create new incident.";
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_002_PopulateCallerName()
        {
            try
            {
                textbox = inc.Textbox_Number;
                flag = textbox.Existed;
                if (flag)
                {
                    textbox.Click();
                    //-- Store incident id
                    incidentId = textbox.Text;
                    Console.WriteLine("-*-[Store]: Incident Id:(" + incidentId + ")");
                    string temp = Base.GData("Caller");
                    lookup = inc.Lookup_Caller;
                    flag = lookup.Existed;
                    if (flag)
                    {
                        flag = lookup.Select(temp);
                        if (!flag) { error = "Cannot populate caller value."; }
                    }
                    else { error = "Cannot get lookup caller."; }
                }
                else
                {
                    error = "Cannot get texbox number.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_003_PopulateCategory()
        {
            try
            {
                string temp = Base.GData("Category");
                combobox = inc.Combobox_Category;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = combobox.SelectItem(temp);
                    if (flag)
                    {
                        inc.WaitLoading();
                    }
                    else { error = "Cannot populate category value."; }
                }
                else
                {
                    error = "Cannot get combobox category.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_004_PopulateSubCategory()
        {
            try
            {
                string temp = Base.GData("Subcategory");
                combobox = inc.Combobox_SubCategory;
                flag = combobox.Existed;
                if (flag)
                {
                    flag = inc.VerifyHaveItemInComboboxList("subcategory", temp);
                    if (flag)
                    {
                        flag = combobox.SelectItem(temp);
                        if (flag)
                        {
                            inc.WaitLoading();
                        }
                        else { error = "Cannot populate sub category value."; }
                    }
                    else error = "Not found item (" + temp + ") in sub category list.";
                }
                else
                {
                    error = "Cannot get combobox sub category.";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_01_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textbox = inc.Textbox_ShortDescription;
                flag = textbox.Existed;
                if (flag)
                {
                    flag = textbox.SetText(temp);
                    if (!flag) { error = "Cannot populate short description value."; }
                }
                else { error = "Cannot get textbox short description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_005_02_PopulateShortDescription()
        {
            try
            {
                string temp = Base.GData("ShortDescription");
                textarea = inc.Textarea_Description;
                flag = textarea.Existed;
                if (flag)
                {
                    flag = textarea.SetText(temp);
                    if (!flag) { error = "Cannot populate description value."; }
                }
                else { error = "Cannot get textarea description."; }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [Test]
        public void Step_006_SaveIncident()
        {
            try
            {
                flag = inc.Save();
                if (!flag) { error = "Error when save incident."; }
                else { inc.WaitLoading(); }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Test]
        public void Step_007_Validate_Auto_AssignmentGroup()
        {
            try
            {
                lookup = inc.Lookup_AssignmentGroup;
                string temp = Base.GData("Auto_AssignmentGroup");
                if (!lookup.Existed || lookup.Text != temp)
                {
                    error = "Incorrect auto assignment group. Runtime: [" + lookup.Text + "]. Expected: [" + temp + "]";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                error = ex.Message;
            }
        }
        //-------------------------------------------------------------------------------------------------
        #endregion End - Scenario of test case (NEED TO UPDATE)
    }
}
