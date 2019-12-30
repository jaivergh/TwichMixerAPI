﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitchAPI.Services;

namespace TwitchAPI.Controllers
{
    public class ViewersController : Controller
    {
        public ViewersController(TwitchService twitch, MixerService mixer)
        {
            this.Twitch = twitch;
            this.Mixer = mixer;
        }

        public TwitchService Twitch { get; }
        public MixerService Mixer { get; }

        public IActionResult Current()
        {
            return View(Twitch.CurrentViewCount);
        }
    }
}