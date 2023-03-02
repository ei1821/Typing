using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using global::System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Security;
using System.Collections.ObjectModel;
using System.Text;
using System.Buffers;

namespace Typing
{
    /// <summary> タイピングに関する諸情報を管理するクラス</summary>
    static class TypeUtils
    {
        /// <summary>かな文字列からローマ字文字列のリストを返す辞書</summary>
        public static readonly ReadOnlyDictionary<string, string[]> kana2romaList = new(new Dictionary<string, string[]>()
        {
            ["ぁ"] = new string[] { "la", "xa" },
            ["あ"] = new string[] { "a" },
            ["ぃ"] = new string[] { "li", "xi" },
            ["い"] = new string[] { "i" },
            ["ぅ"] = new string[] { "lu", "xu" },
            ["う"] = new string[] { "u", "wu" },
            ["ぇ"] = new string[] { "le", "xe" },
            ["え"] = new string[] { "e" },
            ["ぉ"] = new string[] { "lo", "xo" },
            ["お"] = new string[] { "o" },
            ["か"] = new string[] { "ka", /*"ca"*/ },
            ["が"] = new string[] { "ga", },
            ["き"] = new string[] { "ki" },
            ["ぎ"] = new string[] { "gi" },
            ["く"] = new string[] { "ku", "qu" },
            ["ぐ"] = new string[] { "gu" },
            ["け"] = new string[] { "ke" },
            ["げ"] = new string[] { "ge" },
            ["こ"] = new string[] { "ko" },
            ["ご"] = new string[] { "go" },
            ["さ"] = new string[] { "sa" },
            ["ざ"] = new string[] { "za" },
            ["し"] = new string[] { "si", "shi" },
            ["じ"] = new string[] { "ji", "zi", },
            ["す"] = new string[] { "su" },
            ["ず"] = new string[] { "zu" },
            ["せ"] = new string[] { "se" },
            ["ぜ"] = new string[] { "ze" },
            ["そ"] = new string[] { "so" },
            ["ぞ"] = new string[] { "zo" },
            ["た"] = new string[] { "ta" },
            ["だ"] = new string[] { "da" },
            ["ち"] = new string[] { "ti", "chi" },
            ["ぢ"] = new string[] { "di" },
            ["っ"] = new string[] { "ltu", "xtu", "ltsu", "xtsu" },
            ["つ"] = new string[] { "tu", "tsu" },
            ["づ"] = new string[] { "du" },
            ["て"] = new string[] { "te" },
            ["で"] = new string[] { "de" },
            ["と"] = new string[] { "to" },
            ["ど"] = new string[] { "do" },
            ["な"] = new string[] { "na" },
            ["に"] = new string[] { "ni" },
            ["ぬ"] = new string[] { "nu" },
            ["ね"] = new string[] { "ne" },
            ["の"] = new string[] { "no" },
            ["は"] = new string[] { "ha" },
            ["ば"] = new string[] { "ba" },
            ["ぱ"] = new string[] { "pa" },
            ["ひ"] = new string[] { "hi" },
            ["び"] = new string[] { "bi" },
            ["ぴ"] = new string[] { "pi" },
            ["ふ"] = new string[] { "hu" },
            ["ぶ"] = new string[] { "bu" },
            ["ぷ"] = new string[] { "pu" },
            ["へ"] = new string[] { "he" },
            ["べ"] = new string[] { "be" },
            ["ぺ"] = new string[] { "pe" },
            ["ほ"] = new string[] { "ho" },
            ["ぼ"] = new string[] { "bo" },
            ["ぽ"] = new string[] { "po" },
            ["ま"] = new string[] { "ma" },
            ["み"] = new string[] { "mi" },
            ["む"] = new string[] { "mu" },
            ["め"] = new string[] { "me" },
            ["も"] = new string[] { "mo" },
            ["ゃ"] = new string[] { "lya", "xya" },
            ["や"] = new string[] { "ya" },
            ["ゅ"] = new string[] { "lyu", "xyu" },
            ["ゆ"] = new string[] { "yu" },
            ["ょ"] = new string[] { "lyo", "xyo" },
            ["よ"] = new string[] { "yo" },
            ["ら"] = new string[] { "ra" },
            ["り"] = new string[] { "ri" },
            ["る"] = new string[] { "ru" },
            ["れ"] = new string[] { "re" },
            ["ろ"] = new string[] { "ro" },
            ["ゎ"] = new string[] { "lwa", "xwa" },
            ["わ"] = new string[] { "wa" },
            ["ゐ"] = new string[] { "wi" },
            ["ゑ"] = new string[] { "we" },
            ["を"] = new string[] { "wo" },
            ["ん"] = new string[] { "n", "nn", "xn" },
            ["ゔ"] = new string[] { "vu" },
            ["ゕ"] = new string[] { "lka", "xka" },
            ["ゖ"] = new string[] { "lke", "xke" },
            ["っぁ"] = new string[] { "lla", "xxa" },
            ["っぃ"] = new string[] { "lli", "xxi" },
            ["っぅ"] = new string[] { "llu", "xxu" },
            ["っぇ"] = new string[] { "lle", "xxe" },
            ["っぉ"] = new string[] { "llo", "xxo" },
            ["っか"] = new string[] { "kka", /*"ca"*/ },
            ["っが"] = new string[] { "gga", },
            ["っき"] = new string[] { "kki" },
            ["っぎ"] = new string[] { "ggi" },
            ["っく"] = new string[] { "kku" },
            ["っぐ"] = new string[] { "ggu" },
            ["っけ"] = new string[] { "kke" },
            ["っげ"] = new string[] { "gge" },
            ["っこ"] = new string[] { "kko" },
            ["っご"] = new string[] { "ggo" },
            ["っさ"] = new string[] { "ssa" },
            ["っざ"] = new string[] { "zza" },
            ["っし"] = new string[] { "ssi", "sshi" },
            ["っじ"] = new string[] { "zzi", "jji" },
            ["っす"] = new string[] { "ssu" },
            ["っず"] = new string[] { "zzu" },
            ["っせ"] = new string[] { "sse" },
            ["っぜ"] = new string[] { "zze" },
            ["っそ"] = new string[] { "sso" },
            ["っぞ"] = new string[] { "zzo" },
            ["った"] = new string[] { "tta" },
            ["っだ"] = new string[] { "dda" },
            ["っち"] = new string[] { "tti", "cchi" },
            ["っぢ"] = new string[] { "ddi" },
            ["っっ"] = new string[] { "lltu", "xxtu" },
            ["っつ"] = new string[] { "ttu", "ttsu" },
            ["っづ"] = new string[] { "ddu" },
            ["って"] = new string[] { "tte" },
            ["っで"] = new string[] { "dde" },
            ["っと"] = new string[] { "tto" },
            ["っど"] = new string[] { "ddo" },
            ["っは"] = new string[] { "hha" },
            ["っば"] = new string[] { "bba" },
            ["っぱ"] = new string[] { "ppa" },
            ["っひ"] = new string[] { "hhi" },
            ["っび"] = new string[] { "bbi" },
            ["っぴ"] = new string[] { "ppi" },
            ["っふ"] = new string[] { "hhu" },
            ["っぶ"] = new string[] { "bbu" },
            ["っぷ"] = new string[] { "ppu" },
            ["っへ"] = new string[] { "hhe" },
            ["っべ"] = new string[] { "bbe" },
            ["っぺ"] = new string[] { "ppe" },
            ["っほ"] = new string[] { "hho" },
            ["っぼ"] = new string[] { "bbo" },
            ["っぽ"] = new string[] { "ppo" },
            ["っま"] = new string[] { "mma" },
            ["っみ"] = new string[] { "mmi" },
            ["っむ"] = new string[] { "mmu" },
            ["っめ"] = new string[] { "mme" },
            ["っも"] = new string[] { "mmo" },
            ["っゃ"] = new string[] { "llya", "xxya" },
            ["っや"] = new string[] { "yya" },
            ["っゅ"] = new string[] { "llyu", "xxyu" },
            ["っゆ"] = new string[] { "yyu" },
            ["っょ"] = new string[] { "llyo", "xxyo" },
            ["っよ"] = new string[] { "yyo" },
            ["っら"] = new string[] { "rra" },
            ["っり"] = new string[] { "rri" },
            ["っる"] = new string[] { "rru" },
            ["っれ"] = new string[] { "rre" },
            ["っろ"] = new string[] { "rro" },
            ["っゎ"] = new string[] { "llwa", "xxwa" },
            ["っわ"] = new string[] { "wwa" },
            ["っゐ"] = new string[] { "wwi" },
            ["っゑ"] = new string[] { "wwe" },
            ["っを"] = new string[] { "wwo" },
            ["っゔ"] = new string[] { "vvu" },
            ["っゕ"] = new string[] { "llka", "xxka" },
            ["っゖ"] = new string[] { "llke", "xxke" },

            ["きゃ"] = new string[] { "kya" },
            ["きぃ"] = new string[] { "kyi" },
            ["きゅ"] = new string[] { "kyu" },
            ["きぇ"] = new string[] { "kye" },
            ["きょ"] = new string[] { "kyo" },
            ["ぎゃ"] = new string[] { "gya" },
            ["ぎゅ"] = new string[] { "gyu" },
            ["ぎぇ"] = new string[] { "gye" },
            ["ぎょ"] = new string[] { "gyo" },
            ["しゃ"] = new string[] { "sya", "sha" },
            ["しぃ"] = new string[] { "syi" },
            ["しゅ"] = new string[] { "syu", "shu" },
            ["しぇ"] = new string[] { "sye", "she" },
            ["しょ"] = new string[] { "syo", "sho" },
            ["じゃ"] = new string[] { "zya", "ja", "jya" },
            ["じぃ"] = new string[] { "zyi", "jyi" },
            ["じゅ"] = new string[] { "zyu", "ju", "jyu" },
            ["じぇ"] = new string[] { "zye", "je", "jye" },
            ["じょ"] = new string[] { "zyo", "jo", "jyo" },
            ["ちゃ"] = new string[] { "tya", "cha" },
            ["ちぃ"] = new string[] { "tyi" },
            ["ちゅ"] = new string[] { "tyu", "chu" },
            ["ちぇ"] = new string[] { "tye", "che" },
            ["ちょ"] = new string[] { "tyo", "cho" },
            ["ぢゃ"] = new string[] { "dya", },
            ["ぢぃ"] = new string[] { "dyi", },
            ["ぢゅ"] = new string[] { "dyu" },
            ["ぢぇ"] = new string[] { "dye" },
            ["ぢょ"] = new string[] { "dyo" },
            ["にゃ"] = new string[] { "nya" },
            ["にぃ"] = new string[] { "nyi" },
            ["にゅ"] = new string[] { "nyu" },
            ["にぇ"] = new string[] { "nye" },
            ["にょ"] = new string[] { "nyo" },
            ["ひゃ"] = new string[] { "hya" },
            ["ひぃ"] = new string[] { "hyi" },
            ["ひゅ"] = new string[] { "hyu" },
            ["ひぇ"] = new string[] { "hye" },
            ["ひょ"] = new string[] { "hyo" },
            ["びゃ"] = new string[] { "bya" },
            ["びぃ"] = new string[] { "byi" },
            ["びゅ"] = new string[] { "byu" },
            ["びぇ"] = new string[] { "bye" },
            ["びょ"] = new string[] { "byo" },
            ["ぴゃ"] = new string[] { "pya" },
            ["ぴぃ"] = new string[] { "pyi" },
            ["ぴゅ"] = new string[] { "pyu" },
            ["ぴぇ"] = new string[] { "pye" },
            ["ぴょ"] = new string[] { "pyo" },
            ["みゃ"] = new string[] { "mya" },
            ["みぃ"] = new string[] { "myi" },
            ["みゅ"] = new string[] { "myu" },
            ["みぇ"] = new string[] { "mye" },
            ["みょ"] = new string[] { "myo" },
            ["りゃ"] = new string[] { "rya" },
            ["りぃ"] = new string[] { "ryi" },
            ["りゅ"] = new string[] { "ryu" },
            ["りぇ"] = new string[] { "rye" },
            ["りょ"] = new string[] { "ryo" },

            ["っきゃ"] = new string[] { "kkya" },
            ["っきぃ"] = new string[] { "kkyi" },
            ["っきゅ"] = new string[] { "kkyu" },
            ["っきぇ"] = new string[] { "kkye" },
            ["っきょ"] = new string[] { "kkyo" },
            ["っぎゃ"] = new string[] { "ggya" },
            ["っぎゅ"] = new string[] { "ggyu" },
            ["っぎぇ"] = new string[] { "ggye" },
            ["っぎょ"] = new string[] { "ggyo" },
            ["っしゃ"] = new string[] { "ssya", "ssha" },
            ["っしぃ"] = new string[] { "ssyi" },
            ["っしゅ"] = new string[] { "ssyu", "shu" },
            ["っしぇ"] = new string[] { "ssye", "she" },
            ["っしょ"] = new string[] { "ssyo", "sho" },
            ["っじゃ"] = new string[] { "zzya", "jja", "jjya" },
            ["っじぃ"] = new string[] { "zzyi", "jjyi" },
            ["っじゅ"] = new string[] { "zzyu", "jju", "jjyu" },
            ["っじぇ"] = new string[] { "zzye", "jje", "jjye" },
            ["っじょ"] = new string[] { "zzyo", "jjo", "jjyo" },
            ["っちゃ"] = new string[] { "ttya", "ccha" },
            ["っちぃ"] = new string[] { "ttyi" },
            ["っちゅ"] = new string[] { "ttyu", "cchu" },
            ["っちぇ"] = new string[] { "ttye", "cche" },
            ["っちょ"] = new string[] { "ttyo", "ccho" },
            ["っぢゃ"] = new string[] { "ddya", },
            ["っぢぃ"] = new string[] { "ddyi", },
            ["っぢゅ"] = new string[] { "ddyu" },
            ["っぢぇ"] = new string[] { "ddye" },
            ["っぢょ"] = new string[] { "ddyo" },
            ["っひゃ"] = new string[] { "hhya" },
            ["っひぃ"] = new string[] { "hhyi" },
            ["っひゅ"] = new string[] { "hhyu" },
            ["っひぇ"] = new string[] { "hhye" },
            ["っひょ"] = new string[] { "hhyo" },
            ["っびゃ"] = new string[] { "bbya" },
            ["っびぃ"] = new string[] { "bbyi" },
            ["っびゅ"] = new string[] { "bbyu" },
            ["っびぇ"] = new string[] { "bbye" },
            ["っびょ"] = new string[] { "bbyo" },
            ["っぴゃ"] = new string[] { "ppya" },
            ["っぴぃ"] = new string[] { "ppyi" },
            ["っぴゅ"] = new string[] { "ppyu" },
            ["っぴぇ"] = new string[] { "ppye" },
            ["っぴょ"] = new string[] { "ppyo" },
            ["っみゃ"] = new string[] { "mmya" },
            ["っみぃ"] = new string[] { "mmyi" },
            ["っみゅ"] = new string[] { "mmyu" },
            ["っみぇ"] = new string[] { "mmye" },
            ["っみょ"] = new string[] { "mmyo" },
            ["っりゃ"] = new string[] { "rrya" },
            ["っりぃ"] = new string[] { "rryi" },
            ["っりゅ"] = new string[] { "rryu" },
            ["っりぇ"] = new string[] { "rrye" },
            ["っりょ"] = new string[] { "rryo" },

            ["くぁ"] = new string[] { "qa", "qwa" },
            ["くぃ"] = new string[] { "qi", "qwi" },
            ["くぅ"] = new string[] { "qwu" },
            ["くぇ"] = new string[] { "qe", "qwe" },
            ["くぉ"] = new string[] { "qo", "qwo" },
            ["ぐぁ"] = new string[] { "gwa" },
            ["ぐぃ"] = new string[] { "gwi" },
            ["ぐぅ"] = new string[] { "gwu" },
            ["ぐぇ"] = new string[] { "gwe" },
            ["ぐぉ"] = new string[] { "gwo" },
            ["つぁ"] = new string[] { "tsa" },
            ["つぃ"] = new string[] { "tsi" },
            ["つぇ"] = new string[] { "tse" },
            ["つぉ"] = new string[] { "tso" },
            ["てゃ"] = new string[] { "tha" },
            ["てぃ"] = new string[] { "thi" },
            ["てゅ"] = new string[] { "thu" },
            ["てぇ"] = new string[] { "the" },
            ["てょ"] = new string[] { "tho" },
            ["でゃ"] = new string[] { "dha" },
            ["でぃ"] = new string[] { "dhi" },
            ["でゅ"] = new string[] { "dhu" },
            ["でぇ"] = new string[] { "dhe" },
            ["でょ"] = new string[] { "dho" },
            ["とぁ"] = new string[] { "twa" },
            ["とぃ"] = new string[] { "twi" },
            ["とぅ"] = new string[] { "twu" },
            ["とぇ"] = new string[] { "twe" },
            ["とぉ"] = new string[] { "two" },
            ["どぁ"] = new string[] { "dwa" },
            ["どぃ"] = new string[] { "dwi" },
            ["どぅ"] = new string[] { "dwu" },
            ["どぇ"] = new string[] { "dwe" },
            ["どぉ"] = new string[] { "dwo" },
            ["ふぁ"] = new string[] { "fa" },
            ["ふぃ"] = new string[] { "fi" },
            ["ふぇ"] = new string[] { "fe" },
            ["ふぉ"] = new string[] { "fo" },
            ["ふゃ"] = new string[] { "fya" },
            ["ふゅ"] = new string[] { "fyu" },
            ["ふょ"] = new string[] { "fyo" },

            ["っくぁ"] = new string[] { "qqa", "qqwa" },
            ["っくぃ"] = new string[] { "qqi", "qqwi" },
            ["っくぅ"] = new string[] { "qqwu" },
            ["っくぇ"] = new string[] { "qqe", "qqwe" },
            ["っくぉ"] = new string[] { "qqo", "qqwo" },
            ["っぐぁ"] = new string[] { "ggwa" },
            ["っぐぃ"] = new string[] { "ggwi" },
            ["っぐぅ"] = new string[] { "ggwu" },
            ["っぐぇ"] = new string[] { "ggwe" },
            ["っぐぉ"] = new string[] { "ggwo" },
            ["っつぁ"] = new string[] { "ttsa" },
            ["っつぃ"] = new string[] { "ttsi" },
            ["っつぇ"] = new string[] { "ttse" },
            ["っつぉ"] = new string[] { "ttso" },
            ["ってゃ"] = new string[] { "ttha" },
            ["ってぃ"] = new string[] { "tthi" },
            ["ってゅ"] = new string[] { "tthu" },
            ["ってぇ"] = new string[] { "tthe" },
            ["ってょ"] = new string[] { "ttho" },
            ["っでゃ"] = new string[] { "ddha" },
            ["っでぃ"] = new string[] { "ddhi" },
            ["っでゅ"] = new string[] { "ddhu" },
            ["っでぇ"] = new string[] { "ddhe" },
            ["っでょ"] = new string[] { "ddho" },
            ["っとぁ"] = new string[] { "ttwa" },
            ["っとぃ"] = new string[] { "ttwi" },
            ["っとぅ"] = new string[] { "ttwu" },
            ["っとぇ"] = new string[] { "ttwe" },
            ["っとぉ"] = new string[] { "ttwo" },
            ["っどぁ"] = new string[] { "ddwa" },
            ["っどぃ"] = new string[] { "ddwi" },
            ["っどぅ"] = new string[] { "ddwu" },
            ["っどぇ"] = new string[] { "ddwe" },
            ["っどぉ"] = new string[] { "ddwo" },
            ["っふぁ"] = new string[] { "ffa" },
            ["っふぃ"] = new string[] { "ffi" },
            ["っふぇ"] = new string[] { "ffe" },
            ["っふぉ"] = new string[] { "ffo" },
            ["っふゃ"] = new string[] { "ffya" },
            ["っふゅ"] = new string[] { "ffyu" },
            ["っふょ"] = new string[] { "ffyo" },

            ["ー"] = new string[] { "-" },
            ["・"] = new string[] { "/" },
            [" "] = new string[] { " " },
            ["　"] = new string[] { " " },
            ["0"] = new string[] { "0" },
            ["1"] = new string[] { "1" },
            ["2"] = new string[] { "2" },
            ["3"] = new string[] { "3" },
            ["4"] = new string[] { "4" },
            ["5"] = new string[] { "5" },
            ["6"] = new string[] { "6" },
            ["7"] = new string[] { "7" },
            ["8"] = new string[] { "8" },
            ["9"] = new string[] { "9" },
            ["０"] = new string[] { "0" },
            ["１"] = new string[] { "1" },
            ["２"] = new string[] { "2" },
            ["３"] = new string[] { "3" },
            ["４"] = new string[] { "4" },
            ["５"] = new string[] { "5" },
            ["６"] = new string[] { "6" },
            ["７"] = new string[] { "7" },
            ["８"] = new string[] { "8" },
            ["９"] = new string[] { "9" },

        });
        /// <summary>んを入力するときにnではなくnnと入力しなければならなくなるかな文字の配列 </summary>
        public static readonly ReadOnlyCollection<string> must_nn_list = Array.AsReadOnly(new string[] {"あ", "い", "う", "え", "お", "な", "に", "ぬ", "ね", "の", "にゃ", "にぃ", "にゅ", "にぇ", "にょ", "や", "ゆ", "よ", "っや", "っゆ", "っよ" });
        /// <summary>問題文長上限</summary>
        public const int MAX_SENTENCE_LEN = 8192;

