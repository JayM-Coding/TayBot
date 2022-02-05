using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.Api.Helix.Models.EventSub;

namespace TayBot
{
    public class TwitchPubSubClient
    {
        private TwitchPubSub _twitchClient;

        public TwitchPubSubClient()
        {
            Run();
        }
        private void Run()
        {
            _twitchClient = new TwitchPubSub();
            _twitchClient.OnPubSubServiceConnected += onPubSubServiceConnected;
            _twitchClient.OnStreamUp += onStreamUp;
            _twitchClient.OnStreamDown += onStreamDown;
            Console.WriteLine("Running twitch client");
        }

        private void onStreamDown(object sender, OnStreamDownArgs e)
        {
            throw new NotImplementedException();
        }

        private void onStreamUp(object sender, OnStreamUpArgs e)
        {
            throw new NotImplementedException();
        }

        private void onPubSubServiceConnected(object sender, EventArgs e)
        {
            _twitchClient.SendTopics();
        }
    }
}
