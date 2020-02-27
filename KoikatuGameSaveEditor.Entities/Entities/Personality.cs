using System.Diagnostics;

namespace KGSE.Entities {
    // Translations adapted from:
    // https://wiki.anime-sharing.com/hgames/index.php?title=Koikatu/Gameplay/Personalities
    public enum Personality {

        [DebuggerDisplay("セクシー")]
        Sexy = 0,

        [DebuggerDisplay("お嬢様")]
        Heiress = 1,

        [DebuggerDisplay("高飛車")]
        Snobby = 2,

        [DebuggerDisplay("後輩キャラ")]
        Underclassman = 3,

        [DebuggerDisplay("ミステリアス")]
        Mysterious = 4,

        [DebuggerDisplay("電波")]
        Weirdo = 5,

        [DebuggerDisplay("大和撫子")]
        JapaneseIdeal = 6,

        [DebuggerDisplay("ボーイッシュ")]
        Boyish = 7,

        [DebuggerDisplay("純真無垢")]
        Pure = 8,

        [DebuggerDisplay("単純")]
        GirlNextDoor = 9,

        [DebuggerDisplay("邪気眼")]
        Delusional = 10,

        [DebuggerDisplay("母性的")]
        Motherly = 11,

        [DebuggerDisplay("姉御肌")]
        BigSisterly = 12,

        [DebuggerDisplay("ギャル")]
        Gal = 13,

        [DebuggerDisplay("不良少女")]
        Rebel = 14,

        [DebuggerDisplay("野性的")]
        Wild = 15,

        [DebuggerDisplay("意識高いクール")]
        HonorStudent = 16,

        [DebuggerDisplay("ひねくれ")]
        Crabby = 17,

        [DebuggerDisplay("不幸少女")]
        Unlucky = 18,

        [DebuggerDisplay("文学少女")]
        Bookish = 19,

        [DebuggerDisplay("もじもじ")]
        Nervous = 20,

        [DebuggerDisplay("正当派ヒロイン")]
        ClassicHeroine = 21,

        [DebuggerDisplay("ミーハー")]
        FanGirl = 22,

        [DebuggerDisplay("オタク女子")]
        Otaku = 23,

        [DebuggerDisplay("ヤンデレ")]
        PsychoStalker = 24,

        [DebuggerDisplay("ものぐさ")]
        Lazy = 25,

        [DebuggerDisplay("無口")]
        Quiet = 26,

        [DebuggerDisplay("意地っ張り")]
        Stubborn = 27,

        [DebuggerDisplay("のじゃ子")]
        OldFashioned = 28,

        [DebuggerDisplay("素直クール")]
        Indifferent = 29,

        // DLC1

        [DebuggerDisplay("気さく")]
        Friendly = 30,

        [DebuggerDisplay("勝気")]
        Determined = 31,

        [DebuggerDisplay("誠実")]
        Honest = 32,

        [DebuggerDisplay("艶やか")]
        Glamorous = 33,

        // DLC2

        [DebuggerDisplay("帰国子女")]
        Returnee = 34,

        [DebuggerDisplay("方言娘")]
        DialectGirl = 35,

        [DebuggerDisplay("Ｓッ気")]
        Sadistic = 36,

        [DebuggerDisplay("無感情")]
        Emotionless = 37,

        // Emotion Creators Bonus

        [DebuggerDisplay("几帳面")]
        Meticulous = 38,

    }
}
