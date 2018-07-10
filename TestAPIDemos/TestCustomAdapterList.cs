using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using TestAPIDemos.Tools;

namespace TestAPIDemos
{
    [TestFixture]
    public class TestCustomAdapterList : TestRunner
    {
        /// <summary>
        /// Mass wuth all custom adapter list
        /// </summary>
        string[] customAdapterAllList = new[] { "People Names",     "Arnold", "Barry", "Chuck", "David",
                                                "Dog Names",        "Ace", "Bandit", "Cha-Cha", "Deuce",
                                                "Cat Names",        "Fluffy", "Snuggles",
                                                "Fish Names",       "Goldy", "Bubbles"
                                               };
        int[] amountSubItems = new[] { 4, 4, 2, 2 };

        /// <summary>
        /// Count main items of custom adapter lists
        /// </summary>
        int countMainCustomAdapterList = 4;
        int countAllList = 16;

        private void GoToCustomAdapter()
        {
            SwipeTo("down");
            //
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Views")).Click();
            //
            driver.FindElement(By.Id("Expandable Lists")).Click();
            Thread.Sleep(1000);
            //
            driver.FindElement(By.Id("1. Custom Adapter")).Click();
            Thread.Sleep(1000);
        }
        
        /// <summary>
        /// Amount same items
        /// </summary>
        private void amountSame()
        {
            index = 1;
            while (driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).Text != "Fish Names")
            {
                index++;
            }
        }

        /// <summary>
        /// Tests Customer Adepter. Exists 4 item lists: People - , Dog - , Cat - , Fish Names. 
        /// </summary>
        [Test]
        public void TestMainItemsOfLists()
        {
            GoToCustomAdapter();
            //
            int countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
            //
            Assert.AreEqual(countMainCustomAdapterList, countListOfLayout);
            //
            //ForTextView items in List custom adapter
            index = 1;

            for (int i = 0; i < customAdapterAllList.Length; i++)
            {
                if (customAdapterAllList[i].Contains("Names"))
                {
                    Assert.AreEqual(customAdapterAllList[i], driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).GetAttribute("text"));
                    index++;
                }
            }
        }

        /// <summary>
        /// Open all main item on Layout. Check amounts of items all lists.
        /// </summary>
        [Test]
        public void TestOpenAllItems()
        {
            GoToCustomAdapter();
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='People Names']")).Click();
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='Dog Names']")).Click();
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='Cat Names']")).Click();
            //
            int actualCount = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
            //
            index = 1;
            //
            for (int i = 0; i < actualCount; i++)
            {
                Assert.AreEqual(customAdapterAllList[i], driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).GetAttribute("text"));
                index++;
            }
            //
            SwipeTo("down");
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='Fish Names']")).Click();
            // + automated swipe
            amountSame();
            index--;//2 рази одне й теж!
            //
            int begin = actualCount;
            //
            actualCount += (driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count - index);
            Assert.AreEqual(countAllList, actualCount);
            //
            index++;
            for (int i = begin; i < actualCount; i++)
            {
                Assert.AreEqual(customAdapterAllList[i], driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[" + index + "]")).GetAttribute("text"));
                index++;
            }
        }

        /// <summary>
        /// Data provider for tests opens items of lists
        /// </summary>
        private static readonly object[] CustomAdapter =
        {
            new object[] {8, "People Names"},
            new object[] {8, "Dog Names"},
            new object[] {6, "Cat Names"},
            new object[] {6, "Fish Names"}
        };

        /// <summary>
        /// Amount after open and closed main items of list Custom Adapter
        /// </summary>
        [Test]
        [TestCaseSource(nameof(CustomAdapter))]
        public void TestOpenEachItemsSeparately(int expectedCount, string textMainItem)
        {
            GoToCustomAdapter();
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='" + textMainItem + "']")).Click();
            Thread.Sleep(1000);
            //
            int countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
            Assert.AreEqual(expectedCount, countListOfLayout);
            //
            driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='" + textMainItem + "']")).Click();
            Thread.Sleep(1000);
            //
            countListOfLayout = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
            Assert.AreEqual(countMainCustomAdapterList, countListOfLayout);
        }

        /// <summary>
        /// Amount after open and closed main items of list Custom Adapter together
        /// </summary>
        [Test]
        public void TestOpenEachItemsTogether()
        {
            GoToCustomAdapter();
            //
            for (int i = 0; i < customAdapterAllList.Length; i++)
            {
                if (customAdapterAllList[i].Contains("Names"))
                {
                    driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='" + customAdapterAllList[i] + "']")).Click();
                    Thread.Sleep(1000);
                    //
                    int countListWithSubitems = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
                    Assert.AreEqual(amountSubItems[index] + countMainCustomAdapterList, countListWithSubitems);
                    //
                    driver.FindElement(By.XPath("//android.widget.ExpandableListView/android.widget.TextView[@text='" + customAdapterAllList[i] + "']")).Click();
                    Thread.Sleep(1000);
                    //
                    countListWithSubitems = driver.FindElements(By.XPath("//android.widget.ExpandableListView/android.widget.TextView")).Count;
                    Assert.AreEqual(countMainCustomAdapterList, countListWithSubitems);
                    //
                    index++;
                }
            }
        }
    }
}
