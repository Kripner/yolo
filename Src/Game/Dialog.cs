﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace yolo {

    public class DialogInfo {
        List<string> sentences = new List<string>();
        public IReadOnlyList<string> Sentences => sentences;

        public DialogInfo(IEnumerable<string> sentences) {
            this.sentences = new List<string>(sentences);
        }

        public DialogInfo NextDialog { get; init; } = null;
        public Point Size { get; } = new(20, 3);

        public DialogBehavior OpenNewDialogOn(Entity onE) {
            var ctx = onE.Context;
            var player = ctx.Player;
            player.FreezeFor = float.PositiveInfinity;

            var eBanner = new Entity(ctx) {
                IsTemporal = true,
            };

            DialogBehavior result;
            eBanner.Animation = new DialogAnimation() {
                DisplayChars = Size,
            };
            eBanner.Position = onE.Position + World.Up * 1;
            eBanner.Behavior = result = new DialogBehavior(eBanner, this);

            ctx.World.CurrentScene.AddEntity(eBanner);

            return result;
        }

    }

    public class DialogBehavior : Behaviour {

        List<string[]> screensToDisplay = new();
        int screen = 0;

        public DialogBehavior(Entity e, DialogInfo info) : base(e) {
            foreach (var tense in info.Sentences) {
                screensToDisplay.Add(tense.SplitToLines(info.Size.X).ToArray());
            }

            var anim = (DialogAnimation)Entity.Animation;
            anim.Text = string.Join(' ', screensToDisplay[screen]);
        }

        public override void Update() {

            /*if (context.Keyboard.IsKeyPressed(Keys.F)) {
                context.Player.FreezeFor = 0;
                Entity.Destroy();
            }*/
        }
    }
}
