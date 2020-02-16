using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

#nullable enable

namespace YugiohCardDatabase
{

    /// <summary>
    /// カード情報をまとめて表す．
    /// </summary>
    [DataContract]
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        [DataMember]
        private readonly string name;
        [DataMember]
        private readonly string pronunciation;
        [DataMember]
        private readonly string description;
        [DataMember]
        private readonly CardKind[] kinds;
        [DataMember(EmitDefaultValue = false)]
        private readonly string? race;
        [DataMember(EmitDefaultValue = false)]
        private readonly string? attribute;
        [DataMember(EmitDefaultValue = false)]
        private readonly byte? level;
        [DataMember(EmitDefaultValue = false)]
        private readonly byte? rank;
        [DataMember(EmitDefaultValue = false)]
        private readonly PendulumScale? pendulumScale;
        [DataMember(EmitDefaultValue = false)]
        private readonly MonsterLink? link;
        [DataMember(EmitDefaultValue = false)]
        private readonly string? attack;
        [DataMember(EmitDefaultValue = false)]
        private readonly string? defence;

        /// <summary>
        /// 各カードに一意に与えられたカード名を取得する．
        /// </summary>
        [IgnoreDataMember]
        public string IdentityShortName => this.name;
        /// <summary>
        /// カード名の発音を取得する．
        /// </summary>
        [IgnoreDataMember]
        public string Pronunciation => this.pronunciation;
        /// <summary>
        /// カードに記載された効果テキストを取得する．
        /// </summary>
        [IgnoreDataMember]
        public string Description => this.description;
        [IgnoreDataMember]
        public IEnumerable<CardKind> Kinds => this.kinds;
        [IgnoreDataMember]
        public Option<MonsterRace> Race => Option.FromNullableClass(this.race).Map(r => new MonsterRace(r));
        [IgnoreDataMember]
        public Option<MonsterAttribute> Attribute => Option.FromNullableClass(this.attribute).Map(a => new MonsterAttribute(a));
        [IgnoreDataMember]
        public Option<MonsterLevel> Level => Option.FromNullableStruct(this.level).Map(l => new MonsterLevel(l));
        [IgnoreDataMember]
        public Option<MonsterRank> Rank => Option.FromNullableStruct(this.rank).Map(r => new MonsterRank(r));
        [IgnoreDataMember]
        public Option<PendulumScale> PendulumScale => Option.FromNullableStruct(this.pendulumScale);
        [IgnoreDataMember]
        public Option<MonsterLink> Link => Option.FromNullableStruct(this.link);
        [IgnoreDataMember]
        public Option<MonsterAttack> Attack => Option.FromNullableClass(this.attack).Map(a => new MonsterAttack(a));
        [IgnoreDataMember]
        public Option<MonsterDefence> Defence => Option.FromNullableClass(this.defence).Map(b => new MonsterDefence(b));

        public Card(string name,
            string pronunciation,
            string description,
            IEnumerable<CardKind> cardKinds,
            MonsterRace? race,
            MonsterAttribute? attribute,
            MonsterLevel? level,
            MonsterRank? rank,
            PendulumScale? pendulumScale,
            MonsterLink? link,
            MonsterAttack? attack,
            MonsterDefence? defence)
        {
            this.name = name;
            this.pronunciation = pronunciation;
            this.description = description;
            this.kinds = cardKinds.ToArray();
            this.race = race?.Race;
            this.attribute = attribute?.Attribute;
            this.level = level?.Level;
            this.rank = rank?.Rank;
            this.pendulumScale = pendulumScale;
            this.link = link;
            this.attack = attack?.ToString();
            this.defence = defence?.ToString();
        }

        /// <summary>
        /// カードの効果テキスト以外の情報 (カード名，種別，モンスターのレベルなど)を，視覚的に見やすいフォーマットで返す．
        /// </summary>
        /// <returns></returns>
        public string ConstructFormattedInfoWithoutDescription()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.name);
            builder.Append($" ({this.pronunciation})");
            foreach (var kind in this.Kinds)
            {
                builder.Append(" / " + kind);
            }
            this.Attribute.MayAct(attribute => builder.Append(" / " + attribute));
            this.Race.MayAct(race => builder.Append(" / " + race));
            this.Level.MayAct(level => builder.Append(" / " + level));
            this.Rank.MayAct(rank => builder.Append(" / " + rank));
            this.PendulumScale.MayAct(scale => builder.Append(" / " + scale));
            this.Link.MayAct(link => builder.Append(" / " + link));
            this.Attack.MayAct(attack => builder.Append(" / " + attack));
            this.Defence.MayAct(defence => builder.Append(" / " + defence));

            return builder.ToString();
        }

        public bool Equals(Card other) => this.IdentityShortName.Equals(other.IdentityShortName);

        public int CompareTo(Card other)
        {
            // まずはカード種別に順序付けを試みる．
            var maxKind = this.Kinds.Max();
            var otherMaxKind = other.Kinds.Max();
            var kindComparison = maxKind.CompareTo(otherMaxKind);
            if (kindComparison != 0) return kindComparison;
            // カード種別が同一なら，カード名で順序付けする．
            return this.name.CompareTo(other.name);
        }

        public override bool Equals(object obj) => obj is Card c && this.Equals(c);

        public override int GetHashCode() => this.IdentityShortName.GetHashCode();

        public override string ToString() => this.IdentityShortName;
    }
}
