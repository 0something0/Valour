﻿using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Valour.Client.Messages;
using Valour.Client.Planets;
using Valour.Shared.Channels;

namespace Valour.Client.Channels
{
    /*  Valour - A free and secure chat client
     *  Copyright (C) 2020 Vooper Media LLC
     *  This program is subject to the GNU Affero General Public license
     *  A copy of the license should be included - if not, see <http://www.gnu.org/licenses/>
     */

    /// <summary>
    /// The clientside planet cache reduces the need to repeatedly ask the server
    /// for planet resources
    /// </summary>
    public class ClientPlanetChatChannel : PlanetChatChannel
    {

        /// <summary>
        /// Converts to a client version of planet chat channel
        /// </summary>
        public static ClientPlanetChatChannel FromBase(PlanetChatChannel channel, IMapper mapper)
        {
            return mapper.Map<ClientPlanetChatChannel>(channel);
        }

        /// <summary>
        /// Returns the planet this chat channel belongs to
        /// </summary>
        public async Task<ClientPlanet> GetPlanetAsync()
        {
            return await ClientPlanetCache.GetPlanetAsync(Planet_Id);
        }

        /// <summary>
        /// Returns messages from the channel
        /// </summary>
        /// <param name="index">The starting index for the messages</param>
        /// <param name="count">The amount of messages to return</param>
        /// <returns>An enumerable list of planet messages</returns>
        public async Task<IEnumerable<ClientPlanetMessage>> GetMessagesAsync(ulong index = ulong.MaxValue, int count = 10)
        {
            string json = await ClientUserManager.Http.GetStringAsync($"Channel/GetMessages?channel_id={Id}" +
                                                                                           $"&index={index}" +
                                                                                           $"&count={count}");

            IEnumerable<ClientPlanetMessage> messages = JsonConvert.DeserializeObject<List<ClientPlanetMessage>>(json);

            if (messages == null)
            {
                Console.WriteLine("Failed to deserialize messages from GetMessages");
            }

            return messages;
        }

        /// <summary>
        /// Returns messages from the channel
        /// </summary>
        /// <param name="index">The starting index for the messages</param>
        /// <param name="count">The amount of messages to return</param>
        /// <returns>An enumerable list of planet messages</returns>
        public async Task<IEnumerable<ClientPlanetMessage>> GetLastMessagesAsync(int count = 10)
        {
            string json = await ClientUserManager.Http.GetStringAsync($"Channel/GetLastMessages?channel_id={Id}" +
                                                                                             $"&count={count}");

            IEnumerable<ClientPlanetMessage> messages = JsonConvert.DeserializeObject<List<ClientPlanetMessage>>(json);

            if (messages == null)
            {
                Console.WriteLine("Failed to deserialize messages from GetLastMessages");
            }

            return messages;
        }
    }
}
