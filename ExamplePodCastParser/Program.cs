using System;
using System.IO;
using System.Net;
using System.Xml;

namespace ExamplePodCastParser
{
    class Program
    {
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            //This loads the Podcast
            XmlDocument doc = new XmlDocument();
            XmlNodeList items = null;
            DownloadPodcasts(doc, items, "http://rss.art19.com/casefile/", "Casefile");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/CriminalShow", "Criminal");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/freakonomicsradio", "Freakonomics Radio");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/themessagepodcast", "GE Podcast Theater");
            DownloadPodcasts(doc, items, "http://lorepodcast.libsyn.com/rss", "Lore");
            DownloadPodcasts(doc, items, "http://megmeeker.megmeekerpodcast.libsynpro.com/rss", "Parenting Great Kids");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/RevisionistHistory", "Revisionist History");
            DownloadPodcasts(doc, items, "http://feeds.serialpodcast.org/serialpodcast", "Serial");
            DownloadPodcasts(doc, items, "http://softwareengineeringdaily.com/feed/podcast/", "Software Engineering Daily");
            DownloadPodcasts(doc, items, "http://www.howstuffworks.com/podcasts/stuff-you-should-know.rss", "StuffYouShouldKnow");
            DownloadPodcasts(doc, items, "http://www.npr.org/rss/podcast.php?id=510298", "Ted Radio Hour");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/iTunesPodcastTTHealth", "TedTalks Health");                           
            DownloadPodcasts(doc, items, "http://daveramsey.ramsey.libsynpro.com/rss", "The Dave Ramsey Show");
            DownloadPodcasts(doc, items, "http://podcasts.ape-apps.com/my-colony-podcast/feed.xml", "My Colony");
            DownloadPodcasts(doc, items, "http://feeds.stownpodcast.org/stownpodcast", "STownPodcast");
            DownloadPodcasts(doc, items, "http://feed.thisamericanlife.org/talpodcast", "This American Life");

            //Console.ReadLine();
        }

        public static void DownloadPodcasts(XmlDocument doc, XmlNodeList items, string podcastXML, string podcastName)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var numDownloaded = 0;
            doc.Load(podcastXML);

            items = doc.SelectNodes("//item");

            for (int i = 0; i < items.Count; i++)
            {
                var fileTitle = items[i].SelectSingleNode("title").InnerText;
                if(items[i].SelectSingleNode("enclosure") != null)
                {
                    var fileURI = items[i].SelectSingleNode("enclosure").Attributes["url"].Value;
                    if (!Directory.Exists(String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}", podcastName)))
                    {
                        Directory.CreateDirectory(String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}", podcastName));
                    }
                    if (fileURI.Contains("?"))
                    {
                        var fileName = fileURI.Remove(fileURI.IndexOf("?", StringComparison.Ordinal));
                        var fileNameArray = fileName.Split('/');
                        fileName = fileNameArray[fileNameArray.Length - 1];
                        var filePathToDownload = String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}\" + fileName, podcastName);
                        var archiveFilePath = String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}\Archive\" + fileName, podcastName);
                        if (!File.Exists(filePathToDownload))
                        {
                            if (!File.Exists(archiveFilePath))
                            {
                                Console.WriteLine("Downloading {0} podcasts", podcastName);
                                WebClient client = new WebClient();
                                Console.WriteLine("\nDownloading: {0}\nFile location: {1}", fileName, filePathToDownload);
                                client.DownloadFile(fileURI, filePathToDownload);
                                numDownloaded++;
                            }
                        }
                    }
                    else
                    {
                        var fileNameArray = fileURI.Split('/');
                        var fileName = fileNameArray[fileNameArray.Length - 1];
                        var filePathToDownload = String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}\" + fileName, podcastName);
                        var archiveFilePath = String.Format(@"\\192.168.201.9\GoFlex Home Personal\Podcasts\{0}\Archive\" + fileName, podcastName);
                        if (!File.Exists(filePathToDownload))
                        {
                            if (!File.Exists(archiveFilePath))
                            {
                                WebClient client = new WebClient();
                                Console.WriteLine("\nDownloading: {0}\nFile location: {1}", fileName, filePathToDownload);
                                try
                                {
                                    client.DownloadFile(fileURI, filePathToDownload);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine(ex.Source);
                                    Console.WriteLine(ex.StackTrace);
                                    Console.WriteLine(ex.InnerException);
                                }
                                numDownloaded++;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Downloaded {0} {1} podcasts.", numDownloaded, podcastName);
        }
    }
}