using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using The100.Console.Infrastructure;

namespace The100.Console.Domain
{
    public class The100Session
    {
        static readonly string ChromeDriverDirectory = Path.Combine(Environment.CurrentDirectory, "lib");

        readonly Credentials _credentials;

        public The100Session(Credentials credentials)
        {
            _credentials = credentials;
        }

        public SessionResult Create(SessionDefinition sessionDefinition)
        {
            using (var driver = new ChromeDriver(ChromeDriverDirectory))
            {
                NavigateToDashboard(driver);
                LoginIfRequired(driver);
                CheckFriends(driver, sessionDefinition.FriendsToAdd);

                return CreateGamingSession(driver, sessionDefinition);
            }
        }

        void NavigateToDashboard(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(The100Api.BuildUserDashboardUri(_credentials.UserId));
        }

        void LoginIfRequired(IWebDriver driver)
        {
            if (driver.Url != The100Api.LoginUri.AbsoluteUri)
            {
                return;
            }

            var gamerTagText = driver.FindElement(By.Id("gamertag"));
            var passwordText = driver.FindElement(By.Id("password"));
            var submitButton = driver.FindElement(By.Name("commit"));

            gamerTagText.SendKeys(_credentials.GamerTag);
            passwordText.SendKeys(_credentials.Password);
            submitButton.Submit();
        }

        List<The100FriendLink> GetCurrentFriendLinks(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(The100Api.BuildUserUri(_credentials.UserId));

            var links = driver.FindElements(By.XPath("//div[@id='user-profile-friends-tab']/div/h4/a[1]"));

            return links.Select(f => new The100FriendLink(f.GetAttribute("innerHTML"), f.GetAttribute("href"))).ToList();
        }

        void CheckFriends(IWebDriver driver, IEnumerable<string> friendsToAdd)
        {
            var currentFriends = GetCurrentFriendLinks(driver);

            foreach (var friendToAdd in friendsToAdd)
            {
                if (currentFriends.All(f => f.Name != friendToAdd))
                {
                    throw new Exception($"You are not currently friends with '{friendToAdd}' and can't add them to your session. Aborting.");
                }
            }
        }

        static SessionResult CreateGamingSession(IWebDriver driver, SessionDefinition sessionDefinition)
        {
            driver.Navigate().GoToUrl(The100Api.NewGamingSessionUri);

            var moreLink = driver.FindElement(By.LinkText("more options »"));
            moreLink.Click();

            var activitySelect = new SelectElement(driver.FindElement(By.Id("gaming_session_category")));
            var detailsText = driver.FindElement(By.Id("gaming_session_name"));
            var startDate = driver.FindElement(By.Id("gaming_session_start_date"));
            var startTimeHourSelect = new SelectElement(driver.FindElement(By.Id("gaming_session_start_time_4i")));
            var startTimeMinutesSelect = new SelectElement(driver.FindElement(By.Id("gaming_session_start_time_5i")));
            var groupIdSelect = new SelectElement(driver.FindElement(By.Id("gaming_session_group_id")));
            var lightLevelSelect = new SelectElement(driver.FindElement(By.Id("gaming_session_light_level")));
            var submitButton = driver.FindElement(By.Name("commit"));

            activitySelect.SelectByValue(sessionDefinition.Activity);
            detailsText.SendKeys(sessionDefinition.Details);
            startDate.SendKeys(sessionDefinition.StartTime.ToString("dd/MM/yyyy"));
            startTimeHourSelect.SelectByValue(sessionDefinition.StartTime.Hour.ToString("D2"));
            startTimeMinutesSelect.SelectByValue(sessionDefinition.StartTime.Minute.ToString("D2"));
            groupIdSelect.SelectByValue(sessionDefinition.GroupId.ToString());
            lightLevelSelect.SelectByValue(sessionDefinition.LightLevel.ToString());

            submitButton.Submit();

            IWebElement danger;

            if (driver.TryFindElement(By.ClassName("bg-danger"), out danger))
            {
                throw new Exception(danger.Text);
            }

            var sessionId = driver.Url.Replace(The100Api.GamingSessionsUri.ToString(), string.Empty);

            AddFriends(driver, sessionDefinition.FriendsToAdd);

            return new SessionResult(sessionId, sessionDefinition.Details, sessionDefinition.StartTime);
        }

        static void AddFriends(IWebDriver driver, IEnumerable<string> friendsToAdd)
        {
            foreach (var friendToAdd in friendsToAdd)
            {
                var supporterOptionsLink = driver.FindElement(By.LinkText("Supporter options"));
                supporterOptionsLink.Click();

                var friendsSpan = driver.FindElement(By.ClassName("filter-option"));

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => friendsSpan.Displayed);

                friendsSpan.Click();

                var friendSpan = driver.FindElement(By.XPath($"//ul[@class='dropdown-menu inner']/li/a/span[text()='{friendToAdd}']"));

                wait.Until(d => friendSpan.Displayed);

                friendSpan.Click();

                var submitButton = driver.FindElement(By.Name("commit"));
                submitButton.Submit();
            }
        }
    }
}