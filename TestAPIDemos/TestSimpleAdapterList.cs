using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using TestAPIDemos.Tools;

namespace TestAPIDemos
{
    [TestFixture]
    class TestSimpleAdapterList : TestRunner
    {

        int countMainSimpleAdapterList = 20;
        int countChild = 15;
        string lastText = string.Empty;
        string[] descriptionChild = new[] { "This child is even", "This child is odd" };
        bool firstCheck = true;

        /// <summary>
        /// Go to Simple Adapter lists
        /// </summary>
        private void GoToSimpleAdapter()
        {
            SwipeTo("down");
            //
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Views")).Click();
            //
            driver.FindElement(By.Id("Expandable Lists")).Click();
            Thread.Sleep(1000);
            //
            driver.FindElement(By.Id("3. Simple Adapter")).Click();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Gets numbers from text.
        /// </summary>
        public string getNumber(string str)
        {
            string actual = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsDigit(str[i]))
                {
                    actual += str[i];
                }
            }
            return actual;
        }

        /// <summary>
        /// For test number of groups and now when end list.
        /// </summary>
        public void checkLastText(int countsList)
        {
            if (lastText == driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + countsList + "]")).Text)
            {
                firstCheck = false;
            }
            else
            {
                lastText = driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + countsList + "]")).Text;
            }
        }

        /// <summary>
        /// For change index in TextView after scroll that not test few the same.
        /// </summary>
        public void checkIndexGroups(int lastIndex)
        {
            string str = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).Text);
            while (Convert.ToInt32(str) != lastIndex)
            {
                index++;
                str = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).Text);
            }
        }
        
        /// <summary>
        /// For change index in TextView after scroll that not test few the same.
        /// </summary>
        public int checkIndexChild(int lastIndex, int indexChild)
        {
            string str = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[1]")).Text);
            while (Convert.ToInt32(str) != lastIndex && driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[1]")).Text.Contains("Child"))
            {
                indexChild++;
                str = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[1]")).Text);
            }
            return indexChild;
        }

        /// <summary>
        /// Tests all number of groups  ---- 20
        /// </summary>
        [Test]
        public void TestAllNumberGroups()
        {
            GoToSimpleAdapter();
            //
            int countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
            index = 1;
            int lastIndex = 0;
            string actual = string.Empty;
            //
            while (firstCheck)
            {
                checkIndexGroups(lastIndex);
                //
                for (int i = index - 1; i < countListOfLayout; i++)
                {
                    string str = driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).Text;
                    actual = getNumber(str);
                    Assert.AreEqual(lastIndex, Convert.ToInt32(actual));
                    lastIndex++;
                    index++;
                }
                SwipeTo("down");
                Thread.Sleep(1000);
                countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
                index = 1;
                //
                checkLastText(countListOfLayout);
            }
            Assert.AreEqual(countMainSimpleAdapterList, lastIndex);
        }

        /// <summary>
        /// Test text of first child in first group ------- Child 0 in Group 0
        /// </summary>
        [Test]
        public void TestFirstChild()
        {
            GoToSimpleAdapter();
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='Group 0']")).Click();
            Thread.Sleep(10000);
            //
            string actual = driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[1]/android.widget.TextView[1]")).Text;
            //
            Assert.AreEqual("Child 0", actual);
            //
            actual = driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[1]/android.widget.TextView[2]")).Text;
            //
            Assert.AreEqual("This child is even", actual);
        }

        /// <summary>
        /// Test childs in Group 0. Open and Close Group 0
        /// </summary>
        [Test]
        public void TestOpenCloseZeroGrpoup()
        {
            GoToSimpleAdapter();
            //
            int countListOfLayout = 0;
            int indexChild = 1;
            int lastIndex = 0;
            bool checkDescription = true;
            string text = string.Empty;
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='Group 0']")).Click();
            //
            while (lastIndex < countChild)
            {
                if (!firstCheck)
                {
                    SwipeTo("down");
                }
                countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem")).Count;
                //
                indexChild = checkIndexChild(lastIndex, indexChild);
                //
                for (int j = indexChild - 1; j < countListOfLayout; j++)
                {
                    if (checkDescription)
                    {
                        text = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[1]")).Text);
                        Assert.AreEqual(lastIndex, Convert.ToInt32(text));
                        Assert.AreEqual(descriptionChild[0], driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[2]")).Text);
                        checkDescription = false;
                    }
                    else if (!checkDescription)
                    {
                        text = getNumber(driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[1]")).Text);
                        Assert.AreEqual(lastIndex, Convert.ToInt32(text));
                        Assert.AreEqual(descriptionChild[1], driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TwoLineListItem[" + indexChild + "]/android.widget.TextView[2]")).Text);
                        checkDescription = true;
                    }
                    lastIndex++;
                    indexChild++;
                }
                firstCheck = false;
                indexChild = 2;
            }
            Assert.AreEqual(countChild, lastIndex);
            //
            firstCheck = true;
            while (firstCheck)
            {
                SwipeTo("up");
                countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
                if (countListOfLayout > 0)
                {
                    if (driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[1]")).Text.Contains("Group 0"))
                    {
                        driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[1]")).Click();
                        Assert.AreEqual("Group 1", driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[2]")).Text);
                        //
                        firstCheck = false;
                    }
                }
            }
        }
    }
}
