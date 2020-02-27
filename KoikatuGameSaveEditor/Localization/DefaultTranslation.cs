﻿using JetBrains.Annotations;
using System.Collections.Generic;

namespace KGSE.Localization {
    internal static class DefaultTranslation {

        [NotNull]
        public static Translation Value {
            get {
                if (_value == null) {
                    _value = Create();
                }

                return _value;
            }
        }

        [NotNull]
        private static Translation Create() {
            var dict = new Dictionary<string, string> {
                ["app.title"] = "Koikatu Game Save Editor",
                ["app.title.loaded"] = "Koikatu Game Save Editor - {0} ({1})",
                ["app.menu.file"] = "&File",
                ["app.menu.file.open"] = "&Open...",
                ["app.menu.file.save"] = "&Save",
                ["app.menu.file.saveas"] = "Save &As...",
                ["app.menu.file.exit"] = "E&xit",
                ["app.menu.languages"] = "&Languages",
                ["app.menu.help"] = "&Help",
                ["app.menu.help.about"] = "&About",

                ["male.first_name"] = "Given Name",
                ["male.last_name"] = "Family Name",
                ["male.nickname"] = "Nickname",
                ["male.intellect"] = "Intellect",
                ["male.strength"] = "Strength",
                ["male.hentai"] = "H",

                ["female.first_name"] = "Given Name",
                ["female.last_name"] = "Family Name",
                ["female.nickname"] = "Nickname",
                ["female.pers"] = "Personality",
                ["female.pers.0"] = "Sexy",
                ["female.pers.1"] = "Heiress",
                ["female.pers.2"] = "Snobby",
                ["female.pers.3"] = "Underclassman",
                ["female.pers.4"] = "Mysterious",
                ["female.pers.5"] = "Weirdo",
                ["female.pers.6"] = "Japanese ideal",
                ["female.pers.7"] = "Boyish",
                ["female.pers.8"] = "Pure",
                ["female.pers.9"] = "Girl next door",
                ["female.pers.10"] = "Delusional",
                ["female.pers.11"] = "Motherly",
                ["female.pers.12"] = "Big sisterly",
                ["female.pers.13"] = "Gal",
                ["female.pers.14"] = "Rebel",
                ["female.pers.15"] = "Wild",
                ["female.pers.16"] = "Honor student",
                ["female.pers.17"] = "Crabby",
                ["female.pers.18"] = "Unlucky",
                ["female.pers.19"] = "Bookish",
                ["female.pers.20"] = "Nervous",
                ["female.pers.21"] = "Classic heroine",
                ["female.pers.22"] = "Fan girl",
                ["female.pers.23"] = "Otaku",
                ["female.pers.24"] = "Psycho stalker",
                ["female.pers.25"] = "Lazy",
                ["female.pers.26"] = "Quiet",
                ["female.pers.27"] = "Stubborn",
                ["female.pers.28"] = "Old fashioned",
                ["female.pers.29"] = "Indifferent",
                ["female.pers.30"] = "Friendly",
                ["female.pers.31"] = "Determined",
                ["female.pers.32"] = "Honest",
                ["female.pers.33"] = "Glamorous",
                ["female.pers.34"] = "Returnee",
                ["female.pers.35"] = "Dialect girl",
                ["female.pers.36"] = "Sadistic",
                ["female.pers.37"] = "Emotionless",
                ["female.pers.38"] = "Meticulous",
                ["female.weak_point"] = "Weak Point",
                ["female.weak_point.0"] = "Lips",
                ["female.weak_point.1"] = "Breasts",
                ["female.weak_point.2"] = "Crotch",
                ["female.weak_point.3"] = "Anal",
                ["female.weak_point.4"] = "Butts",
                ["female.weak_point.5"] = "Nipples",
                ["female.weak_point.6"] = "None",
                ["female.answers"] = "Answers",
                ["female.answers.animal"] = "Likes animals",
                ["female.answers.eat"] = "Likes eating",
                ["female.answers.cook"] = "Likes cooking",
                ["female.answers.exercise"] = "Likes exercising",
                ["female.answers.study"] = "Studies hard",
                ["female.answers.fashionable"] = "Chic",
                ["female.answers.blackCoffee"] = "Accepts black coffee",
                ["female.answers.spicy"] = "Accepts spicy food",
                ["female.answers.sweet"] = "Likes sweet food",
                ["female.pref"] = "Preferences",
                ["female.pref.kiss"] = "Kissing",
                ["female.pref.aibu"] = "Caress",
                ["female.pref.anal"] = "Anal sex",
                ["female.pref.massage"] = "Vibrator",
                ["female.pref.notCondom"] = "Raw insertion",
                ["female.traits"] = "Traits",
                ["female.traits.hinnyo"] = "Small bladder",
                ["female.traits.harapeko"] = "Starved",
                ["female.traits.donkan"] = "Insensitive",
                ["female.traits.choroi"] = "Simple",
                ["female.traits.bitch"] = "Slutty",
                ["female.traits.mutturi"] = "Promiscuous",
                ["female.traits.dokusyo"] = "Bookworm",
                ["female.traits.ongaku"] = "Likes music",
                ["female.traits.kappatu"] = "Lively",
                ["female.traits.ukemi"] = "Submissive",
                ["female.traits.friendly"] = "Friendly",
                ["female.traits.kireizuki"] = "Neat",
                ["female.traits.taida"] = "Lazy",
                ["female.traits.sinsyutu"] = "Elusive",
                ["female.traits.hitori"] = "Loner",
                ["female.traits.undo"] = "Sporty",
                ["female.traits.majime"] = "Diligent",
                ["female.traits.likeGirls"] = "Likes girls",
                ["female.feeling"] = "Feeling",
                ["female.relation"] = "Relation",
                ["female.relation.0"] = "Friends",
                ["female.relation.1"] = "Lovers",
                ["female.h_degree"] = "H Degree",
                ["female.h_count"] = "H Count",
                ["female.intimacy"] = "Intimacy",
                ["female.is_angry"] = "Is in angry mood",
                ["female.is_club_member"] = "Is Koikatu club member",
                ["female.has_date"] = "On a date",
                ["female.d.breasts"] = "Breasts",
                ["female.d.crotch"] = "Crotch",
                ["female.d.anal"] = "Anal",
                ["female.d.butts"] = "Butts",
                ["female.d.nipples"] = "Nipples",
                ["female.d.vi"] = "V. Insertion",
                ["female.d.ai"] = "A. Insertion",
                ["female.d.serving"] = "Serving",

                ["misc.gender"] = "Gender",
                ["misc.gender.0"] = "Male",
                ["misc.gender.1"] = "Female",
            };

            return new Translation("English (Neutral) [Default]", "en-neutral", dict);
        }

        private static Translation _value;

    }
}
