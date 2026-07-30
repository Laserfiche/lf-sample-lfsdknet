/*
   Copyright (c) Laserfiche.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using Laserfiche.RepositoryAccess;
using Laserfiche.RepositoryAccess.Activity;

namespace Laserfiche.Samples
{
    static class NotificationApp
    {
        static void Main(string[] args)
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";

            try
            {
                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);

                    // initialize an instance of NotificationManager
                    using (NotificationManager notiManager = new NotificationManager(session))
                    {
                        // establish a connection between the server and RA's NotificationManager
                        notiManager.Connect();

                        // subscribe to events on entries that occur in other sessions
                        notiManager.Subscribe(NotificationActivities.AllEntry, NotificationSubscriptionOptions.OtherSessionsOnly);

                        // wait up to a minute to receive a notification
                        Notification notification = notiManager.WaitForNotification(60 * 1000);

                        // if there is a notification, print it out
                        if (notification != null)
                        {
                            Console.Write("ActivityType:");
                            Console.WriteLine(notification.ActivityType);
                            Console.Write("Activity Number:");
                            Console.WriteLine(notification.SequenceNumber);
                        }
                    }

                    // log out of the repository
                    session.LogOut();
                }
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Hit enter to exit.");
            Console.ReadLine();
        }
    }
}
