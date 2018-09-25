namespace System.Collections.Generic
{
	/// <summary>
	/// A Bi-Directional Dictionary where keys are also values, and values are also keys.
	/// *** NO DUPLICATE KEYS OR VALUES ***.
	/// </summary>
	/// <typeparam name="TFlip"></typeparam>
	/// <typeparam name="TFlop"></typeparam>
	public class FlipFlopDictionary<TFlip, TFlop> : IDictionary<TFlip, TFlop>
	{
		private readonly IDictionary<TFlip, TFlop> Flip = new Dictionary<TFlip, TFlop>();
		private readonly IDictionary<TFlop, TFlip> Flop = new Dictionary<TFlop, TFlip>();

		public TFlop this[TFlip flip]
		{
			get => this.Flip[flip];
			set
			{
				this.Flip[flip] = value;
				this.Flop[value] = flip;
			}
		}
		public TFlip this[TFlop flop]
		{
			get => this.Flop[flop];
			set
			{
				this.Flop[flop] = value;
				this.Flip[value] = flop;
			}
		}

		public ICollection<TFlip> Keys => this.Flip.Keys;

		public ICollection<TFlop> Values => this.Flop.Keys;

		public int Count => this.Flip.Count;

		public bool IsReadOnly => this.Flip.IsReadOnly;

		public void Add(TFlip flip, TFlop flop)
		{
			this.Flip.Add(flip, flop);
			this.Flop.Add(flop, flip);
		}

		public void Add(KeyValuePair<TFlip, TFlop> flipFlop)
		{
			var flopFlip = new KeyValuePair<TFlop, TFlip>(flipFlop.Value, flipFlop.Key);
			this.Flip.Add(flipFlop);
			this.Flop.Add(flopFlip);
		}

		public void Clear()
		{
			this.Flip.Clear();
			this.Flop.Clear();
		}

		public bool Contains(KeyValuePair<TFlip, TFlop> flipFlop) => this.Flip.Contains(flipFlop);
		public bool Contains(KeyValuePair<TFlop, TFlip> flopFlip) => this.Flop.Contains(flopFlip);

		public bool ContainsKey(TFlip flip) => this.Flip.ContainsKey(flip);
		public bool ContainsKey(TFlop flop) => this.Flop.ContainsKey(flop);

		public void CopyTo(KeyValuePair<TFlip, TFlop>[] array, int arrayIndex) => this.Flip.CopyTo(array, arrayIndex);
		public void CopyTo(KeyValuePair<TFlop, TFlip>[] array, int arrayIndex) => this.Flop.CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<TFlip, TFlop>> GetEnumerator() => this.Flip.GetEnumerator();

		public bool Remove(TFlip flipKey)
		{
			var flopKey = this.Flip[flipKey];
			return this.Flip.Remove(flipKey) && this.Flop.Remove(flopKey);
		}
		public bool Remove(TFlop flopKey)
		{
			var flipKey = this.Flop[flopKey];
			return this.Flop.Remove(flopKey) && this.Flip.Remove(flipKey);
		}

		public bool Remove(KeyValuePair<TFlip, TFlop> flipFlop)
		{
			var flopFlip = new KeyValuePair<TFlop, TFlip>(flipFlop.Value, flipFlop.Key);
			return this.Flip.Remove(flipFlop) && this.Flop.Remove(flopFlip);
		}

		public bool TryGetValue(TFlip flip, out TFlop flop) => this.Flip.TryGetValue(flip, out flop);
		public bool TryGetValue(TFlop flop, out TFlip flip) => this.Flop.TryGetValue(flop, out flip);

		IEnumerator IEnumerable.GetEnumerator() => this.Flip.GetEnumerator();
	}
}