        /// <summary>
        /// cが半角文字か判定する
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>cが半角文字であればTrueを返す</returns>
        public static bool ifHalfWidth(char c)
        {
            return c is >= (char)0x20 and <= (char)0x7F;
        }


        /// <summary>
        /// 特殊キー以外の文字を入力受付する
        /// </summary>
        /// <returns></returns>
        public static char getChar()
        {
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                // Ignore if Alt or Ctrl is pressed.
                if ((keyinfo.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
                    continue;
                if ((keyinfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                    continue;
                // Ignore if KeyChar value is \u0000.
                // 押されたキーが Unicode 文字にマップされない場合 (たとえば、ユーザーが F1 キーまたは Home キーを押した場合)、プロパティの KeyChar 値は \U0000
                if (keyinfo.KeyChar == '\u0000') continue;
                // Ignore tab key.
                if (keyinfo.Key == ConsoleKey.Tab) continue;
                // Ignore backspace.
                if (keyinfo.Key == ConsoleKey.Backspace) continue;
                // Ignore escape key
                if (keyinfo.Key == ConsoleKey.Escape) continue;
                // Ignore Eneter
                if (keyinfo.Key == ConsoleKey.Enter) continue;

                break;
            } while (true);
            if (Char.IsLetter(keyinfo.KeyChar))
            {
                // 入力された文字がアルファベットなどの文字の場合
                if ((keyinfo.Modifiers & ConsoleModifiers.Shift) == 0)
                {
                    // Shiftキーが押されていない場合は、入力された文字をそのままバッファに追加する
                    return keyinfo.KeyChar;
                }
                else
                {
                    // Shiftキーが押されている場合は、CapsLockキーの状態に応じて大文字または小文字にする
                    // Windowsだけ 後々それ以外のOSに対応します
                    if (Console.CapsLock)
                        return Char.ToLower(keyinfo.KeyChar);
                    else
                        return Char.ToUpper(keyinfo.KeyChar);
                }
            }

            return keyinfo.KeyChar;
        }

        public static string ToJSON<T>(T obj) where T : notnull
        {
            var options = new JsonSerializerOptions
            {
                // JavaScriptEncoder.Createでエンコードしない文字を指定するのも可
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                // 読みやすいようインデントを付ける
                WriteIndented = true
            };
            return JsonSerializer.Serialize(obj, options);
        }

        public static void Write<T>(T obj, string path = "") where T : notnull
        {
            if (path == "")
            {
                path = DateTime.Now.ToFileTime().ToString() + "_" + obj.GetType().Name + ".txt";
            }
            // await File.WriteAllTextAsync(path, ToJSON(obj));
            File.WriteAllText(path, ToJSON(obj));
            return;
        }

        public static T FromJSON<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // optionsは省略可
            T sentence = JsonSerializer.Deserialize<T>(json, options) ?? throw new JsonException();
            return sentence;
        }

        public static T Read<T>(string filepath)
        {
            string json = System.IO.File.ReadAllText(filepath);
            return FromJSON<T>(json);
        }

    }

    /// <summary>1つの単語について管理するクラス</summary>
    class TypeWord
    {
        public readonly string kana;
        public readonly string romaji;
        public int idx;

        public TypeWord(string kana, string romaji, int idx = 0)
        {
            this.kana = kana;
            this.romaji = romaji;
            this.idx = idx;
        }
    }

    /// <summary>
    /// 1つの入力お題について管理するクラス
    /// </summary>
    class TypeSentence
    {
        /// <summary>お題の文章</summary>
        public string Sentence { get; set; }
        /// <summary>表示する文章</summary>
        public string Text { get; set; }
        /// <summary> Sentenceを入力するための正式な入力ローマ字列 </summary>
        public string Formally_entered_string { get; set; } = "";
        /// <summary>実際の入力ローマ字列</summary>
        public string Acturally_entered_string { get; set; } = "";
        public List<TimeSpan> TimeSpanList { get; set; } = new();

        /// <summary>グラフの辺 <br></br>edge[v][i] = {s, t, u}の時, v文字目からu文字目にかな文字列s, ローマ字列t</summary>
        List<TypeWord>[] edge;
        /// <summary>入力中の単語の候補 {かな文字列, ローマ字列, index}</summary>
        public List<TypeWord> current = new(); // できる限りインスタンスを使いまわす
        /// <summary>入力中の文字番号</summary>
        public int idx;

        string prev_romaji;

        Stopwatch sw = new();

        public TypeSentence(string Sentence, string Text = "")
        {
            this.Sentence = Sentence;    // 問題文

            // 問題文のオートマトンっぽいグラフの構築
            edge = new List<TypeWord>[Sentence.Length];
            // for (int i = 0; i < Sentence.Length; i++) { edge[i] = new List<TypeWord>(); }
            // 尺取り法っぽくノードの検索をしてエッジの追加をしていく
            for (int l = 0, r = 1; l < Sentence.Length; l++, r = l + 1)
            {
                edge[l] = new List<TypeWord>();
                // 半角文字を追加
                if (TypeUtils.ifHalfWidth(Sentence[l]))
                {
                    edge[l].Add(new TypeWord(Sentence[l].ToString(), Sentence[l].ToString()));
                }
                else
                {
                    while (r <= Sentence.Length)
                    {
                        string kana_substr = Sentence[l..r];
                        if (TypeUtils.kana2romaList.TryGetValue(kana_substr, out string[]? val) == false) break;
                        foreach (var romaji in val)
                        {
                            edge[l].Add(new TypeWord(kana_substr, romaji));
                        }
                        r++;
                    }
                    // 都合のいい順番に並べる
                    edge[l].Sort((x, y) => x.kana.Length == y.kana.Length ? x.romaji.Length - y.romaji.Length : y.kana.Length - x.kana.Length);
                }

            }

            // んをnnと入力しなければならない場合
            for (int i = 0; i < Sentence.Length; ++i)
            {
                edge[i].RemoveAll(val => val.kana == "ん" && val.romaji == "n" && (i + val.romaji.Length == Sentence.Length || TypeUtils.must_nn_list.IndexOf(edge[i + val.romaji.Length][0].kana) != -1));
            }

            MakeCandidate(0);

            idx = 0;
            prev_romaji = "";
            this.Text = Text;
            if (Text == "") this.Text = this.Sentence;
        }


        /// <summary>
        /// _start文字目から入力される可能性のある単語群の生成(非破壊的)
        /// </summary>
        /// <param name="_start">入力する文字の添え字</param>
        /// <returns>候補のリストを返す</returns>
        public List<TypeWord> MakedCandidate(int _start) => MC(new(), _start);

        /// <summary>
        /// _start文字目から入力される可能性のある単語群の生成(破壊的)
        /// </summary>
        /// <param name="_start">入力する文字の添え字</param>
        /// <returns></returns>
        public List<TypeWord> MakeCandidate(int _start)
        {
            current.Clear();
            return MC(current, _start);
        }

        private List<TypeWord> MC(List<TypeWord> list, int _start)
        {
            list.AddRange(edge[_start]);
            return list;
        }

        /// <summary>
        /// 今後入力されるローマ字列を返す
        /// </summary>
        /// <returns></returns> 
        //---------------------------------------stringbuilder使えそう
        public string InputCandStr()
        {
            StringBuilder romaji = new();
            ReadOnlySpan<char> tmp = Span<char>.Empty;

            int _next = 0;

            foreach (var val in current)
            {
                if (_next < val.kana.Length)
                {
                    _next = val.kana.Length;
                    tmp = val.romaji.AsSpan()[val.idx..];
                }
            }
            _next += idx;
            romaji.Append(tmp);

            while (_next < Sentence.Length)
            {
                var v = edge[_next][0];
                _next += v.kana.Length;
                romaji.Append(v.romaji);
            }

            return romaji.ToString();
        }

        /// <summary>
        /// 問題文の入力が完了していればtrueを返す
        /// </summary>
        /// <returns></returns>
        public bool hasEndedEntry()
        {
            return idx == Sentence.Length;
        }

        public bool getChar()
        {
            var c = TypeUtils.getChar();
            Console.Write(c);
            var ret = inputChar(c);
            return ret;
        }
        public void TimerStart() { sw.Start(); }
        public void TimerStop() { sw.Stop(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns>問題文に対し正しい入力であればtrueを返す</returns>
        public bool inputChar(char c)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
                TimeSpanList.Add(sw.Elapsed);
            }
            sw.Restart();

            Acturally_entered_string += c;

            // "ん"のnn入力を許容する
            if (c == 'n' && prev_romaji == "n")
            {
                Formally_entered_string += c;
                prev_romaji = "";
                return true;
            }


            bool exists_collect_input = false;

            // 入力文字が候補と一致しているか検索
            foreach (var val in current)
            {
                // 各候補ローマ字列の入力待ち文字が入力された場合 trueにする
                if (val.romaji[val.idx] == c)
                {
                    exists_collect_input = true;
                    break;
                }
            }

            // 候補と一致する入力である場合
            if (exists_collect_input)
            {
                Formally_entered_string += c;
                // 候補から外れた候補群を削除
                current.RemoveAll(val => val.romaji[val.idx] != c);

                for (int i = 0; i < current.Count; ++i)
                {

                    // 入力をかな1文字分満たした場合
                    if (current[i].romaji.Length == current[i].idx + 1)
                    {
                        prev_romaji = current[i].romaji;
                        idx += current[i].kana.Length;

                        // 全文字入力した場合
                        if (idx == Sentence.Length)
                        {
                            break;
                        }

                        MakeCandidate(idx);
                        break;
                    }
                    else
                    {
                        current[i].idx++; 
                    }
                }
            }


            return exists_collect_input;
        }

        /// <summary>
        /// 入力に要した時間
        /// </summary>
        /// <returns></returns>
        public TimeSpan sumTime()
        {
            return TimeSpanList.Aggregate((p, x) => p + x);
        }

    }
    class GetTypeSentence : IEquatable<GetTypeSentence>
    {
        /// <summary>お題の文章</summary>
        public string Sentence { get; set; } = "";
        /// <summary>表示する文章</summary>
        public string Text { get; set; } = "";
        /// <summary> Sentenceを入力するための正式な入力ローマ字列 </summary>
        public string Formally_entered_string { get; set; } = "";
        /// <summary>実際の入力ローマ字列</summary>
        public string Acturally_entered_string { get; set; } = "";

        public bool Equals(GetTypeSentence? other)
        {
            return Sentence == other?.Sentence &&
            Text == other?.Text &&
            Formally_entered_string == other?.Formally_entered_string &&
            Acturally_entered_string == other?.Acturally_entered_string;
        }
        // public List<TimeSpan> TimeSpanList { get; } = new();
    }

    class TypeGame
    {
        public TypeSentence[] Sentences { get; set; }
        /// <summary>入力中の問題番号</summary>
        public int problem_no = 0;
        public int sentence_idx
        {
            get
            {
                if (isGameCompleted())
                    return -1;
                else
                    return CurrentSentence().idx;
            }
        }

        public int N
        {
            get { return Sentences.Length; }
        }

        public bool is_sentence_moving = true;

        public TypeGame(int n = 10)
        {
            Sentences = new TypeSentence[n];
            MakeSentences(n);

        }

        void MakeSentences(int n = 10)
        {
            /*
            var text = new TextGeneration(n);
            var textList = text.GetText();
            */

            string[] t1 = { "七転び八起き", "あさはぱん", "デッドデッドデーモンズデデデデデストラクション", "シャバダバ酒場じゃ朝から生　鼻高々なかなか頭が空", "木村カエラ", "ONE PIECE", "燕雀安んぞ鴻鵠の志を知らんや", "qwerty", "ンジャメナ", "グリムジョー・ジャガージャック" },
                     t2 = { "ななころびやおき", "あさはぱん", "でっどでっどでーもんずででででですとらくしょん", "しゃばだばさかばじゃあさからなま はなたかだかなかなかあたまがから", "きむらかえら", "ONE PIECE", "えんじゃくいずくんぞこうこくのこころざしをしらんや", "qwerty", "んじゃめな", "ぐりむじょー・じゃがーじゃっく" };

            Tuple<string, string>[] textList = new Tuple<string, string>[n];
            for (int i = 0; i < n; ++i)
            {
                textList[i] = new Tuple<string, string>(t1[i], t2[i]);
            }
            for (int i = 0; i < n; ++i)
            {
                Sentences[i] = new TypeSentence(textList[i].Item2, textList[i].Item1);
            }
        }

        public void Start()
        {
            Sentences[0].TimerStart();
        }
        void Next()
        {
            CurrentSentence().TimerStop();
            problem_no++;
            if (!isGameCompleted())
                CurrentSentence().TimerStart();
        }

        public TypeSentence CurrentSentence()
        {
            return Sentences[problem_no];
        }

        public bool isGameCompleted()
        {
            return problem_no == N;
        }

        public bool inputChar(char c)
        {
            is_sentence_moving = false;

            var is_collect = CurrentSentence().inputChar(c);


            if (CurrentSentence().hasEndedEntry())
            {
                is_sentence_moving = true;
                Next();
            }
            else
            {
                Console.WriteLine(" " + CurrentSentence().InputCandStr());
            }

            return is_collect;
        }

        public bool getChar()
        {
            var c = TypeUtils.getChar();
            Console.Write(c);
            var ret = inputChar(c);
            return ret;
        }

        /// <summary>
        /// 入力に要した時間
        /// </summary>
        /// <returns></returns>
        public TimeSpan sumTime()
        {
            TimeSpan sum = new();
            foreach (var sentence in Sentences)
            {
                sum += sentence.TimeSpanList.Aggregate((p, x) => p + x);
            }
            return sum;
        }

        public (int, int) input_str_length()
        {
            int act_sum = 0, f_sum = 0;
            foreach (var sentence in Sentences)
            {
                act_sum += sentence.Acturally_entered_string.Length;
                f_sum += sentence.Formally_entered_string.Length;
            }
            return (act_sum, f_sum);
        }

    }
    class GetTypeGame : IEquatable<GetTypeGame>
    {
        public GetTypeSentence[] Sentences { get; set; } = Array.Empty<GetTypeSentence>();

        public bool Equals(GetTypeGame? other)
        {
            if (other is null) return false;
            return Sentences.SequenceEqual(other.Sentences);
        }

        public int N
        {
            get { return Sentences.Length; }
        }
    }

    class TypeFileFormat
    {
        public string Username { get; set; }
        public List<TypeGame> GameHistory { get; set; }

        public List<DateTime> GameDatetime { get; set; }

        public TypeFileFormat()
        {
            Username = string.Empty;
            GameHistory = new List<TypeGame>();
            GameDatetime = new List<DateTime>();

        }
        TypeFileFormat(string username, TypeGame[] gameHistory, DateTime[] gameDatetime)
        {
            Username = username;
            GameHistory = gameHistory.ToList();
            GameDatetime = gameDatetime.ToList();
        }
    }


    static class Programs
    {
        static void Main()
        {
            TypeFileFormat game_history;
            if (File.Exists("data.txt"))
            {
                game_history = TypeUtils.Read<TypeFileFormat>("data.txt");
            }
            else
            {
                game_history = new();
            }

            TypeGame game = new(10);

            string usernaem;

            if (game_history.Username != string.Empty)
            {
                usernaem = game_history.Username;
            }

            if (game_history.Username == string.Empty)
            {
                game_history.Username = "No Name";
            }


            while (!game.isGameCompleted())
            {
                if (game.is_sentence_moving)
                {
                    Console.WriteLine("");
                    Console.WriteLine(game.CurrentSentence().Text);
                    Console.WriteLine(game.CurrentSentence().Sentence);
                }

                var is_collect = game.getChar();
                if (!is_collect)
                {
                    //Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }


            // タイピングの結果を出力
            game_history.GameHistory.Add(game);
            game_history.GameDatetime.Add(DateTime.Now);
            TypeUtils.Write(game_history, "data.txt");


            Console.WriteLine("");
            int act_len, f_len;
            (act_len, f_len) = game.input_str_length();
            Console.WriteLine("スコア : {0}", f_len / game.sumTime().TotalMinutes * f_len / act_len * 10);
            // Console.WriteLine("正確性 : {0}%", (double)f_len / act_len * 100);
            // Console.WriteLine("入力時間 : {0} tps: {1}", game.sumTime().TotalSeconds, f_len / game.sumTime().TotalSeconds);


            Console.WriteLine("過去の記録");
            for (int i = 0; i < game_history.GameHistory.Count; i++)
            {
                DateTime dt = game_history.GameDatetime[i];
                (act_len, f_len) = game_history.GameHistory[i].input_str_length();
                var score = f_len / game_history.GameHistory[i].sumTime().TotalMinutes * f_len / act_len * 10;
                Console.WriteLine("{0} : score: {1}", dt.ToString("F"), score);
            }

            // ここからndjson

            TypeGame[] typeGames = new TypeGame[] { game, game };

            TypeUtils.Write(typeGames, "a.txt");

            string fn = "b.ndjson";
            using (Stream stream = new FileStream(fn, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // typeGames.ToNDJson(stream, false);
                var t = typeGames.ToNDJsonAsync(stream);
                while (!t.IsCompleted)
                {
                    Thread.Sleep(1);
                }
                Console.WriteLine(t.Status);
                Console.WriteLine(t.Exception);
            }

            List<GetTypeGame> tg1 = TypeUtils.FromJSON<List<GetTypeGame>>(File.ReadAllText("a.txt", Encoding.UTF8));
            List<GetTypeGame> tg2;

            using (Stream stream = new FileStream(fn, FileMode.OpenOrCreate, FileAccess.Read))
            {
                tg2 = Extends.FromNDJson(stream);
            }
            Console.WriteLine(tg1.SequenceEqual(tg2));
        }
    }

    static class Extends
    {
        static readonly byte[] Utf8NewLine = Encoding.UTF8.GetBytes(Environment.NewLine);

        public static void ToNDJson(this IReadOnlyCollection<TypeGame> games, Stream stream, bool leaveOpen = false)
        {
            var options = new JsonSerializerOptions
            {
                // JavaScriptEncoder.Createでエンコードしない文字を指定するのも可
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            foreach (TypeGame game in games)
            {
                JsonSerializer.Serialize(stream, game, options);
                stream.Write(Utf8NewLine);
            }
            if (!leaveOpen)
            {
                stream.Close();
            }
        }

        public static async Task ToNDJsonAsync(this IReadOnlyCollection<TypeGame> games, Stream stream, bool leaveOpen = false, CancellationToken ct = default)
        {
            var options = new JsonSerializerOptions
            {
                // JavaScriptEncoder.Createでエンコードしない文字を指定するのも可
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            foreach (TypeGame game in games)
            {
                await JsonSerializer.SerializeAsync(stream, game, options, ct);
                await stream.WriteAsync(Utf8NewLine, ct);
            }
            if (!leaveOpen)
            {
                stream.Close();
            }
        }

        public static List<GetTypeGame> FromNDJson(Stream stream)
        {
            List<GetTypeGame> games = new();

            byte[] buff = ArrayPool<byte>.Shared.Rent(1024);
            int index = 0;
            try
            {
                while (true)
                {
                    int n = stream.Read(buff.AsSpan(index));
                    if (n == 0) return games;
                    Span<byte> readBuff = buff.AsSpan(index, n);

                    int i = readBuff.IndexOf(Utf8NewLine);
                    if (i == -1)
                    {
                        index += readBuff.Length;
                        byte[] newBuff = ArrayPool<byte>.Shared.Rent(buff.Length * 2);
                        buff.CopyTo(newBuff.AsSpan());
                        readBuff = Span<byte>.Empty;
                        ArrayPool<byte>.Shared.Return(buff);
                        buff = newBuff;
                    }
                    else
                    {
                        index += i;
                        // Console.WriteLine(Encoding.UTF8.GetString(buff.AsSpan(0, index)));
                        if (JsonSerializer.Deserialize<GetTypeGame>(buff.AsSpan(0, index)) is GetTypeGame game)
                        {
                            games.Add(game);
                        }
                        else
                        {
                            throw new Exception("Json Reading Failed");
                        }
                        buff[index..].CopyTo(buff.AsSpan());
                        index = buff.Length - index;
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buff);
            }
        }
    }
}