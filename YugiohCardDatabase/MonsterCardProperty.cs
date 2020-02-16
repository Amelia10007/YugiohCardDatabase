using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable enable

namespace YugiohCardDatabase
{
    /// <summary>
    /// モンスターカードの種族．
    /// </summary>
    [DataContract]
    public class MonsterRace : IEquatable<MonsterRace>
    {
        [DataMember]
        public readonly string Race;

        public MonsterRace(string race) => this.Race = race.TrimEnd('族');

        public bool Equals(MonsterRace other) => this.Race.Equals(other.Race);

        public override string ToString() => $"{this.Race}族";
    }

    /// <summary>
    /// モンスターカードの属性．
    /// </summary>
    [DataContract]
    public class MonsterAttribute : IEquatable<MonsterAttribute>
    {
        [DataMember]
        public readonly string Attribute;

        public MonsterAttribute(string attribute) => this.Attribute = attribute.Replace("属性", "").TrimEnd('属');

        public bool Equals(MonsterAttribute other) => this.Attribute.Equals(other.Attribute);

        public override string ToString() => $"{this.Attribute}属性";
    }

    /// <summary>
    /// モンスターのレベル．
    /// </summary>
    [DataContract]
    public struct MonsterLevel
    {
        [DataMember]
        public readonly byte Level;

        public MonsterLevel(byte level) => this.Level = level;

        public override string ToString() => $"★{this.Level}";
    }

    /// <summary>
    /// エクシーズモンスターのランク．
    /// </summary>
    [DataContract]
    public struct MonsterRank
    {
        [DataMember]
        public readonly byte Rank;

        public MonsterRank(byte rank) => this.Rank = rank;

        public override string ToString() => $"★{this.Rank}";
    }

    /// <summary>
    /// ペンデュラムモンスターのペンデュラムスケール．
    /// </summary>
    [DataContract]
    public struct PendulumScale
    {
        [DataMember]
        public readonly byte Red;
        [DataMember]
        public readonly byte Blue;

        public PendulumScale(byte red, byte blue)
        {
            this.Red = red;
            this.Blue = blue;
        }

        public override string ToString() => $"ペンデュラムスケール: 赤{this.Red}/青{this.Blue}";
    }

    /// <summary>
    /// リンクモンスターのリンク数およびリンクマーカー方向．
    /// </summary>
    [DataContract]
    public struct MonsterLink
    {
        [DataMember]
        private readonly string[] arrows;

        [IgnoreDataMember]
        public bool Up => this.arrows.Contains(nameof(this.Up));
        [IgnoreDataMember]
        public bool UpperRight => this.arrows.Contains(nameof(this.UpperRight));
        [IgnoreDataMember]
        public bool Right => this.arrows.Contains(nameof(this.Right));
        [IgnoreDataMember]
        public bool LowerRight => this.arrows.Contains(nameof(this.LowerRight));
        [IgnoreDataMember]
        public bool Low => this.arrows.Contains(nameof(this.Low));
        [IgnoreDataMember]
        public bool LowerLeft => this.arrows.Contains(nameof(this.LowerLeft));
        [IgnoreDataMember]
        public bool Left => this.arrows.Contains(nameof(this.Left));
        [IgnoreDataMember]
        public bool UpperLeft => this.arrows.Contains(nameof(this.UpperLeft));

        [IgnoreDataMember]
        public int LinkCount => this.arrows.Length;

        public MonsterLink(bool up, bool upperRight, bool right, bool lowerRight, bool low, bool lowerLeft, bool left, bool upperLeft)
        {
            List<string> arrows = new List<string>();
            if (up) arrows.Add(nameof(this.Up));
            if (upperRight) arrows.Add(nameof(this.UpperRight));
            if (right) arrows.Add(nameof(this.Right));
            if (lowerRight) arrows.Add(nameof(this.LowerRight));
            if (low) arrows.Add(nameof(this.Low));
            if (lowerLeft) arrows.Add(nameof(this.LowerLeft));
            if (left) arrows.Add(nameof(this.Left));
            if (upperLeft) arrows.Add(nameof(this.UpperLeft));

            this.arrows = arrows.ToArray();
        }

        public override string ToString()
        {
            List<string> markers = new List<string>();
            if (this.Up) markers.Add("上");
            if (this.UpperRight) markers.Add("右上");
            if (this.Right) markers.Add("右");
            if (this.LowerRight) markers.Add("右下");
            if (this.Low) markers.Add("下");
            if (this.LowerLeft) markers.Add("左下");
            if (this.Left) markers.Add("左");
            if (this.UpperLeft) markers.Add("左上");

            return $"リンク:{markers.Count}/" + markers.Aggregate((acc, next) => acc + "/" + next);
        }
    }

    /// <summary>
    /// モンスターの攻撃力．
    /// </summary>
    [DataContract]
    public class MonsterAttack
    {
        /// <summary>
        /// 攻撃力が一意に定まっていない状態．カード表記の`?`に相当する．
        /// </summary>
        public static readonly MonsterAttack Variable = new MonsterAttack("?");

        [DataMember]
        private readonly string status;

        [IgnoreDataMember]
        public Option<int> Status => int.TryParse(this.status, out var s) ? Option.Some(s) : Option.None<int>();

        [IgnoreDataMember]
        public bool IsFixedStatus => int.TryParse(this.status, out var _);

        internal MonsterAttack(string status) => this.status = status;

        public override string ToString() => $"ATK {this.status}";

        public static MonsterAttack Fixed(int status) => new MonsterAttack(status.ToString());
    }

    /// <summary>
    /// モンスターの守備力．
    /// </summary>
    [DataContract]
    public class MonsterDefence
    {
        /// <summary>
        /// 守備力が一意に定まっていない状態．カード表記の`?`に相当する．
        /// </summary>
        public static readonly MonsterDefence Variable = new MonsterDefence("?");

        [DataMember]
        private readonly string status;

        [IgnoreDataMember]
        public Option<int> Status => int.TryParse(this.status, out var s) ? Option.Some(s) : Option.None<int>();

        [IgnoreDataMember]
        public bool IsFixedStatus => int.TryParse(this.status, out var _);

        internal MonsterDefence(string status) => this.status = status;

        public override string ToString() => $"DEF {this.status}";

        public static MonsterDefence Fixed(int status) => new MonsterDefence(status.ToString());
    }
}
