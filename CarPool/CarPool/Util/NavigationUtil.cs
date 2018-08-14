using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using CarPool;

namespace Util
{
    public class NavigationUtil
    {
        public static void SwitchDetailPage(Type pageType, String title)
        {

            var page = (Page)Activator.CreateInstance(pageType);
            page.Title = title;
            SwitchDetailPage(page);
        }

        public static void SwitchDetailPage(Page page)
        {
            try
            {
                if (page != null)
                {
                    CPMasterDetailPage.GetMasterDetail().Detail = new NavigationPage(page);
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Cannot go to page because " + ex.Message);
            }
        }
    }
}
