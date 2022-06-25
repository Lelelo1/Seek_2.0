using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogicLibrary.Utils;
using LogicLibrary.ViewModels;

namespace LogicLibrary.Models.Analytics
{

    // given in the sequence of creation, 'VisualShownEvent' created first
    //[Serializable]
    public class BubbleShown : BaseAnalyticsEvent
    {

        public string Name { get; }
        double Distance { get; }
        string LocalizedDistance { get; }

        public BubbleShown(string name, double distance, string localizedDistance)
        {
            Name = name;
            Distance = distance;
            LocalizedDistance = localizedDistance;
        }
    }

    //[Serializable]
    public class AppStart : BaseAnalyticsEvent
    {
        public string CameraPermission { get; }
        public string LocationWhenInUsePermission { get; }

        public AppStart(string cameraPermission, string locationWhenInUsePermission)
        {
            CameraPermission = cameraPermission;
            LocationWhenInUsePermission = locationWhenInUsePermission;
        }

    }

    //[Serializable]
    public class AppExit : BaseAnalyticsEvent
    {

        public string Trigger { get; }

        public AppExit(string trigger)
        {
            Trigger = trigger;
        }

    }


    /* // removed!
    [Serializable]
    public class Permission : BaseAnalyticsEvent
    {

        public string PermissionName { get; }
        public string PermissionStatus { get; }
        public Permission(string permissionName, string permissionStatus)
        {
            PermissionName = permissionName;
            PermissionStatus = permissionStatus;
        }

        public Xamarin.Essentials.PermissionStatus GetEssentialsStatus()
        {
            return Enum.Parse<Xamarin.Essentials.PermissionStatus>(PermissionStatus);
        }
    }
    */

    //[Serializable]
    public class Search : BaseAnalyticsEvent
    {
        public string Text { get; }
        public int Results { get; }
        public string Location { get; }
        public string SearchTrigger { get; }
        public Search(string text, int results, string location, string searchTrigger)
        {
            Text = text;
            Results = results;
            Location = location;
            SearchTrigger = searchTrigger;
        }
    }

    //[Serializable]
    public class KeyboardOpened : BaseAnalyticsEvent
    {

    }

    //[Serializable]
    public class AppLeaving : BaseAnalyticsEvent
    {
        public string SessionText { get; }
        public string Location { get; }
        public double Distance { get; }

        public AppLeaving(string sessionText, string location, double distance)
        {
            SessionText = sessionText;
            Location = location;
            Distance = distance;

        }

        public static string ToText(List<string> list)
        {
            if (Ext.IsNullOrEmpty(list))
            {
                return null;
            }

            string sessionTextString = null;
            list.ForEach(t => sessionTextString += t + ", ");

            // rem extra ", "
            sessionTextString.Remove(sessionTextString.Length - 2, 2);

            return sessionTextString;
        }
    }

    //[Serializable]
    public class AppResume : BaseAnalyticsEvent
    {

    }

    //[Serializable]
    public class BubbleTapped : BaseAnalyticsEvent
    {
        public string Name { get; }
        public double Distance { get; }
        public string LocalizedDistance { get; }

        public BubbleTapped(string name, double distance, string localizedDistance)
        {
            Name = name;
            Distance = distance;
            LocalizedDistance = localizedDistance;
        }
    }

    //[Serializable]
    public class ArrowShown : BaseAnalyticsEvent
    {
        public string Name { get; }
        public double Distance { get; }
        public string LocalizedDistance { get; }

        public ArrowShown(string name, double distance, string localizedDistance)
        {
            Name = name;
            Distance = distance;
            LocalizedDistance = localizedDistance;
        }
    }

    public class ARPageShown : BaseAnalyticsEvent
    {
        public string CameraPermission { get; }
        public string LocationWhenInUsePermission { get; }

        public ARPageShown(string cameraPermission, string locationWhenInUsePermission)
        {
            CameraPermission = cameraPermission;
            LocationWhenInUsePermission = locationWhenInUsePermission;
        }
    }

    // ..don't how or if should upgrade C# language version for sweater 'new ()' instead of 'new Dictionary<string, object>()' syntax
    // I have always had it set tto default, I am not sure if it is updating..

    // 'EventName' was previously an enum conisting of these events, in time of creation order:
    // visual_shown, renamed bubble_shown
    // app_start,
    // app_exit,
    // permission,
    // search


    // --------
    //
}