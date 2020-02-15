using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

#nullable enable

namespace YugiohCardDatabase
{
    /// <summary>
    /// カードの分類を表す．
    /// </summary>
    public class CardKind : IEquatable<CardKind>, IComparable<CardKind>
    {
        public static readonly CardKind NormalMonster = new CardKind("通常");
        public static readonly CardKind EffectMonster = new CardKind("効果");
        public static readonly CardKind DualMonster = new CardKind("デュアル");
        public static readonly CardKind SpiritMonster = new CardKind("スピリット");
        public static readonly CardKind TunarMonster = new CardKind("チューナー");
        public static readonly CardKind ReverseMonster = new CardKind("リバース");
        public static readonly CardKind ToonMonster = new CardKind("トゥーン");
        public static readonly CardKind SpecialSummonMonster = new CardKind("特殊召喚");
        public static readonly CardKind RitualMonster = new CardKind("儀式");
        public static readonly CardKind FusionMonster = new CardKind("融合");
        public static readonly CardKind SynchroMonster = new CardKind("シンクロ");
        public static readonly CardKind XyzMonster = new CardKind("エクシーズ");
        public static readonly CardKind PendulumMonster = new CardKind("ペンデュラム");
        public static readonly CardKind LinkMonster = new CardKind("リンク");
        public static readonly CardKind NormalSpell = new CardKind("通常魔法");
        public static readonly CardKind RitualSpell = new CardKind("儀式魔法");
        public static readonly CardKind EquipSpell = new CardKind("装備魔法");
        public static readonly CardKind FieldSpell = new CardKind("フィールド魔法");
        public static readonly CardKind ContinuousSpell = new CardKind("永続魔法");
        public static readonly CardKind QuickSpell = new CardKind("速攻魔法");
        public static readonly CardKind NormalTrap = new CardKind("通常罠");
        public static readonly CardKind ContinuousTrap = new CardKind("永続罠");
        public static readonly CardKind CounterTrap = new CardKind("カウンター罠");

        private static readonly CardKind[] monsterCardKinds = new[]
        {
            NormalMonster,
            EffectMonster,
            DualMonster,
            SpiritMonster,
            TunarMonster,
            ReverseMonster,
            ToonMonster,
            SpecialSummonMonster,
            RitualMonster,
            FusionMonster,
            SynchroMonster,
            XyzMonster,
            PendulumMonster,
            LinkMonster
        };
        private static readonly CardKind[] extraDeckCardKinds = new[]
        {
            FusionMonster,
            SynchroMonster,
            XyzMonster,
            LinkMonster,
        };
        private static readonly CardKind[] spellCardKinds = new[]
        {
            NormalSpell,
            RitualSpell,
            EquipSpell,
            FieldSpell,
            ContinuousSpell,
            QuickSpell,
        };
        private static readonly CardKind[] trapCardKinds = new[]
        {
            NormalTrap,
            ContinuousTrap,
            CounterTrap,
        };

        private static readonly IReadOnlyDictionary<CardKind, int> comparisonOrder = new Dictionary<CardKind, int>()
        {
            { NormalMonster, 10 },
            { EffectMonster, 20 },
            { DualMonster,  30 },
            { SpiritMonster,  40 },
            { TunarMonster, 50 },
            { ReverseMonster, 60 },
            { ToonMonster, 70 },
            { SpecialSummonMonster, 80 },
            { RitualMonster, 90 },
            { FusionMonster, 100 },
            { SynchroMonster, 110 },
            { XyzMonster, 120 },
            { PendulumMonster, 130 },
            { LinkMonster, 140 },
            { NormalSpell, 150 },
            { RitualSpell, 160 },
            { EquipSpell, 170 },
            { FieldSpell, 180 },
            { ContinuousSpell, 190 },
            { QuickSpell, 200 },
            { NormalTrap, 210 },
            { ContinuousTrap, 220 },
            { CounterTrap, 230 },
        };

        private readonly string kind;

        /// <summary>
        /// この分類のカードがモンスターカードに属しているか取得する．
        /// </summary>
        [JsonIgnore]
        public bool IsMonster => monsterCardKinds.Any(k => k.kind == this.kind);
        /// <summary>
        /// この分類のカードがエクストラデッキに入るカードか取得する．
        /// </summary>
        [JsonIgnore]
        public bool IsExtra => extraDeckCardKinds.Any(k => k.kind == this.kind);
        /// <summary>
        /// この分類のカードが魔法カードか取得する．
        /// </summary>
        [JsonIgnore]
        public bool IsSpell => spellCardKinds.Any(k => k.kind == this.kind);
        /// <summary>
        /// この分類のカードが罠カードか取得する．
        /// </summary>
        [JsonIgnore]
        public bool IsTrap => trapCardKinds.Any(k => k.kind == this.kind);

        private CardKind(string kind) => this.kind = kind;

        public bool Equals(CardKind other) => this.kind.Equals(other.kind);

        public int CompareTo(CardKind other) => comparisonOrder[this].CompareTo(comparisonOrder[other]);

        public override bool Equals(object obj) => obj is CardKind kind && this.Equals(kind);

        public override int GetHashCode() => this.kind.GetHashCode();

        public override string ToString() => this.kind;
    }
}
