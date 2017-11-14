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
            //DownloadPodcasts(doc, items, "http://lifelistened.com/feed/the-mom-hour/", "The Mom Hour");
            DownloadPodcasts(doc, items, "http://feeds.feedburner.com/SlateMomAndDadAreFighting", "Mom And Dad Are Fighting");
            DownloadPodcasts(doc, items, "https://rss.art19.com/the-longest-shortest-time", "The Longest Shortest Time");
            DownloadPodcasts(doc, items, "https://www.npr.org/rss/podcast.php?id=510321", "Wow In The World");
            DownloadPodcasts(doc, items, "https://rss.art19.com/tumble", "Tumble Science Podcast For Kids");
            DownloadPodcasts(doc, items, "http://www.abc.net.au/radio/programs/shortandcurly/feed/7388142/podcast.xml", "Short and Curly");
            DownloadPodcasts(doc, items, "http://feeds.gimletmedia.com/ScienceVs", "Science VS");
            DownloadPodcasts(doc, items, "http://feeds.megaphone.fm/TWU6263225887", "36 Questions");
            DownloadPodcasts(doc, items, "http://feeds.gimletmedia.com/hearreplyall", "Reply All");
            DownloadPodcasts(doc, items, "https://timferriss.libsyn.com/rss", "Tim Ferriss Show");
            DownloadPodcasts(doc, items, "https://www.npr.org/rss/podcast.php?id=510313", "How I Built This With Guy Raz");
            DownloadPodcasts(doc, items, "http://www.manager-tools.com/podcasts/basics-rss.xml", "Manager Tools Basics");
            DownloadPodcasts(doc, items, "http://www.manager-tools.com/podcasts/feed/rss2", "Manager Tools Most Recent Podcasts");
            DownloadPodcasts(doc, items, "https://grantbaldwin.libsyn.com/rss", "How Did You Get Into That");
            DownloadPodcasts(doc, items, "https://feeds.feedburner.com/snap-judgment-spooked", "Snap Judgement");
            DownloadPodcasts(doc, items, "https://feeds.podtrac.com/tBPkjrcL0_m0", "Coding Blocks");
            DownloadPodcasts(doc, items, "https://www.functionalgeekery.com/feed/mp3/", "Functional Geekery");
            DownloadPodcasts(doc, items, "https://feeds.feedburner.com/netrocksfullmp3downloads", "Dot NET Rocks");
            DownloadPodcasts(doc, items, "http://s.ch9.ms/Shows/Visual-Studio-Toolbox/feed/mp3", "Visual Studio Toolbox");
            DownloadPodcasts(doc, items, "https://www.npr.org/rss/podcast.php?id=510289", "Planet Money");
            DownloadPodcasts(doc, items, "https://rss.art19.com/my-favorite-murder-with-karen-kilgariff-and-georgia-hardstark-fb", "My Favorite Murder");
            DownloadPodcasts(doc, items, "http://casefile.libsyn.com/rss", "Casefile");

            /*
             * Not Working
             * DownloadPodcasts(doc, items, "http://feeds.misfitrad.io/haappening", "This Is Actually Happening");
             * //DownloadPodcasts(doc, items, "http://citydadsgroup.com/blog/podcast/", "City Dads Group");
             * DownloadPodcasts(doc, items, "http://rss.art19.com/casefile/", "Casefile");
             * //DownloadPodcasts(doc, items, "http://www.scummymummies.com/1/feed", "Summy Mummies");
             * DownloadPodcasts(doc, items, "https://www.thenakedscientists.com/naked_scientists_podcast.xml", "Naked Scientists");
             */

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
                if (items[i].SelectSingleNode("enclosure") != null)
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